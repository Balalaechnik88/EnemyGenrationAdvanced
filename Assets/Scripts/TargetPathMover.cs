using System.Collections;
using UnityEngine;

public class TargetPathMover : MonoBehaviour
{
    [Header("Path Settings")]
    [SerializeField] private Transform[] _pathWaypoints;

    [Header("Movement Settings")]
    [SerializeField] private float _movementSpeed = 2f;
    [SerializeField] private float _waypointReachDistance = 0.1f;
    [SerializeField] private bool _isLooping = true;

    [Header("Gizmos Settings")]
    [SerializeField] private Color _waypointColor = Color.yellow;
    [SerializeField] private Color _pathLineColor = Color.yellow;
    [SerializeField] private float _waypointGizmoRadius = 0.15f;

    private Coroutine _movementRoutine;

    private void OnEnable()
    {
        if (_pathWaypoints == null || _pathWaypoints.Length == 0)
        {
            Debug.LogWarning($"TargetPathMover [{name}]: не заданы точки маршрута.");
            return;
        }

        _movementRoutine = StartCoroutine(MoveAlongPath());
    }

    private void OnDisable()
    {
        if (_movementRoutine != null)
        {
            StopCoroutine(_movementRoutine);
            _movementRoutine = null;
        }
    }

    private IEnumerator MoveAlongPath()
    {
        if (_pathWaypoints.Length == 0)
            yield break;

        int currentWaypointIndex = 0;
        float squaredWaypointReachDistance = _waypointReachDistance * _waypointReachDistance;

        while (enabled)
        {
            Transform currentWaypoint = _pathWaypoints[currentWaypointIndex];

            if (currentWaypoint == null)
            {
                Debug.LogWarning($"TargetPathMover [{name}]: waypoint с индексом {currentWaypointIndex} не назначен.");
                yield break;
            }

            Vector3 targetPosition = currentWaypoint.position;

            while (enabled)
            {
                Vector3 directionToWaypoint = targetPosition - transform.position;
                float squaredDistanceToWaypoint = directionToWaypoint.sqrMagnitude;

                if (squaredDistanceToWaypoint <= squaredWaypointReachDistance)
                    break;

                Vector3 movementDirection = directionToWaypoint.normalized;
                transform.position += movementDirection * _movementSpeed * Time.deltaTime;

                yield return null;
            }

            currentWaypointIndex++;

            if (currentWaypointIndex >= _pathWaypoints.Length)
            {
                if (_isLooping)
                {
                    currentWaypointIndex = 0;
                }
                else
                {
                    yield break;
                }
            }

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if (_pathWaypoints == null || _pathWaypoints.Length == 0)
            return;

        Gizmos.color = _waypointColor;

        for (int i = 0; i < _pathWaypoints.Length; i++)
        {
            Transform waypoint = _pathWaypoints[i];

            if (waypoint == null)
                continue;

            Gizmos.DrawSphere(waypoint.position, _waypointGizmoRadius);
        }

        Gizmos.color = _pathLineColor;

        for (int i = 0; i < _pathWaypoints.Length - 1; i++)
        {
            Transform from = _pathWaypoints[i];
            Transform to = _pathWaypoints[i + 1];

            if (from == null || to == null)
                continue;

            Gizmos.DrawLine(from.position, to.position);
        }

        if (_isLooping && _pathWaypoints.Length > 1)
        {
            Transform first = _pathWaypoints[0];
            Transform last = _pathWaypoints[_pathWaypoints.Length - 1];

            if (first != null && last != null)
            {
                Gizmos.DrawLine(last.position, first.position);
            }
        }
    }
}