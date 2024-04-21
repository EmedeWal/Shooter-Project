using UnityEngine;

public class PlayerShootRegular : MonoBehaviour
{
    private PlayerManager _playerManager;
    private PlayerData _playerData;

    private void Awake()
    {
        _playerManager = GetComponent<PlayerManager>();
        _playerData = GetComponent<PlayerData>();
    }

    private void OnEnable()
    {
        _playerManager.ShootRegularInputPerformed += PlayerShootRegular_ShootRegularInputPerformed;
        _playerManager.ShootRegularInputCanceled += PlayerShootRegular_ShootRegularInputCanceled;
    }

    private void OnDisable()
    {
        _playerManager.ShootRegularInputPerformed -= PlayerShootRegular_ShootRegularInputPerformed;
        _playerManager.ShootRegularInputCanceled -= PlayerShootRegular_ShootRegularInputCanceled;
    }

    private void PlayerShootRegular_ShootRegularInputPerformed()
    {
        GameObject currentGun = _playerData.GetCurrentGun();
        currentGun.GetComponent<GunManager>().OnShootRegularPerformed();
    }
    private void PlayerShootRegular_ShootRegularInputCanceled()
    {
        GameObject currentGun = _playerData.GetCurrentGun();
        currentGun.GetComponent<GunManager>().OnShootRegularCanceled();
    }
}
