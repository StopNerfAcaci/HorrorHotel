using System;
using Gameplay.CoreSystem;
using UnityEngine;
using UnityServiceLocator;
using VitalRouter;

public class Interaction : CoreComponents, IVisitable
{
    [Header("References")] [SerializeField]
    private Camera playerCamera;

    [Header("Raycast")] [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask itemLayerMask = ~0;

    [Header("Hold / Inspect")] [SerializeField]
    private Vector3 holdLocalOffset = new Vector3(0f, 0f, 1.2f);

    private IInteractable _hoveredItem;

    private Mediator<Interaction> mediator;

    // public static Action<ItemSO> OnInspectItem;
    private Router router;

    private void Start()
    {
        ServiceLocator.Global.Get<Router>(out router);
        mediator = ServiceLocator.Global.Get<Mediator<Interaction>>();
        
        mediator.Register(this);
    }

    private void OnDestroy()
    {
        mediator.Unregister(this);
    }

    private void Reset()
    {
        playerCamera = GetComponentInChildren<Camera>();
    }

    public override void LogicUpdate()
    {
        HandleHoverRaycast();
    }

    // ---------- Hover detection (before pickup) ----------

    private void HandleHoverRaycast()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f));

        IInteractable newHover = null;
        bool hasTarget = false;
        Vector3 targetWorldPos = default;
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, itemLayerMask))
        {
            newHover = hit.collider.GetComponentInParent<IInteractable>();
            if (newHover != null)
            {
                targetWorldPos = GetInteractableCenter(newHover, hit.collider);
                hasTarget = true;
            }
        }

        if (newHover != _hoveredItem)
        {
            _hoveredItem?.SetHighlighted(false);
            _hoveredItem = newHover;
            _hoveredItem?.SetHighlighted(true);
            router.PublishAsync(new InspectCommand(targetWorldPos, hasTarget));
        }
    }

    // ---------- Pickup / Inspect ----------
    private void BeginInspect(IInteractable item)
    {
        router.PublishAsync(new EndInspectCommand());
        item.Interact(new InteractContext()
        {
            // Source = _messagePayload,
            NewTransform = playerCamera.transform,
            Offset = holdLocalOffset,
        });

        _hoveredItem = null;
    }

    public bool TryPressed(out IInteractable item)
    {
        item = _hoveredItem;
        if (_hoveredItem == null) return false;
        BeginInspect(_hoveredItem);
        return true;
    }

    private static Vector3 GetInteractableCenter(IInteractable interactable, Collider fallback)
    {
        var root = (interactable as Component)?.transform;
        var col = root != null ? root.GetComponent<Collider>() : null;
        return col != null ? col.bounds.center : fallback.bounds.center;
    }

    public bool HasItem() => _hoveredItem != null;
    public void Accept(IVisitor visitor) => visitor.Visit(this);
}

public struct InspectCommand : ICommand
{
    public Vector3 position;
    public bool isHovering;

    public InspectCommand(Vector3 position, bool isHovering)
    {
        this.position = position;
        this.isHovering = isHovering;
    }
}

public struct EndInspectCommand : ICommand
{
}