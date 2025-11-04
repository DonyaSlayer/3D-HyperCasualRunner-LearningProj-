using System.Collections;
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
        if (randomPrefab.gameObject.tag == "Enemy") 
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


    //ЗАЛИШИТИ
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
    IEnumerator EnemySpawnCoroutine(GameObject randomPrefab, Transform randomPoint) 
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
}



