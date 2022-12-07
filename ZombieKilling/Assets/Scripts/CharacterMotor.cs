using UnityEngine;

public class CharacterMotor : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _moveSpeed;

    private void FixedUpdate()
    {
        ReadLocomotionInput();
    }

    private void ReadLocomotionInput()
    {
        _rigidbody.velocity = _inputManager.LocomotionInputValues * _moveSpeed * Time.deltaTime;
    }
}
