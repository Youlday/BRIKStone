using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class EnemyEntity : MonoBehaviour
{
    [Header("Здоровье сущности")] [SerializeField]
    private int maxHealth = 5;

    private int _currentHealth;

    private PolygonCollider2D _polygonCollider;


    private void Awake()
    {
        _polygonCollider = GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    // Получение урона 
    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        DetectDeath();
    }

    public void PolygonColliderTurnOff()
    {
        _polygonCollider.enabled = false;
    }

    public void PolygonColliderTurnOn()
    {
        _polygonCollider.enabled = true;
    }

    private void DetectDeath()
    {
        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}