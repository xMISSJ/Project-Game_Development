using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walk : MasterLegs {

    public float slowDown;

	// Use this for initialization
	new void Start () {
        base.Start();
	}

    // Update is called once per frame
    public override void Move(float horizontal, float vertical)
    {

        RotateToCam(ref horizontal, ref vertical);

        velocity.y = Accelerate(vertical, velocity.y);        //Sets the vertical speed of the player. (Vertical being relative to the player itself. SO back and forth)
        velocity.x = Accelerate(horizontal, velocity.x);        //Sets the horizontal speed of the player. (Basically sidestepping. To its own left and right)

        
        moveDirection = (Quaternion.AngleAxis(gameObject.transform.eulerAngles.y, Vector3.up) * new Vector3(horizontal * maxSpeed, 0, vertical * maxSpeed) * Time.deltaTime);

            if (vertical != 0 || horizontal != 0)
            {
                gameObject.transform.GetChild(0).transform.rotation =  Quaternion.LookRotation(new Vector3(vertical, 0, -horizontal)) * transform.parent.rotation;
            }

        Gravity();
    }
}
