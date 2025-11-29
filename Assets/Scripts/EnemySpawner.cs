using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private EnemySpawnPoint[] _enemySpawnPoints;

    [Header("Spawn Timing")]
    [SerializeField] private float _spawnIntervalSeconds = 2f;

    private Coroutine _spawnRoutine;

    private void Awake()
    {
        if (_enemySpawnPoints == null || _enemySpawnPoints.Length == 0)
        {
            Debug.LogError("EnemySpawner: массив точек спавна пуст!");
        }

        if (_spawnIntervalSeconds <= 0f)
        {
            _spawnIntervalSeconds = 2f;
        }
    }

    private void OnEnable()
    {
        if (_spawnRoutine == null &&
            _enemySpawnPoints != null &&
            _enemySpawnPoints.Length > 0)
        {
            _spawnRoutine = StartCoroutine(SpawnLoop());
        }
    }

    private void OnDisable()
    {
        if (_spawnRoutine != null)
        {
            StopCoroutine(_spawnRoutine);
            _spawnRoutine = null;
        }
    }

    private IEnumerator SpawnLoop()
    {
        var spawnDelay = new WaitForSeconds(_spawnIntervalSeconds);

        while (enabled)
        {
            SpawnEnemyFromRandomPoint();
            yield return spawnDelay;
        }
    }

    private void SpawnEnemyFromRandomPoint()
    {
        if (_enemySpawnPoints == null || _enemySpawnPoints.Length == 0)
            return;

        int randomIndex = Random.Range(0, _enemySpawnPoints.Length);
        EnemySpawnPoint selectedSpawnPoint = _enemySpawnPoints[randomIndex];

        if (selectedSpawnPoint.CanSpawn == false)
            return;

        selectedSpawnPoint.SpawnEnemy();
    }
}