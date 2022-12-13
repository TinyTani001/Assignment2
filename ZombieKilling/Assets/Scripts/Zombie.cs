using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private ZombieAnimator _zombieAnimator;
    [SerializeField] private float _walkSpeed, _runSpeed;

    private bool _playerFound;

    private int _agentState;

    private void Start()
    {
        StartCoroutine(DetectPlayer());
        _navMeshAgent.speed = _walkSpeed;
    }

    private void FixedUpdate()
    {
        if(_playerFound)
        {
            _navMeshAgent.SetDestination(SingletonManager.Instance.Player.transform.position);
            EvaluateAgentState();
        }
    }

    private void EvaluateAgentState()
    {
        int oldState = _agentState;
        if (_navMeshAgent.velocity.sqrMagnitude < 0.0001f) _agentState = 0;
        if (_navMeshAgent.velocity.sqrMagnitude > 0f) _agentState = 1;
        if(oldState != _agentState) { _zombieAnimator.OnAgentStateUpdated(_agentState); }
    }

    IEnumerator DetectPlayer()
    {
        while (SingletonManager.Instance.Player == null)
        {
            yield return null;
        }
        _playerFound = true;
    }
}
