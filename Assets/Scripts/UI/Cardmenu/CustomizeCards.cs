using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CustomizeCards : MonoBehaviour
{
    public int playerPrefix;                                //This determines which player is controlling this menu.
    public int targetPlayer = 0;                            //The player who's item you change.
    public bool gotSelected;                                //Wether the cursor is on a card right now.
    public int ShuffleCost;                                 //How much money will one shuffle cost?
    public Dictionary<bodyParts, int> bodyPartTranslater;
    public GameObject changePlayer;                         //The object that assigns the player his new attributes.
    public List<Card> cards;                                // List in which contains items with card images.
    Transform startPosition;                                // The location (start) where the cards will be added.
    public playerState play;                                //Checks what phase the player is at. (In game? Selected weapon? Confirming option? Ready to play)
    public GameObject cardPrefab;                           // Variable to store the button of the card in.
    [SerializeField]
    ItemCatalogue itemsList;
    int cardCount = 0;                                      //Amount of cards that have been spawned
    public cardObjectScript selectedCard;                   //What card your cursor got selected.
    private List<GameObject> gottenItems;
    public int ID;                                                         //The ID is used to when calling an item catalogue.

    [SerializeField]
    int rangeModifier = 8;                //This modifier will be divided by the amount of players. This will decide how big someones tier range is (So what powerlevels they can get).

    //All objects that we need to refer to later on. This is all just fancy menu stuff.
    public GameObject showDescription;
    public GameObject startScreen;
    public GameObject playerHand;
    public GameObject closedForBusiness;
    public GameObject TargetBanner;
    public GameObject PlayerBanner;
    public GameObject shuffleButton;
    public GameObject confirmButton;
    public GameObject readyScreen;
    public GameObject magnifier;
    ///////////////////////////////////////////////////////////////////////////////////

    public int amountOfCards;                               //How many cards will be given to this player.
    int currentSelected = 0;

    public enum bodyParts
    {
        LeftArm,
        RightArm,
        Head,
        Body,
        Legs
    }

    public enum playerState
    {
        Wait,                //Waiting for first button press to start playing
        PressStartToBeginCards,              //Selecting enemy cards
        ChooseCard,             //Confirming selection
        Ready,                //Player is ready to play and has nothing else left to do.
        Disabled,                 //No player joined in this stage.
        PressedConfirm,

    }

    // Use this for initialization.
    void Start()
    {
        //Creates a dictionary so we can use the bodyParts enum instead of the ID numbers of each body part.
        bodyPartTranslater = new Dictionary<bodyParts, int>();
        bodyPartTranslater.Add(bodyParts.Head, 1);
        bodyPartTranslater.Add(bodyParts.RightArm, 2);
        bodyPartTranslater.Add(bodyParts.LeftArm, 3);
        bodyPartTranslater.Add(bodyParts.Legs, 4);
        bodyPartTranslater.Add(bodyParts.Body, 0);

        //Creates a dictionary for all the input the player can have.

        cards = new List<Card>();
        gottenItems = new List<GameObject>();
        startPosition = transform;
        play = playerState.Wait;
    }

    public void ShuffleButton()       //Shuffles the player's cards.
    {
        DeleteCards();
        SpawnCards(Card.cardType.all);
    }

    public void SpawnCards(Card.cardType spawnCardType)
    {
        cards.Clear();
        gottenItems.Clear();            //Clears the list of gotten items. So you can get all cards again.
        //Randomly Selecting a card:
        for (int i = 0; i < amountOfCards; i++)
        {
            GameObject cardItem;
            cardItem = itemsList.getItemObject(targetPlayer, (int)(rangeModifier / GameManager.instance.AmountOfPlayers()), gottenItems);        //The second parameter is the range the itemlist has too look at.
            gottenItems.Add(cardItem);
            ItemInfo info = cardItem.GetComponent<ItemInfo>();
            cards.Add(new Card(info.itemType, info.itemName, info.itemDescription, info.itemIcon, Card.cardState.unused, cardItem));
        }
        cardsInWorld();
    }

    // Method to spawn the cards on the screen.
    private void cardsInWorld()
    {
        foreach (Card card in cards)
        {
            // Checks whether the state of the card is used or not.
            if (card.state == Card.cardState.unused)
            {
                cardCount++;
                // Makes a new button object and instantiate this object as button.
                GameObject placedCard = Instantiate(cardPrefab);
                prepareCard(card, placedCard);
              
            }
        }
    }

    //Removes the physical cards.
    public void DeleteCards()
    {
        cardCount -= transform.childCount;
        cards.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }


    }

    //Assigns a controller to the hand
    public void SetPrefix(int prefix)
    {
        playerPrefix = prefix;
        playerHand.GetComponent<controlCursor>().playerPrefix = prefix;
    }
    //Shows the item description.
    public void ShowDescription(string name, string description)
    {
        showDescription.GetComponentInChildren<Text>().text = "<size=60><b>Titel:</b> " + name + "</size><size=50><b>\nDescription:</b> <i>" + description + "</i></size>";
        showDescription.SetActive(true);
    }

    public void HideDescription()
    {
        showDescription.SetActive(false);
    }

    // Change card parameters.
    private void prepareCard(Card card, GameObject cardObject)
    {
        ItemInfo info = cardObject.GetComponent<ItemInfo>();
        info.itemName = card.name;
        info.itemDescription = card.description;
        info.itemType = card.type;
        info.itemIcon = card.image;
        info.prefab = card.prefab;
        cardObject.GetComponent<Image>().sprite = card.image;
        cardObject.GetComponent<cardObjectScript>().customizeCards = this;

        RectTransform rectangle;
        cardObject.transform.SetParent(gameObject.transform, false);
        cardObject.transform.SetAsFirstSibling();
        card.addCard(cardObject.gameObject);
        rectangle = cardObject.gameObject.GetComponent<RectTransform>();


        rectangle.anchorMin = new Vector2(((1f / amountOfCards) * cardCount) - (0.5f / amountOfCards), 0.5f);
        rectangle.anchorMax = new Vector2(((1f / amountOfCards) * cardCount) - (0.5f / amountOfCards), 0.5f);

        // r.anchorMax = new Vector2((1f / amountOfCards * i), 0.5f);
        rectangle.localPosition = new Vector3(rectangle.localPosition.x, 0, rectangle.localPosition.z);
    }
}