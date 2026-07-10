using System;
using Gameplay.Inventory;
using HSM;
using UnityEngine;
using Utils.Extensions;

public class PlayerInteraction : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private Camera playerCamera;

    [Header("Raycast")] [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask itemLayerMask = ~0;

    [Header("Hold / Inspect")] [SerializeField]
    private Vector3 holdLocalOffset = new Vector3(0f, 0f, 1.2f);

    [SerializeField] private float rotateSpeed = 120f;

    [Header("Input")] [SerializeField] private KeyCode pickupKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode confirmKey = KeyCode.E;
    [SerializeField] private KeyCode cancelKey = KeyCode.Escape;

    private PlayerStateDriver player;
    private WorldItem _hoveredItem;
    private WorldItem _heldItem;
    private bool _isInspecting;
    private float _yaw;
    private float _pitch;
    private Vector2 lastPointed;

    public PlayerInventory Inventory { get; private set; }

    //Put this in awake
    public void Initialize(PlayerStateDriver player)
    {
        this.player = player;
        Inventory = new PlayerInventory.Builder().Build();
    }

    private void OnEnable()
    {
        player.Reader.Interact += OnInteract;
        player.Reader.Cancel += OnCancelPressed;
        player.Reader.Hold += OnHold;
        player.Reader.Pointed += GetLastPoint;
    }


    private void OnDisable()
    {
        player.Reader.Interact -= OnInteract;
        player.Reader.Cancel -= OnCancelPressed;
        player.Reader.Hold -= OnHold;
        player.Reader.Pointed -= GetLastPoint;
    }
    private void OnHold()
    {
        if (!_isInspecting) return;
        _yaw += lastPointed.x * rotateSpeed * Time.deltaTime;
        _pitch += -lastPointed.y * rotateSpeed * Time.deltaTime;
 
        _heldItem.transform.localRotation = Quaternion.Euler(_pitch, _yaw, 0f);
    }

    private void GetLastPoint(Vector2 pos)
    {
        lastPointed = pos;
    }

    private void OnInteract()
    {
        ConfirmPickup();
    }

    private void OnCancelPressed()
    {
        if (!_isInspecting)
        {
            if (_hoveredItem == null) return;
            BeginInspect(_hoveredItem);
        }

    }

    private void Reset()
    {
        playerCamera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        if (!_isInspecting)
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
        player.SetItem(item);
        item.SetHighlighted(false);
        item.CacheOriginalTransform();
        item.SetPhysicsEnabled(false);

        item.transform.SetParent(playerCamera.transform, worldPositionStays: false);
        item.transform.localPosition = holdLocalOffset;
        item.transform.localRotation = Quaternion.identity;

        _heldItem = item;
        _hoveredItem = null;
        _isInspecting = true;
        _yaw = 0f;
        _pitch = 0f;
    }


    private void ConfirmPickup()
    {
        Inventory.AddItem(_heldItem.itemData, _heldItem.quantity);

        _heldItem.SetInactive();
        Destroy(_heldItem.gameObject);
        _heldItem = null;
        _isInspecting = false;
    }
}