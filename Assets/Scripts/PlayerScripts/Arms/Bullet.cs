using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public GameObject player;
    public float force;
    public float damage;
    Vector3 velocity;
    void OnCollisionEnter(Collision collision)
    {
       
        GameObject hit = collision.collider.transform.root.gameObject;
        if(player.name != hit.gameObject.name)
        {
            if (hit.tag == "Player")
            {
                hit.GetComponent<MasterBody>().TakeDamage(player, damage, force, velocity);
            }
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        velocity = GetComponent<Rigidbody>().velocity;
    }
}
