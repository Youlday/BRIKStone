using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Sword : MonoBehaviour
{
    [Header("Настройки оружия")] 
    [SerializeField] private int damage = 5;

    private CapsuleCollider2D _swordCollider;

    private void Awake()
    {
        _swordCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        _swordCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision is CapsuleCollider2D)
        {
            if (collision.TryGetComponent(out EnemyEntity enemyEntity))
            {
                enemyEntity.TakeDamage(damage);
            }
        }
    }

    public void AttackColliderTurnOn()
    {
        _swordCollider.enabled = true;
    }

    public void AttackColliderTurnOff()
    {
        _swordCollider.enabled = false;
    }
    
}