using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private float _health = 1000f;

    public void TakeDamage(float amount)
    {
        _health -= amount;
        if (_health <= 0f)
            Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
