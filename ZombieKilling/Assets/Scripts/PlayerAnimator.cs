using UnityEngine;

public class PlayerAnimator : AnimationEventReceiver
{
    [SerializeField] private Animator _characterAnimator;
    [SerializeField] private CharacterMotor _characterMotor;
    [SerializeField] private InputManager _inputManager;

    private void Start()
    {
        _inputManager.OnLocomotionInputUpdated += inputValue =>
        {
            _characterAnimator.SetFloat("HorizontalInput", inputValue.x);
            _characterAnimator.SetFloat("VerticalInput", inputValue.z);
        };
        _characterMotor.OnGroundedStatusChanged += isGrounded =>
        {
            _characterAnimator.SetBool("Jump", !isGrounded);
        };
    }
}
