using UnityEngine;
using UnityEngine.Events;

public class ZombieAnimator : AnimationEventReceiver
{
    [SerializeField] private Animator _animator;
    [SerializeField] private UnityEvent _onHitConnected, _onHitReactionCompleted;

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
                _onHitConnected.Invoke();
                break;
            case "HitCompleted":
                RandomizeHitAnimation();
                break;
            case "HitReactionCompleted":
                _onHitReactionCompleted.Invoke();
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

    public void HitReceived()
    {
        _animator.SetTrigger("Hit");
    }

    public void OnDeath()
    {
        _animator.SetTrigger("Dead");
    }
}
