using UnityEngine;
using UnityEngine.InputSystem; 

public class Player : MonoBehaviour
{
    [SerializeField] private float movingSpeed = 5f;
    [SerializeField] private float runningSpeed = 8f;
    
    private Rigidbody2D _rb;
    
    private Animator _animator;
    
    private Vector2 _movement;
    
    private bool IsShiftPressed()
    {
        return Keyboard.current.shiftKey.isPressed;
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
            _animator.SetFloat("Horizontal", _movement.x);
            _animator.SetFloat("Vertical", _movement.y);
        }
        
        _animator.SetFloat("Speed", _movement.sqrMagnitude);
        
        bool isRunning = IsShiftPressed() && _movement != Vector2.zero;
        _animator.SetBool("IsRunning", isRunning);
    }

    private void FixedUpdate()
    {
        float currentSpeed = IsShiftPressed() ? runningSpeed : movingSpeed;
        _rb.linearVelocity = _movement.normalized * currentSpeed;
    }
}