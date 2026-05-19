using Unity.VisualScripting;
using UnityEngine;

public class KnockBack : MonoBehaviour
{   
    [SerializeField] private int _knockBackForce;
    [SerializeField] private float _knockBackMovementTimerMax = 3f;

    private float _knockBackMovingTimer;
    
    private Rigidbody2D _rb;

    public bool IsGettingBack { get; private set; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _knockBackMovingTimer -= Time.deltaTime;
        if (_knockBackMovingTimer <= 0)
        {
            StopKnockBackMovement();
        }
    }

    public void GetKnockBack(Transform damageSource)
    {
        IsGettingBack = true;
        _knockBackMovingTimer = _knockBackMovementTimerMax;
        Vector2 difference = (transform.position - damageSource.position).normalized * _knockBackForce;
        _rb.AddForce(difference, ForceMode2D.Impulse);
    }

    public void StopKnockBackMovement()
    {
        _rb.linearVelocity = Vector2.zero;
        IsGettingBack = false;
    }
}
