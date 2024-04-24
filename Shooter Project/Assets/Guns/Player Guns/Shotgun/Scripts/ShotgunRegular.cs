using UnityEngine;

public class ShotgunRegular : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Transform _firePoint;
    private GunStateManager _gunStateManager;
    private AmmoManager _ammoManager;
    private Camera _camera;
    private LayerMask _layerMask;

    [Header("GRAPHICS")]
    [SerializeField] private GameObject muzzleFlash, _bulletHoleGraphics;

    [Header("AUDIO")]
    [SerializeField] private AudioClip _fireClip;
    private AudioSource _audioSource;

    //public CamShake camShake;
    //public float camShakeMagnitude, camShakeDuration;

    [Header("STATS")]
    [SerializeField] private int _damage;
    [SerializeField] private float _spread, _range, _fireDuration, _fireCD;
    [SerializeField] private int _ammoConsumption = 1;
    [SerializeField] private int _pellets;

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
    }

    private void OnEnable()
    {
        PlayerShoot.ShootPerformed += ShotgunRegular_ShootPerformed;
    }

    private void OnDisable()
    {
        PlayerShoot.ShootPerformed -= ShotgunRegular_ShootPerformed;

        ResetState();
    }

    private void ShotgunRegular_ShootPerformed()
    {
        if (CanShoot()) ShootPellets();
    }

    private void ShootPellets()
    {
        for (int i = 0; i < _pellets; i++) Shoot();

        Invoke(nameof(ResetToRecharging), _fireDuration);
    }

    private void Shoot()
    {
        if (_ammoManager.ClipEmpty(_ammoConsumption)) return;

        _gunStateManager.UpdateState(GunStateManager.GunState.Firing);

        Vector3 direction = CalculateSpread();
        Vector3 position = _camera.transform.position;

        if (Physics.Raycast(position, direction, out RaycastHit hitInfo, _range, _layerMask))
        {
            if (hitInfo.transform.TryGetComponent<Health>(out var health)) health.Damage(_damage);

            Instantiate(_bulletHoleGraphics, hitInfo.point, Quaternion.Euler(0, 180, 0));
        }

        Instantiate(muzzleFlash, _firePoint);
        PlayAudioClip(_fireClip);
        _ammoManager.SpendAmmo(_ammoConsumption);

        //ShakeCamera
        //camShake.Shake(camShakeDuration, camShakeMagnitude);
    }

    private bool CanShoot()
    {
        if (_gunStateManager.State != GunStateManager.GunState.Idle) return false;
        return true;
    }

    private Vector3 CalculateSpread()
    {
        float x = Random.Range(-_spread, _spread);
        float y = Random.Range(-_spread, _spread);
        return _camera.transform.forward + new Vector3(x, y, 0);
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
