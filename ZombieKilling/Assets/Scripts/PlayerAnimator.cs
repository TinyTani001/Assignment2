using UnityEngine;

public class PlayerAnimator : AnimationEventReceiver
{
    [SerializeField] private Animator _characterAnimator;
    [SerializeField] private CharacterMotor _characterMotor;
    [SerializeField] private Player _player;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private float _layerWeightShiftTime;
    [SerializeField] private PlayerDataSO _playerData;

    private float _layerWeightShiftRefValue, _upperBodyLayerFinalWeight;

    private void Start()
    {
        _inputManager.OnLocomotionInputUpdated += inputValue =>
        {
            _characterAnimator.SetFloat("HorizontalInput", inputValue.x);
            _characterAnimator.SetFloat("VerticalInput", inputValue.z);
            _upperBodyLayerFinalWeight = inputValue.sqrMagnitude > 0f ? 1f : 0f;
            _layerWeightShiftRefValue = 0f;
        };
        _characterMotor.OnGroundedStatusChanged += isGrounded =>
        {
            _characterAnimator.SetBool("Jump", !isGrounded);
        };
        _playerData.OnPlayerTookDamage += damageAmount =>
        {
            if (!_playerData.IsPlayerDead)
                _characterAnimator.SetBool("HitReaction", true);
        };
        _playerData.OnPlayerDead += () => _characterAnimator.SetTrigger("Dead");
    }

    private void Update()
    {
        float upperBodyCurrentLayerWeight = Mathf.SmoothDamp(_characterAnimator.GetLayerWeight(1), _upperBodyLayerFinalWeight, ref _layerWeightShiftRefValue, _layerWeightShiftTime);
        _characterAnimator.SetLayerWeight(1, upperBodyCurrentLayerWeight);
        _characterAnimator.SetLayerWeight(2, Mathf.InverseLerp(1f, 0f, upperBodyCurrentLayerWeight));
    }

    public override void OnAnimEvent(string eventID)
    {
        base.OnAnimEvent(eventID);
        switch (eventID)
        {
            case "HitReactionStarted":
                _characterAnimator.SetBool("HitReaction", false);
                break;
        }
    }
}
