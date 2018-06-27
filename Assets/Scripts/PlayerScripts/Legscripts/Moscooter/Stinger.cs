using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stinger : MonoBehaviour
{
    // Parameter variables for the TakeDamage() method from MasterBody.cs.
    public float force;
    public float damageAmount;
    public Vector3 velocity;

    public GameObject parent;

    // Use this for initialization
    void Start()
    {
        // This is so the root of this gameobject becomes the parent (the player).
        parent = transform.root.gameObject;
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Checks whether you hit the player with a stinger.
        if (hit.gameObject.tag == "Player" && !GameObject.Equals(hit.transform.root.gameObject, parent))
        {
            // The Rigidbody for this object will be the RigidBody that's attached to the player.
            Rigidbody body = hit.collider.attachedRigidbody;
            // This enemy will be assigned as the enemy so we can put this in DoPoisonDamage.
            GameObject enemy;

            // The enemy is the person who collides with trail.
            enemy = hit.transform.root.gameObject;

            enemy.transform.root.GetComponent<MasterBody>().TakeDamage(enemy, damageAmount, force, velocity);

            if (body != null && !body.isKinematic)
                // Makes it so the stinger (game object of this script) pushes the player.
                body.velocity += hit.controller.velocity;
        }
    }
}
