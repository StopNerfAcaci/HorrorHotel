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

    private WorldItem _hoveredItem;
    

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

        WorldItem newHover = null;

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, itemLayerMask))
        {
            newHover = hit.collider.GetComponentInParent<WorldItem>();
        }

        if (newHover != _hoveredItem)
        {
            if (_hoveredItem != null) _hoveredItem.SetHighlighted(false);
            _hoveredItem = newHover;
            if (_hoveredItem != null) _hoveredItem.SetHighlighted(true);
        }
    }

    // ---------- Pickup / Inspect ----------
    private void BeginInspect(WorldItem item)
    {
        item.SetHighlighted(false);
        item.CacheOriginalTransform();
        item.SetPhysicsEnabled(false);

        item.transform.SetParent(playerCamera.transform, worldPositionStays: false);
        item.transform.localPosition = holdLocalOffset;
        item.transform.localRotation = Quaternion.identity;
        
        _hoveredItem = null;
    }

    public bool TryPressed(out WorldItem item)
    {
        item = _hoveredItem;
        if (_hoveredItem == null) return false;
        BeginInspect(_hoveredItem);
        return true;
    }
}