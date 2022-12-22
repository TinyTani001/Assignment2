using UnityEngine;

public class ZombieAnimator : AnimationEventReceiver
{
    [SerializeField] private Animator _animator;

    public void OnAgentStateUpdated(int stateIndex)
    {
        if (stateIndex == 0)
        {
            SetIdleVariation(Random.Range(1, 3));
            SetWalk(false);
            SetRun(false);
            SetHitAnimationVariation(0);
        }
        else if (stateIndex == 1)
        {
            SetIdleVariation(0f);
            SetWalk(true);
            SetRun(false);
            SetHitAnimationVariation(0);
        }else if(stateIndex == 2)
        {
            SetIdleVariation(0f);
            SetWalk(false);
            SetRun(true);
            SetHitAnimationVariation(0);
        }
        else if (stateIndex == 3)
        {
            SetIdleVariation(0f);
            SetWalk(false);
            SetRun(false);
            RandomizeHitAnimation();
        }
    }

    public override void OnAnimEvent(string eventID)
    {
        base.OnAnimEvent(eventID);
        switch(eventID)
        {
            case "Hit":
                break;
            case "HitCompleted":
                RandomizeHitAnimation();
                break;
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

    private void SetRun(bool toValue)
    {
        _animator.SetBool("Run", toValue);
    }

    private void RandomizeHitAnimation()
    {
        SetHitAnimationVariation(Random.Range(1, 4));
    }

    private void SetHitAnimationVariation(int toValue)
    {
        _animator.SetInteger("AttackVariation", toValue);
    }
}
