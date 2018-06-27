using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getHit : MonoBehaviour {

    public float health;
    AudioSource audioSource;

    // Use this for initialization
    void Start () {
        health = 10f;
    }
	
	// Update is called once per frame
	void Update () {
		if (health < 0)
        {
            Destroy(gameObject);
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Bullet"))
        {
            GetComponent<AudioSource>().Play();
            health -= collision.gameObject.GetComponent<Bullet>().damage;
        Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag.Equals("Bullet"))
        {
            GetComponent<AudioSource>().Play();
            health -= collision.gameObject.GetComponent<Bullet>().damage;
        }

        //Melee Collision with this wall is handled withing the hammer and sickle script.
    }
}
