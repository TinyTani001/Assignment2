using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private CharacterMotor _characterMotor;

    public Action OnHitReceived;

    private void Start()
    {
        _inputManager.OnJumpInput += _characterMotor.Jump;
        SingletonManager.Instance.Player = this;
    }

    private void FixedUpdate()
    {
        _characterMotor.Move(_inputManager.LocomotionInputValues);
    }

    public void HitPlayer()
    {
        OnHitReceived.Invoke();
    }
}
