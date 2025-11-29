using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [Header("Enemy Type For This Spawn Point")]
    [SerializeField] private Enemy _enemyPrefabForThisPoint;
    [SerializeField] private float _enemyMovementSpeed = 3f;

    [Header("Target For This Spawn Point")]
    [SerializeField] private Transform _assignedTarget;

    [Header("Gizmos")]
    [SerializeField] private Color _gizmoColor = Color.green;
    [SerializeField] private float _gizmoRadius = 0.25f;
    [SerializeField] private Color _directionLineColor = Color.cyan;

    public bool CanSpawn => _enemyPrefabForThisPoint != null && _assignedTarget != null;

    public Enemy SpawnEnemy()
    {
        if (CanSpawn == false)
        {
            Debug.LogWarning($"EnemySpawnPoint [{name}]: нельз€ заспавнить врага Ч не назначен префаб или цель.");
            return null;
        }

        Vector3 spawnPosition = transform.position;
        Quaternion spawnRotation = transform.rotation;

        Enemy enemyInstance =
            Object.Instantiate(_enemyPrefabForThisPoint, spawnPosition, spawnRotation);

        enemyInstance.Initialize(_assignedTarget, _enemyMovementSpeed);

        return enemyInstance;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawSphere(transform.position, _gizmoRadius);

        if (_assignedTarget != null)
        {
            Gizmos.color = _directionLineColor;
            Gizmos.DrawLine(transform.position, _assignedTarget.position);
        }
    }
}