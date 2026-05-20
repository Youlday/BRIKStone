using System;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(EnemyAI))]

public class EnemyEntity : MonoBehaviour
{
<<<<<<< Updated upstream
<<<<<<< Updated upstream

    
    public event EventHandler OntakeHit;
    public event EventHandler OnDeath;
    [Header("Здоровье сущности")] [SerializeField]
    private int maxHealth = 5;
=======
    [SerializeField] private EnemySO _enemySO;
    public event EventHandler OntakeHit;
    public event EventHandler OnDeath;
>>>>>>> Stashed changes
=======
    [SerializeField] private EnemySO _enemySO;
    public event EventHandler OntakeHit;
    public event EventHandler OnDeath;
>>>>>>> Stashed changes

    private int _currentHealth;

    private PolygonCollider2D _polygonCollider;
    private CapsuleCollider2D _capsuleCollider;
    
    private EnemyAI _enemyAI;

    private void Awake()
    {
        _polygonCollider = GetComponent<PolygonCollider2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _enemyAI = GetComponent<EnemyAI>();
    }

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    // Получение урона 
    public void TakeDamage(int damage)
    {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
=======
        if (_currentHealth <= 0) return; 

>>>>>>> Stashed changes
=======
        if (_currentHealth <= 0) return; 

>>>>>>> Stashed changes
        _currentHealth -= damage;
        OntakeHit?.Invoke(this, EventArgs.Empty);
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
<<<<<<< Updated upstream
<<<<<<< Updated upstream
            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _capsuleCollider.enabled = false;
        _polygonCollider.enabled = false;
        _enemyAI.SetDeathState();
       // Debug.Log("Attack");
    }
=======
=======
>>>>>>> Stashed changes
            _capsuleCollider.enabled = false;
            _polygonCollider.enabled = false;
            _enemyAI.SetDeathState();
            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out Player player))
        {
            player.TakeDamage(transform, _enemySO.enemyDamageAmount);
        }
    }

<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
}