using System.Collections;
using UnityEngine;

public class RifleRegular : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Transform _firePoint;
    private GunStateManager _gunStateManager;
    private AmmoManager _ammoManager;
    private Camera _camera;
    private LayerMask _layerMask;

    [Header("GRAPHICS")]
    [SerializeField] private GameObject projectileGFX, _bulletHoleGraphics;

    [Header("AUDIO")]
    [SerializeField] private AudioClip _chargingClip;
    [SerializeField] private AudioClip _fireClip;
    private AudioSource _audioSource;


    //public CamShake camShake;
    //public float camShakeMagnitude, camShakeDuration;

    [Header("STATS")]
    [SerializeField] private int _damage, _range;
    [SerializeField] private float _spread, _fireDuration, _fireCD;
    [SerializeField] private int _ammoConsumption = 10;

    [Header("CHARGING")]
    [SerializeField] private float _chargeDuration = 1;
    [SerializeField] private int _maxChargeLevel = 5;
    [SerializeField] private int _maxChargeLevelDamageBonus = 50;
    [SerializeField] private int _maxChargeLevelRangeBonus = 50;
    [SerializeField] private int _maxChargeLevelAdditionalAmmoConsumption = 10;

    private float _chargeRate;
    private int _bonusDamagePerChargeLevel;
    private int _bonusRangePerChargeLevel;
    private int _additionalAmmoConsumptionPerChargeLevel;
    private float _originalSpread;

    private int _bonusDamage;
    private int _bonusRange;
    private int _bonusAmmo;
    private float _spreadModifier;


    private void Awake()
    {
        _gunStateManager = GetComponent<GunStateManager>();
        _ammoManager = GetComponent<AmmoManager>();
        _audioSource = GetComponent<AudioSource>();
        _camera = GetComponentInParent<Camera>();
    }

    private void Start()
    {
        _layerMask = ~LayerMask.GetMask("Player");

        _chargeRate = _chargeDuration / _maxChargeLevel;
        _bonusDamagePerChargeLevel = _maxChargeLevelDamageBonus / _maxChargeLevel;
        _bonusRangePerChargeLevel = _maxChargeLevelRangeBonus / _maxChargeLevel;
        _additionalAmmoConsumptionPerChargeLevel = _maxChargeLevelAdditionalAmmoConsumption / _maxChargeLevel;

        _originalSpread = _spread;
        _spreadModifier = _spread / _maxChargeLevel;
    }

    private void OnEnable()
    {
        PlayerShoot.ShootPerformed += AssaultRifleRegular_ShootPerformed;
        PlayerShoot.ShootCanceled += AssaultRifleRegular_ShootCanceled;
    }

    private void OnDisable()
    {
        PlayerShoot.ShootPerformed -= AssaultRifleRegular_ShootPerformed;
        PlayerShoot.ShootCanceled -= AssaultRifleRegular_ShootCanceled;

        ResetState();
    }

    private void AssaultRifleRegular_ShootPerformed()
    {
        StartShooting();
    }

    private void AssaultRifleRegular_ShootCanceled()
    {
        StopShooting();
    }

    private void StartShooting()
    {
        if (CanShoot()) StartCoroutine(ChargeShot());
    }

    private void StopShooting()
    {
        if (_gunStateManager.State != GunStateManager.GunState.Charging) return;

        StopAllCoroutines();
        Invoke(nameof(ResetToRecharging), _fireDuration);
        ReleaseShot(_damage + _bonusDamage, _range + _bonusRange, _ammoConsumption + _bonusAmmo);
    }

    private IEnumerator ChargeShot()
    {
        UpdateState(GunStateManager.GunState.Charging);

        PlayAudioClip(_chargingClip);

        _bonusDamage = 0;
        _bonusRange = 0;
        _bonusAmmo = 0;
        _spread = _originalSpread;

        int chargeLevel = 0;

        while (true)
        {
            yield return new WaitForSeconds(_chargeRate);

            _bonusDamage += _bonusDamagePerChargeLevel;
            _bonusRange += _bonusRangePerChargeLevel;
            _bonusAmmo += _additionalAmmoConsumptionPerChargeLevel;
            _spread -= _spreadModifier;

            chargeLevel++;

            if (chargeLevel == _maxChargeLevel || _ammoManager.ClipEmpty(_ammoConsumption + _bonusAmmo)) break;
        }

        StopShooting();
    }

    private void ReleaseShot(int damage, int range, int ammoConsumption)
    {
        _gunStateManager.UpdateState(GunStateManager.GunState.Firing);

        Vector3 direction = CalculateSpread();
        Vector3 position = _camera.transform.position;

        if (Physics.Raycast(position, direction, out RaycastHit hitInfo, range, _layerMask))
        {
            if (hitInfo.transform.TryGetComponent<Health>(out var health)) health.Damage(damage);

            Instantiate(_bulletHoleGraphics, hitInfo.point, Quaternion.Euler(0, 180, 0));
        }

        Instantiate(projectileGFX, _firePoint.position, _firePoint.rotation);
        PlayAudioClip(_fireClip);
        _ammoManager.SpendAmmo(ammoConsumption);

        //ShakeCamera
        //camShake.Shake(camShakeDuration, camShakeMagnitude);
    }

    private bool CanShoot()
    {
        if (_ammoManager.ClipEmpty(_ammoConsumption) || _gunStateManager.State != GunStateManager.GunState.Idle) return false;
        return true;
    }

    private Vector3 CalculateSpread()
    {
        float x = Random.Range(-_spread, _spread);
        float y = Random.Range(-_spread, _spread);
        return _camera.transform.forward + new Vector3(x, y, 0);
    }

    private void SetToCharging()
    {
        UpdateState(GunStateManager.GunState.Charging);
    }

    private void ResetToRecharging()
    {
        UpdateState(GunStateManager.GunState.Recharging);
        Invoke(nameof(ResetToIdle), _fireCD);
    }

    private void ResetToIdle()
    {
        UpdateState(GunStateManager.GunState.Idle);
    }

    private void ResetState()
    {
        UpdateState(GunStateManager.GunState.Idle);
        StopAllCoroutines();
        CancelInvoke();
    }
    private void UpdateState(GunStateManager.GunState state)
    {
        _gunStateManager.UpdateState(state);
    }

    private void PlayAudioClip(AudioClip audioClip)
    {
        _audioSource.Stop();
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }
}
