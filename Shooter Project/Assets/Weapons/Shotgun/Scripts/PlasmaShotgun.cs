using UnityEngine;

public class PlasmaShotgun : MonoBehaviour, IWeapon
{
    [Header("REFERENCES")]
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private Animator _animator;
    private AmmoManager _ammoManager;
    private Transform _firePoint;
    private int _layerMask;

    [Header("GENERAL: FIRING")]
    [SerializeField] private float _fireDelay = 0.3f;
    private bool _isFiring = false;

    [Header("REGULAR MODE")]
    [SerializeField] private int _damage = 5;
    [SerializeField] private int _pellets = 25;
    [SerializeField] private float _fireRange = 40f;
    [SerializeField] private float _spread = 2f;
    [SerializeField] private float _fireRate = 1f;
    private bool _canFire = true;

    [Header("ALTERNATIVE MODE")]
    [SerializeField] private int _alternateDamage = 100;
    [SerializeField] private float _alternateFireRange = 60f;

    [Header("RELOADING")]
    [SerializeField] private float _reloadTime = 1f;
    private bool _isReloading;

    [Header("AUDIO")]
    [SerializeField] private AudioClip _fireClip;
    [SerializeField] private AudioClip _alternateFireClip;
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

    public void Enter()
    {
        _ammoManager.OnAmmoUpdate();
    }

    public void Shoot()
    {
        if (CanFire() && _canFire)
        {
            _canFire = false;

            Fire(_damage, _pellets, _spread, _fireRange, _fireClip);
        }
    }

    public void CancelShoot()
    {
        return;
    }

    public void ShootAlternative()
    {
        if (CanFire())
        {
            CancelInvoke(nameof(ResetFire));
            _canFire = true;

            Fire(_alternateDamage, 1, 0, _alternateFireRange, _alternateFireClip);
        }
    }

    public void Reload()
    {
        if (_isReloading || !_ammoManager.CanReload()) return;

        _ammoManager.Reload();

        _isReloading = true;
        _canFire = true;

        _audioSource.Stop();
        SwitchAudioClip(_reloadClip);

        Invoke(nameof(ResetReload), _reloadTime);
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

        _isFiring = true;
        CancelInvoke(nameof(ResetFire));
        CancelInvoke(nameof(EndFire));
        Invoke(nameof(ResetFire), _fireRate);
        Invoke(nameof(EndFire), _fireDelay);
    }

    private bool CanFire()
    {
        if (_ammoManager.ClipCount <= 0)
        {
            if (!_audioSource.isPlaying) SwitchAudioClip(_emptyClip);
            return false;
        }
        else if (_isReloading || _isFiring)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void ResetFire()
    {
        _canFire = true;
    }

    private void EndFire()
    {
        _isFiring = false;
    }

    private void ResetReload()
    {
        _isReloading = false;
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
