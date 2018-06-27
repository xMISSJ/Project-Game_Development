using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject explosive;
    public float radius = 5.0f; // radius of explosion
    public float upForce = 0f; // OPTIONAL: lifts players up the ground as it explodes  

    [SerializeField]
    private GameObject explodeEffect;
    GameObject particleObject;

    public void Explode()
    {
        Vector3 explodePos = explosive.transform.position; // grab position of gameobject
        Collider[] colliders = Physics.OverlapSphere(explodePos, radius); // overlapsphere, any gameobject that has a collider, will be put in this array      

        foreach (Collider hit in colliders) // want to get component of each collider
        {
            MasterBody player = hit.gameObject.GetComponent<MasterBody>();
            GameObject GO = hit.transform.gameObject;

            // knockback thanks to the HammerAndSickle/MasterBody class
            if (player != null)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position); // position between explosive and player

                // If the player is far from the explosion belt, it receives less damage and vice versa.
                float farDmg = 2;
                float closeDmg = 5;

                if (distance == farDmg)
                {
                    player.TakeDamage(transform.gameObject, 2.0f, 10f, (GO.transform.position - explosive.transform.position).normalized + new Vector3(0, (-1) - (GO.transform.position - explosive.transform.position).normalized.y, 0));
                }

                if (distance == closeDmg)
                {
                    player.TakeDamage(transform.gameObject, 5.0f, 10f, (GO.transform.position - explosive.transform.position).normalized + new Vector3(0, (-1) - (GO.transform.position - explosive.transform.position).normalized.y, 0));
                }
            }
        }
        // Instantiate the particle effect upon hit. Code is from deathEffect particle (masterbody). 
        particleObject = Instantiate(explodeEffect);
        particleObject.transform.position = transform.position;
    }
}
