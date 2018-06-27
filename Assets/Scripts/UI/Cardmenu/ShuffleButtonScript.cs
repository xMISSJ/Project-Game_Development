using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShuffleButtonScript : MonoBehaviour {

    //This script will use the info from the info script to draw the proper card and information.
    bool selected;
    [SerializeField]
    int requiredCoins = 1;      //How many coins one shuffle costs

    [SerializeField]
    Text coinText;      //How many coins one shuffle costs
    [SerializeField]
    CurrentCoins currentCoins;
    bool clickable;

    [SerializeField]
    Color originalColor;

    public CustomizeCards customizeCards;

    private void Start()
    {
        coinText.text = requiredCoins.ToString();

        if (GameManager.instance.playerCoins[customizeCards.ID] < requiredCoins)
        {
            clickable = false;
            GetComponent<Image>().color = Color.grey;
        }
        else
        {
            clickable = true;
            GetComponent<Image>().color = originalColor;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (selected && customizeCards.playerPrefix != 0)       //If the card is selected.
        {
            if (Input.GetButtonDown("P" + customizeCards.playerPrefix + "_Confirm") && GameManager.instance.playerCoins[customizeCards.ID] >= requiredCoins)
            {
                GameManager.instance.playerCoins[customizeCards.ID] -= requiredCoins;
                GameManager.instance.scoreCounter.updateScore(customizeCards.ID, "coin", -1);
                customizeCards.ShuffleButton();
                if (GetComponent<AudioSource>() != null)
                    GetComponent<AudioSource>().Play();
                //Play shuffle sound effect
                if (GameManager.instance.playerCoins[customizeCards.ID] < requiredCoins)
                {
                    clickable = false;
                    GetComponent<Image>().color = Color.grey;
                    currentCoins.UpdateText();
                }
            }
            else
            {
                //Play negative sound effect
            }
        }
    }



    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerCursor" && clickable)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            selected = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerCursor" && clickable)
        {
            transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            selected = false;
        }
    }
}
