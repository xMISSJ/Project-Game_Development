using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheelscript : MasterLegs {
    
    public float rotationSpeed = 200;
    public Quaternion rot;
    Quaternion rotTo;
    float speed = 0;

    //Audio related stuff
    AudioLowPassFilter audioLowPass;
    AudioSource audioSource;
    [SerializeField]
    float lowPassTop;
    [SerializeField]
    float lowPassBottom;
    [SerializeField]
    float lowPitch;
    [SerializeField]
    float highPitch;
    [SerializeField]
    float lowVolume;
    [SerializeField]
    float HighVolume;

    // Use this for initialization
    new void Start () {
        base.Start();
        rot = new Quaternion();
        rotTo = new Quaternion();
        audioLowPass = GetComponentInChildren<AudioLowPassFilter>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    public override void Move(float horizontal, float vertical)
    {
        RotateToCam(ref horizontal, ref vertical);
        // calculate current acceleration
        speed = Accelerate(horizontal, vertical, speed);

        audioLowPass.cutoffFrequency = Mathf.Lerp(lowPassTop, lowPassBottom, speed / maxSpeed); //Hoeveelste van maxspeed de current speed is (Bijv 40 %).
        audioSource.pitch = Mathf.Lerp(lowPitch, highPitch, speed / maxSpeed);
        audioSource.volume = Mathf.Lerp(lowVolume, HighVolume, speed / maxSpeed);
        if (horizontal !=0 || vertical != 0)
        {   // get the angle to rotate towards
            rotTo = Quaternion.LookRotation(transform.root.rotation * new Vector3(horizontal, 0, vertical));
        }
        // rot represents the current rotation
        // rotate rot slowly towards rotTo
        rot = Quaternion.RotateTowards(rot, rotTo, rotationSpeed * Time.deltaTime);

        // set rotation to rot - the rotation of the parent so that these are not interfering with each other
        gameObject.transform.localRotation = Quaternion.Euler(rot.eulerAngles.x, rot.eulerAngles.y - transform.root.transform.rotation.eulerAngles.y , rot.eulerAngles.z);
        // set direction and speed
        moveDirection = gameObject.transform.rotation * (Vector3.forward * speed * Time.deltaTime );
        Gravity();
    }
}
