using System;
using UnityEngine;
using UnityEngine.AI;
using BRIKStone.Utils;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    // Переменные которые можно изменять в меню изменения объекта Unity
    [SerializeField] private State startingState;
    [SerializeField] private float roamingDistanceMax;
    [SerializeField] private float roamingDistanceMin;
    [SerializeField] private float roamingTimerMax;

    [Header("Атакующий энтити или нет")] [SerializeField]
    private bool isAttackingEnemy = false;

    private float _attackingDistance = 2f;

    private float _attackRate = 2f;
    private float _nextAttackTime = 0f;
    
    
    // Враг преследует гг или нет 
    [SerializeField] private bool isChasingEnemy = false;
    private float _chasingDistance = 4f;
    private float _chasingMultiplayer = 2f;

    // Переменные
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private State _state;
    private float _roamingTimer;
    private Vector3 _roamPosition;
    private Vector3 _startingPosition;

    private float _roamingSpeed;
    private float _chasingSpeed;

    public event EventHandler OnEnemyAttack;


    // Хэширование для оптимизации
    private static readonly int HorizontalKey = Animator.StringToHash("Horizontal");
    private static readonly int VerticalKey = Animator.StringToHash("Vertical");
    private static readonly int IsRoamingKey = Animator.StringToHash("IsRoaming");

    private enum State
    {
        idle,
        Roaming,
        Chase,
        Attacking,
        Death
    }

    // Включение объектов через Awake
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _animator = GetComponentInChildren<Animator>();
        _navMeshAgent.updateUpAxis = false;
        _state = startingState;

        _roamingSpeed = _navMeshAgent.speed;
        _chasingSpeed = _navMeshAgent.speed * _chasingMultiplayer;
        _startingPosition = transform.position;
    }

    // State-машина
    private void Update()
    {
        StateHandler();
    }


    private void StateHandler()
    {
        switch (_state)
        {
            case State.Roaming:
                if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                {
                    _state = State.idle;
                    _roamingTimer = roamingTimerMax;
                }

                break;

            case State.Chase:
                ChasingTarget();
                CheckCurrentState();

                break;
            case State.Attacking:
                AttackingTarget();
                CheckCurrentState();

                break;
            case State.Death:
                break;

            default:
            case State.idle:
                _roamingTimer -= Time.deltaTime;
                if (_roamingTimer <= 0)
                {
                    _state = State.Roaming;
                    StartRoaming();
                }

                break;
        }

        UpdateEnemyAnimations();
    }


    private void CheckCurrentState()
    {
        float distasnceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        State newState = State.Roaming;

        if (isChasingEnemy)
        {
            if (distasnceToPlayer <= _chasingDistance)
            {
                newState = State.Chase;
            }
        }

        if (isAttackingEnemy)
        {
            if (distasnceToPlayer <= _attackingDistance)
            {
                newState = State.Attacking;
            }
        }

        if (newState != _state)
        {
            if (newState == State.Chase)
            {
                _navMeshAgent.ResetPath();
                _navMeshAgent.speed = _chasingSpeed;
            }
            else if (newState == State.Roaming)
            {
                _roamingTimer = 0f;
                _navMeshAgent.speed = _roamingSpeed;
            }
            else if (newState == State.Attacking)
            {
                _navMeshAgent.ResetPath();
            }


            _state = newState;
        }
    }


    private void ChasingTarget()
    {
        _navMeshAgent.SetDestination(Player.Instance.transform.position);
    }

    private void AttackingTarget()
    {
        if (Time.time > _nextAttackTime)
        {
            OnEnemyAttack?.Invoke(this, EventArgs.Empty);

            _nextAttackTime = Time.time + _attackRate;
        }
    }


    // Логика направления движения
    private void UpdateEnemyAnimations()
    {
        Vector3 moveDirection = _navMeshAgent.desiredVelocity;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            moveDirection.Normalize();
            _animator.SetFloat(HorizontalKey, moveDirection.x);
            _animator.SetFloat(VerticalKey, moveDirection.y);
        }

        _animator.SetBool(IsRoamingKey, _state == State.Roaming);
    }

    private void StartRoaming()
    {
        _roamPosition = GetRoamingPosition();
        _navMeshAgent.SetDestination(_roamPosition);
    }

    // Получение позиции для просчитывания пути
    private Vector3 GetRoamingPosition()
    {
        return _startingPosition + Utils.GetRandomDir() * Random.Range(roamingDistanceMin, roamingDistanceMax);
    }
}