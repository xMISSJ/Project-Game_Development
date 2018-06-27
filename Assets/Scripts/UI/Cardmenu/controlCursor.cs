using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controlCursor : MonoBehaviour {

    [SerializeField] Sprite handNormal;
    [SerializeField] Sprite handClick;
    public int playerPrefix;
    [SerializeField] float speedMultiplier = 3f;
    Vector2 screenSize;
    [SerializeField] GameObject window;             //Reference to the top window, to determine how far the hand can move
    Rect handRectangle;
    
	// Use this for initialization
	void Start () {
        //screenSize = new Vector2(400, 135.5f);
        screenSize = window.GetComponent<RectTransform>().rect.size;
        handRectangle = GetComponent<RectTransform>().rect;
    }
	
	// Update is called once per frame
	void Update () {
        
        if (playerPrefix != 0)
        MoveHand();

        if (playerPrefix != 0)
        {
            if (Input.GetButtonDown("P" + playerPrefix + "_Confirm"))
            {
                GetComponent<AudioSource>().Play();
            }
        }

    }

    void MoveHand()
    {
        Vector3 move = new Vector3(Input.GetAxis("P" + playerPrefix + "_Horizontal") * speedMultiplier, Input.GetAxis("P" + playerPrefix + "_Vertical") * speedMultiplier, 0f);

        //Collision with sides of the walls
        if (transform.localPosition.x > screenSize.x && move.x > 0)
        {
            move.x = 0;
        }
        if (transform.localPosition.x < 0 && move.x < 0)
        {
            move.x = 0;
        }
        if (transform.localPosition.y < -screenSize.y && move.y < 0)
        {
            move.y = 0;
        }
        if (transform.localPosition.y > 0 && move.y > 0)
        {
            move.y = 0;
        }
        ///////////////////////////////
        transform.Translate(move);


    }
}
