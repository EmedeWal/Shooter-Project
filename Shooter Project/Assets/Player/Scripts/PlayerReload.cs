using UnityEngine;

public class PlayerReload : MonoBehaviour
{
    private PlayerManager _playerManager;

    public delegate void Delegate_ReloadPerformed();
    public static event Delegate_ReloadPerformed ReloadPerformed;

    private void Awake()
    {
        _playerManager = GetComponent<PlayerManager>();
    }

    private void OnEnable()
    {
        _playerManager.ReloadInputPerformed += PlayerReload_ReloadInputPerformed;
    }

    private void OnDisable()
    {
        _playerManager.ReloadInputPerformed -= PlayerReload_ReloadInputPerformed;
    }

    private void PlayerReload_ReloadInputPerformed()
    {
        OnReloadPerformed();
    }

    private void OnReloadPerformed()
    {
        ReloadPerformed?.Invoke();
    }
}
