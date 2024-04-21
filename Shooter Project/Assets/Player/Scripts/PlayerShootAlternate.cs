using UnityEngine;

public class PlayerShootAlternate : MonoBehaviour
{
    private PlayerData _playerData;

    private void Awake()
    {
        _playerData = GetComponent<PlayerData>();
    }

    private void OnEnable()
    {
        GetComponent<PlayerManager>().ShootAlternateInputPerformed += PlayerShootAlternate_ShootAlternatePerformed;
    }

    private void OnDisable()
    {
        GetComponent<PlayerManager>().ShootAlternateInputPerformed -= PlayerShootAlternate_ShootAlternatePerformed;
    }

    private void PlayerShootAlternate_ShootAlternatePerformed()
    {
        GameObject currentGun = _playerData.GetCurrentGun();
        currentGun.GetComponent<GunManager>().OnShootAlternatePerformed();
    }
}
