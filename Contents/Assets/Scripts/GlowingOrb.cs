using UnityEngine;
using System.Collections;

public class GlowingOrb : MonoBehaviour
{
    public int numJumps;
    private bool disappear = false;
    public GameObject inner;
    Coroutine flickerCoroutine = null;
    public HitPoints hitPoints;
    private bool collidedWithPlayer;

    Color nothingness = new Color(0, 0, 0, 0);

    private void Update()
    {
        if (disappear)
        {
            FadeOut();
        }

        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space) ||
            Input.GetButton("Jump") || Input.GetKey(KeyCode.Space))
        {
            if (collidedWithPlayer && flickerCoroutine == null)
            {
                flickerCoroutine = StartCoroutine(Flicker());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!disappear)
        {
            GameManager.sharedInstance.playerScript.extraJumps = 1;
            RespondToJump();
            collidedWithPlayer = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        RespondToJump();
        collidedWithPlayer = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!disappear)
        {
            GameManager.sharedInstance.playerScript.extraJumps--;
            collidedWithPlayer = false;
        }
    }

    private void FadeOut()
    {
        foreach (Transform child in transform)
        {
            ParticleSystem particles = child.gameObject.GetComponent<ParticleSystem>();
            var main = particles.main;
            main.startColor = new ParticleSystem.MinMaxGradient(nothingness);
        }
    }

    // makes the glowing orb brighter when player uses it
    IEnumerator Flicker()
    {
        Vector3 currentScale = inner.transform.localScale;
        float currentScaleX = inner.transform.localScale.x;
        for (float f = currentScaleX; f <= 0.4f; f += 0.06f)
        {
            inner.transform.localScale = new Vector3(f, f, f);
            yield return new WaitForSeconds(0.02f);
        }
        inner.transform.localScale = currentScale;
        flickerCoroutine = null;
    }

    private void RespondToJump()
    {
        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space) ||
            Input.GetButton("Jump") || Input.GetKey(KeyCode.Space))
        {
            if (flickerCoroutine == null)
            {
                flickerCoroutine = StartCoroutine(Flicker());
            }

            if (numJumps > 0)
            {
                numJumps--;
                if (numJumps == 0)
                {
                    disappear = true;
                }
            }
            else
            {
                disappear = true;
            }
        }
    }
}
