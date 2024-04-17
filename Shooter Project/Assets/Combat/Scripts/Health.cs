using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    private int _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void Damage(int amount)
    {
        _currentHealth -= amount;

        Debug.Log($"{gameObject.name} took {amount} damage.");

        if (_currentHealth <= 0)
        {
            Debug.Log($"{gameObject.name} has died.");
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
