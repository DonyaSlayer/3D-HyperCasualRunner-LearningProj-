using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    //=======Inicialization of TRANSFORMS to spawn if they are nulls + calling the main spawning methods=======
    private void Start()
    {
        if (_pointsToSpawn == null || _pointsToSpawn.Length == 0)
        {
            InitializeSpawnPoints();
        }
        SpawnObjectsInChunk();
        SpawnCoins(); 
    }

    //=======Void for Inicialization of TRANSFORMS to spawn=======
    private void InitializeSpawnPoints()
    {
        _pointsToSpawn = GetComponentsInChildren<Transform>()
            .Where(t => t != transform) 
            .ToArray();
    }

    //=======Void for spawning ALL OF OBJECTS (enemies, arches, boxes). Realized by 50/50 chaces=======
    private void SpawnObjectsInChunk()
    {
        if (chunkSpawner.totalChunksCreated <= chunkSpawner.initialEmptyChunkCount)
        {
            return;
        }
        Transform randomPoint = _pointsToSpawn[Random.Range(0, _pointsToSpawn.Length)];

        //=======Checking of possibility to spawn arches or boxes=======
        bool canSpawnBuff = chunkSpawner.CanSpawnArch() || chunkSpawner.CanSpawnBox();
        SpawnType type;
        if (canSpawnBuff)
        {
            type = (Random.Range(0, 2) == 0) ? SpawnType.Enemy : SpawnType.BuffOrDebuff;
        }
        else
        {
            type = SpawnType.Enemy;
        }


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


    //=======Logic of enemy spawn (realization through the getters in ChunkSpawner)=======
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


    //=======Spawn logic of Archs and Boxes (realization through the getters + voids in ChunkSpawner)=======
    private void SpawnBuffOrDebuffLogic(GameObject prefab, Transform spawnPoint)
    {
        if (prefab.CompareTag("Arch"))
        {
            if (chunkSpawner.CanSpawnArch())
            {
                GameObject archContainer = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation, transform);
                archContainer.GetComponent<ArchController>().ArchTargetChoosing();
                chunkSpawner.IncrementArchCount();
            }
        }

        else if (prefab.CompareTag("BlueBox") || prefab.CompareTag("GreenBox"))
        {
            if (chunkSpawner.CanSpawnBox())
            {
                Vector3 spawnPosition = spawnPoint.position + Vector3.down * 0.5f;
                Quaternion spawnRotation = Quaternion.Euler(0, 90f, 0);
                Instantiate(prefab, spawnPosition, spawnRotation, transform);
                chunkSpawner.IncrementBoxCount();
            }
        }
    }

    //=======Spawn logic of Coins=======
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


    //=======Coroutine for spawning Enemies and spawn delay beetwen the spawns(also here realized the delay for empty chunks)=======
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


