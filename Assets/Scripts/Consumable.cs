using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    Animator animator;
    Coroutine disappearCoroutine;
    ParticleSystem particles;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        particles = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Only one pineapple has a particle system ATM so remove this if statement
            // when done exerimenting as well as the GetComponent and declaration.
            if (particles != null)
            {
                particles.Stop();
            }

            if (gameObject.activeInHierarchy)
            {
                animator.SetTrigger("collected");
               
                if (disappearCoroutine == null)
                {
                    disappearCoroutine = StartCoroutine(Disappear());
                }
            }
        }
    }

    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}