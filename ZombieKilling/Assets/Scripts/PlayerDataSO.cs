using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData.asset", menuName = "Scriptable Objects/Player Data")]
public class PlayerDataSO : ScriptableObject
{
    [SerializeField, Min(0)] private int _playerHealth = 100;

    public Action OnPlayerDead;
    public Action<int> OnPlayerTookDamage;

    public bool IsPlayerDead { get; private set; }

    public Player Player { get; set; }

    public int CurrentPlayerHealth { get; private set; }

    public void InitializeData()
    {
        CurrentPlayerHealth = _playerHealth;
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

    public void ResetData()
    {
        OnPlayerDead = null;
        OnPlayerTookDamage = null;
        IsPlayerDead = false;
        Player = null;
    }
}
