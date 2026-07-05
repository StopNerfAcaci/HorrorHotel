using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Input/InputReader")]
public class InputReader : ScriptableObject, GameInput.IPlayerActions
{
    public event UnityAction<Vector2> Move;
    public event UnityAction<Vector2, bool> Look;

    public event UnityAction<bool> Jump = delegate { };
    public event UnityAction<bool> Roll = delegate { };
    public event UnityAction<bool> Attack = delegate { };
    public bool Sprint;
    public event UnityAction Interact = delegate { };
    public event UnityAction<float> Zoom = delegate { };

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

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Jump.Invoke(true);
        }
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
    }

    public void OnNext(InputAction.CallbackContext context)
    {
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        Sprint = context.ReadValue<bool>();
    }

    private bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";

    public void OnRoll(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                Roll.Invoke(true);
                break;
            case InputActionPhase.Canceled:
                Roll.Invoke(false);
                break;
        }
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        Zoom.Invoke(context.ReadValue<float>());
    }
}
