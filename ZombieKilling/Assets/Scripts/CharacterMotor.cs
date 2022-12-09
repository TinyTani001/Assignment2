using UnityEngine;

public class CharacterMotor : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _jumpToApexTime;
    [SerializeField] private LayerMask _groundLayer;

    private float _gravity, _jumpForce, _velocityY;
    private bool _isGrounded;

    private void Start()
    {
        _gravity = -(2f * _jumpHeight) / Mathf.Pow(_jumpToApexTime, 2f);
        _jumpForce = Mathf.Abs(_gravity) * _jumpToApexTime;
        _inputManager.OnJumpInput += () => { if (_isGrounded) _velocityY = _jumpForce; };
    }

    private void FixedUpdate()
    {
        ReadLocomotionInput();
    }

    private void ReadLocomotionInput()
    {
        _velocityY += _gravity * Time.fixedDeltaTime;
        _rigidbody.velocity = (_inputManager.LocomotionInputValues * _moveSpeed * Time.deltaTime) + Vector3.up * _velocityY;
        if (IsGrounded()) _velocityY = 0f;
    }

    private bool IsGrounded()
    {
        _isGrounded = false;
        if (_rigidbody.velocity.y > -0.001f && _rigidbody.velocity.y <= 0f)
        {
            _isGrounded = true;
        }
        if (_velocityY < 0f && Physics.Raycast(transform.position + Vector3.up * 0.01f, Vector3.down, out RaycastHit hit, 0.2f, _groundLayer))
        {
            _isGrounded = true;
        }
        if (_isGrounded) _rigidbody.angularVelocity = Vector3.zero;
        return _isGrounded;
    }
}
