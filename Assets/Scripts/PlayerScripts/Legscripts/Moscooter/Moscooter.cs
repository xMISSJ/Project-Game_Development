using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moscooter : MasterLegs {
    float speed;
    public float rotationSpeed;
    public Quaternion rot;
    Quaternion rotTo;

    // Use this for initialization.
    void Start ()
    {
        speed = 4;
        rotationSpeed = 200;
	}

    public override void Move(float horizontal, float vertical)
    {
        // Makes it so the camera moves in the same direction as the input. Also ref, because the changes are immediately applied. No return needed.
        RotateToCam(ref horizontal, ref vertical);

        speed = Accelerate(horizontal, vertical, speed);

        if (horizontal != 0 || vertical != 0)
        {   // get the angle to rotate towards
            rotTo = Quaternion.LookRotation(transform.root.rotation * new Vector3(horizontal, 0, vertical));
        }
        // rot represents the current rotation
        // rotate rot slowly towards rotTo
        rot = Quaternion.RotateTowards(rot, rotTo, rotationSpeed * Time.deltaTime);

        // set rotation to rot - the rotation of the parent so that these are not interfering with each other
        gameObject.transform.localRotation = Quaternion.Euler(rot.eulerAngles.x, rot.eulerAngles.y - transform.root.transform.rotation.eulerAngles.y, rot.eulerAngles.z);
        // set direction and speed
        moveDirection = gameObject.transform.rotation * (Vector3.forward * speed * Time.deltaTime);
        Gravity();
    }

    private void LateUpdate()
    {
        
    }
}
