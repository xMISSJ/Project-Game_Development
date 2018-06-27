using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportLegs : MasterLegs {

    public GameObject pad;
    float time;
    public float teleportPause = 1;
    
	// Use this for initialization
	new void Start () {
        base.Start();
	}

    // Update is called once per frame
    public override void Move(float horizontal, float vertical)
    {
        RotateToCam(ref horizontal, ref vertical);
        pad.transform.position += new Vector3(horizontal, 0, vertical).normalized * maxSpeed * Time.deltaTime;
        moveDirection.y -= gravity;
        controller.Move(moveDirection);
        time += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && time > teleportPause)
        {
            time = 0;
            body.transform.position = pad.transform.position;
            pad.transform.localPosition = new Vector3(0, pad.transform.localPosition.y, 0);
        }
    }
}
