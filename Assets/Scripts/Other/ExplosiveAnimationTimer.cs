using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveAnimationTimer : MonoBehaviour {

    private Animator animator;

    // Use this for initialization
    void Start () {

        animator = GetComponent<Animator>();
        StartCoroutine(BeforeAnimationPlays());

        // disable animation at start, because it needs to go off once the countdown reaches 0
        animator.enabled = false;
    }

    public IEnumerator BeforeAnimationPlays(float countDown = 10)
    {
        float timer = countDown;

        // timer counts down, according to cooldown before round starts (masterbody script)
        while (timer > 0)
        {
           
            yield return new WaitForSeconds(1.0f);
            timer--;

        }
        // start the animation once cooldown reaches 0
        if (timer == 0)
        {
            // animator.Play("ExplosionColorChange");
            animator.enabled = true;
        }
    }
}
