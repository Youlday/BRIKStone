<<<<<<< Updated upstream
=======
using System;
using System.Collections;
>>>>>>> Stashed changes
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [Header("Настройки скорости персонажа")]
    [SerializeField] private float movingSpeed = 3f;
    [SerializeField] private float runningSpeed = 5f;
    
    [Header("Настройки атаки")]
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private MonoBehaviour equippedSword;

    [Header("Звуки шагов")]
    [SerializeField] private AudioClip[] stepSounds; 

    private Rigidbody2D _rb;
    private Animator _animator;
    private AudioSource _audioSource;
    private Vector2 _movement;
    private float _lastAttackTime;
    
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

    public void PlaySwingSound()
    {
        if (_audioSource)
        {
            _audioSource.Play();
        }
    }

    public void PlayStepSound()
    {
        if (_audioSource && stepSounds != null && stepSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, stepSounds.Length);
            _audioSource.pitch = Random.Range(0.85f, 1.15f);
            _audioSource.PlayOneShot(stepSounds[randomIndex]);
        }
    }

    public void OnAttackStart()
    {
        if (equippedSword != null)
        {
            equippedSword.SendMessage("AttackColliderTurnOn", SendMessageOptions.DontRequireReceiver);
        }
    }

    public void OnAttackEnd()
    {
        if (equippedSword != null)
        {
            equippedSword.SendMessage("AttackColliderTurnOff", SendMessageOptions.DontRequireReceiver);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
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