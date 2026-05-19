using System;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(EnemyAI))]

public class EnemyEntity : MonoBehaviour
{

    
    public event EventHandler OntakeHit;
    public event EventHandler OnDeath;
    [Header("Здоровье сущности")] [SerializeField]
    private int maxHealth = 5;

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
}