using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private CharacterMotor _characterMotor;

    private void Start()
    {
        _inputManager.OnJumpInput += _characterMotor.Jump;
        SingletonManager.Instance.Player = this;
    }

    private void FixedUpdate()
    {
        _characterMotor.Move(_inputManager.LocomotionInputValues);
    }
}
