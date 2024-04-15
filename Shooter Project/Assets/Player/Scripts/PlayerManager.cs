using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour 
{
    public event Action<Vector2> MovementInput;
    public event Action<Vector2> RotationInput;
    public event Action JumpInput;
    public event Action DashInput;

    public void OnMovementInput(InputAction.CallbackContext context)
    {
        MovementInput?.Invoke(context.ReadValue<Vector2>());
    }   

    public void OnRotationInput(InputAction.CallbackContext context)
    {
        RotationInput?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.performed) JumpInput?.Invoke();
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.performed) DashInput?.Invoke();
    }
}
