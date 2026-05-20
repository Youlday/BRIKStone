using UnityEngine;

public class SlimeVisual : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;

    [SerializeField] private EnemyEntity enemyEntity;

    // Переменные
    private Animator _animator;

    // Хэш для оптимизации
    private static readonly int HorizontalKey = Animator.StringToHash("Horizontal");
    private static readonly int VerticalKey = Animator.StringToHash("Vertical");
    private static readonly int IsRoamingKey = Animator.StringToHash("IsRoaming");
    private static readonly int AnimSpeedKey = Animator.StringToHash("AnimSpeed");
    private static readonly int AttackKey = Animator.StringToHash("Attack");
    private static readonly int HitKey = Animator.StringToHash("TakeHit");
    private static readonly int DeathKey =  Animator.StringToHash("IsDead");
    
    SpriteRenderer _spriteRenderer;

    // "Пробуждение объектов"
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (enemyAI == null)
        {
            enemyAI = GetComponentInParent<EnemyAI>();
        }
    }


    private void Start()
    {
        enemyAI.OnEnemyAttack += _enemyAI_onEnemyAttack;
        enemyEntity.OntakeHit += _enemyEntity_OnTakeHit;
        enemyEntity.OnDeath += _enemyEntity_OnDeath;
    }

    private void OnDestroy()
    {
<<<<<<< Updated upstream
        if (enemyEntity != null)
        {
            enemyEntity.OntakeHit -= _enemyEntity_OnTakeHit;
=======
        if (enemyAI != null) enemyAI.OnEnemyAttack -= _enemyAI_onEnemyAttack;
        if (enemyEntity != null)
        {
            enemyEntity.OntakeHit -= _enemyEntity_OnTakeHit;
            enemyEntity.OnDeath -= _enemyEntity_OnDeath;
>>>>>>> Stashed changes
        }
    }

    private void _enemyEntity_OnTakeHit(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(HitKey);
    }

    private void _enemyEntity_OnDeath(object sender, System.EventArgs e)
    {
        _animator.SetBool(DeathKey, true);
        _spriteRenderer.sortingOrder = -1;
    }


    private void Update()
    {
        if (enemyAI == null) return;
        
        UpdateEnemyAnimations();
    }

    private void UpdateEnemyAnimations()
    {
        Vector3 moveDirection = enemyAI.MovementVelocity;
        Vector2 movement = new Vector2(moveDirection.x, moveDirection.y);

        if (movement != Vector2.zero)
        {
            _animator.SetFloat(HorizontalKey, movement.x);
            _animator.SetFloat(VerticalKey, movement.y);
        }

        _animator.SetBool(IsRoamingKey, enemyAI.CurrentState == EnemyAI.State.Roaming);

        // Ускорение анимации при преследовании игрока
        _animator.SetFloat(AnimSpeedKey, enemyAI.GetRoamingAnimationSpeed());
    }

    public void TriggerAttackAnimationTurnOff()
    {
        enemyEntity.PolygonColliderTurnOff();
    }

    public void TriggerAttackAnimationTurnOn()
    {
        enemyEntity.PolygonColliderTurnOn();
    }

    private void _enemyAI_onEnemyAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(AttackKey);
    }
}