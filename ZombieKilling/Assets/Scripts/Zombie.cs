using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private ZombieAnimator _zombieAnimator;
    [SerializeField] private float _walkSpeed, _runSpeed;
    [SerializeField] private float _patrolDestinationRadius;
    [SerializeField] private Vector2 _minMaxPatrolRerouteTime;

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

    private bool GetRandomNavmeshDestination(out Vector3 destinationPoint)
    {
        Vector3 sourcePosition = transform.position + Random.insideUnitSphere * _patrolDestinationRadius;
        if(NavMesh.SamplePosition(sourcePosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            destinationPoint = hit.position;
            return true;
        }
        destinationPoint = Vector3.zero;
        return false;
    }

    IEnumerator DetectPlayer()
    {
        while (SingletonManager.Instance.Player == null)
        {
            yield return null;
        }
        _playerFound = true;
        StartCoroutine(SetPatrolRoute());
    }

    IEnumerator SetPatrolRoute()
    {
        bool canStopSearching = false;
        Vector3 patrolDestination = Vector3.zero;
        while(!canStopSearching)
        {
            canStopSearching = GetRandomNavmeshDestination(out patrolDestination);
            yield return null;
        }
        Debug.DrawRay(patrolDestination, Vector3.up, Color.red, 10f);
        _navMeshAgent.SetDestination(patrolDestination);
        float reRouteTime = Time.time + Random.Range(_minMaxPatrolRerouteTime.x, _minMaxPatrolRerouteTime.y);
        yield return new WaitUntil(()=>Time.time > reRouteTime);
        StartCoroutine(SetPatrolRoute());
    }
}
