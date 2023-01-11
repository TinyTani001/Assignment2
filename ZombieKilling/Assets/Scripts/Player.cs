using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private CharacterMotor _characterMotor;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnPosition, _meshTransform;
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
        if (!_playerData.IsPlayerDead)
        {
            Vector3 flatPlayerPosition = transform.position;
            flatPlayerPosition.y = _inputManager.MousePosition.y;
            Vector3 delta = _inputManager.MousePosition - flatPlayerPosition;
            if(delta.sqrMagnitude > 0.01f)
            {
                _meshTransform.localRotation = Quaternion.LookRotation(delta);
            }
            _playerData.SetLocomotionAnimationDirection(_meshTransform.InverseTransformDirection(_inputManager.LocomotionInputValues));
            if (Time.time > _bulletFireTime) {
                _bulletFireTime = Time.time + _playerData.FireRate;
                Instantiate(_bulletPrefab, _bulletSpawnPosition.position, _meshTransform.rotation);
            }
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
