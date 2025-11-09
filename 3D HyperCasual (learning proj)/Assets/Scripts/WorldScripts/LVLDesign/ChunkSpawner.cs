using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{

    [Header("Chunk Settings")]
    [SerializeField] private GameObject _chunkPrefab; 
    public int chunksCountOnStart; 
    public float chunkLenght;
    public int initialEmptyChunkCount = 20;

    [Header("Wave Control")]
    [Tooltip("Загальна кількість чанків, що з'явилися")]
    public int totalChunksSpawned = 0; 
    [Tooltip("Поточна хвиля ворогів")]
    public int currentWave = 1;
    [Tooltip("Чанків між хвилями. Після досягнення, хвиля збільшується.")]
    public int _chunksPerWave = 10;
    public int totalChunksCreated = 0;


    [Header("Wave Item Limits")]
    [Tooltip("Максимальна кількість арок, які можуть з'явитися в межах однієї хвилі.")]
    [SerializeField] private int _waveMaxArches = 5; 
    [Tooltip("Максимальна кількість ящиків, які можуть з'явитися в межах однієї хвилі.")]
    [SerializeField] private int _waveMaxBoxes = 3;

    private int _currentWaveArchesCount = 0;
    private int _currentWaveBoxesCount = 0;

    [Header("Enemy Prefabs")]
    [Tooltip("Префаби ЗВИЧАЙНИХ (ближній бій) ворогів.")]
    [SerializeField] private GameObject[] _meleeEnemyPrefabs;
    [Tooltip("Префаби СТРІЛЯЮЧИХ (дальній бій) ворогів.")]
    [SerializeField] private GameObject[] _shootingEnemyPrefabs; 
    [Tooltip("Хвиля, з якої починають спавнитися стріляючі вороги.")]
    [SerializeField] private int _shootingEnemyStartWave = 10; 
    public GameObject[] GetMeleePrefabs() => _meleeEnemyPrefabs;
    public GameObject[] GetShootingPrefabs() => _shootingEnemyPrefabs;
    public int GetShootingStartWave() => _shootingEnemyStartWave;



    private void Start() 
    {
        for (int i = 0; i < chunksCountOnStart; i++)
        { 
            
            SpawnChunk(new Vector3 (0,-2,chunkLenght * i));
        }

    }
    public void StepOnChunk(Chunk currentChunk) 
    {
        SpawnChunk(currentChunk.transform.position + (Vector3.forward * chunksCountOnStart * chunkLenght));
        totalChunksSpawned++;
        int waveProgressCount = totalChunksSpawned - initialEmptyChunkCount;
        if (waveProgressCount > 0 && (waveProgressCount / _chunksPerWave) >= currentWave)
        {
            currentWave++;
            Debug.Log($"Wave {currentWave - 1} completed! Starting Wave {currentWave}!");
        }
    }
    public bool CanSpawnArch()
    {
        return _currentWaveArchesCount < _waveMaxArches;
    }

    public void IncrementArchCount()
    {
        _currentWaveArchesCount++;
    }

    public bool CanSpawnBox()
    {
        return _currentWaveBoxesCount < _waveMaxBoxes;
    }

    public void IncrementBoxCount()
    {
        _currentWaveBoxesCount++;
    }
    public void SpawnChunk(Vector3 spawnPosition) 
    {
        Chunk newChunk = Instantiate(_chunkPrefab, spawnPosition, Quaternion.identity, transform).GetComponent<Chunk>();
        newChunk.chunkSpawner = this;
        totalChunksCreated++;
    }
}
