using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private CharacterMotor _characterMotor;
    [SerializeField] private PlayerDataSO _playerData;

    private void Awake()
    {
        _playerData.InitializeData();
    }

    private void Start()
    {
        _inputManager.OnJumpInput += _characterMotor.Jump;
        _playerData.Player = this;
    }

    private void FixedUpdate()
    {
        if (_playerData.IsPlayerDead) return;
        _characterMotor.Move(_inputManager.LocomotionInputValues);
    }

    private void OnDestroy()
    {
        _playerData.ResetData();
    }

    public void HitPlayer()
    {
        if (_playerData.IsPlayerDead) return;
        _playerData.DamagePlayer(30);
    }
}
