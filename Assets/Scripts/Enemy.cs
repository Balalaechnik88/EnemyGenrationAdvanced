using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _defaultMovementSpeed = 3f;
    [SerializeField] private float _stoppingDistance = 0.1f;

    private Transform _currentTarget;
    private float _currentMovementSpeed;
    private bool _isInitialized;

    private void Awake()
    {
        _currentMovementSpeed = _defaultMovementSpeed;
    }

    public void Initialize(Transform target, float movementSpeed)
    {
        _currentTarget = target;
        _currentMovementSpeed = movementSpeed;
        _isInitialized = true;
    }

    private void Update()
    {
        if (_isInitialized == false || _currentTarget == null)
            return;

        Vector3 directionToTarget = _currentTarget.position - transform.position;

        float squaredDistanceToTarget = directionToTarget.sqrMagnitude;
        float squaredStoppingDistance = _stoppingDistance * _stoppingDistance;

        if (squaredDistanceToTarget <= squaredStoppingDistance)
            return;

        Vector3 normalizedDirection = directionToTarget.normalized;
        transform.position += normalizedDirection * _currentMovementSpeed * Time.deltaTime;
    }
}