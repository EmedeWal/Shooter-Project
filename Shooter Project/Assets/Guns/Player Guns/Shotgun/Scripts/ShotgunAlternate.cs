using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunAlternate : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private Animator _animator;
    private AmmoManager _ammoManager;
    private GunManager _gunManager;
    private Transform _firePoint;
    private int _layerMask;

    [Header("VARIABLES")]
    [SerializeField] private int _damage = 100;
    [SerializeField] private float _fireRange = 60f;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private float _fireDuration = 0.3f;

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
        _gunManager.ShootAlternatePerformed += ShotgunAlternate_ShootAlternatePerformed;
    }

    private void OnDisable()
    {
        _gunManager.ShootAlternatePerformed -= ShotgunAlternate_ShootAlternatePerformed;
    }

    private void ShotgunAlternate_ShootAlternatePerformed()
    {
        if (_ammoManager.ClipEmpty()) return;

        _gunManager.UpdateState(GunManager.GunState.Firing);
        Fire(_damage, _fireRange, _fireClip);
    }

    private void Fire(int damage, float fireRange, AudioClip fireClip)
    {
        FireSetup(fireClip);

        if (Physics.Raycast(_firePoint.position, _firePoint.forward, out RaycastHit hit, fireRange, _layerMask))
        {
            if (hit.transform.TryGetComponent(out Health health))
            {
                health.Damage(damage);
            }
        }

        Invoke(nameof(ResetToRecharging), _fireDuration);
        Invoke(nameof(ResetToIdle), _fireRate);
    }

    private void FireSetup(AudioClip fireClip)
    {
        _ammoManager.SpendAmmo(1);
        _animator.SetTrigger("Fire");

        MuzzleFlash(_muzzle);
        SwitchAudioClip(fireClip);

    }

    private void ResetToRecharging()
    {
        _gunManager.UpdateState(GunManager.GunState.Recharging);
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
