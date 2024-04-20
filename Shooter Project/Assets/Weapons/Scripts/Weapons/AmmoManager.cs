using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    [Header("AMMO")]
    [SerializeField] private int _clipCapacity;
    private int _clipCount;
    private int _maxAmmo;
    private int _currentAmmo;

    public int ClipCount => _clipCount;

    public delegate void UpdateAmmo(int clipCount, int currentAmmo);
    public static event UpdateAmmo updateAmmo;

    private void OnEnable()
    {
        _clipCount = _clipCapacity;
        _maxAmmo = _clipCapacity * 4;
        _currentAmmo = _maxAmmo;
    }

    public void SpendAmmo(int amount)
    {
        _clipCount -= amount;
        OnAmmoUpdate();
    }

    public bool CanReload()
    {
        if (_currentAmmo <= 0 || _clipCount == _clipCapacity) return false;
        return true;
    }

    public void Reload()
    {
        int missingBullets = _clipCapacity - _clipCount;

        if (_currentAmmo >= missingBullets)
        {
            _clipCount = _clipCapacity;
            _currentAmmo -= missingBullets;
        }
        else
        {
            _clipCount += _currentAmmo;
            _currentAmmo = 0;   
        }

        OnAmmoUpdate();
    }

    public void OnAmmoUpdate()
    {
        updateAmmo?.Invoke(_clipCount, _currentAmmo);
    }
}
