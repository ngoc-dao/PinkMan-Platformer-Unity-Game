using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public HitPoints hitPoints;
    private GameObject spawnedObject;

    void Start()
    {
        // Prevents two copies of player to spawn. Player is spawned through GameManager.
        if (!prefabToSpawn.CompareTag("Player"))
        {
            SpawnObject();
        }
    }

    private void Update()
    {
        if (hitPoints.instantDeath || hitPoints.value <= 0)
        {
            if (!prefabToSpawn.CompareTag("Player"))
            {
                Destroy(spawnedObject);
                SpawnObject();
            }
        }
    }

    public GameObject SpawnObject()
    {
        if (prefabToSpawn != null)
        {
            spawnedObject = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
            return spawnedObject;
        }
        return null;
    }
}
