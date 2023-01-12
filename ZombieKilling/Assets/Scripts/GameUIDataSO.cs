using System;
using System.Runtime.InteropServices;
using UnityEngine;

[CreateAssetMenu(fileName = "GameUIData.asset", menuName = "Scriptable Objects/Game UI Data")]
public class GameUIDataSO : ScriptableObject
{
    [SerializeField] private float _resultScreenDelay = 3f;

    public Action<float> OnFireRateUpdated;
    public Action<int> OnDamageUpdated, OnHealthUpdated;

    public float ResultScreenDelay => _resultScreenDelay;

    public void ClearResources()
    {
        OnFireRateUpdated= null;
        OnDamageUpdated= null;
        OnHealthUpdated= null;
    }
}
