using UnityEngine;

public class Celebration : MonoBehaviour
{
    bool alreadyTriggered;
    public ParticleSystem confetti;
    public ParticleSystem confettiBurst;
    public ParticleSystem fireworks1;
    public ParticleSystem fireworks2;
    public float fireworksTimer;
    bool startFireworksTimer;

    private void Start()
    {
        confetti.Stop();
        confettiBurst.Stop();
        fireworks1.Stop();
        fireworks2.Stop();
    }

    private void Update()
    {
        if (startFireworksTimer)
        {
            fireworksTimer += Time.deltaTime;
        }
        
        if (fireworksTimer > 5)
        {
            fireworks1.Stop();
            fireworks2.Stop();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!alreadyTriggered && collision.gameObject.CompareTag("Player"))
        {
            confetti.Play();
            confettiBurst.Play();
            if (fireworksTimer < 5)
            {
                fireworks1.Play();
                fireworks2.Play();
            }
            alreadyTriggered = true;
            startFireworksTimer = true;
        }
    }
}
