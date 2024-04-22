using UnityEngine;

public class GunStateManager : MonoBehaviour
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
        if (State == GunState.Reloading) return;

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
    }
}
