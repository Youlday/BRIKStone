using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Настройки скорости персонажа")]
    [SerializeField] private float movingSpeed = 5f;
    [SerializeField] private float runningSpeed = 8f;
    
    [Header("Настройки атаки")]
    [SerializeField] private float attackCooldown = 0.5f;

    private Rigidbody2D _rb;
    private Animator _animator;
    private Vector2 _movement;
    private float _lastAttackTime;
    
    // Хэшированные вызовы
    private static readonly int AttackKey = Animator.StringToHash("Attack");
    private static readonly int HorizontalKey = Animator.StringToHash("Horizontal");
    private static readonly int VerticalKey = Animator.StringToHash("Vertical");
    private static readonly int SpeedKey = Animator.StringToHash("Speed");
    private static readonly int IsRunningKey = Animator.StringToHash("IsRunning");


    private bool IsShiftPressed()
    {
        return Keyboard.current.shiftKey.isPressed;
    }
    
    private void AttemptAttack()
    {
        if (Time.time - _lastAttackTime >= attackCooldown)
        {
            PerformAttack();
        }
    }
    
    private void PerformAttack()
    {
        _lastAttackTime = Time.time;
        
        if (_animator)
        {
            _animator.SetTrigger(AttackKey);
        }
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        if (_movement != Vector2.zero)
        {
            _animator.SetFloat(HorizontalKey, _movement.x);
            _animator.SetFloat(VerticalKey, _movement.y);
        }

        _animator.SetFloat(SpeedKey, _movement.sqrMagnitude);

        bool isRunning = IsShiftPressed() && _movement != Vector2.zero;
        _animator.SetBool(IsRunningKey, isRunning);
        
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            AttemptAttack();
        }
        
    }

    private void FixedUpdate()
    {
        float currentSpeed = IsShiftPressed() ? runningSpeed : movingSpeed;
        _rb.linearVelocity = _movement.normalized * currentSpeed;
    }
}