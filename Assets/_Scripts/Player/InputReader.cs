using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Input/InputReader")]
public class InputReader : ScriptableObject, GameInput.IPlayerActions
{
    public event UnityAction<Vector2> Move;
    public event UnityAction<Vector2, bool> Look;

    public event UnityAction<bool> Attack = delegate { };
    public bool Sprint;
    public event UnityAction Interact = delegate { };
    // NOTE: repurposing the existing (previously unused) OnClick callback as
    // a "cancel / put back" action for the inspect flow, since it wasn't
    // wired to anything yet. If you already use Click for UI interaction,
    // add a dedicated "Cancel" action to your Input Actions asset instead
    // (bind it to Escape / right-click) and route it here the same way.
    public event UnityAction Cancel = delegate { };
    public event UnityAction Hold = delegate { };
    public event UnityAction<Vector2> Pointed = delegate { };
    
    GameInput inputAction;
    private Vector2 direction;
    public Vector2 Direction => direction;
    public void EnablePlayerActions()
    {
        if (inputAction == null)
        {
            inputAction = new GameInput();
            inputAction.Player.SetCallbacks(this);
        }

        inputAction.Player.Enable();
    }

    public void DisablePlayerActions()
    {
        direction = Vector2.zero;
        Sprint = false;

        if (inputAction != null)
            inputAction.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
        Move?.Invoke(direction);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Look?.Invoke(context.ReadValue<Vector2>(), IsDeviceMouse(context));
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Cancel.Invoke();
        }
        if (context.phase == InputActionPhase.Performed)
        {
            Hold?.Invoke();
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Attack.Invoke(true);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Interact.Invoke();
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
    }
    
    public void OnSprint(InputAction.CallbackContext context)
    {
        Sprint = context.performed;
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        Pointed?.Invoke(context.ReadValue<Vector2>());
    }

    private bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";

}
