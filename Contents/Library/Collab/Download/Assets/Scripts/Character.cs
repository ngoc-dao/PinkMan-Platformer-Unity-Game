using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float startingHitPoints;

    public virtual IEnumerator FlickerCharacter(float flickerDuration)
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(flickerDuration);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public virtual void KillCharacter()
    {
        Destroy(gameObject);
    }
}
