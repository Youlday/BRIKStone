using System;
using UnityEngine;
using UnityEngine.AI;
using BRIKStone.Utils;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    // Переменные которые можно изменять в меню изменения объекта Unity
    [SerializeField] private State startingState;
    [Header("Максимальная дистанция патрулирования")] 
    [SerializeField] private float roamingDistanceMax;
    [Header("Минимальная  дистанция патрулирования")] 
    [SerializeField] private float roamingDistanceMin;
    [Header("Отдых после патрулирования")] 
    [SerializeField] private float roamingTimerMax;

    [Header("Атакующий энтити или нет")] [SerializeField]
    private bool isAttackingEnemy = false;
    
    [Header("Дистанция атаки")] [SerializeField]
    private float _attackingDistance = 1f;

    [Header("Скорость Атаки")] [SerializeField]
    private float _attackRate = 2f;
    private float _nextAttackTime = 0f;


    // Враг преследует гг или нет 
    [Header("Преследование игрока")] [SerializeField]
    private bool isChasingEnemy = false;

    private float _chasingDistance = 4f;
    private float _chasingMultiplier = 2f;

    // Переменные
    private NavMeshAgent _navMeshAgent;
    private State _state;
    private float _roamingTimer;
    private Vector3 _roamPosition;
    private Vector3 _startingPosition;

    private float _roamingSpeed;
    private float _chasingSpeed;

    public event EventHandler OnEnemyAttack;

    // Связь со SlimeVisual(Любой Visual частью врагов)
    public State CurrentState => _state;
    public Vector3 MovementVelocity => _navMeshAgent != null ? _navMeshAgent.desiredVelocity : Vector3.zero;

    public enum State
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
        _navMeshAgent.updateUpAxis = false;
        _state = startingState;

        _roamingSpeed = _navMeshAgent.speed;
        _chasingSpeed = _navMeshAgent.speed * _chasingMultiplier;
        _startingPosition = transform.position;
    }

    // State-машина
    private void Update()
    {
        StateHandler();
    }

    public void SetDeathState()
    {
        _navMeshAgent.ResetPath();
        _state = State.Death;
    }

    private void StateHandler()
    {
        switch (_state)
        {
            case State.Roaming:
                CheckCurrentState();
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
                CheckCurrentState();
                _roamingTimer -= Time.deltaTime;
                if (_roamingTimer <= 0)
                {
                    _state = State.Roaming;
                    StartRoaming();
                }

                break;
        }
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

    public float GetRoamingAnimationSpeed()
    {
        return _navMeshAgent.speed / _roamingSpeed;
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