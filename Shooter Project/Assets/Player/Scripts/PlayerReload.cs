using UnityEngine;

public class PlayerReload : MonoBehaviour
{
    private PlayerData _playerData;

    private void Awake()
    {
        _playerData = GetComponent<PlayerData>();
    }

    private void OnEnable()
    {
        GetComponent<PlayerManager>().ReloadInputPerformed += PlayerReload_ReloadInputPerformed;
    }

    private void OnDisable()
    {
        GetComponent<PlayerManager>().ReloadInputPerformed -= PlayerReload_ReloadInputPerformed;
    }

    private void PlayerReload_ReloadInputPerformed()
    {
        GameObject currentGun = _playerData.GetCurrentGun();
        currentGun.GetComponent<GunManager>().OnReloadPerformed();
    }
}
