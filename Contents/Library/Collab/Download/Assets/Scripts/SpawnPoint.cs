using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject prefabToSpawn;

    void Start()
    {
        // Prevents two copies of player to spawn. Player is spawned through GameManager.
        if (!prefabToSpawn.CompareTag("Player"))
        {
            SpawnObject();
        }
    }

    public GameObject SpawnObject()
    {
        if (prefabToSpawn != null)
        {
            return Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        }
        return null;
    }
}
