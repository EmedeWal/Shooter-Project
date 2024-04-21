using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [SerializeField] private LayerMask _collisionMask;
    private Collider _coll;

    private void OnTriggerEnter(Collider other)
    {
        if ((_collisionMask & (1 << other.gameObject.layer)) != 0)
        {
            Destroy(gameObject);
        }
    }
}
