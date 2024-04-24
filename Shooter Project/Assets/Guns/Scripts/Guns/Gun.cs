using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    private GunStateManager _gunStateManager;

    protected void SetVariables(GunStateManager gunStateManager)
    {
        _gunStateManager = gunStateManager;
    }

    protected bool CanShoot()
    {
        if (_gunStateManager.State != GunStateManager.GunState.Idle) return false;
        return true;
    }

}
