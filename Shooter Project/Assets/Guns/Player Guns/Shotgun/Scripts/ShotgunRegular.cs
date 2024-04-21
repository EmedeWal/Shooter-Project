using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunRegular : MonoBehaviour
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
    [SerializeField] private int _damage = 5;
    [SerializeField] private int _pellets = 25;
    [SerializeField] private float _fireRange = 40f;
    [SerializeField] private float _spread = 2f;
    [SerializeField] private float _fireRate = 1f;
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
        _gunManager.ShootRegularPerformed += ShotgunRegular_ShootRegularPerformed;
    }

    private void OnDisable()
    {
        _gunManager.ShootRegularPerformed -= ShotgunRegular_ShootRegularPerformed;
    }

    private void ShotgunRegular_ShootRegularPerformed()
    {
        _gunManager.UpdateState(GunManager.GunState.Firing);
        Fire(_damage, _pellets, _spread, _fireRange, _fireClip);
    }

    private void Fire(int damage, int pellets, float spread, float fireRange, AudioClip fireClip)
    {
        FireSetup(fireClip);

        int tableSize = Mathf.FloorToInt(Mathf.Sqrt(pellets));
        float initialOffset = spread * Mathf.FloorToInt(tableSize / 2);

        float yOffset = -initialOffset;

        for (int row = 0; row < tableSize; row++)
        {
            float xOffset = -initialOffset;

            for (int col = 0; col < tableSize; col++)
            {
                Vector3 direction = Quaternion.Euler(xOffset, yOffset, 0) * _firePoint.forward;
                FirePellet(_firePoint.position, direction, damage, fireRange);

                xOffset += spread;
            }

            yOffset += spread;
        }

        Invoke(nameof(ResetToRecharging), _fireDuration);
        Invoke(nameof(ResetToIdle), _fireRate);
    }

    private void FirePellet(Vector3 origin, Vector3 direction, int damage, float range)
    {
        if (Physics.Raycast(origin, direction, out RaycastHit hit, range, _layerMask))
        {
            if (hit.transform.TryGetComponent(out Health health))
            {
                health.Damage(damage);
            }
        }
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
