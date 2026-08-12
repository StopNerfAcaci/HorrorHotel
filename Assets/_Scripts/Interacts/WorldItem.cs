using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WorldItem : MonoBehaviour, IItem
{
    [Header("Item Data")] public ItemSO itemData;
    [SerializeField] private Outline outline;
    public ItemSO Item => itemData;

    public Color highlightColor = Color.yellow;

    private Collider _collider;
    private Rigidbody _rigidbody;
    private Transform _originalParent;
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private Color[] _originalColors;
    private MaterialPropertyBlock _mpb;


    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
        OnValidate();

        _mpb = new MaterialPropertyBlock();
    }
    
    /// <summary>Call before picking up so we can restore state on cancel.</summary>
    public void CacheOriginalTransform()
    {
        SetHighlighted(false);
        _originalParent = transform.parent;
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
    }

    public void SetPhysicsEnabled(bool enabled)
    {
        _collider.enabled = enabled;
        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = !enabled;
            _rigidbody.useGravity = enabled;
        }
    }

    public void RestoreToWorld()
    {
        transform.SetParent(_originalParent, true);
        transform.position = _originalPosition;
        transform.rotation = _originalRotation;
        SetPhysicsEnabled(true);
    }

    public void SetHighlighted(bool on)
    {
        var mode = outline.OutlineMode;
        mode = on? Outline.Mode.OutlineVisible : Outline.Mode.OutlineHidden;
        outline.OutlineMode = mode;
    }

    public Transform Transform => transform;

    public async UniTask Use()
    {
        GlobalSettings.Inventory.Get().AddItem(itemData);
        transform.DOKill();
        await transform.DOMoveY(-4f, .5f).SetEase(Ease.OutBack);
        gameObject.SetActive(false);
    }

    public bool CanPerform() => true;
    
    public void Interact(InteractContext context)
    {
        CacheOriginalTransform();
        SetPhysicsEnabled(false);

        transform.SetParent(context.NewTransform, worldPositionStays: false);
        transform.localPosition = context.Offset;
        transform.localRotation = Quaternion.identity;
    }

    private void OnValidate()
    {
        if(outline == null) outline = GetComponent<Outline>();
    }
}