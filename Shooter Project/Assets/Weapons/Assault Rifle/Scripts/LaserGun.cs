using System.Collections;
using UnityEngine;

public class LaserGun : MonoBehaviour, IWeapon
{
    [Header("REFERENCES")]
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private Transform _regularMuzzle;
    [SerializeField] private Transform _alternativeMuzzle;
    [SerializeField] private Animator animator;
    private AmmoManager _ammoManager;
    private Transform _firePoint;
    private LayerMask _layerMask;

    [Header("DAMAGE")]
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _fireRate = 0.1f;
    [SerializeField] private float _fireRange = 40f;
    private bool _canFire = true;

    [Header("ALTERNATIVE MODE")]
    [SerializeField] private int _bursts = 5;
    [SerializeField] private int _damageBonus = 10;
    [SerializeField] private float _rangeBonus = 20f;
    [SerializeField] private float _burstPause = 0.05f;
    [SerializeField] private float _burstCD = 0.5f;

    [Header("RELOADING")]
    [SerializeField] private float _reloadTime = 1f;
    private bool _isReloading;

    [Header("AUDIO")]
    [SerializeField] private AudioClip _fireClip;
    [SerializeField] private AudioClip _emptyClip;
    [SerializeField] private AudioClip _reloadClip;
    private AudioSource _audioSource;

    private void Awake()
    {
        _ammoManager = GetComponent<AmmoManager>();
        _audioSource = GetComponent<AudioSource>();
        _firePoint = GetComponentInParent<Transform>();
    }

    private void Start()
    {
        _layerMask = ~LayerMask.GetMask("Player");
    }

    public void Enter()
    {
        _ammoManager.OnAmmoUpdate();
    }

    public void Shoot()
    {
        if (CanFire())
        {
            StartCoroutine(AutomaticFire());
        }
    }

    public void CancelShoot()
    {
        _canFire = true;
        StopAllCoroutines();
    }

    public void ShootAlternative()
    {
        if (CanFire())
        {
            StartCoroutine(BurstFire());
        }
    }

    public void Reload()
    {
        if (_isReloading || !_ammoManager.CanReload()) return;

        _ammoManager.Reload();

        StopAllCoroutines();
        _isReloading = true;
        _canFire = true;

        _audioSource.Stop();
        SwitchAudioClip(_reloadClip);

        Invoke(nameof(ResetReload), _reloadTime);
    }

    private IEnumerator AutomaticFire()
    {
        _canFire = false;

        while (true)
        {
            Fire(_damage, _fireRange, _regularMuzzle);
            yield return new WaitForSeconds(_fireRate);
        }
    }

    private IEnumerator BurstFire()
    {
        _canFire = false;

        for (int i = 0; i < _bursts; i++)
        {
            Fire(_damage + _damageBonus, _fireRange + _rangeBonus, _alternativeMuzzle);
            yield return new WaitForSeconds(_burstPause);
        }

        Invoke(nameof(ResetFire), _burstCD);
    }

    private void Fire(int damage, float range, Transform origin)
    {
        if (_ammoManager.ClipCount <= 0)
        {
            if (!_audioSource.isPlaying) SwitchAudioClip(_emptyClip);
            return;
        }

        FireSetup(origin);

        if (Physics.Raycast(_firePoint.position, _firePoint.forward, out RaycastHit hitInfo, range, _layerMask))
        {
            if (hitInfo.transform.TryGetComponent<Health>(out var health)) health.Damage(damage);
        }
    }

    private void FireSetup(Transform origin)
    {
        MuzzleFlash(origin);
        SwitchAudioClip(_fireClip);
        animator.SetTrigger("Fire");
        _ammoManager.SpendAmmo(1);
    }

    private bool CanFire()
    {
        if (!_canFire || _isReloading)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void ResetFire()
    {
        _canFire = true;
    }

    private void ResetReload()
    {
        _isReloading = false;
    }

    private void SwitchAudioClip(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    private void MuzzleFlash(Transform origin)
    {
        Instantiate(_muzzleFlash, origin);
    }
}
