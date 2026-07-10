using Gameplay.Inventory;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WorldItem : MonoBehaviour, IInteractable
{
    [Header("Item Data")]
    public ItemSO itemData;
    public int quantity = 1;

    [Header("Optional Highlight")]
    public Renderer[] highlightRenderers;
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

        if (highlightRenderers == null || highlightRenderers.Length == 0)
            highlightRenderers = GetComponentsInChildren<Renderer>();

        _mpb = new MaterialPropertyBlock();
    }

    /// <summary>Call before picking up so we can restore state on cancel.</summary>
    public void CacheOriginalTransform()
    {
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

    public void Perform()
    {
        
    }
}