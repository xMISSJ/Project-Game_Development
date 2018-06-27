using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cardObjectScript : MonoBehaviour {

    //This script will use the info from the info script to draw the proper card and information.
    bool selected;
    bool moving;    //Is the card moving.
    ItemInfo info;
    [SerializeField] GameObject border;
    [SerializeField] GameObject nameTag;
    [SerializeField] GameObject descriptionText;
    public CustomizeCards customizeCards;
    public CustomizeCards.bodyParts targetBodyPart;
    int transitionTime = 10;

    public Rect startRectangle;                            //We want to move the card back and forth between two points. It saves its start location here.
    Rect goal;

    // Use this for initialization
    void Start () {
        startRectangle = this.GetComponent<RectTransform>().rect;

        info = GetComponent<ItemInfo>();            //Gets all the item info.

        nameTag.GetComponent<Text>().text = "(" + targetBodyPart + ") " + info.itemName;
        descriptionText.GetComponent<Text>().text = info.itemDescription;
        //Todo: SCript to draw the gotten info text on card

    }

    public GameObject BodyPart()
    {
        return info.prefab;
    }
    //Methode Propertie die body part object returned
	
	// Update is called once per frame
	void Update () {
        if (selected && customizeCards.play == CustomizeCards.playerState.ChooseCard)       //If the card is selected.
        {
            if (Input.GetButtonDown("P" + customizeCards.playerPrefix + "_LeftGun") || Input.GetButtonDown("P" + customizeCards.playerPrefix + "_RightGun"))
            {
                customizeCards.ShowDescription(info.itemName, info.itemDescription);

            }
            if (Input.GetButtonUp("P" + customizeCards.playerPrefix + "_LeftGun") || Input.GetButtonUp("P" + customizeCards.playerPrefix + "_RightGun"))
            {
                customizeCards.HideDescription();
                //TODO dont execute this all the time during the update event
            }
        }
	}

    

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerCursor" && customizeCards.play == CustomizeCards.playerState.ChooseCard)
        {
            transform.localScale = new Vector3(0.6f, 0.6f, 0.6f) ;
            selected = true;
            border.SetActive(true);
            transform.SetAsFirstSibling();
            customizeCards.selectedCard = this;
            customizeCards.gotSelected = true;
            customizeCards.magnifier.SetActive(true);

        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        customizeCards.HideDescription();
        //TODO dont execute this all the time during the update event

        if (collision.gameObject.tag == "PlayerCursor" && customizeCards.play == CustomizeCards.playerState.ChooseCard)
        {
            Deselect();
        }
    }

    public void Deselect()
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        transform.SetAsLastSibling();
        selected = false;
        border.SetActive(false);
        customizeCards.gotSelected = false;
        customizeCards.magnifier.SetActive(false);
    }

}
