using System.Collections;
using UnityEngine;

public class AssaultRifleRegular : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private Animator _animator;
    private AmmoManager _ammoManager;
    private GunManager _gunManager;
    private Transform _firePoint;
    private LayerMask _layerMask;

    [Header("VARIABLES")]
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _fireRate = 0.1f;
    [SerializeField] private float _fireRange = 40f;

    [Header("AUDIO")]
    [SerializeField] private AudioClip _fireClip;
    private AudioSource _audioSource;

    private void Awake()
    {
        _ammoManager = GetComponent<AmmoManager>();
        _gunManager = GetComponent<GunManager>();
        _audioSource = GetComponent<AudioSource>();
        _firePoint = GetComponentInParent<Transform>();
    }

    private void Start()
    {
        _layerMask = ~LayerMask.GetMask("Player");
    }

    private void OnEnable()
    {
        _gunManager.ShootRegularPerformed += AssaultRifleRegular_OnShootRegularPerformed;
        _gunManager.ShootRegularCanceled += AssaultRifleRegular_OnShootRegularCanceled;
    }

    private void OnDisable()
    {
        _gunManager.ShootRegularPerformed -= AssaultRifleRegular_OnShootRegularPerformed;
        _gunManager.ShootRegularCanceled -= AssaultRifleRegular_OnShootRegularCanceled;
    }

    private void AssaultRifleRegular_OnShootRegularPerformed()
    {
        _gunManager.UpdateState(GunManager.GunState.Firing);
        StartCoroutine(AutomaticFire());
    }

    private void AssaultRifleRegular_OnShootRegularCanceled()
    {
        _gunManager.UpdateState(GunManager.GunState.Recharging);
        StopAllCoroutines();
        Invoke(nameof(ResetToIdle), _fireRate);
    }

    private IEnumerator AutomaticFire()
    {
        while (true)
        {
            Fire(_damage, _fireRange, _muzzle);
            yield return new WaitForSeconds(_fireRate);
        }
    }

    private void Fire(int damage, float range, Transform origin)
    {
        if (_ammoManager.ClipEmpty()) return;

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
        _animator.SetTrigger("Fire");
        _ammoManager.SpendAmmo(1);
    }

    private void ResetToIdle()
    {
        _gunManager.UpdateState(GunManager.GunState.Idle);
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
