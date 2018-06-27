using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MasterArm
{


    public float floatUp = 3.5f;
    public float floatDown = 0.01f;

    AudioSource aS;
    public bool soundStart = false;

    // Use this for initialization
    void Start()
    {

        base.Start();
        aS = GetComponent<AudioSource>();

    }
    // plays a sound once the player attempts to shoot
    public override void Fire()
    {
        soundStart = true;

        if (soundStart)
        {
            if (!aS.isPlaying)
            {
                aS.Play();
                soundStart = false;
            }
        }
    }

    void FixedUpdate()
    {
        // You need an empty object, and you put the balloon as a child (if you don't use localPosition the position is zero/below the arena)
        transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Sin(Time.timeSinceLevelLoad) * 0.5f, transform.localPosition.z);

    }
}
