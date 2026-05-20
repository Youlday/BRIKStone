using System;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(EnemyAI))]

public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private EnemySO _enemySO;
    public event EventHandler OntakeHit;
    public event EventHandler OnDeath;

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
        _currentHealth = _enemySO.enemyHealth;
    }

    // Получение урона 
    public void TakeDamage(int damage)
    {
        if (_currentHealth <= 0) return; 

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
            _capsuleCollider.enabled = false;
            _polygonCollider.enabled = false;
            _enemyAI.SetDeathState();
            OnDeath?.Invoke(this, EventArgs.Empty);
            
            if (SlimeTracker.Instance != null)
                SlimeTracker.Instance.RegisterSlimeDeath();
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out Player player))
        {
            player.TakeDamage(transform, _enemySO.enemyDamageAmount);
        }
    }

}