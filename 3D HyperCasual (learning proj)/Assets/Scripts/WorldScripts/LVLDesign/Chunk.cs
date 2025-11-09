/*using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Chunk : MonoBehaviour
{
    public ChunkSpawner chunkSpawner;
    [SerializeField] private Transform[] _pointsToSpawn;//ЗАЛИШИТИ
    [SerializeField] private Transform[] _coinSpawnPoints;//ЗАЛИШИТИ
    [SerializeField] private GameObject[] _prefabsToSpawn; //ЗАЛИШИТИ
    [SerializeField] private GameObject _coinPrefab;//ЗАЛИШИТИ



    [Header("Enemy Settings")]
    [SerializeField] private int _enemiesMinCount;//ЗАЛИШИТИ
    [SerializeField] private int _enemiesMaxCount;//ЗАЛИШИТИ
    [SerializeField] private float _enemySpawnTimer;//ЗАЛИШИТИ


    private void Start()
    {
        Transform randomPoint = _pointsToSpawn[Random.Range(0, _pointsToSpawn.Length)]; 
        GameObject randomPrefab = _prefabsToSpawn[Random.Range(0, _prefabsToSpawn.Length)]; 
        if (randomPrefab.gameObject.tag == "Enemy"|| randomPrefab.gameObject.tag == "Shooting Enemy") 
        { 
            StartCoroutine(EnemySpawnCoroutine(randomPrefab, randomPoint)); 
        } 
        else if (randomPrefab.gameObject.tag == "BlueBox"|| randomPrefab.gameObject.tag == "GreenBox")
        { 
            Instantiate(randomPrefab, randomPoint.position + Vector3.up * 0.5f, randomPoint.rotation, transform); 
        }
        else if (randomPrefab.gameObject.tag == "Arch")
        {
            Instantiate(randomPrefab, randomPoint.position, randomPoint.rotation, transform);
        }
        if (_pointsToSpawn == null || _pointsToSpawn.Length == 0)
            _pointsToSpawn = GetComponentsInChildren<Transform>();
        SpawnCoins();
    }


    
    private void SpawnCoins()
    {
        if (_coinSpawnPoints == null || _coinSpawnPoints.Length == 0 || _coinPrefab == null)
            return;

        foreach (Transform point in _coinSpawnPoints)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-1.8f, 1.8f), 0.5f, Random.Range(-1.8f, 1.8f));
            Instantiate(_coinPrefab, point.position + randomOffset, point.rotation);
        }
    }*/
//ЗАЛИШИТИ (ПОКИ ЯК КОМЕНТАР, ДЛЯ ПОДАЛЬШОЇ ЛОГІКИ)
/*private IEnumerator SpawnEnemies(Transform point)
{
    int count = Random.Range(_enemiesMinCount, _enemiesMaxCount);
    for (int i = 0; i < count; i++)
    {
        Instantiate(_enemyPrefab, point.position, point.rotation, transform);
        yield return new WaitForSeconds(_enemySpawnTimer);
    }
}*/
//ЗАЛИШИТИ
/*IEnumerator EnemySpawnCoroutine(GameObject randomPrefab, Transform randomPoint) 
{ 
    int randomCount = Random.Range(_enemiesMinCount, _enemiesMaxCount); 
    for (int i = 0; i < randomCount; i++) 
    { Instantiate(randomPrefab, randomPoint.position, randomPoint.rotation, transform); 
        yield return new WaitForSeconds(_enemySpawnTimer); 
    } 
}

//ЗАЛИШИТИ
public void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Player"))
    {
        chunkSpawner.StepOnChunk(this);
    }
}
}*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SpawnType { Enemy, BuffOrDebuff }

public class Chunk : MonoBehaviour
{
    public ChunkSpawner chunkSpawner;
    [SerializeField] private Transform[] _pointsToSpawn;
    [SerializeField] private Transform[] _coinSpawnPoints;
    [SerializeField] private GameObject[] _prefabsToSpawn;
    [SerializeField] private GameObject _coinPrefab;

    [Header("Enemy Settings")]
    [SerializeField] private int _enemiesBaseMinCount = 3;
    [SerializeField] private int _enemiesBaseMaxCount = 9;
    [SerializeField] private float _enemySpawnTimer;

    private void Start()
    {
        if (_pointsToSpawn == null || _pointsToSpawn.Length == 0)
        {
            InitializeSpawnPoints();
        }
        SpawnObjectsInChunk();
        SpawnCoins(); 
    }
    private void InitializeSpawnPoints()
    {
        _pointsToSpawn = GetComponentsInChildren<Transform>();
    }
    private void SpawnObjectsInChunk()
    {
        Transform randomPoint = _pointsToSpawn[Random.Range(0, _pointsToSpawn.Length)];
        SpawnType type = (Random.Range(0, 2) == 0) ? SpawnType.Enemy : SpawnType.BuffOrDebuff;
        switch (type)
        {
            case SpawnType.Enemy:
                
                SpawnEnemyLogic(randomPoint);
                break;

            case SpawnType.BuffOrDebuff:
                
                GameObject randomPrefab = _prefabsToSpawn[Random.Range(0, _prefabsToSpawn.Length)];
                SpawnBuffOrDebuffLogic(randomPrefab, randomPoint);
                break;
        }
    }
    private SpawnType DetermineSpawnType(GameObject prefab)
    {
        if (prefab.CompareTag("Enemy") || prefab.CompareTag("Shooting Enemy"))
        {
            return SpawnType.Enemy;
        }
        return SpawnType.BuffOrDebuff;
    }
    private void SpawnEnemyLogic(Transform spawnPoint)
    {
        GameObject[] availableEnemies;
        if (chunkSpawner.currentWave >= chunkSpawner.GetShootingStartWave())
        {
            List<GameObject> combinedList = new List<GameObject>();
            combinedList.AddRange(chunkSpawner.GetMeleePrefabs());
            combinedList.AddRange(chunkSpawner.GetShootingPrefabs());
            availableEnemies = combinedList.ToArray();
        }
        else
        {
            availableEnemies = chunkSpawner.GetMeleePrefabs();
        }

        GameObject enemyToSpawn = availableEnemies[Random.Range(0, availableEnemies.Length)];
        int progression = chunkSpawner.currentWave / 10;
        int enemiesMaxCount = _enemiesBaseMaxCount + progression;
        StartCoroutine(EnemySpawnCoroutine(enemyToSpawn, spawnPoint, enemiesMaxCount));
    }

    private void SpawnBuffOrDebuffLogic(GameObject prefab, Transform spawnPoint)
    {
        if (prefab.CompareTag("Arch"))
        {
            if (chunkSpawner.CanSpawnArch())
            {
                Instantiate(prefab, spawnPoint.position, spawnPoint.rotation, transform);
                chunkSpawner.IncrementArchCount();
            }
        }

        else if (prefab.CompareTag("BlueBox") || prefab.CompareTag("GreenBox"))
        {
            if (chunkSpawner.CanSpawnBox())
            {
                Instantiate(prefab, spawnPoint.position + Vector3.up * 0.5f, spawnPoint.rotation, transform);
                chunkSpawner.IncrementBoxCount();
            }
        }
    }

    private void SpawnCoins()
    {
        if (_coinSpawnPoints == null || _coinSpawnPoints.Length == 0 || _coinPrefab == null)
            return;

        foreach (Transform point in _coinSpawnPoints)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-1.8f, 1.8f), 0.5f, Random.Range(-1.8f, 1.8f));
            Instantiate(_coinPrefab, point.position + randomOffset, point.rotation);
        }
    }

    IEnumerator EnemySpawnCoroutine(GameObject randomPrefab, Transform randomPoint, int enemiesMaxCount)
    {
        int randomCount = Random.Range(_enemiesBaseMinCount, enemiesMaxCount);
        if (chunkSpawner.totalChunksSpawned > 0 && chunkSpawner.totalChunksSpawned % chunkSpawner._chunksPerWave < 5)
        {
            randomCount = 0; 
        }

        for (int i = 0; i < randomCount; i++)
        {
            Instantiate(randomPrefab, randomPoint.position, randomPoint.rotation, transform);
            yield return new WaitForSeconds(_enemySpawnTimer);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            chunkSpawner.StepOnChunk(this);
        }
    }
}


