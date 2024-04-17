using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour 
{
    public event Action<Vector2> MovementInputValue;
    public event Action<Vector2> RotationInputValue;
    public event Action JumpInputPerformed;
    public event Action DashInputPerformed;
    public event Action GrenadeInputPerformed;
    public event Action ShootInputPerformed;
    public event Action ShootInputCanceled;
    public event Action ShootAlternativeInputPerformed;
    public event Action ReloadInputPerformed;
    public event Action SwapWeaponInputPerformed;

    public void OnMovementInput(InputAction.CallbackContext context)
    {
        MovementInputValue?.Invoke(context.ReadValue<Vector2>());
    }   

    public void OnRotationInput(InputAction.CallbackContext context)
    {
        RotationInputValue?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.performed) JumpInputPerformed?.Invoke();
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.performed) DashInputPerformed?.Invoke();
    }
    public void OnGrenadeInput(InputAction.CallbackContext context)
    {
        if (context.performed) GrenadeInputPerformed?.Invoke();
    }

    public void OnShootInput(InputAction.CallbackContext context)
    {
        if (context.performed) ShootInputPerformed?.Invoke();
        if (context.canceled) ShootInputCanceled?.Invoke();
    }

    public void OnShootAlternativeInput(InputAction.CallbackContext context)
    {
        if (context.performed) ShootAlternativeInputPerformed?.Invoke();
    }

    public void OnReloadInput(InputAction.CallbackContext context)
    {
        if (context.performed) ReloadInputPerformed?.Invoke();
    }

    public void OnSwapWeaponInput(InputAction.CallbackContext context)
    {
        if (context.performed) SwapWeaponInputPerformed?.Invoke();
    }
}
