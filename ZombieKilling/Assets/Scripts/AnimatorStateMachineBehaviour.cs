using UnityEngine;

public class AnimatorStateMachineBehaviour : StateMachineBehaviour
{
    [SerializeField] private string _onStateEnterEvent, _onStateExitEvent;

    private AnimationEventHandler _animationEventHandler;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        if(_animationEventHandler == null) _animationEventHandler = animator.GetComponent<AnimationEventHandler>();
        if (!string.IsNullOrEmpty(_onStateEnterEvent)) _animationEventHandler.OnAnimEvent(_onStateEnterEvent);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        if(_animationEventHandler == null) _animationEventHandler = animator.GetComponent<AnimationEventHandler>();
        if (!string.IsNullOrEmpty(_onStateExitEvent)) _animationEventHandler.OnAnimEvent(_onStateExitEvent);
    }
}
