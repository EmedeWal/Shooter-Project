using System.Collections;
using UnityEngine;

public class RifleAlternate : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Transform _firePoint;
    private GunStateManager _gunStateManager;
    private AmmoManager _ammoManager;
    private Camera _camera;
    private LayerMask _layerMask;

    [Header("GRAPHICS")]
    [SerializeField] private GameObject _beamGFX, _bulletHoleGraphics;

    [Header("AUDIO")]
    [SerializeField] private AudioClip _chargingClip;
    [SerializeField] private AudioClip _fireClip;
    private AudioSource _audioSource;


    //public CamShake camShake;
    //public float camShakeMagnitude, camShakeDuration;

    [Header("STATS")]
    [SerializeField] private int _damage;
    [SerializeField] private float _spread, _range, _fireDuration, _fireCD;
    [SerializeField] private int _ammoConsumption = 1;
    [SerializeField] private float _damageRate;
    [SerializeField] private float _ammoConsumptionRate;

    [Header("CHARGING")]
    [SerializeField] private float _chargeDuration = 1;
    [SerializeField] private int _maxChargeLevel = 5;
    [SerializeField] private int _maxChargeLevelDamageBonus = 50;
    [SerializeField] private int _maxChargeLevelAdditionalAmmoConsumption = 10;

    private float _chargeRate;
    private int _bonusDamagePerChargeLevel;
    private int _additionalAmmoConsumptionPerChargeLevel;

    private int _bonusDamage;
    private int _bonusAmmo;

    private GameObject _currentBeam;

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
        _additionalAmmoConsumptionPerChargeLevel = _maxChargeLevelAdditionalAmmoConsumption / _maxChargeLevel;
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
        if (_gunStateManager.State == GunStateManager.GunState.Charging)
        {
            StopAllCoroutines();
            StartCoroutine(ReleaseBeam(_damage + _bonusDamage, _ammoConsumption + _bonusAmmo));
        }
        else if (_gunStateManager.State == GunStateManager.GunState.Firing)
        {
            StopAllCoroutines();
            BeamEnd();
        }
    }

    private IEnumerator ChargeShot()
    {
        UpdateState(GunStateManager.GunState.Charging);

        PlayAudioClip(_chargingClip);

        _bonusDamage = 0;
        _bonusAmmo = 0;

        int chargeLevel = 0;

        while (true)
        {
            yield return new WaitForSeconds(_chargeRate);

            _bonusDamage += _bonusDamagePerChargeLevel;
            _bonusAmmo += _additionalAmmoConsumptionPerChargeLevel;

            chargeLevel++;

            if (chargeLevel == _maxChargeLevel || _ammoManager.ClipEmpty(_ammoConsumption + _bonusAmmo)) break;
        }

        StopShooting();
    }

    private IEnumerator ReleaseBeam(int damage, int ammoConsumption)
    {
        _gunStateManager.UpdateState(GunStateManager.GunState.Firing);

        _currentBeam = Instantiate(_beamGFX, _firePoint);
        PlayAudioClip(_fireClip);

        InvokeRepeating(nameof(SpendAmmo), 0, _ammoConsumptionRate);

        Vector3 position = _firePoint.position;
        Vector3 direction = _firePoint.forward;

        while (!_ammoManager.ClipEmpty(ammoConsumption))
        {
            if (Physics.Raycast(position, direction, out RaycastHit hitInfo, _range, _layerMask))
            {
                if (hitInfo.transform.TryGetComponent<Health>(out var health)) health.Damage(damage);

                Instantiate(_bulletHoleGraphics, hitInfo.point, Quaternion.Euler(0, 180, 0));
            }

            yield return new WaitForSeconds(1 / _damageRate);

            //ShakeCamera
            //camShake.Shake(camShakeDuration, camShakeMagnitude);
        }

        BeamEnd();
    }

    private void BeamEnd()
    {
        Destroy(_currentBeam);
        CancelInvoke(nameof(SpendAmmo));
        Invoke(nameof(ResetToRecharging), _fireDuration);
    }

    private void SpendAmmo()
    {
        _ammoManager.SpendAmmo(_ammoConsumption + _bonusAmmo);
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
        if (_currentBeam != null) Destroy(_currentBeam);

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
