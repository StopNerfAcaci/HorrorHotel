using Gameplay.CoreSystem;
using Horror.Events;
using UnityEngine;
using UnityEngine.Events;

public class Interaction : CoreComponents, IVisitable
{
    public static event UnityAction<Vector3, bool> OnHover;
    public static event UnityAction<IItem> OnInspect;
    public static event UnityAction OnFinishInspect;
    public static event UnityAction<NPC> onSelectedUsable;
    public static event UnityAction<NPC> onDeselectedUsable;

    [Header("References")] [SerializeField]
    private Camera playerCamera;

    [Header("Raycast")] [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask itemLayerMask = ~0;

    [Header("Hold / Inspect")] [SerializeField]
    private Vector3 holdLocalOffset = new Vector3(0f, 0f, 1.2f);

    private IInteractable _hoveredInteractable;
    private GameObject selection;

    private void Reset()
    {
        playerCamera = GetComponentInChildren<Camera>();
    }

    public override void LogicUpdate()
    {
        HandleHoverRaycast();
    }

    private void HandleHoverRaycast()
    {
        if (playerCamera == null) return;
        if (!((Behaviour)this).enabled || (Time.timeScale <= 0f)) return;

        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f));

        IInteractable newHover = null;
        bool hasTarget = false;
        Vector3 targetWorldPos = default;
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, itemLayerMask))
        {
            newHover = hit.collider.GetComponent<IInteractable>();
            if (newHover != null)
            {
                targetWorldPos = newHover.GetInteractableCenter(hit.collider);
                hasTarget = true;
            }
        }

        if (newHover != _hoveredInteractable)
        {
            _hoveredInteractable = newHover;
            OnHover?.Invoke(targetWorldPos, hasTarget);
        }
    }

    private void HandleInteraction(IInteractable interactable)
    {
        switch (interactable)
        {
            case IItem item:
                OnInspect?.Invoke(item);
                break;
            case NPC npc:
                SetCurrentNPC(npc);
                break;
        }
    }

    public void SetCurrentNPC(NPC npc)
    {
        // if ((Object)(object)npc == (Object)(object)this._hoveredInteractable)
        // {
        //     Debug.Log("Same interactable");
        //     return;
        // }
        if ((Object)(object)npc == (Object)null)
        {
            DeselectTarget();
            return;
        }

        if ((Object)_hoveredInteractable != (Object)null && (Object)_hoveredInteractable != (Object)(object)npc)
        {
            DeselectTarget();
        }
        _hoveredInteractable = npc;
        var hoveredNPC = _hoveredInteractable as NPC;
        hoveredNPC.disabled -= OnUsableDisabled;
        hoveredNPC.disabled += OnUsableDisabled;
        selection = hoveredNPC.gameObject;
        OnSelectedUsableObject(npc);
    }

    protected void DeselectTarget()
    {
        var hoveredNPC = _hoveredInteractable as NPC;
        if ((Object)(object)hoveredNPC != (Object)null)
        {
            hoveredNPC.disabled -= OnUsableDisabled;
        }

        OnDeselectedUsableObject(hoveredNPC);
        _hoveredInteractable = null;
    }

    protected void OnDeselectedUsableObject(NPC npc)
    {
        onDeselectedUsable?.Invoke(npc);
        if ((Object)(object)npc != (Object)null)
        {
            npc.OnDeselectUsable();
        }
    }

    private void OnUsableDisabled(NPC npc)
    {
        if (npc == this._hoveredInteractable)
        {
            DeselectTarget();
        }
    }

    private void OnSelectedUsableObject(NPC npc)
    {
        onSelectedUsable?.Invoke(npc);
        if (npc != null)
        {
            npc.OnSelectUsable();
        }
    }

    private void BeginInspect(IInteractable interactable)
    {
        HandleInteraction(interactable);
        interactable.Interact(new InteractContext()
        {
            // Source = _messagePayload,
            NewTransform = playerCamera.transform,
            Offset = holdLocalOffset,
        });

        _hoveredInteractable = null;
    }

    public bool TryPressed(out IInteractable item)
    {
        item = _hoveredInteractable;
        if (_hoveredInteractable == null) return false;
        BeginInspect(_hoveredInteractable);
        return true;
    }


    public bool HasItem() => _hoveredInteractable != null;
    public void Accept(IVisitor visitor) => visitor.Visit(this);

    public void TryConfirm(IInteractable interactable, System.Action onComplete = null)
    {
        switch (interactable)
        {
            case IItem item:
                HandleItemInteraction(item);
                onComplete?.Invoke();
                break;
            case NPC npc:
                var line = npc.GetDialogueDataThroughId();
                if (line != null)
                {
                    EventBus.Raise(new DialogueEventData(npc));
                }
                else
                {
                    onComplete?.Invoke();
                }
                break;
        }
        OnFinishInspect?.Invoke();
    }
    private void HandleItemInteraction(IItem item)
    {
        item.Confirm();
    }
}