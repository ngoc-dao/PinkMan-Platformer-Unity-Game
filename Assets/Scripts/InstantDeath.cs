using UnityEngine;


public class InstantDeath : MonoBehaviour
{
    public HitPoints hitPoints;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hitPoints.instantDeath = true;
            hitPoints.playerIsHit = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hitPoints.instantDeath = true;
            hitPoints.playerIsHit = true;
        }
    }
}
