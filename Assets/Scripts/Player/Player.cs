using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPlayerDeath;

    [Header("Настройки скорости персонажа")] [SerializeField]
    private float movingSpeed = 3f;

    [SerializeField] private float runningSpeed = 5f;

    [Header("Здоровье героя")] [SerializeField]
    private int maxHealth = 10;

    [Header("Неуязвимость после получения урона")] [SerializeField]
    private float _damageRecoveryTime = 0.5f;

    [Header("Настройки атаки")] [SerializeField]
    private float attackCooldown = 0.5f;

    [SerializeField] private MonoBehaviour equippedSword;

    [Header("Звуки шагов")] [SerializeField]
    private AudioClip[] stepSounds;

    private Rigidbody2D _rb;
    private KnockBack _knockBack;

    private Animator _animator;
    private AudioSource _audioSource;
    private Vector2 _movement;
    private float _lastAttackTime;

    private int _currentHealth;
    private bool _canTakeDamage;
    private bool _IsAlive;

    private static readonly int AttackKey = Animator.StringToHash("Attack");
    private static readonly int HorizontalKey = Animator.StringToHash("Horizontal");
    private static readonly int VerticalKey = Animator.StringToHash("Vertical");
    private static readonly int SpeedKey = Animator.StringToHash("Speed");
    private static readonly int IsRunningKey = Animator.StringToHash("IsRunning");
    private static readonly int IsDieKey = Animator.StringToHash("IsDie");


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

    private void Start()
    {
        _currentHealth = maxHealth;
        _canTakeDamage = true;
        _IsAlive = true;
        Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
    }

    private void Player_OnPlayerDeath(object sender, System.EventArgs e)
    {
        _animator.SetBool("IsDie", true);
    }

    public void TakeDamage(Transform damageSource, int damage)
    {
        if (_canTakeDamage && _IsAlive)
        {
            _canTakeDamage = false;
            _currentHealth = Mathf.Max(0, _currentHealth -= damage);
            _knockBack.GetKnockBack(damageSource);

            StartCoroutine(DamageRecoveryRoutine());
        }

        DetectDeath();
    }

    private void DetectDeath()
    {
        if (_currentHealth <= 0 && _IsAlive)
        {
            DisableMovement();
            _IsAlive = false;
            _canTakeDamage = false;
            _knockBack.StopKnockBackMovement();
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        }
    }


    // Корутина неуязвимости
    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(_damageRecoveryTime);
        _canTakeDamage = true;
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
        _knockBack = GetComponent<KnockBack>();
    }

    private void Update()
    {
        if (!_IsAlive) return;
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
        if (_knockBack.IsGettingBack || !_IsAlive ) return;
        float currentSpeed = IsShiftPressed() ? runningSpeed : movingSpeed;
        _rb.linearVelocity = _movement.normalized * currentSpeed;
    }

    public void DisableMovement()
    {
        _rb.linearVelocity = Vector2.zero;
        _movement = Vector2.zero;
        _animator.SetFloat(SpeedKey, 0);
        _animator.SetBool(IsRunningKey, false);
    }
}