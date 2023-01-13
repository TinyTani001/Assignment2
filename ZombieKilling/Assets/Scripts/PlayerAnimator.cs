using UnityEngine;

public class PlayerAnimator : AnimationEventReceiver
{
    [SerializeField] private Animator _characterAnimator;
    [SerializeField] private CharacterMotor _characterMotor;
    [SerializeField] private Player _player;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private float _layerWeightShiftTime;
    [SerializeField] private PlayerDataSO _playerData;
    [SerializeField] private ZombieSpawnDataSO _zombieSpawnData;

    private float _layerWeightShiftRefValue, _upperBodyLayerFinalWeight;

    private void Start()
    {
        _inputManager.OnLocomotionInputUpdated += inputValue =>
        {
            if (_zombieSpawnData.RemainingZombiesCount > 0)
            {
                _upperBodyLayerFinalWeight = inputValue.sqrMagnitude > 0f ? 1f : 0f;
                _layerWeightShiftRefValue = 0f;
            }
        };
        _characterMotor.OnGroundedStatusChanged += isGrounded =>
        {
            if (_zombieSpawnData.RemainingZombiesCount > 0)
            {
                _characterAnimator.SetBool("Jump", !isGrounded);
            }
        };
        _playerData.OnPlayerTookDamage += damageAmount =>
        {
            if (!_playerData.IsPlayerDead)
                _characterAnimator.SetBool("HitReaction", true);
        };
        _playerData.OnPlayerDead += () => _characterAnimator.SetTrigger("Dead");
        _zombieSpawnData.OnZombieCountUpdated += zombieCount =>
        {
            if(zombieCount == 0)
            {
                _upperBodyLayerFinalWeight = 0f;
                _layerWeightShiftRefValue = 0f;
                _characterAnimator.SetFloat("HorizontalInput", 0f);
                _characterAnimator.SetFloat("VerticalInput", 0f);
            }
        };
    }

    private void Update()
    {
        float upperBodyCurrentLayerWeight = Mathf.SmoothDamp(_characterAnimator.GetLayerWeight(1), _upperBodyLayerFinalWeight, ref _layerWeightShiftRefValue, _layerWeightShiftTime);
        _characterAnimator.SetLayerWeight(1, upperBodyCurrentLayerWeight);
        _characterAnimator.SetLayerWeight(2, Mathf.InverseLerp(1f, 0f, upperBodyCurrentLayerWeight));
        if (_zombieSpawnData.RemainingZombiesCount > 0)
        {
            _characterAnimator.SetFloat("HorizontalInput", _playerData.LocomotionAnimationDirection.x);
            _characterAnimator.SetFloat("VerticalInput", _playerData.LocomotionAnimationDirection.z);
        }
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
