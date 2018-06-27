using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltExplosionBody : MonoBehaviour
{

    /* Some pseudo code for belt of random explosion 
     * Declare boolean (activate bool = body explodes) and float 
     * use random.value (between 2 number. It has % amount of chance to activate the bool to true == body explode)
     * If body explodes, decrease health (make use of healthbar)
     */

    [SerializeField]
    int explosionChance;
    public bool activateRandom = false;
    public Explosion explode;
    MasterBody player;

    // Use this for initialization
    void Start()
    {
        player = gameObject.GetComponentInParent<MasterBody>();
    }

    // If a bullet hits a player, it activates the chance the body will explode (and the player will die instantly)
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            float randVal = Random.Range(0, 100);

            if (randVal < explosionChance) //chance explosion will go off
            {
                Explosion explosionObject = Instantiate(explode, transform);

                player.health = 0;
                player.PlayerDies();
                explode.Explode();
            }
        }
    }
}
