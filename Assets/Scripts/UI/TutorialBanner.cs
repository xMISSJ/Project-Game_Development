using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBanner : MonoBehaviour {

    canvasState state = canvasState.outofScreen;
    float normalY = -20;
    float heldDownY = -34;
    float outOfScreenY = 450;

    [SerializeField]
    float acceleration = 1.5f;
    float speed;

    private enum canvasState
    {
        onScreen,
        heldDown,
        movingUp,
        outofScreen,
        movingDown
    }
	// Use this for initialization
	void Start () {
		if (GameManager.instance.firstRound)
        { GetComponent<AudioSource>().Play(); }
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("HelpButton") && state == canvasState.onScreen)
        {
            state = canvasState.heldDown;
            transform.localPosition = new Vector3(0, heldDownY);
        } else 
        if (Input.GetButtonUp("HelpButton") && state == canvasState.heldDown)
        {
            state = canvasState.movingUp;
            GetComponent<AudioSource>().Play();
        } else 

        if (Input.GetButtonDown("HelpButton") && state == canvasState.outofScreen)
        {
            state = canvasState.movingDown;
            GetComponent<AudioSource>().Play();
        }

        //Go out of the screen.
        if (state == canvasState.movingUp)
        {
            transform.localPosition += new Vector3(0, speed);
            speed += acceleration;
            if (transform.localPosition.y > outOfScreenY)
            {
                state = canvasState.outofScreen;
                speed = 0;
                transform.localPosition = new Vector3(0, outOfScreenY);
            }
        } else

        //Appear in the screen.
        if (state == canvasState.movingDown)
        {
            transform.localPosition -= new Vector3(0, speed);
            speed += acceleration;
            if (transform.localPosition.y < heldDownY)
            {
                state = canvasState.onScreen;
                speed = 0;
                transform.localPosition = new Vector3(0, normalY);
            }
        }
    }

    public bool isMenuActivated()
    {
        if (state == canvasState.outofScreen)
        { return false; }
        else { return true; }
    }

    public void moveDown()
    {
        if (state == canvasState.outofScreen)
        {
            state = canvasState.movingDown;
        }
    }
}
