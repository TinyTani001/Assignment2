using UnityEngine;

public class ZombieAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void OnAgentStateUpdated(int stateIndex)
    {
        if (stateIndex == 0)
        {
            SetIdleVariation(Random.Range(1, 3));
            SetWalk(false);
        }
        else if (stateIndex == 1)
        {
            SetIdleVariation(0f);
            SetWalk(true);
        }
    }

    private void SetIdleVariation(float variation)
    {
        _animator.SetFloat("IdleVariation", variation);
    }

    private void SetWalk(bool toValue)
    {
        _animator.SetBool("Walking", toValue);
    }
}
