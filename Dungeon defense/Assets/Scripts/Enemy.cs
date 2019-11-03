using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 1000f;
    private float _health = 0f;
    [SerializeField] private Slider _healthbar;
    public GameObject goal { get; set; }
    private void Start()
    {
        _healthbar.minValue = _health;
        _healthbar.maxValue = _maxHealth;
    }

    private void Update()
    {
        if (goal != null)
            if (Vector3.Distance(transform.position, goal.transform.position) < 2f)
            {
                GameManager.Instance.playerHealth -= 1;
                Die();
            }
    }

    public void TakeDamage(float amount)
    {
        _health += amount;
        _healthbar.value = _health;
        if (_health >= _maxHealth)
            Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
