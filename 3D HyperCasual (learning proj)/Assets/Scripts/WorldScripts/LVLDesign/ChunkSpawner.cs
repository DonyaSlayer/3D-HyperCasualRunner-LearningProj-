
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{

    [Header("Chunk Settings")]
    [SerializeField] private GameObject _chunkPrefab; //ЗАЛИШИТИ
    public int chunksCountOnStart; //ЗАЛИШИТИ
    public float chunkLenght; //ЗАЛИШИТИ



    private void Start() //ЗАЛИШИТИ
    {
        for (int i = 0; i < chunksCountOnStart; i++)
        { 
            
            SpawnChunk(new Vector3 (0,-2,chunkLenght * i));
        }

    }
    public void StepOnChunk(Chunk currentChunk) //ЗАЛИШИТИ
    {
        SpawnChunk(currentChunk.transform.position + (Vector3.forward * chunksCountOnStart * chunkLenght));
    }

    public void SpawnChunk(Vector3 spawnPosition) //ЗАЛИШИТИ (DO IF UMOV)
    {
        Chunk newChunk = Instantiate(_chunkPrefab, spawnPosition, Quaternion.identity, transform).GetComponent<Chunk>();
        newChunk.chunkSpawner = this;
    }
}

