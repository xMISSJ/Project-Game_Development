using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmButtonScript : MonoBehaviour {

    //This script will use the info from the info script to draw the proper card and information.
    bool selected;
    public CustomizeCards customizeCards;
    CardMenuState cardMenuState;


    private void Start()
    {
        cardMenuState = GameObject.Find("GameManagerObject").GetComponent<CardMenuState>();
    }
    // Update is called once per frame
    void Update()
    {
        if (selected && customizeCards.playerPrefix != 0)       //If the card is selected.
        {
            if (Input.GetButtonDown("P" + customizeCards.playerPrefix + "_Confirm"))
            {
                customizeCards.play = CustomizeCards.playerState.PressedConfirm;
                customizeCards.confirmButton.SetActive(false);
                customizeCards.playerHand.SetActive(false);
                customizeCards.readyScreen.SetActive(true);
                cardMenuState.CheckIfEveryoneIsReady();
                //Show screen that says ready.


            }
        }
    }



    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerCursor")
        {
            transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            selected = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerCursor")
        {
            transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            selected = false;
        }
    }
}
