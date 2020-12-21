using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class Turtle : Enemy
{
    public float playerThrustY; // 20
    public float playerThrustX;
    private bool isHit;

    Rigidbody2D playerRB;
    Animator animator;
    public HitPoints hitPoints;
    Coroutine delayCoroutine = null;

    void Start()
    {
        animator = GetComponent<Animator>();
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

    IEnumerator Delay(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        isHit = false;
        delayCoroutine = null;
    }
}
