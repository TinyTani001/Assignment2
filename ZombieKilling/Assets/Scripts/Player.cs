using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private CharacterMotor _characterMotor;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnPosition;
    [SerializeField] private PlayerDataSO _playerData;

    private float _bulletFireTime;

    private void Awake()
    {
        _playerData.InitializeData();
    }

    private void Start()
    {
        _inputManager.OnJumpInput += _characterMotor.Jump;
        _playerData.Player = this;
        _bulletFireTime = Time.time + _playerData.FireRate;
    }

    private void FixedUpdate()
    {
        if (_playerData.IsPlayerDead) return;
        _characterMotor.Move(_inputManager.LocomotionInputValues);
    }

    private void Update()
    {
        if (!_playerData.IsPlayerDead && Time.time > _bulletFireTime)
        {
            _bulletFireTime = Time.time + _playerData.FireRate;
            Instantiate(_bulletPrefab, _bulletSpawnPosition.position, Quaternion.identity);
        }
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
