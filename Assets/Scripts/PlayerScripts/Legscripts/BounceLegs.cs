using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceLegs : MasterLegs
{

    public float rotationSpeed = 200;
    public float bounceTime = 2;
    public float LastBounce;
    public float bounceForce = 20;
    public float maxDistance = 1;
    private Vector3 moveForce;

    //Vector3 moveDirection = Vector3.zero;
    // Use this for initialization

    new void Start()
    {
        base.Start();
        LastBounce = 0;
    }


    public override void Move(float horizontal, float vertical)
    {
        //Bouncelegs don't use the acceleration script, so thats why i instead use the maxSpeed variable;
        RotateToCam(ref horizontal, ref vertical);
        if (vertical != 0 || horizontal != 0)
        {
            gameObject.transform.GetChild(0).transform.rotation = Quaternion.LookRotation(new Vector3(vertical, 0, -horizontal));
        }
        
        if (controller.isGrounded)
        {
            moveDirection = Vector3.zero;
                if (LastBounce < Time.realtimeSinceStartup  && (horizontal != 0 || vertical != 0))
                {
                if (GetComponent<AudioSource>() != null)
                {
                    GetComponent<AudioSource>().Play();
                }
                    LastBounce = Time.realtimeSinceStartup + bounceTime;
                    moveForce = Quaternion.AngleAxis(gameObject.transform.eulerAngles.y, Vector3.up) * new Vector3(horizontal * maxSpeed, bounceForce, vertical * maxSpeed);
                    moveForce = Vector3.ClampMagnitude(moveForce, maxDistance);
                    moveDirection = moveForce;
            }
        }

        Gravity();
        //moveDirection.y -= gravity * Time.deltaTime;
         //   controller.Move(moveDirection);
           
    }
}
