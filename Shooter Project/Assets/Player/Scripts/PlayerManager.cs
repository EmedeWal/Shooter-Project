using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour 
{
    public enum PlayerState
    {
        Idle,
        Dashing
    }

    public PlayerState State = PlayerState.Idle;

    public void UpdateState(PlayerState newState)
    {
        State = newState;

        switch (State)
        {
            case PlayerState.Idle:
                break;

            case PlayerState.Dashing:
                break;
        }
    }

    public event Action<Vector2> MovementInputValue;
    public event Action<Vector2> RotationInputValue;
    public event Action JumpInputPerformed;
    public event Action DashInputPerformed;
    public event Action GrenadeInputPerformed;
    public event Action ShootRegularInputPerformed;
    public event Action ShootRegularInputCanceled;
    public event Action ShootAlternateInputPerformed;
    public event Action ReloadInputPerformed;
    public event Action SwapGunInputPerformed;

    public void OnMovementInput(InputAction.CallbackContext context)
    {
        if (State == PlayerState.Dashing) return;

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

    public void OnShootRegularInput(InputAction.CallbackContext context)
    {
        if (context.performed) ShootRegularInputPerformed?.Invoke();
        if (context.canceled) ShootRegularInputCanceled?.Invoke();
    }

    public void OnShootAlternativeInput(InputAction.CallbackContext context)
    {
        if (context.performed) ShootAlternateInputPerformed?.Invoke();
    }

    public void OnReloadInput(InputAction.CallbackContext context)
    {
        if (context.performed) ReloadInputPerformed?.Invoke();
    }

    public void OnSwapGunInput(InputAction.CallbackContext context)
    {
        if (context.performed) SwapGunInputPerformed?.Invoke();
    }
}
