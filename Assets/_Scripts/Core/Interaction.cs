using System;
using Gameplay.CoreSystem;
using UnityEngine;

public class Interaction : CoreComponents
{
    [Header("References")] [SerializeField]
    private Camera playerCamera;

    [Header("Raycast")] [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask itemLayerMask = ~0;

    [Header("Hold / Inspect")] [SerializeField]
    private Vector3 holdLocalOffset = new Vector3(0f, 0f, 1.2f);

    private IInteractable _hoveredItem;
    

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
    
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, itemLayerMask))
        {
            newHover = hit.collider.GetComponentInParent<IInteractable>();
        }
    
        if (newHover != _hoveredItem)
        {
            // if (_hoveredItem != null) _hoveredItem.SetHighlighted(false);
            _hoveredItem = newHover;
            // if (_hoveredItem != null) _hoveredItem.SetHighlighted(true);
        }
    }

    // ---------- Pickup / Inspect ----------
    private void BeginInspect(IInteractable item)
    {
        Debug.Log("Begin inspect: " + item);
        item.Interact(new InteractContext()
        {
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
}