using System;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(EnemyAI))]

public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private EnemySO _enemySO;
    
    [Header("Звук смерти")]
    [SerializeField] private AudioClip deathSound;
    
    [Tooltip("Усиление звука (сколько раз запустить одновременно)")]
    [SerializeField] [Range(1, 5)] private int soundBoost = 1;

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
            if (deathSound != null)
            {
                GameObject tempAudio = new GameObject("TempDeathAudio");
                AudioSource tempSource = tempAudio.AddComponent<AudioSource>();
                
                tempSource.spatialBlend = 0f;
                tempSource.volume = 1f;
                
                for (int i = 0; i < soundBoost; i++)
                {
                    tempSource.PlayOneShot(deathSound);
                }
                
                Destroy(tempAudio, deathSound.length);
            }

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