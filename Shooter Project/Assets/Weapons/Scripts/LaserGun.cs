using System.Collections;
using UnityEngine;

public class LaserGun : MonoBehaviour, IWeapon
{
    private AmmoManager _ammoManager;
    private Transform _firePoint;

    [Header("DAMAGE")]
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _fireRate = 0.1f;
    [SerializeField] private float _fireRange = 40f;
    private Coroutine _coroutine;
    private bool canFire = true;

    [Header("ALTERNATIVE MODE")]
    [SerializeField] private int _bursts = 5;
    [SerializeField] private int _damageBonus = 10;
    [SerializeField] private float _rangeBonus = 20f;
    [SerializeField] private float _burstPause = 0.05f;
    [SerializeField] private float _burstCD = 0.5f;

    [Header("RELOADING")]
    [SerializeField] private float _reloadTime = 1f;
    private bool isReloading;

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

    public void Enter()
    {
        _ammoManager.OnAmmoUpdate();
    }

    public void Shoot()
    {
        if (!isReloading && canFire) _coroutine = StartCoroutine(AutomaticFire());
    }

    private IEnumerator AutomaticFire()
    {
        while (true)
        {
            Fire(_damage, _fireRange);
            yield return new WaitForSeconds(_fireRate);
        }
    }

    public void ShootCancel()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    public void ShootAlternative()
    {
        if (!isReloading && canFire)
        {
            ShootCancel();
            StartCoroutine(BurstFire());
        }
    }

    private IEnumerator BurstFire()
    {
        canFire = false;

        for (int i = 0; i < _bursts; i++)
        {
            Fire(_damage + _damageBonus, _fireRange + _rangeBonus);
            yield return new WaitForSeconds(_burstPause);
        }

        Invoke(nameof(ResetFire), _burstCD);
    }


    private void ResetFire()
    {
        canFire = true;
    }

    private void Fire(int damage, float range)
    {
        if (_ammoManager.ClipCount <= 0)
        {
            if (!_audioSource.isPlaying) SwitchAudioClip(_emptyClip);
            return;
        }

        SwitchAudioClip(_fireClip);

        _ammoManager.SpendAmmo(1);

        RaycastHit hitInfo;
        if (Physics.Raycast(_firePoint.position, _firePoint.forward, out hitInfo, range))
        { 
            Health health = hitInfo.transform.GetComponent<Health>();
            if (health != null) health.Damage(damage);
            Debug.Log($"Hit {hitInfo.transform.name} at a distance of {hitInfo.distance}");
        }
    }

    public void Reload()
    {
        if (isReloading || !_ammoManager.CanReload()) return;

        _ammoManager.Reload();

        StopAllCoroutines();
        isReloading = true;
        canFire = true;

        _audioSource.Stop();
        SwitchAudioClip(_reloadClip);

        Invoke(nameof(ResetReload), _reloadTime);  
    }

    private void ResetReload()
    {
        isReloading = false;
    }

    private void SwitchAudioClip(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
