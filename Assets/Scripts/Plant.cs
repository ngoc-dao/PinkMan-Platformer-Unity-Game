using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class Plant : Enemy
{
    public float playerThrustY; // 20
    public float playerThrustX;
    private bool isHit;

    Rigidbody2D playerRB;
    Animator animator;
    public HitPoints hitPoints;
    Coroutine delayCoroutine = null;
    public GameObject bulletPrefab;
    Coroutine shootCoroutine;
    public Transform shootPoint;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(ShootBullets());
    }

    private void Update()
    {
        if (isHit && delayCoroutine == null)
        {
            delayCoroutine = StartCoroutine(Delay(3));
        }
        else if (delayCoroutine == null)
        {
            animator.SetInteger("idle", 2);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Checks if player collider on enemy from top, if so, kills enemy and plays animation
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRB = collision.gameObject.GetComponent<Rigidbody2D>();

            hitPoints.playerIsHit = true;
            animator.SetTrigger("spikesOut");
            animator.SetInteger("idle", 1);

            bool hitFromRight = playerRB.gameObject.transform.position.x >= transform.position.x;
            bool hitFromLeft = playerRB.gameObject.transform.position.x < transform.position.x;
            bool hitFromAbove = playerRB.gameObject.transform.position.y + 0.1f >= transform.position.y;
            bool hitFromBelow = playerRB.gameObject.transform.position.y + 0.1f < transform.position.y;

            if (hitFromRight && hitFromAbove)
            {
                playerRB.velocity = new Vector2(playerThrustX, playerThrustY / 2);
            }
            else if (hitFromLeft && hitFromAbove)
            {
                playerRB.velocity = new Vector2(-playerThrustX, playerThrustY / 2);
            }
            else if (hitFromRight && hitFromBelow)
            {
                playerRB.velocity = new Vector2(playerThrustX, -playerThrustY / 2);
            }
            else
            {
                playerRB.velocity = new Vector2(-playerThrustX, -playerThrustY / 2);
            }
        }
    }

    public IEnumerator ShootBullets()
    {
        while (true)
        {
            if (shootCoroutine == null)
            {
                shootCoroutine = StartCoroutine(Shoot());
                animator.SetTrigger("shoot");
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator Delay(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        isHit = false;
        delayCoroutine = null;
    }

    public IEnumerator Shoot()
    {
        Instantiate(bulletPrefab, shootPoint.position, transform.rotation);
        yield return new WaitForSeconds(Random.Range(0.5f, 1f));
        shootCoroutine = null;
    }
}
