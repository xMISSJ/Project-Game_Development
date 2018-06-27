using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItsTheFinalCountdown : MonoBehaviour
{

    public Text countdown;
    public MasterBody[] mb; // gets executed at the card menu scene
    public float roundCD;

    // Use this for initialization
    public void StartRoundCooldown()
    {

        // gets executed at the start of the round, then you will know how many players are in the game
        mb = new MasterBody[GameManager.instance.amountOfPlayer];

        // Add all players, so it's possible to access their masterbodies (and correspondening variables)
        for (int i = 0; i < GameManager.instance.playersHolder.Length; i++)
        {
            mb[i] = GameManager.instance.playersHolder[i].GetComponent<MasterBody>();
        }

        // checks if there's a Canvas in the scene
        if (GameObject.FindGameObjectWithTag("Canvas") != null)
        {
            //
            // eerst zoek je canvas, dan zoekt ie naar de kinderen (transform.find zoekt een laag onder zich, werkt alleen voor canvas)
            if (GameObject.FindGameObjectWithTag("Canvas").transform.Find("Countdown_Text"))
            {
                // vind gameobject van de tekst. countdown is verwijzing naar de tekst , auto-assign text in inspector 
                countdown = GameObject.FindGameObjectWithTag("Canvas").transform.Find("Countdown_Text").GetComponent<Text>();
                StartCoroutine(BeforeRoundStart());

            }
        }
    }

    public IEnumerator BeforeRoundStart(float roundCDreduction = 3)
    {
        roundCD = roundCDreduction;

        // disable the shooting, player death and movement (lid flapping only)     
        while (roundCD > -1)
        {
            countdown.text = ("" + roundCD);
            for (int i = 0; i < mb.Length; i++)
            {
                mb[i].cdFunction = true;
                mb[i].canTakeDamage = false;
                mb[i].canAct = false;
            }
            roundCD--;
            yield return new WaitForSeconds(1.0f);
        }
        // activates shooting, takedamage after the 'GO'
        if (roundCD == -1)
        {
            for (int i = 0; i < mb.Length; i++)
            {
                mb[i].cdFunction = false;
                mb[i].canTakeDamage = true;
                mb[i].canAct = true;
            }

            countdown.text = ("GO");
            yield return new WaitForSeconds(1.0f);
        }
        //hides UI
        countdown.GetComponent<Text>().enabled = false;
    }
}
