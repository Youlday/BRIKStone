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

    // "Пробуждение объектов"
    private void Awake()
    {
        _animator = GetComponent<Animator>();

        if (enemyAI == null)
        {
            enemyAI = GetComponentInParent<EnemyAI>();
        }
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

        // Ускоряемся при погоне
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
}