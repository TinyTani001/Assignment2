using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    [SerializeField] private AnimationEventReceiver _animationEventReceiver;
    public void OnAnimEvent(string eventID)
    {
        _animationEventReceiver.OnAnimEvent(eventID);
    }
}
