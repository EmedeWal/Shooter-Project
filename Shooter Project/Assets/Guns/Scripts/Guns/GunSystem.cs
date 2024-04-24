using UnityEngine;

public class GunSystem : MonoBehaviour
{ 
    [Header("STATS")]
    [SerializeField] private int _damage;
    [SerializeField] private float _spread, _range, _fireDuration, _fireRate, _fireCD, _shotDelay;
    [SerializeField] private float _shots; 
    [SerializeField] private int _ammoConsumption = 1;
    [SerializeField] private bool _automaticFire;

    private float _shotsToFire;

    [Header("REFERENCES")]
    [SerializeField] private Transform _firePoint;
    private GunStateManager _gunStateManager;
    private AmmoManager _ammoManager; 
    private Camera _camera;
    private LayerMask _layerMask;

    [Header("GRAPHICS")]
    [SerializeField] private GameObject muzzleFlash, _bulletHoleGraphics;

    //public CamShake camShake;
    //public float camShakeMagnitude, camShakeDuration;

    private void Awake()
    {
        _gunStateManager = GetComponent<GunStateManager>();
        _ammoManager = GetComponent<AmmoManager>();
        _camera = GetComponentInParent<Camera>();
    }

    private void Start()
    {
        _layerMask = ~LayerMask.GetMask("Player");
    }

    private void OnEnable()
    {
        PlayerShoot.ShootPerformed += GunSystem_ShootPerformed;
        PlayerShoot.ShootCanceled += GunSystem_ShootCanceled;
    }

    private void OnDisable()
    {
        PlayerShoot.ShootPerformed -= GunSystem_ShootPerformed;
        PlayerShoot.ShootCanceled -= GunSystem_ShootCanceled;
    }

    private void GunSystem_ShootPerformed()
    {
        StartShooting();
    }

    private void GunSystem_ShootCanceled()
    {
        StopShooting();
    }

    private void StartShooting()
    {
        if (_automaticFire)
        {
            InvokeRepeating(nameof(CheckShoot), 0, 1 / _fireRate);
        }
        else
        {
            CheckShoot();
        }
    }

    private void StopShooting()
    {
        CancelInvoke(nameof(CheckShoot));
    }

    private void CheckShoot()
    {
        if (!CanShoot()) return;

        _gunStateManager.UpdateState(GunStateManager.GunState.Firing);

        _shotsToFire = _shots;

        Shoot();
    }

    private void Shoot()
    {
        Vector3 direction = CalculateSpread();
        Vector3 position = _camera.transform.position;

        if (Physics.Raycast(position, direction, out RaycastHit hitInfo, _range, _layerMask))
        {
            if (hitInfo.transform.TryGetComponent<Health>(out var health)) health.Damage(_damage);

            Instantiate(_bulletHoleGraphics, hitInfo.point, Quaternion.Euler(0, 180, 0));
        }

        Instantiate(muzzleFlash, _firePoint);

        _ammoManager.SpendAmmo(_ammoConsumption);

        _shotsToFire--;

        if (_shotsToFire > 0)
        {
            Invoke(nameof(Shoot), _shotDelay);
            return;
        }

        //ShakeCamera
        //camShake.Shake(camShakeDuration, camShakeMagnitude);

        Invoke(nameof(ResetToRecharging), _fireDuration);
        Invoke(nameof(ResetToIdle), _fireCD);
    }

    private void ResetToRecharging()
    {
        UpdateState(GunStateManager.GunState.Recharging);
    }

    private void ResetToIdle()
    {
        UpdateState(GunStateManager.GunState.Idle);
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

    private void UpdateState(GunStateManager.GunState state)
    {
        _gunStateManager.UpdateState(state);
    }
}
