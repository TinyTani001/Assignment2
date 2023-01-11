using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameUIData.asset", menuName = "Scriptable Objects/Game UI Data")]
public class GameUIDataSO : ScriptableObject
{
    public Action<float> OnFireRateUpdated;
    public Action<int> OnDamageUpdated, OnHealthUpdated;

    public void ClearResources()
    {
        OnFireRateUpdated= null;
        OnDamageUpdated= null;
        OnHealthUpdated= null;
    }
}
