using System.Collections;
using UnityEngine;

public class PlayerSwapGun : MonoBehaviour
{
    [SerializeField] private int _index = -1;
    private PlayerData _playerData;

    private void Awake()
    {
        _playerData = GetComponent<PlayerData>();
    }
    private void Start()
    {
        SwapGun();
    }

    private void OnEnable()
    {
        GetComponent<PlayerManager>().SwapGunInputPerformed += PlayerSwapGun_SwapGunInputPerformed;
    }

    private void OnDisable()
    {
        GetComponent<PlayerManager>().SwapGunInputPerformed -= PlayerSwapGun_SwapGunInputPerformed;
    }

    private void PlayerSwapGun_SwapGunInputPerformed()
    {
        SwapGun();
    }

    private void SwapGun()
    {
        GameObject[] guns = _playerData.GetGuns();

        _index++;

        if (_index >= guns.Length) _index = 0;

        foreach (var weapon in guns)
        {
            weapon.SetActive(false);
        }

        GameObject currentWeapon = guns[_index];
        currentWeapon.SetActive(true);
        _playerData.SetCurrentGun(currentWeapon);

        currentWeapon.GetComponent<GunManager>().Activate();
    }
}
