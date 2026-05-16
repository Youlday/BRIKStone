using UnityEngine;
using UnityEngine.AI;
using BRIKStone.Utils;

public class EnemyAI : MonoBehaviour
{
    // Переменные которые можно изменять в меню изменения объекта Unity
    [SerializeField] private State startingState;
    [SerializeField] private float roamingDistanceMax;
    [SerializeField] private float roamingDistanceMin;
    [SerializeField] private float roamingTimerMax;

    
    // Переменные
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private State _state;
    private float _roamingTimer;
    private Vector3 _roamPosition;
    private Vector3 _startingPosition;
    
    
    
    
    // Хэширование для оптимизации
    private static readonly int HorizontalKey = Animator.StringToHash("Horizontal");
    private static readonly int VerticalKey = Animator.StringToHash("Vertical");
    private static readonly int IsRoamingKey = Animator.StringToHash("IsRoaming");

    private enum State
    {
        idle,
        Roaming
    }
    
    // Включение объектов через Awake
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _animator = GetComponentInChildren<Animator>();
        _navMeshAgent.updateUpAxis = false;
        _state = startingState;
    }

    // State-машина
    private void Update()
    {
        switch (_state)
        {
            case State.idle:
                _roamingTimer -= Time.deltaTime;
                if (_roamingTimer <= 0)
                {
                    _state = State.Roaming;
                    StartRoaming(); 
                }
                break;

            case State.Roaming:
                if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                {
                    _state = State.idle;
                    _roamingTimer = roamingTimerMax;
                }
                break;
        }

        UpdateEnemyAnimations();
    }
    
    //Логика направления движения
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
        _startingPosition = transform.position;
        _roamPosition = GetRoamingPosition();
        _navMeshAgent.SetDestination(_roamPosition);
    }
    
    // Получение позиции для просчитывания пути
    private Vector3 GetRoamingPosition()
    {
        return _startingPosition + Utils.GetRandomDir() * Random.Range(roamingDistanceMin, roamingDistanceMax);
    }
}