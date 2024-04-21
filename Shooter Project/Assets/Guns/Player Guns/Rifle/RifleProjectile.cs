using UnityEngine;

public class RifleProjectile : MonoBehaviour
{
    [SerializeField] private float _force = 300;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rb.AddForce(1000 * _force * Time.deltaTime * transform.forward, ForceMode.Impulse);
    }

}
