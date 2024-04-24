using System;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    [Header("AMMO")]
    [SerializeField] private int _clipCapacity;
    private int _clipCount;
    private int _maxAmmo;
    private int _currentAmmo;

    public int ClipCount => _clipCount;

    [Header("RELOADING")]
    [SerializeField] private float _reloadTime = 1f;

    [Header("AUDIO")]
    [SerializeField] private AudioClip _emptyClip;
    [SerializeField] private AudioClip _reloadClip;
    private AudioSource _audioSource;

    public delegate void Delegate_UpdateAmmo(int clipCount, int currentAmmo);
    public static event Delegate_UpdateAmmo UpdateAmmo;

    private GunStateManager _gunStateManager;

    public event Action ReloadStart;

    private void Awake()
    {
        _gunStateManager = GetComponent<GunStateManager>();
        _audioSource = GetComponent<AudioSource>();
        
        SetInitialVariables();
    }

    private void OnEnable()
    {
        PlayerReload.ReloadPerformed += AmmoManager_ReloadPerformed;
    }

    private void OnDisable()
    {
        PlayerReload.ReloadPerformed -= AmmoManager_ReloadPerformed;
    }

    private void AmmoManager_ReloadPerformed()
    {
        Reload();
    }

    public void Reload()
    {
        if (!CanReload()) return;

        OnReloadStart();

        SwitchAudioClip(_reloadClip);

        Invoke(nameof(FinishReload), _reloadTime);
    }

    private void FinishReload()
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

        OnReloadComplete();
    }

    private void OnReloadStart()
    {
        _gunStateManager.UpdateState(GunStateManager.GunState.Reloading);
        ReloadStart?.Invoke();
    }

    private void OnReloadComplete()
    {
        _gunStateManager.OverrideState(GunStateManager.GunState.Idle);
    }

    public bool CanReload()
    {
        if (_currentAmmo <= 0 || _clipCount == _clipCapacity || _gunStateManager.State == GunStateManager.GunState.Firing) return false;
        return true;
    }

    public bool ClipEmpty(int ammoConsumption)
    {
        if (_clipCount < ammoConsumption)
        {
            if (!_audioSource.isPlaying) SwitchAudioClip(_emptyClip);
            return true;
        }
        return false;
    }

    public void SpendAmmo(int amount)
    {
        _clipCount -= amount;
        if (_clipCount < 0) _clipCount = 0;
        OnAmmoUpdate();
    }

    public void OnAmmoUpdate()
    {
        UpdateAmmo?.Invoke(_clipCount, _currentAmmo);
    }

    private void SwitchAudioClip(AudioClip newClip)
    {
        _audioSource.clip = newClip;
        _audioSource.Play();
    }

    private void SetInitialVariables()
    {
        _clipCount = _clipCapacity;
        _maxAmmo = _clipCapacity * 4;
        _currentAmmo = _maxAmmo;
    }
}
