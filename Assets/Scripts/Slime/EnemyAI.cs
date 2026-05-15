using UnityEngine;
using UnityEngine.AI;
using BRIKStone.Utils;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State startingState;
    [SerializeField] private float roamingDistanceMax;
    [SerializeField] private float roamingDistanceMin;
    [SerializeField] private float roamingTimerMax;

    private NavMeshAgent _navMeshAgent;
    private State _state;
    private float _roamingTimer;
    private Vector3 _roamPosition;
    private Vector3 _startingPosition;

    private enum State
    {
        idle,
        Roaming
    }

    private void Start()
    {
        _startingPosition = transform.position;
    }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _state = startingState;
    }

    private void Update()
    {
        switch (_state)
        {
            default:
            case State.idle:
                break;
            case State.Roaming:
                _roamingTimer -= Time.deltaTime;
                if (_roamingTimer <= 0)
                    Roaming();
                _roamingTimer = roamingTimerMax;
                break;
        }
    }

    private void Roaming()
    {
        _roamPosition = GetRoamingPosition();
        _navMeshAgent.SetDestination(_roamPosition);
    }

    private Vector3 GetRoamingPosition()
    {
        return _startingPosition + Utils.GetRandomDir() * UnityEngine.Random.Range(roamingDistanceMin, roamingDistanceMax);
    }
}