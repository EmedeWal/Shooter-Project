using System.Collections;
using UnityEngine;

public class EnergyRifle : MonoBehaviour, IWeapon
{
    public enum State
    {
        Idle,
        Charging,
        Firing,
        Recharging,
        Reloading
    }

    [SerializeField] private State _state = State.Idle;

    [Header("REFERENCES")]
    [SerializeField] private Animator animator;
    private AmmoManager _ammoManager;
    private Transform _firePoint;

    [Header("GENERAL")]
    [SerializeField] private float _fireRate = 1.5f;

    [Header("REGULAR")]
    [SerializeField] private GameObject _projectileGFX;
    [SerializeField] private Transform _regularMuzzle;
    private LayerMask _layerMask;

    [SerializeField] private int _damage = 50;
    [SerializeField] private int _chargeDamageBonus = 10;
    private int _bonusDamage;

    [SerializeField] private int _fireRange = 50;
    [SerializeField] private int _chargeRangeBonus = 10;
    private int _bonusRange;

    [SerializeField] private int _chargeCapacity = 10;
    [SerializeField] private float _chargeRate = 0.1f;

    [Header("ALTERNATE")]
    [SerializeField] private GameObject _aPrefab;
    [SerializeField] private Transform _aMuzzle;
    [SerializeField] private float _aAmmoConsumptionRate = 1f;
    [SerializeField] private float _aChargeTime = 2f;
    [SerializeField] private float _aDamageRate = 0.1f;
    [SerializeField] private int _aDamage = 10;
    private GameObject _aInstance;

    [Header("RELOADING")]
    [SerializeField] private float _reloadTime = 1f;

    [Header("AUDIO")]
    [SerializeField] private AudioClip _chargingClip;
    [SerializeField] private AudioClip _fireClip;
    [SerializeField] private AudioClip _alternateClip;
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

    private void UpdateState(State newState)
    {
        _state = newState;

        switch (newState)
        {
            case State.Idle:
                break;

            case State.Charging:
                break;

            case State.Firing:
                break;
        }
    }

    public void Enter()
    {
        _ammoManager.OnAmmoUpdate();
    }

    #region Regular 
    public void Shoot()
    {
        if (_state == State.Idle && !ClipEmpty())
        {
            StartCoroutine(ChargeShot());
        }
    }

    public void CancelShoot()
    {
        if (_state != State.Charging) return;

        ReleaseShot(_damage + _bonusDamage, _fireRange + _bonusRange);
    }

    private IEnumerator ChargeShot()
    {
        UpdateState(State.Charging);

        SwitchAudioClip(_chargingClip);

        _bonusDamage = 0;
        _bonusRange = 0;
        int chargeLevel = 0;

        while (true)
        {
            yield return new WaitForSeconds(_chargeRate);
            _bonusDamage += _chargeDamageBonus;
            _bonusRange += _chargeRangeBonus;
            chargeLevel++;

            if (chargeLevel == _chargeCapacity) break;
        }

        CancelShoot();
    }

    private void ReleaseShot(int damage, float range)
    {
        Fire(damage, range);
    }

    private void FireSetup()
    {
        _ammoManager.SpendAmmo(1);

        SwitchAudioClip(_fireClip);
        //_animator.SetTrigger("Fire");

        Invoke(nameof(ResetFire), _fireRate);

        Instantiate(_projectileGFX, _regularMuzzle.position, _regularMuzzle.rotation);
    }

    private void Fire(int damage, float range)
    {
        UpdateState(State.Firing);

        FireSetup();

        if (Physics.Raycast(_firePoint.position, _firePoint.forward, out RaycastHit hitInfo, range, _layerMask))
        {
            if (hitInfo.transform.TryGetComponent<Health>(out var health)) health.Damage(damage);
        }
    }

    private void ResetFire()
    {
        UpdateState(State.Idle);
    }
    #endregion

    #region Alternate
    public void ShootAlternative()
    {
        if (_state == State.Idle && !ClipEmpty())
        {
            StartCoroutine(ChargeAlternate());
        }
    }
    private IEnumerator ChargeAlternate()
    {
        UpdateState(State.Charging);

        SwitchAudioClip(_chargingClip);

        yield return new WaitForSeconds(_aChargeTime);

        StartCoroutine(CastAlternate());
    }

    private IEnumerator CastAlternate()
    {
        UpdateState(State.Firing);

        _aInstance = Instantiate(_aPrefab, _aMuzzle);

        SwitchAudioClip(_alternateClip);
        _audioSource.loop = true;

        while (!ClipEmpty())
        {
            yield return new WaitForSeconds(_aAmmoConsumptionRate);

            _ammoManager.SpendAmmo(1);
        }

        _audioSource.loop = false;
        _audioSource.Stop();

        EndAlternate();
    }

    private void EndAlternate()
    {
        UpdateState(State.Idle);

        Destroy(_aInstance);
    }
    #endregion

    #region Reloading
    public void Reload()
    {
        if (_state == State.Charging || _state == State.Firing || !_ammoManager.CanReload()) return;

        UpdateState(State.Reloading);
        StopAllCoroutines();

        _ammoManager.Reload();

        SwitchAudioClip(_reloadClip);

        Invoke(nameof(ResetReload), _reloadTime);
    }

    private void ResetReload()
    {
        UpdateState(State.Idle);
    }
    #endregion

    private bool ClipEmpty()
    {
        if (_ammoManager.ClipCount <= 0)
        {
            if (!_audioSource.isPlaying) SwitchAudioClip(_emptyClip);
            return true;
        }
        return false;
    }

    private void SwitchAudioClip(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
