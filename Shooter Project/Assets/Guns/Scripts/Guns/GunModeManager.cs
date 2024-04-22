using UnityEngine;
using UnityEngine.Events;

public class GunModeManager : MonoBehaviour
{
    [SerializeField] private UnityEvent SwapToRegularMode;
    [SerializeField] private UnityEvent SwapToAlternateMode;

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
        OnSwapToAlternateMode();
    }

    private void GunManager_SwapModeCanceled()
    {
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
