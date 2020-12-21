using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float waitTime;

    private PlatformEffector2D effector;
    public HitPoints hitPoints;


    // This variable allows individual platforms to detect the player 
    private bool collidedWithPlayer;

    float m_waitTime;


    private void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
        m_waitTime = waitTime;
    }

    private void Update()
    {
        if (hitPoints.value <= 0)
        {
            effector.rotationalOffset = 0f;
        }

        if (Input.GetButtonUp("Jump"))
        {
            m_waitTime = waitTime;
        }

        if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && collidedWithPlayer)
        {
            if (m_waitTime <= 0)
            {
                effector.rotationalOffset = 180f;
            }
            else
            {
                m_waitTime -= Time.deltaTime;
            }
        }

        if (Input.GetButton("Jump") ||
            GameManager.sharedInstance.playerScript.rb2d.velocity.y >= 0.01 &&
            GameManager.sharedInstance.playerScript != null)
        {
            effector.rotationalOffset = 0f;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collidedWithPlayer = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collidedWithPlayer = false;
        }
    }
}
