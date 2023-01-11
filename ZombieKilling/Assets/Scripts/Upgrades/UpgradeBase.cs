using UnityEngine;

public abstract class UpgradeBase : ScriptableObject
{
    public abstract void ActivateUpgrade();
    public abstract void DeactivateUpgrade();
}
