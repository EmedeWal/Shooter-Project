using System;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _weapons;
    private IWeapon _currentWeapon;
    private int _weaponIndex = -1;

    private void Start()
    {
        SwapWeapon();
    }

    private void OnEnable()
    {
        PlayerManager pm = GetComponent<PlayerManager>();
        pm.ShootInputPerformed += WeaponManager_ShootInputPerformed;
        pm.ShootInputCanceled += WeaponManager_ShootInputCanceled;
        pm.ShootAlternativeInputPerformed += WeaponManager_ShootAlternativeInputPerformed;
        pm.ReloadInputPerformed += WeaponManager_ReloadInputPerformed;
        pm.SwapWeaponInputPerformed += WeaponManager_SwapWeaponInputPerformed;
    }

    private void OnDisable()
    {
        PlayerManager pm = GetComponent<PlayerManager>();
        pm.ShootInputPerformed -= WeaponManager_ShootInputPerformed;
        pm.ShootInputCanceled -= WeaponManager_ShootInputCanceled;
        pm.ShootAlternativeInputPerformed -= WeaponManager_ShootAlternativeInputPerformed;
        pm.ReloadInputPerformed -= WeaponManager_ReloadInputPerformed;
        pm.SwapWeaponInputPerformed -= WeaponManager_SwapWeaponInputPerformed;
    }

    private void WeaponManager_ShootInputPerformed()
    {
        _currentWeapon.Shoot();
    }

    private void WeaponManager_ShootInputCanceled()
    {
        _currentWeapon.ShootCancel();
    }

    private void WeaponManager_ShootAlternativeInputPerformed()
    {
        _currentWeapon.ShootAlternative();
    }

    private void WeaponManager_ReloadInputPerformed()
    {
        _currentWeapon.Reload();
    }

    private void WeaponManager_SwapWeaponInputPerformed()
    {
        SwapWeapon();
    }

    private void SwapWeapon()
    {
        _weaponIndex++; 
        if (_weaponIndex >= _weapons.Length) _weaponIndex = 0;

        foreach (var weapon in _weapons) weapon.SetActive(false);
        _weapons[_weaponIndex].SetActive(true);
        _currentWeapon = _weapons[_weaponIndex].GetComponent<IWeapon>();
        _currentWeapon.Enter();
    }
}
