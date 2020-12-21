using System.Collections;
using UnityEngine;

public class PlayerController : Character
{
    private float moveInput;
    public float jumpSpeed; // 18
    public float movementSpeed; // 6
    private bool isFacingRight;
    private bool isFacingLeft;

    // for faster falling and high jumps
    private float normalGravityScale = 8;
    private float fallingGravityScale = 9;
    private float lowJumpGravityScale = 4;

    //For ground detection
    private bool isGrounded = false;
    public float groundCheckRadius; // 0.2
    public Transform groundCheck;
    public LayerMask groundLayer;

    //for multiple jumps
    [HideInInspector] public int extraJumps; 
    public int extraJumpsValue; // 1
    public int jumpCount;

    // for wall jumps and wall sliding
    private bool isOnWall = false;
    public float wallCheckRadius; // 0.2
    public Transform wallCheck;
    public LayerMask wallLayer;
    public bool wallSliding = false;
    public float wallSlidingSpeed; // 1

    public Rigidbody2D rb2d;
    Animator animator;
    public HealthBar healthBarPrefab;
    [HideInInspector] public HealthBar healthBar;
    public HitPoints hitPoints;
    public Coroutine delayCoroutine = null;
    public GameObject disappearPrefab;

    private void OnEnable()
    {
        ResetCharacter();
        GameManager.sharedInstance.playerScript = this;
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isGrounded && !isOnWall)
        {
            extraJumps = extraJumpsValue;
            jumpCount = 0;
        }
        // To fix bug that cause jumpCount to reset to zero. isGrounded returns
        // true immediatelely after jumping because the groundCheck circle still overlaps with ground
        else if (jumpCount == 0)
        {
            jumpCount = 1;
        }
        else if (!isGrounded && isOnWall && moveInput != 0)
        {
            wallSliding = true;

            // To jump from walls, prevents jumping on one wall continously
            if (isFacingRight && moveInput == -1 || isFacingLeft && moveInput == 1)
            {
                extraJumps = 1;
            }
        }
        else
        {
            wallSliding = false;
        }

        if (wallSliding)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Clamp(rb2d.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }

        // If not jumping then play moving animation. Jumping animation takes precedence.
        if (Input.GetButtonDown("Jump") && (isGrounded || extraJumps > 0))
        {
            if (extraJumps > 0)
            {
                extraJumps--;
            }

            if (jumpCount > 0)
            {
                animator.SetTrigger("doubleJump");
            }

            rb2d.velocity = Vector2.up * jumpSpeed;
            jumpCount++;
        }        
        else if (isGrounded)
        {
            animator.SetFloat("movementDirection", Mathf.Abs(moveInput));
        }

        AdjustGravityScale();
        SetJumpingAndFallingAnimations();
        SetFacing();
    }

    private void FixedUpdate()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        // If the player is not attacked and movement inputs are not delayed,
        // move player according to movement inputs.
        if (!hitPoints.playerIsHit && delayCoroutine == null)
        {
            rb2d.velocity = new Vector2(moveInput * movementSpeed, rb2d.velocity.y);  
        }
       
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isOnWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CanBePickedUp"))
        {
            // TODO: Implement point system for consumables
        }
        if (collision.gameObject.CompareTag("CheckPoint"))
        {
            GameManager.sharedInstance.playerSpawnPoint.transform.position = collision.gameObject.transform.position;
        }
    }

    public override void KillCharacter()
    {
        base.KillCharacter();
        Instantiate(disappearPrefab, transform.position, Quaternion.identity);
        Destroy(healthBar.gameObject, GameManager.sharedInstance.respawnDelay);
    }

    public void ResetCharacter()
    {
        print("player reset");
        healthBar = Instantiate(healthBarPrefab);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
    }

    private void SetFacing()
    {
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            isFacingRight = true;
            isFacingLeft = false;
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
            isFacingLeft = true;
            isFacingRight = false;
        }
    }

    private void SetJumpingAndFallingAnimations()
    {
        if (rb2d.velocity.y > 0.0001)
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
        }
        else if (rb2d.velocity.y < -0.0001)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }
    }

    private void AdjustGravityScale()
    {
        // Adjust the gravity scale for faster falling and jumping higher when button is held
        // Faster falling makes jumping more smooth like Mario.
        if (rb2d.velocity.y > 0)
        {
            if (!Input.GetButton("Jump"))
            {
                rb2d.gravityScale = normalGravityScale;
            }
            else
            {
                rb2d.gravityScale = lowJumpGravityScale;
            }
        }
        else if (rb2d.velocity.y < 0)
        {
            rb2d.gravityScale = fallingGravityScale;
        }
    }

    public IEnumerator Delay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        delayCoroutine = null;
    }
}
