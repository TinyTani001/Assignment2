using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private Animator _characterAnimator;
    [SerializeField] private InputManager _inputManager;

    private void Start()
    {
        _inputManager.OnLocomotionInputUpdated += inputValue =>
        {
            _characterAnimator.SetFloat("HorizontalInput", inputValue.x);
            _characterAnimator.SetFloat("VerticalInput", inputValue.z);
        };
    }
}
