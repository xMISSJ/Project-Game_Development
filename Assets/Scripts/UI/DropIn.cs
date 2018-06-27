using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropIn : MonoBehaviour {

    float velocity;
    float gravity = 19.62f;
    float bounceFriction = 0.3f;
    dropinState move = dropinState.wait;
    int bounces = 2;
    GameManager.gameState roomToGoTo;
    [SerializeField]
    Text winnerText;
    [SerializeField]
    string[] possibleTexts;

    AudioSource audioSource;

    enum dropinState
    {
        wait,
        dropIn,
        fadeOut,
        MoveOut
    }

    private void Start()
    {
        transform.SetAsLastSibling();
        if (GetComponent<AudioSource>() != null)
        audioSource = GetComponent<AudioSource>();
    }

    public void Drop(GameManager.gameState roomToGoTo)
    {
        GetComponent<Image>().color = GameManager.instance.transitionColor;
        move = dropinState.dropIn;
        this.roomToGoTo = roomToGoTo;
        winnerText.enabled = false;
    }

    //Draws a text on the screen
    public void Drop(GameManager.gameState roomToGoTo, int winner)
    {
        winnerText.enabled = true;
        GetComponent<Image>().color = GameManager.instance.transitionColor;
        move = dropinState.dropIn;
        this.roomToGoTo = roomToGoTo;
        GameManager.instance.winnerText = string.Format(possibleTexts[Random.Range(0, possibleTexts.Length)], winner);
        winnerText.text = GameManager.instance.winnerText;
    }

    public void Drop(Color textureColor, GameManager.gameState roomToGoTo)
    {
        if (textureColor != null)
        {
            GetComponent<Image>().color = textureColor;
        }
        move = dropinState.dropIn;
        this.roomToGoTo = roomToGoTo;
        winnerText.enabled = false;
    }

    public void Drop(Color textureColor, GameManager.gameState roomToGoTo, int bounces)
    {
        if (textureColor != null)
        {
            GetComponent<Image>().color = textureColor;
        }
        move = dropinState.dropIn;
        this.roomToGoTo = roomToGoTo;
        this.bounces = bounces;
        winnerText.enabled = false;
    }

    public void Drop(Color textureColor, GameManager.gameState roomToGoTo, int bounces, float bounceFriction)
    {
        if (textureColor != null)
        {
            GetComponent<Image>().color = textureColor;
        }
        move = dropinState.dropIn;
        this.roomToGoTo = roomToGoTo;
        this.bounces = bounces;
        this.bounceFriction = bounceFriction;
        winnerText.enabled = false;
    }

    public void moveOut()
    {
        if (Random.Range(1f, -1f) < 0)
        {
            gravity = -gravity;
        }
        transform.localPosition = new Vector3(0, 0, 0);
        GetComponent<Image>().color = GameManager.instance.transitionColor;
        move = dropinState.MoveOut;
        velocity = 2;
        if (GameManager.instance.winnerText != "")
        {
            winnerText.enabled = true;
            winnerText.text = GameManager.instance.winnerText;
        }
        else
        {
            winnerText.enabled = false;
        }
    }


    void Update () {
        if (move == dropinState.dropIn)
        { velocity -= gravity; 
        transform.localPosition += new Vector3(0, velocity) * Time.deltaTime;

        if (transform.localPosition.y < 0 && bounces > 0)
        {
                audioSource.Play();
            transform.localPosition = new Vector3(0, 0, 0);
            velocity = -(velocity * bounceFriction);
            bounces -= 1;
        }
        else
        if (bounces == 0 && transform.localPosition.y > 1)
        {
            move = dropinState.wait;
            velocity = 0;
            transform.localPosition = new Vector3(0, 0, 0);
            GameManager.instance.transitionGoing = false;           //Makes it so you can spawn another DropIn again.
            GameManager.instance.readyPlayers = 0;           //Resets the amount of players who are ready
                winnerText.enabled = false;
            GameManager.instance.SceneSwitcher(roomToGoTo);
            }
        } else
        if (move == dropinState.MoveOut)
        {
            velocity += gravity;
            transform.localPosition += new Vector3(velocity, 0) * Time.deltaTime;

            if (transform.localPosition.x > GetComponent<RectTransform>().rect.width || transform.localPosition.x < -GetComponent<RectTransform>().rect.width)
            {
                GameManager.instance.winnerText = "";
                move = dropinState.wait;
                velocity = 0;
                GameManager.instance.transitionGoing = false;           //Makes it so you can spawn another DropIn again.
                Destroy(gameObject);
            }
        }

    }
}
