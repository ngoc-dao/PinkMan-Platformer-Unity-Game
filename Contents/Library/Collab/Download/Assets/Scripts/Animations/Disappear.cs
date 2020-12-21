using UnityEngine;

public class Disappear : MonoBehaviour
{
    Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        // if the animation state has finished playing once, remove disappearing GameObject.
        // Should be > 1 but > 0.95 to prevent next frame to occur.
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95)
        {
            Destroy(gameObject);
        }
    }
}