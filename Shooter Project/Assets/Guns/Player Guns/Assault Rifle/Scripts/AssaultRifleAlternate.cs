using System.Collections;
using UnityEngine;

public class AssaultRifleAlternate : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private Animator animator;
    private AmmoManager _ammoManager;
    private GunManager _gunManager;
    private Transform _firePoint;
    private LayerMask _layerMask;

    [Header("VARIABLES")]
    [SerializeField] private int _bursts = 5;
    [SerializeField] private int _damage = 20;
    [SerializeField] private float _fireRange = 60f;
    [SerializeField] private float _burstPause = 0.05f;
    [SerializeField] private float _rechargeDelay = 0.15f;
    [SerializeField] private float _burstCD = 0.5f;

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
        _gunManager.ShootAlternatePerformed += AssaultRifleAlternate_ShootAlternatePerformed;
    }

    private void OnDisable()
    {
        _gunManager.ShootAlternatePerformed -= AssaultRifleAlternate_ShootAlternatePerformed;
    }

    private void AssaultRifleAlternate_ShootAlternatePerformed()
    {
        _gunManager.UpdateState(GunManager.GunState.Firing);
        CancelInvoke(nameof(ResetToIdle));  
        StartCoroutine(BurstFire());
    }

    private IEnumerator BurstFire()
    {
        for (int i = 0; i < _bursts; i++)
        {
            Fire(_damage, _fireRange, _muzzle);
            yield return new WaitForSeconds(_burstPause);
        }

        Invoke(nameof(ResetToRecharing), _rechargeDelay);
        Invoke(nameof(ResetToIdle), _burstCD);
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
        animator.SetTrigger("Fire");
        _ammoManager.SpendAmmo(1);
    }

    private void ResetToRecharing()
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
