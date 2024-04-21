using System;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public enum GunState
    {
        Idle,
        Charging,
        Firing,
        Recharging,
        Reloading
    }

    public GunState State = GunState.Idle;

    public void UpdateState(GunState state)
    {
        // New idle state should not override Firing or Reloading
        if ((State == GunState.Firing || State == GunState.Reloading) && state == GunState.Idle) return;

        State = state;

        Debug.Log($"State was updated to: {State}");

        switch (State)
        {
            case GunState.Idle:
                break;

            case GunState.Charging:
                break;

            case GunState.Firing:
                break;

            case GunState.Recharging:
                break;

            case GunState.Reloading:
                break;
        }
    }

    private AmmoManager _ammoManager;

    public event Action ShootRegularPerformed;
    public event Action ShootRegularCanceled;
    public event Action ShootAlternatePerformed;
    public event Action ReloadPerformed;

    private void Awake()
    {
        _ammoManager = GetComponent<AmmoManager>();
    }

    public void Activate()
    {
        _ammoManager.OnAmmoUpdate();
    }

    public void Deactivate()
    {

    }

    public void OnShootRegularPerformed()
    {
        if (State == GunState.Idle)
        {
            ShootRegularPerformed?.Invoke();
        }
    }

    public void OnShootRegularCanceled()
    {
        if (State == GunState.Charging || State == GunState.Firing)
        {
            ShootRegularCanceled?.Invoke();
        }
    }

    public void OnShootAlternatePerformed()
    {
        if (State == GunState.Idle || State == GunState.Recharging)
        {
            ShootAlternatePerformed?.Invoke();
        }
    }

    public void OnReloadPerformed()
    {
        if (State != GunState.Firing && State != GunState.Reloading)
        {
            ReloadPerformed?.Invoke();
        }
    }
}
