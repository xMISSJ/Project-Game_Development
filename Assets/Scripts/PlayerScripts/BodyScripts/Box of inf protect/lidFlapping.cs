using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class creates both the phisics of the flaps and the imortality / hiding behavior of the box
public class lidFlapping : MonoBehaviour {
    float lastY;                                // the last velocity on the y axis
    public float scaleTo = 2.6f;                // the size of the box when hidden inside
    public float botRot = -55;                  // the lowest point in angle of the lids/flaps
    public float topRot = 0;                    // the higest point
    public float closedRot = 150;               // the angle of the lids when closed
    public float rot;                           // current rotation of the lids
    public float timeToSafe = 1;                // the time it takes for the box to close
    public float safeTime = 5;                  // the time the box is closed
    public float cooldown = 10;                 // the time before the box can beclosed again
    float flapSpeed = 0;                        // current speed of flaps
    public float acceleration = 1;              // acceleration of the flaps
    public float friction = 0.5f;               // the friction on the flaps
    bool safe = false;                          // if the player is hiding or not
    bool animationStarted;                      // if the box is transforming (used for playing sounds)
    MasterBody mB;
    CharacterController cC;
    List<GameObject> flaps;                     // the lids/flaps on the box
    Vector3 startScale;                         // the untransformed size of the box
	// Use this for initialization
	 void Start () {
        startScale = transform.localScale;                  // store for reseting later
        cC = GetComponentInParent<CharacterController>();
        mB = GetComponentInParent<MasterBody>();
        rot = topRot;                                       // set start position lids
        lastY = cC.velocity.y;

        // get the lids on the box
        flaps = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).name == "flapjoint") {
                flaps.Add(transform.GetChild(i).gameObject);
            }
        }
    }
	// this calculates the angle of the lids based on vertical velocity
    void flap()
    {
        
        transform.localScale = startScale;  // reset scale, this could be off due to close()
        flapSpeed -= friction;
        // changes flapspeed based on vertical velocity of player.
        if (lastY < (int)cC.velocity.y)
        {
            flapSpeed -= acceleration;
        }
        if (lastY > (int)cC.velocity.y)
        {
            flapSpeed += acceleration;
        }
        lastY = (int)cC.velocity.y * 10;
        lastY /= 10;
        rot += flapSpeed;                   // add speed to current rotation

        // enforce rotation limits
        if (rot < botRot)
        {
            rot = botRot;
            flapSpeed = -flapSpeed / 2;     // bounce lid from lower limit
        }

        if (rot > topRot)
        {
            rot = topRot;
            flapSpeed = 0;
        }
    }

    float boxedTime = 0;                    // time left in boxed state
    //used for closing and opening box
    void close()
    {
        // checks if box is open 
        if (!safe)
        {
            //plays the closing sound
            if (!animationStarted)
            {
                GetComponent<AudioSource>().Play();
                animationStarted = true;
            }
            // closing the lids
            flapSpeed += acceleration / 2;
            rot += flapSpeed;
            // if box is fully closed
            if (rot > closedRot)
            {
                boxedTime = safeTime;       // set time left in box
                safe = true;                
                mB.canMove = false;
                mB.canAct = false;
                mB.canTakeDamage = false;
                rot = closedRot;
                flapSpeed = 0;          
                animationStarted = false;
            }
            transform.localScale = ((startScale) * (1 + (((scaleTo -1)/ (closedRot - botRot)) * (rot - botRot)))) ; // set size based on point in transition

        }
        else //if box is closed
        {
            boxedTime -= Time.deltaTime;
            mB.canMove = false;
            mB.canAct = false;
            mB.canTakeDamage = false;
            // if time in box is over or player started moving
            if (boxedTime <= 0 || (Mathf.Abs(Input.GetAxis("P" + mB.playerPrefix + "_Horizontal"))+ Mathf.Abs(Input.GetAxis("P" + mB.playerPrefix + "_Vertical"))) >0)
            {
                boxedTime = 0;
                flapSpeed -= acceleration / 8;
                rot += flapSpeed;
                if (rot < botRot) //if box is fully open
                {
                   
                    safe = false;
                    mB.canMove = true;
                    mB.canAct = true;
                    mB.canTakeDamage = true;
                    rot = botRot;
                    flapSpeed = 0;
                    stoppedTime = Time.timeSinceLevelLoad + cooldown;
                }
                transform.localScale = ((startScale) * (1 + (((scaleTo - 1) / (closedRot - botRot)) * (rot - botRot)))); // set size based on point in transition
            }
        }
    }

    float stoppedTime = 0;
	// Update is called once per frame
	 void Update () {
        // if player is boxed or standing still
        if(safe || new Vector2(cC.velocity.x, cC.velocity.y).magnitude < 1 * Time.deltaTime)
        {
            // if cooldown is over close box
            if (stoppedTime < Time.timeSinceLevelLoad)
            {
                close();
            }
            else
            {
                flap();
            }
        }
        else
        {
            //add small amount of time if cooldown is over but player is not standing still
            if(stoppedTime < Time.timeSinceLevelLoad + timeToSafe)
            {
                stoppedTime = Time.timeSinceLevelLoad + timeToSafe;
            }
           
            flap();
        }
        // set rotation of flaps to rot
        foreach (GameObject f in flaps)
        {
            f.transform.rotation = Quaternion.Euler(f.transform.rotation.eulerAngles.x, f.transform.rotation.eulerAngles.y, rot);
        }
    }
}
