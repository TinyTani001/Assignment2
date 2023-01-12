using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private CharacterMotor _characterMotor;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnPosition, _meshTransform;
    [SerializeField] private PlayerDataSO _playerData;
    [SerializeField] private AudioSource _bulletSoundAudio, _playerAudio;

    private float _bulletFireTime;

    private void Awake()
    {
        _playerData.BaseBulletSpawnPattern += (bulletObject, bulletSpawnPoint, playerAimRotation) =>
        {
            Instantiate(bulletObject, _bulletSpawnPosition.position, playerAimRotation);
        };

        _playerData.OnPlayerDead += () =>
        {
            _playerAudio.clip = _playerData.PlayerDeadSound;
            _playerAudio.Play();
        };

        _playerData.InitializeData();
    }

    private void Start()
    {
        _inputManager.OnJumpInput += _characterMotor.Jump;
        _playerData.Player = this;
        _bulletFireTime = Time.time + _playerData.FireRate;
        StartCoroutine(InitializePlayerStatUI());
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
            if (delta.sqrMagnitude > 0.01f)
            {
                _meshTransform.localRotation = Quaternion.LookRotation(delta);
            }
            _playerData.SetLocomotionAnimationDirection(_meshTransform.InverseTransformDirection(_inputManager.LocomotionInputValues));
            if (Time.time > _bulletFireTime)
            {
                _bulletFireTime = Time.time + _playerData.FireRate;
                _playerData.SpawnBullet.Invoke(_bulletPrefab, _bulletSpawnPosition.position, _meshTransform.rotation);
                _bulletSoundAudio.clip = _playerData.BulletSound;
                _bulletSoundAudio.Play();
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
        if (_playerData.IsPlayerDead) return;
        _playerAudio.clip = _playerData.PlayerHitSound;
        _playerAudio.Play();
    }

    IEnumerator InitializePlayerStatUI()
    {
        yield return new WaitUntil(() => _playerData.GameUIData.OnHealthUpdated != null);
        _playerData.GameUIData.OnHealthUpdated.Invoke(_playerData.CurrentPlayerHealth);
        _playerData.GameUIData.OnDamageUpdated.Invoke(_playerData.BulletDamage);
        _playerData.GameUIData.OnFireRateUpdated.Invoke(_playerData.FireRate);
    }
}
