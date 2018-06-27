using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerAndSickle : MasterArm
{
    private GameObject rotatingPoint;               // the axis around wich the hammer rotates
    public project projection;                      // the object used to project the image on the floor on hit
    public float maxRotation = 90;                  // the maximum rotation of the hammer
    public float normal = 0;                        // the default rotation of the hammer
    public float smooth = 20;                       // the speed of the swing
    private Vector3 projectPos;                     // the position of the projection relative to player
    private Quaternion projectRot;                  // the rotation of the projection relative to player
    public AttackBox attackBox;                     // the area that the hammer can reach
    public float cooldown = 0.5f;                   // time between swings
    float currentCooldown = 0;                      // time left to next swing
    public float damage = 1;                        
    public float knockBack = 15;
    public float fadeTime = 10;                     // the time it takes for the projection fade
    private bool attack = false;                    // is true if currently attacking/swinging
    playerStats pS;                                 // used for sharing attacker and victoms coins on attack


    // Use this for initialization
    new void Start()
    {
        base.Start();
        pS = GameManager.instance.GetComponent<playerStats>();

        rotatingPoint = transform.GetChild(0).gameObject;
        projectPos = projection.transform.localPosition;
        projectRot = projection.transform.localRotation;
        projection.transform.parent = null;
    }

    // when the player attacs
    public override void Fire()
    {
        // test if cooldown is over since last attack
        if (currentCooldown > Time.timeSinceLevelLoad )
        {
            return;
        }
        currentCooldown = Time.timeSinceLevelLoad + cooldown;

        //used for animation
        attack = true;

        //get every body in the attack area;
        foreach (Collider col in attackBox.collider)
        {

            //damaging the wall in the player confirm part of the game
            if (col.tag == "DestroyableWall")
            {
                getHit targetGetHit = col.GetComponent<getHit>();
                targetGetHit.health -= damage;
                targetGetHit.GetComponent<AudioSource>().Play();        //Plays the get hit sound effect.
            }

            GameObject GO = col.transform.root.gameObject;
            if(GO.tag == "Player")                          // if a player is hit
            {
                MasterBody EnemyMb = GO.GetComponent<MasterBody>();
                MasterBody mb = gameObject.transform.root.GetComponent<MasterBody>();
                
                // get playerdata to later divide the coins
                playerStats.playerData enemy = pS.players.Find(x => x.playerNr.Equals(EnemyMb.playerID)); // get the stat from the enemy 
                playerStats.playerData player = pS.players.Find(x => x.playerNr.Equals(mb.playerID));     // get stats from self

                int enValue = (player.coins - enemy.coins) / 2; // determine how many coins the enemy gets
                int plValue = (enemy.coins - player.coins) / 2; // determine how many coins the player gets

                // add the determined amound coins to each player
                pS.addCoin(EnemyMb.name, enValue);
                pS.addCoin(mb.name, plValue);
                
                //give the enemy some damage and knockback
                EnemyMb.TakeDamage(gameObject.transform.root.gameObject, damage, knockBack, (GO.transform.position - transform.position).normalized + new Vector3(0,(-1)- (GO.transform.position - transform.position).normalized.y, 0));
            }
        }
        Shoot(); // play attacking sound
    }

    // Update is called once per frame
    void Update()
    { 
        // if the player is currently attacking
        if (attack == true)
        {
            // rotate the hammer down
            rotatingPoint.transform.localEulerAngles = new Vector3(rotatingPoint.transform.localRotation.x, 90, Mathf.Lerp(rotatingPoint.transform.localEulerAngles.z, maxRotation, Time.deltaTime * smooth));
            
            // check if the image is currently being projected
            if (!projection.Projecting)
            {
                // place a projection 
                projection.Project(transform.position + (transform.rotation * projectPos), Quaternion.Inverse(transform.rotation) * projectRot);
                projection.Projecting = true;
            }
            
            // if hammer has reached max rotation (down) stop rotation going down
            if (rotatingPoint.transform.localEulerAngles.z >= maxRotation-1)
            {

                attack = false;
            }
         
        }
        else // if player is no longer attacking (or hammer is completly down)
        {       
            // check if a projection is happening
            if (projection.Projecting)
            {
                // stop that projection
                projection.Projecting = false;
                projection.fade(fadeTime); // creates a duplicate that fades in time
            }
            //rotate to top
            rotatingPoint.transform.localEulerAngles = new Vector3(rotatingPoint.transform.localRotation.x, 90, Mathf.Lerp(rotatingPoint.transform.localEulerAngles.z, normal, Time.deltaTime * smooth));
        }
    }
}
