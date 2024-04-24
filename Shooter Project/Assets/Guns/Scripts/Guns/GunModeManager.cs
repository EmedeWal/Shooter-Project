using UnityEngine;
using UnityEngine.Events;

public class GunModeManager : MonoBehaviour
{
    private GunStateManager _gunStateManager;

    [SerializeField] private UnityEvent SwapToRegularMode;
    [SerializeField] private UnityEvent SwapToAlternateMode;

    private void Awake()
    {
        _gunStateManager = GetComponent<GunStateManager>();
    }

    private void OnEnable()
    {
        PlayerSwapMode.SwapModePerformed += GunManager_SwapModePerformed;
        PlayerSwapMode.SwapModeCanceled += GunManager_SwapModeCanceled;
    }

    private void OnDisable()
    {
        PlayerSwapMode.SwapModePerformed -= GunManager_SwapModePerformed;
        PlayerSwapMode.SwapModeCanceled -= GunManager_SwapModeCanceled;
    }


    private void GunManager_SwapModePerformed()
    {
        //if (_gunStateManager.IsIdle())
        //{
        //    OnSwapToAlternateMode();
        //}

        OnSwapToAlternateMode();
    }

    private void GunManager_SwapModeCanceled()
    {
        //if (_gunStateManager.IsIdle())
        //{
        //    OnSwapToRegularMode();
        //}

        OnSwapToRegularMode();
    }

    private void OnSwapToRegularMode()
    {
        SwapToRegularMode.Invoke();
    }

    private void OnSwapToAlternateMode()
    {
        SwapToAlternateMode.Invoke();
    }
}
