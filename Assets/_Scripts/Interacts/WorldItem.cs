using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Inventory;
using UnityEngine;
using UnityServiceLocator;

[RequireComponent(typeof(Collider))]
public class WorldItem : MonoBehaviour, IItem
{
    [Header("Item Data")] public ItemSO itemData;
    public int quantity = 1;

    [Header("Optional Highlight")] public Renderer[] highlightRenderers;
    public Color highlightColor = Color.yellow;

    private Collider _collider;
    private Rigidbody _rigidbody;
    private Transform _originalParent;
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private Color[] _originalColors;
    private MaterialPropertyBlock _mpb;

    private PlayerInventory inventory;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();

        if (highlightRenderers == null || highlightRenderers.Length == 0)
            highlightRenderers = GetComponentsInChildren<Renderer>();

        _mpb = new MaterialPropertyBlock();

    }

    private void Start()
    {
        ServiceLocator.For(this).Get(out inventory);
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
        foreach (var r in highlightRenderers)
        {
            if (r == null) continue;
            r.GetPropertyBlock(_mpb);
            _mpb.SetColor("_EmissionColor", on ? highlightColor : Color.black);
            r.SetPropertyBlock(_mpb);
        }
    }

    public Transform Transform => transform;

    public async UniTask Use()
    {
        inventory.AddItem(itemData);
        transform.DOKill();
        await transform.DOMoveY(-6f, 1f).SetEase(Ease.OutBack);
        gameObject.SetActive(false);
        // Object.Destroy(gameObject);
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
}