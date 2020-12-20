using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public float speed; // 6
    Rigidbody2D rb;
    public HitPoints hitPoints;
    public Transform spawnPoint;
    float respawnDelay;
    public GameObject explosionPrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        StartCoroutine(BulletRespawn());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hitPoints.playerIsHit = true;
        }

        if (!collision.gameObject.CompareTag("CameraConfiner") && !collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }

    public IEnumerator BulletRespawn()
    {
        respawnDelay = Random.Range(1.0f, 3.0f);
        yield return new WaitForSeconds(respawnDelay);
        transform.position = spawnPoint.position;
        rb.velocity = transform.right * speed;
    }
}
