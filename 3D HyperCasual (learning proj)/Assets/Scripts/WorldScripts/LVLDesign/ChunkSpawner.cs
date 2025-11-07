
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{

    [Header("Chunk Settings")]
    [SerializeField] private GameObject _chunkPrefab; 
    public int chunksCountOnStart; 
    public float chunkLenght; 



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
    }

    public void SpawnChunk(Vector3 spawnPosition) 
    {
        Chunk newChunk = Instantiate(_chunkPrefab, spawnPosition, Quaternion.identity, transform).GetComponent<Chunk>();
        newChunk.chunkSpawner = this;
    }
}

