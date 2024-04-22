using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    private PlayerManager _playerManager;

    public delegate void Delegate_ShootPerformed();
    public static event Delegate_ShootPerformed ShootPerformed;

    public delegate void Delegate_ShootCanceled();
    public static event Delegate_ShootCanceled ShootCanceled;

    private void Awake()
    {
        _playerManager = GetComponent<PlayerManager>();
    }

    private void OnEnable()
    {
        _playerManager.ShootInputPerformed += PlayerShoot_ShootInputPerformed;
        _playerManager.ShootInputCanceled += PlayerShoot_ShootInputCanceled;
    }

    private void OnDisable()
    {
        _playerManager.ShootInputPerformed -= PlayerShoot_ShootInputPerformed;
        _playerManager.ShootInputCanceled -= PlayerShoot_ShootInputCanceled;
    }

    private void PlayerShoot_ShootInputPerformed()
    {
        OnShootPerformed();
    }
    private void PlayerShoot_ShootInputCanceled()
    {
        OnShootCanceled();
    }

    private void OnShootPerformed()
    {
        ShootPerformed?.Invoke();
    }

    private void OnShootCanceled()
    {
        ShootCanceled?.Invoke();
    }
}
