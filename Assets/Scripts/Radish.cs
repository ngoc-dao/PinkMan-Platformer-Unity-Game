using System.Collections;
using UnityEngine;

public class Radish : Enemy
{
    public Transform topCheck;
    public float topCheckRadius; // 0.4
    public LayerMask topCheckLayer; // Player
    public bool hitFromAbove;
    public float playerThrust; // 22
    public float walkSpeed;
    public float pauseTime;

    Rigidbody2D rb2d;
    Coroutine disappearCoroutine;
    Coroutine pauseCoroutine;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        hitFromAbove = Physics2D.OverlapCircle(topCheck.position, topCheckRadius, topCheckLayer);

        if (pauseCoroutine == null)
        {
            rb2d.velocity = new Vector2(walkSpeed, rb2d.velocity.y);
        }
        
        SetFacing();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Checks if player collider on enemy from top, if so, kills enemy and plays animation
        if (collision.gameObject.CompareTag("Player") && hitFromAbove)
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            print("Hit!");
            if (rb != null)
            {
                rb.velocity = Vector2.up * playerThrust;
            }

            animator.SetTrigger("disappear");
            if (disappearCoroutine == null)
            {
                disappearCoroutine = StartCoroutine(Disappear());
            }
        }               
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Endpoint"))
        {
            if (pauseCoroutine == null)
            {
                pauseCoroutine = StartCoroutine(PauseAndTurn(pauseTime));
            }
        }
            
    } 

    // for playing death animation before removing enemy
    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
    
    private IEnumerator PauseAndTurn(float time)
    {
        rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        yield return new WaitForSeconds(time);
        rb2d.velocity = new Vector2(walkSpeed, rb2d.velocity.y);
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
