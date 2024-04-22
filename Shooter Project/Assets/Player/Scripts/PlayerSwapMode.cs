using UnityEngine;

public class PlayerSwapMode : MonoBehaviour
{
    private PlayerManager _playerManager;

    public delegate void Delegate_SwapModePerformed();
    public static event Delegate_SwapModePerformed SwapModePerformed;

    public delegate void Delegate_SwapModeCanceled();
    public static event Delegate_SwapModeCanceled SwapModeCanceled;

    private void Awake()
    {
        _playerManager = GetComponent<PlayerManager>();
    }

    private void OnEnable()
    {
        _playerManager.SwapModeInputPerformed += PlayerSwapMode_SwapModeInputPerformed;
        _playerManager.SwapModeInputCanceled += PlayerSwapMode_SwapModeInputCanceled;
    }

    private void OnDisable()
    {
        _playerManager.SwapModeInputPerformed -= PlayerSwapMode_SwapModeInputPerformed;
        _playerManager.SwapModeInputCanceled -= PlayerSwapMode_SwapModeInputCanceled;
    }

    private void PlayerSwapMode_SwapModeInputPerformed()
    {
        OnSwapModePerformed();
    }
    private void PlayerSwapMode_SwapModeInputCanceled()
    {
        OnSwapModeCanceled();
    }

    private void OnSwapModePerformed()
    {
        SwapModePerformed?.Invoke();
    }

    private void OnSwapModeCanceled()
    {
        SwapModeCanceled?.Invoke();
    }
}
