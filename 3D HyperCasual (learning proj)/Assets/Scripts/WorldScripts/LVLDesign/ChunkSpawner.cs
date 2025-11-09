using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{

    [Header("Chunk Settings")]

    [SerializeField] private GameObject _chunkPrefab; 
    public int chunksCountOnStart; 
    public float chunkLenght;
    public int initialEmptyChunkCount = 15; //=======Empty chunks on the start=======

    [Header("Wave Control")]
    [Tooltip("Total chunks that has spawned or created count/current wave count/ chunks per wave count")]

    public int totalChunksSpawned = 0; 
    public int currentWave = 1;
    public int _chunksPerWave = 10;
    public int totalChunksCreated = 0;


    [Header("Wave Item Limits")]
    [Tooltip("Max arches and boxes, that can be created per wave")]

    [SerializeField] private int _waveMaxArches = 5;
    [SerializeField] private int _waveMaxBoxes = 3;

    private int _currentWaveArchesCount = 0;
    private int _currentWaveBoxesCount = 0;

    [Header("Enemy Prefabs")]
    [Tooltip("Prefabs of shooting and default enemies / wave for shooting enemies spawning from (with the getters for them)")]

    [SerializeField] private GameObject[] _meleeEnemyPrefabs;
    [SerializeField] private GameObject[] _shootingEnemyPrefabs; 
    [SerializeField] private int _shootingEnemyStartWave = 10;

    //=======Getters for references=======
    public GameObject[] GetMeleePrefabs() => _meleeEnemyPrefabs;
    public GameObject[] GetShootingPrefabs() => _shootingEnemyPrefabs;
    public int GetShootingStartWave() => _shootingEnemyStartWave;


    //=======Creation of the start chunks=======
    private void Start() 
    {
        for (int i = 0; i < chunksCountOnStart; i++)
        { 
            
            SpawnChunk(new Vector3 (0,-2,chunkLenght * i));
        }

    }

    //=======Creation of the new chunks  + totalChunksSpawning counting + refreshing the Wawe count (COMPLINED WITH PROGRESSION LOGIC) + Ignoring of the start chunks=======
    public void StepOnChunk(Chunk currentChunk) 
    {
        SpawnChunk(currentChunk.transform.position + (Vector3.forward * chunksCountOnStart * chunkLenght));
        totalChunksSpawned++;
        int waveProgressCount = totalChunksSpawned - initialEmptyChunkCount;
        if (waveProgressCount > 0 && (waveProgressCount / _chunksPerWave) >= currentWave)
        {
            currentWave++;
            _currentWaveArchesCount = 0;
            _currentWaveBoxesCount = 0;
            Debug.Log($"Wave {currentWave - 1} completed! Starting Wave {currentWave}!");
        }
    }

    //=======Checking if the arch can be spawned=======
    public bool CanSpawnArch()
    {
        return _currentWaveArchesCount < _waveMaxArches;
    }

    //=======Rising the arch count per wave=======
    public void IncrementArchCount()
    {
        _currentWaveArchesCount++;
    }
    //=======Checking if the boxes can be spawned=======
    public bool CanSpawnBox()
    {
        return _currentWaveBoxesCount < _waveMaxBoxes;
    }
    //=======Rising the boxes count per wave=======
    public void IncrementBoxCount()
    {
        _currentWaveBoxesCount++;
    }
    //=======Creating new =======
    public void SpawnChunk(Vector3 spawnPosition) 
    {
        Chunk newChunk = Instantiate(_chunkPrefab, spawnPosition, Quaternion.identity, transform).GetComponent<Chunk>();
        newChunk.chunkSpawner = this;
        totalChunksCreated++;
    }
}
