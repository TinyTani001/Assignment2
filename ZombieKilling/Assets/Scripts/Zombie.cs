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
    [SerializeField] private float _patrolDestinationRadius, _timeToHoldPositionAfterKillingPlayer;
    [SerializeField] private Vector2 _minMaxRerouteTime;
    [SerializeField, Range(0f, 1f)] private float _playerDetectionRange = 0.2f;
    [SerializeField] private PlayerDataSO _playerData;

    private bool _playerFound, _chasingPlayer, _playerInSight, _weKilledPlayer;

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
        if (_playerFound)
        {
            EvaluateAgentState();

            if(_playerData.IsPlayerDead) return;

            Vector3 zombiePosition = transform.position;

            Vector3 flatPlayerPosition = _playerData.Player.transform.position;
            flatPlayerPosition.y = zombiePosition.y;
            Vector3 zombieToPlayerDirection = (flatPlayerPosition - zombiePosition).normalized;
            float playerZombieSqrdDistance = (zombiePosition - flatPlayerPosition).sqrMagnitude;
            float dot = Vector3.Dot(transform.forward, zombieToPlayerDirection);
            if (dot >= _playerDetectionDotProduct && playerZombieSqrdDistance < _patrolDestinationRadius * _patrolDestinationRadius)
            {
                _chasingPlayer = true;
                if (playerZombieSqrdDistance <= _navMeshAgent.stoppingDistance * _navMeshAgent.stoppingDistance)
                {
                    _playerInSight = true;
                }
            }
            else _playerInSight = false;
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

    public void OnHitConnected()
    {
        if (_playerInSight)
        {
            _playerData.Player.HitPlayer();
            if (_playerData.CurrentPlayerHealth == 0) _weKilledPlayer = true;
        }
    }

    private void EvaluateAgentState()
    {
        int oldState = _agentState;
        if (!_chasingPlayer)
        {
            if (_navMeshAgent.velocity.sqrMagnitude < 0.0001f) _agentState = 0;
            if (_navMeshAgent.velocity.sqrMagnitude > 0f) _agentState = 1;
        }
        else
        {
            if (_navMeshAgent.velocity.sqrMagnitude < 0.0001f) _agentState = 3;
            if (_navMeshAgent.velocity.sqrMagnitude > 0f) _agentState = 2;
        }
        if (oldState != _agentState)
        {
            if (_agentState >= 2) _navMeshAgent.speed = _runSpeed;
            else _navMeshAgent.speed = _walkSpeed;
            _zombieAnimator.OnAgentStateUpdated(_agentState);
        }
    }

    private bool GetRandomNavmeshDestination(ref Vector3 destinationPoint)
    {
        Vector3 sourcePosition = transform.position + Random.insideUnitSphere * _patrolDestinationRadius;
        if (NavMesh.SamplePosition(sourcePosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            destinationPoint = hit.position;
            return true;
        }
        destinationPoint = Vector3.zero;
        return false;
    }

    IEnumerator DetectPlayer()
    {
        while (_playerData.Player == null)
        {
            yield return null;
        }
        _playerFound = true;
        StartCoroutine(GotoPlayer());
    }

    IEnumerator GotoPlayer()
    {
        _navMeshAgent.SetDestination(_playerData.Player.transform.position);
        float reRouteTime = Time.time + (!_chasingPlayer ? Random.Range(_minMaxRerouteTime.x, _minMaxRerouteTime.y) : 1f);
        yield return new WaitUntil(() => Time.time > reRouteTime || _playerData.IsPlayerDead);
        if (_playerData.IsPlayerDead)
        {
            _chasingPlayer = false;
            _playerInSight = false;
            if (_weKilledPlayer) yield return new WaitForSeconds(_timeToHoldPositionAfterKillingPlayer);
            StartCoroutine(TakeRandomPath());
        }
        else
            StartCoroutine(GotoPlayer());
    }

    IEnumerator TakeRandomPath()
    {
        Vector3 destination = Vector3.zero;
        while (!GetRandomNavmeshDestination(ref destination)) { yield return null; }
        _navMeshAgent.SetDestination(destination);
        float reRouteTime = Time.time + Random.Range(_minMaxRerouteTime.x, _minMaxRerouteTime.y);
        yield return new WaitUntil(() => Time.time > reRouteTime);
        StartCoroutine(TakeRandomPath());
    }
}
