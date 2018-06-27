using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailHandler : MonoBehaviour
{
    // We use this so the players aren't poisoned twice.
    public bool poisoned;

    public ParticleSystem poisonEffecto;

    public GameObject parent;

    Poison poisonScript;
    public float damage;
    float poisonDamage;

    // Use this for initialization.
    void Start()
    {
        poisonDamage = damage;
        poisoned = false;

        // This is so the root of this gameobject becomes the parent (the player).
        parent = transform.root.gameObject;
        transform.parent = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Checks whether this gameobject is a player, but isn't the player itself. Root is here the most upperparent.
        if (other.gameObject.tag == "Player" && !GameObject.Equals(other.transform.root.gameObject, parent) && !poisoned)
        {
            float poisonDuration = 2f;
            int poisonCap = 4;
            MasterBody masterBodyScript = gameObject.transform.root.gameObject.GetComponent<MasterBody>();

            poisoned = true;

            // This enemy will be assigned as the enemy so we can put this in DoPoisonDamage.
            GameObject enemy;

            // The enemy is the person who collides with trail.
            enemy = other.transform.root.gameObject;

            // Checks whether enemy already has the poison script.
            if (enemy.GetComponent<Poison>() == null)
            {
                // Gives poison script.
                poisonScript = enemy.AddComponent<Poison>();
            }

            poisonScript.parent = parent;
            // Our poisonEffecto in this script, gets the value of the poisonEffect from the Poison.cs.
            poisonScript.poisonEffect = poisonEffecto;
            poisonScript.StartCoroutine(poisonScript.DoPoisonDamage(poisonDuration, poisonCap, poisonDamage, enemy));
        }
    }
}
