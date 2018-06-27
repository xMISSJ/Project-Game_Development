using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card 
{
    public string name;
    public cardType type;
    public cardState state;

    public Sprite image;
    public string description;
    bool selected;

    public GameObject card;
    public GameObject borderCard;
    public GameObject prefab;

    // Card class in which variables are assigned to for the class.
    public  Card(cardType type, string name, string description, Sprite image, cardState state, GameObject prefab)
    {   
        // Assigns for each card a state and makes this the current.
        this.state = state;
        this.type = type;
        this.name = name;
        this.description = description;
        this.image = image;
        this.prefab = prefab;
        
    }

    // Method to add card.
    public void addCard(GameObject c)
    {
        card = c;
        borderCard = card.transform.GetChild(0).gameObject;
        card.GetComponent<cardObjectScript>().targetBodyPart = GetRandomTarget(type);
    }

    private CustomizeCards.bodyParts GetRandomTarget(cardType type)
    {
        switch (type)
        {
            case cardType.arms:
                if (Random.Range(0, 2) == 1)
                {
                    return CustomizeCards.bodyParts.LeftArm;
                }
                else
                {
                    return CustomizeCards.bodyParts.RightArm;
                }
            case cardType.legs:
                return CustomizeCards.bodyParts.Legs;
            case cardType.head:
                return CustomizeCards.bodyParts.Head;
            case cardType.body:
                return CustomizeCards.bodyParts.Body;
            default:
                Debug.Log("Fatal error, Card type was assigned wrong.");
                return CustomizeCards.bodyParts.Legs;
        }
    }

    // Enumerator for the card type.
    public enum cardType
    {
        arms = 2,
        legs = 4,
        body = 0,
        head = 1,
        all = 5
    }

    // Enumerator for the card state.
    public enum cardState
    {
        used = 0,
        unused = 1,
    }
}
