using System;
using UnityEngine;

public class CharacterMotor : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _jumpToApexTime;
    [SerializeField] private LayerMask _groundLayer;

    public Action<bool> OnGroundedStatusChanged;

    private float _gravity, _jumpForce, _velocityY;
    private bool _isGrounded = true, _walking;
    private Vector3 _moveDirection;

    private void Start()
    {
        _gravity = -(2f * _jumpHeight) / Mathf.Pow(_jumpToApexTime, 2f);
        _jumpForce = Mathf.Abs(_gravity) * _jumpToApexTime;
        _moveDirection = Vector3.zero;
    }

    private void FixedUpdate()
    {
        _velocityY += _gravity * Time.fixedDeltaTime;
        _rigidbody.velocity = (_moveDirection * (_walking ? _walkSpeed : _runSpeed) * Time.deltaTime) + Vector3.up * _velocityY;
        if (IsGrounded()) _velocityY = 0f;
        _moveDirection = Vector3.zero;
    }

    public void Move(Vector3 direction, bool walking = true)
    {
        _moveDirection = direction;
        _walking = walking;
    }

    public void Jump()
    {
        if (_isGrounded) _velocityY = _jumpForce;
    }

    private bool IsGrounded()
    {
        bool wasGrounded = _isGrounded;
        _isGrounded = false;
        if (_velocityY < 0f && Physics.Raycast(transform.position + Vector3.up * 0.01f, Vector3.down, out RaycastHit hit, 0.2f, _groundLayer))
        {
            _isGrounded = true;
        }
        if (wasGrounded != _isGrounded) OnGroundedStatusChanged?.Invoke(_isGrounded);
        if (_isGrounded) _rigidbody.angularVelocity = Vector3.zero;
        return _isGrounded;
    }
}
