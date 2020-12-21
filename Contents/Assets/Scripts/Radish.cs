using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class Radish : Enemy
{
    public float playerThrustY; // 20
    public float playerThrustX;
    public float walkSpeed;
    public float pauseTime;
    public bool canWander = true;

    public Transform topCheck;
    public float topCheckRadius; // 0.4
    public LayerMask topCheckLayer; // Player
    public bool hitFromAbove;


    Rigidbody2D rb2d;
    Rigidbody2D playerRB;
    Coroutine pauseCoroutine;
    Animator animator;
    public GameObject disappearPrefab;
    public HitPoints hitPoints;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        animator.SetFloat("speed", Mathf.Abs(rb2d.velocity.x));
    }

    void FixedUpdate()
    {
        hitFromAbove = Physics2D.OverlapCircle(topCheck.position, topCheckRadius, topCheckLayer);

        if (canWander && pauseCoroutine == null)
        {
            rb2d.velocity = new Vector2(walkSpeed, rb2d.velocity.y);
        }
        else
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }
        
        SetFacing();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Checks if player collider on enemy from top, if so, kills enemy and plays animation
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRB = collision.gameObject.GetComponent<Rigidbody2D>();

            if (hitFromAbove)
            {
                print("Hit!");
                if (playerRB != null)
                {
                    playerRB.velocity = Vector2.up * playerThrustY;
                }
                Destroy(gameObject);
                Instantiate(disappearPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                hitPoints.playerIsHit = true;

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
                    playerRB.velocity = new Vector2(-playerThrustX,-playerThrustY / 2);
                }
                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Endpoint"))
        {
            if (canWander && pauseCoroutine == null)
            {
                pauseCoroutine = StartCoroutine(PauseAndTurn(pauseTime));
            }
        }
    }

    private IEnumerator PauseAndTurn(float time)
    {
        rb2d.velocity = new Vector2(0, rb2d.velocity.y);

        // Random.Range returns int inclusive of min & exclusive of max
        animator.SetInteger("idle", Random.Range(1, 3));

        yield return new WaitForSeconds(time);
        walkSpeed *= -1;
        pauseCoroutine = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(topCheck.position, topCheckRadius);
    }

    private void SetFacing()
    {
        if (walkSpeed > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else if (walkSpeed < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }
}
