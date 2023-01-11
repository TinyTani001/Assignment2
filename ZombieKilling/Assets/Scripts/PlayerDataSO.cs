using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData.asset", menuName = "Scriptable Objects/Player Data")]
public class PlayerDataSO : ScriptableObject
{
    [SerializeField, Min(0)] private int _playerHealth = 100;
    [SerializeField, Min(0f)] private float _baseFireRate = 0.6f;
    [SerializeField, Min(0)] private int _baseBulletDamage = 25;

    public Action OnPlayerDead;
    public Action<int> OnPlayerTookDamage;

    public bool IsPlayerDead { get; private set; }

    public Player Player { get; set; }

    public int CurrentPlayerHealth { get; private set; }

    public float FireRate { get; private set; }

    public int BulletDamage { get; private set; }

    public Vector3 LocomotionAnimationDirection { get; private set; }

    public void InitializeData()
    {
        CurrentPlayerHealth = _playerHealth;
        FireRate = _baseFireRate;
        BulletDamage = _baseBulletDamage;
    }

    public void DamagePlayer(int damageAmount)
    {
        CurrentPlayerHealth = Mathf.Clamp(CurrentPlayerHealth - damageAmount, 0, _playerHealth);
        if (CurrentPlayerHealth == 0)
        {
            IsPlayerDead = true;
            OnPlayerTookDamage?.Invoke(CurrentPlayerHealth);
            OnPlayerDead?.Invoke();
        }
        else
            OnPlayerTookDamage?.Invoke(CurrentPlayerHealth);
    }

    public void SetLocomotionAnimationDirection(Vector3 toDirection) => LocomotionAnimationDirection = toDirection;

    public void ResetData()
    {
        OnPlayerDead = null;
        OnPlayerTookDamage = null;
        IsPlayerDead = false;
        Player = null;
        LocomotionAnimationDirection = Vector3.zero;
    }
}
