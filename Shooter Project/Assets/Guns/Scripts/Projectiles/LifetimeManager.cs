using UnityEngine;

public class LifetimeManager : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 0.5f;

    private void Start()
    {
        Invoke(nameof(DestroyGameObject), _lifeTime);
    }

    private void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
