using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Zombie : MonoBehaviour
{
    [SerializeField] private bool _showDebugGizmos;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private ZombieAnimator _zombieAnimator;
    [SerializeField] private float _walkSpeed, _runSpeed;
    [SerializeField] private float _patrolDestinationRadius;
    [SerializeField] private Vector2 _minMaxPatrolRerouteTime;
    [SerializeField, Range(0f, 1f)] private float _playerDetectionRange = 0.2f;

    private bool _playerFound;

    private int _agentState;
    private float _playerDetectionDotProduct;

    private void Start()
    {
        StartCoroutine(DetectPlayer());
        _navMeshAgent.speed = _walkSpeed;
        _playerDetectionDotProduct = Vector3.Dot(transform.forward, Quaternion.AngleAxis(Mathf.Lerp(0f, 180f, _playerDetectionRange), Vector3.up) * transform.forward);
    }

    private void FixedUpdate()
    {
        if(_playerFound)
        {
            EvaluateAgentState();

            Vector3 zombiePosition = transform.position;

            Vector3 flatPlayerPosition =  SingletonManager.Instance.Player.transform.position;
            flatPlayerPosition.y = zombiePosition.y;
            Vector3 zombieToPlayerDirection = (flatPlayerPosition - zombiePosition).normalized;
            Debug.DrawRay(transform.position, transform.forward, Color.red);
            Debug.DrawRay(transform.position, zombieToPlayerDirection, Color.green);
            float dot = Vector3.Dot(transform.forward, zombieToPlayerDirection);
            if (dot >= _playerDetectionDotProduct && (zombiePosition - flatPlayerPosition).sqrMagnitude < _patrolDestinationRadius * _patrolDestinationRadius)
            {
                Debug.Log("Player detected");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!_showDebugGizmos) return;
        Gizmos.color = Color.green;
        Vector3 dir = transform.forward;
        Vector3 startPosition = transform.position;
        Gizmos.DrawLine(startPosition, startPosition + (Quaternion.AngleAxis(Mathf.Lerp(0f, 180f, _playerDetectionRange), Vector3.up) * dir * _patrolDestinationRadius));
        Gizmos.DrawLine(startPosition, startPosition + (Quaternion.AngleAxis(Mathf.Lerp(360f, 180f, _playerDetectionRange), Vector3.up) * dir * _patrolDestinationRadius));
        Vector3 origin = transform.position;
        Vector3 startRotation = transform.right * _patrolDestinationRadius;
        Vector3 lastPosition = origin + startRotation;
        float angle = 0;
        while (angle <= 360)
        {
            angle += 360 / 20;
            Vector3 nextPosition = origin + (Quaternion.Euler(0, angle, 0) * startRotation);
            Gizmos.DrawLine(lastPosition, nextPosition);

            lastPosition = nextPosition;
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
        //StartCoroutine(SetPatrolRoute());
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
        _navMeshAgent.SetDestination(patrolDestination);
        float reRouteTime = Time.time + Random.Range(_minMaxPatrolRerouteTime.x, _minMaxPatrolRerouteTime.y);
        yield return new WaitUntil(()=>Time.time > reRouteTime);
        StartCoroutine(SetPatrolRoute());
    }
}
