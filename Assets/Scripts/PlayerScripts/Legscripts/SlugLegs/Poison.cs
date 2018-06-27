using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour {

    // Parameter variables for the DoPoisonDamage method.
    float poisonDuration;
    int poisonCap;              // The maximum ticks of poison damage that a player can take.
    float poisonDamage;

    public GameObject parent;

    int damageCount;
    float currentCount;

    public ParticleSystem poisonEffect;
    ParticleSystem poisonEffect2;

    GameObject enemy;

    // Parameter variables for the TakeDamage() method from MasterBody.cs.
    public float force;
    public float damage;
    Vector3 velocity;

    TrailHandler trailHandlerScript;

    // Use this for initialization.
    void Start ()
    {
        poisonDuration = 2f;
        poisonCap = 4;
        poisonDamage = damage;
    }

    public IEnumerator DoPoisonDamage(float damageDuration, int damageCount, float damageAmount, GameObject enemy)
    {
        currentCount = 1;

        this.enemy = enemy;
            // Gives damage.
            poisonEffect2 = Instantiate(poisonEffect, enemy.transform.root);
        // If the damageCounter has reached (this means x amount times getting the damage).
        while (currentCount <= damageCount)
        {

            enemy.transform.root.GetComponent<MasterBody>().TakeDamage(parent.gameObject, damageAmount, force, velocity);
            // Coroutine is used so that the effect is paused. This effect is what we'd like to achieve with damage over time.
            yield return new WaitForSeconds(damageDuration);
            currentCount++;
        }

        // This so that the poison effect is excecuted fully, before being cleared (removed).
        if (currentCount >= damageCount)
        {
            Destroy(poisonEffect2);
            Destroy(this);
        }
    }
}
