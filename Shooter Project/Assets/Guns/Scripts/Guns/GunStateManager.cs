using UnityEngine;

public class GunStateManager : MonoBehaviour
{
    public delegate void Delegate_GunStateUpdate(GunState newState);
    public static event Delegate_GunStateUpdate GunStateUpdate;

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
        if (State == GunState.Reloading) return;

        ChangeState(state);
    }

    public void OverrideState(GunState state)
    {
        ChangeState(state);
    }

    private void ChangeState(GunState state)
    {
        State = state;

        //Debug.Log($"State was updated to: {State}");

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

        OnGunStateUpdate();
    }

    private void OnGunStateUpdate()
    {
        GunStateUpdate?.Invoke(State);
    }

    public bool IsIdle()
    {
        if (State == GunState.Idle) return true;
        return false;
    }
}
