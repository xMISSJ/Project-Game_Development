using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardMenuState : GameManagerState
{

    public CustomizeCards[] startScreen = new CustomizeCards[4];
    public int amountOfCards = 3;                                   //Amount of cards that will be spawned.
    [HideInInspector]
    public bool[] playerActive = new bool[5];                       //Which of 4 controllers are known to be active. This is set to true once the controller in question pressses the start button.
    [SerializeField]
    int playersRequiredToStart = 2;                                 //How many players you need before you can start a new game.

    public GameObject[] defaultPrefabs;                             //The standard items the player begins with.
    List<int> availablePlayers = new List<int>();

    //The two objects are added in the inspector. So we can enable them and disable them when everyone has readied up.
    GameObject playable;
    GameObject menu;
    GameObject helpMenu;
    GameObject textBanner;                                          //The banner that shows if you want to press start to select cards.

    bool pressedStart;
    int maxPlayers = 5;                                             //The max amount of players that we check input for.
    // Use this for initialization
    public override void gameStateStart()
    {
        //This code limits the gamemanager in what scenes it could change to.
        availableTransitions = new GameManager.gameState[1];
        availableTransitions[0] = GameManager.gameState.inArena;
        //-----
        base.gameStateStart();

        //if this is the first round give the players the standard prefabs from a list.
        if (GameManager.instance.firstRound)
        {
            for (int i = 0; i < 5; i++)
            {
                GameManager.instance.prefabArray[i] = (GameObject[])defaultPrefabs.Clone();
            }
        }

        //Because the Gamemanager is a persistant object, We have to reassign all objects whenever the cardmenu gets loaded.
        if (textBanner == null)
        {
            textBanner = GameObject.Find("PressStartBanner");
            textBanner.SetActive(false);
        }
        if (playable == null)
        {
            playable = GameObject.Find("Playable");
            playable.SetActive(false);
        }
        if (helpMenu == null)
        {
            helpMenu = GameObject.Find("TutorialBanner");
            if (GameManager.instance.firstRound)
            { helpMenu.GetComponent<TutorialBanner>().moveDown(); } //if it is the first round, it will automatically show the controls.
        }
        if (menu == null)
        {
            menu = GameObject.Find("Menu");
        }
        for (int i = 0; i < startScreen.Length; i++)
        {
            if (startScreen[i] == null)
            {
                startScreen[i] = GameObject.Find("Cards" + i).GetComponent<CustomizeCards>();
            }
        }
        isThisFirstRound(); //If it is the first round it will skip the part where players can enter the game, and it will go straight to the card menu.

        //Plays the game's music when it starts.
        if (GameManager.instance.firstRound == true)
        {
            //Plays the main song.
            GameManager.instance.SongPlay(1, true);
            GameManager.instance.SongTransition(GameManager.musicState.fadeOut);
        }
    }

    // Update is called once per frame
    public override void gameStateUpdate()
    {
        base.gameStateUpdate();
        {

            //Waiting for player input
            pressedStart = false;
            int i = 1;
            // If the menu isn't showing the help tutorial thing, then receive input.
            if (!helpMenu.GetComponent<TutorialBanner>().isMenuActivated())
            {
                while (i < maxPlayers)   //Checks for all players/controllers the input
                {
                    //This part looks for any of the players input
                    if (Input.GetButtonDown("P" + (i) + "_Confirm"))
                    {
                        PlayerConfirms(i);   
                    }
                    else
                    if (Input.GetButtonDown("P" + (i) + "_Cancel"))
                    {
                        if (playerActive[i] == true)
                        {
                            PlayerCancels(i);
                        }
                    }
                    else
                    //If enough players have joined and one of them presses start
                    if (Input.GetButtonDown("StartButton") && pressedStart == false)
                    {
                        if (playerActive[i] == true)
                        {
                            pressedStart = true;            //Used this bool to put a delay on the check. Just so this does not get executed for other players if someone else already pressed it.
                            PlayerPressesStart();                //Opens the card menu for everyone.
                            break;
                        }
                    }
                    i++;
                }
            }
        }

    }

    public void isThisFirstRound()
    {
        //Automatically closes the press a to start menu when it's not the first round.
        if (!GameManager.instance.firstRound)
        {
            assignTarget();         //Assigns a random target for each player.
            ChangeTargetBanner();
            for (int i = 0; i < startScreen.Length; i++)
            {
                if (GameManager.instance.screenActive[i])
                {
                    playerActive[GameManager.instance.playerLinks[i]] = true;
                    startScreen[i].SetPrefix(GameManager.instance.playerLinks[i]);
                    startScreen[i].play = CustomizeCards.playerState.ChooseCard;          //Sets player state to selecting card.
                    startScreen[i].amountOfCards = amountOfCards;
                    startScreen[i].SpawnCards(Card.cardType.all);
                    startScreen[i].startScreen.SetActive(false);
                    startScreen[i].playerHand.SetActive(true);
                    startScreen[i].closedForBusiness.SetActive(false);
                }
                else if (!GameManager.instance.screenActive[i])
                {
                    startScreen[i].play = CustomizeCards.playerState.Disabled;          //Sets player state to selecting card.
                    startScreen[i].closedForBusiness.SetActive(true);
                }
            }

        }
        else            //If this is the first round, then change all colors to the player colors.
        {

            for (int i = 0; i < startScreen.Length; i++)
            {
                startScreen[i].startScreen.GetComponent<Image>().color = GameManager.instance.playerColors[i + 1];
            }
        }
    }

    public void PlayerConfirms(int playerPrefix)  
    {
        for (int i = 0; i < startScreen.Length; i++)
        {
            if (GameManager.instance.firstRound)         //You can only add players in the first round
            {
                //Adds player and assigns him a screen if he is not yet in.
                if (startScreen[i].play == CustomizeCards.playerState.Wait && playerActive[playerPrefix] == false)
                {
                    startScreen[i].playerPrefix = playerPrefix;
                    GameManager.instance.playerLinks[i] = playerPrefix;                //Sets player ID equal to the available player slot. So we have a refrence for which controller belongs to which player ID.
                    startScreen[i].playerHand.GetComponent<controlCursor>().playerPrefix = playerPrefix;
                    startScreen[i].play = CustomizeCards.playerState.PressStartToBeginCards;
                    GameManager.instance.screenActive[i] = true;
                    playerActive[playerPrefix] = true;

                    if (AmountOfPlayers() >= playersRequiredToStart)
                    {
                        textBanner.SetActive(true);
                    }

                    startScreen[i].startScreen.GetComponentInChildren<Text>().text = "Player " + (i + 1) + " is ready to Shuffle!";

                    break;  //Stops this loop so you dont assign anymore screens after this one for this loop. 

                }
            }

            if (startScreen[i].play == CustomizeCards.playerState.ChooseCard && startScreen[i].playerPrefix == playerPrefix && startScreen[i].gotSelected == true)        //Checks who pressed the button
            {
                startScreen[i].confirmButton.SetActive(true);
                startScreen[i].shuffleButton.SetActive(false);
                startScreen[i].play = CustomizeCards.playerState.Ready;
                break;
            }
        }
    }

    public void PlayerCancels(int playerPrefix)                     //this is what gets called when a player pressesthe cancel button.
    {
        for (int i = 0; i < startScreen.Length; i++)                //Checks for each screen    
        {
            if (startScreen[i].playerPrefix == playerPrefix)        //If the player for that screen was the one who pressed the button.             
            {
                if (GameManager.instance.firstRound)                //You can only remove a player if you're in the first round.
                {
                    //Removes player and resets their screen.
                    if (startScreen[i].play == CustomizeCards.playerState.PressStartToBeginCards)
                    {
                        startScreen[i].startScreen.SetActive(true);
                        GameManager.instance.screenActive[i] = false;
                        playerActive[playerPrefix] = false;             //Removes player out of list of players.
                        startScreen[i].startScreen.GetComponentInChildren<Text>().text = "Press X to Start.";
                        GameManager.instance.playerLinks[i] = 0;

                        if (AmountOfPlayers() < playersRequiredToStart)
                        {
                            textBanner.SetActive(false);
                        }
                        startScreen[i].play = CustomizeCards.playerState.Wait;

                    }
                    //If the player is already in the card select screen. If it is not the first round it should kick everyone out!
                    else if (startScreen[i].play == CustomizeCards.playerState.ChooseCard)
                    {

                        for (int j = 0; j < startScreen.Length; j++)     //Changes all screens out of the menus.
                        {
                            if (startScreen[j].play == CustomizeCards.playerState.ChooseCard || startScreen[j].play == CustomizeCards.playerState.Ready || startScreen[j].play == CustomizeCards.playerState.PressedConfirm)            //All menus that are not the first two will be returned to one of the first two.
                            {
                                startScreen[j].readyScreen.SetActive(false);
                                startScreen[j].shuffleButton.SetActive(false);
                                startScreen[j].confirmButton.SetActive(false);
                                startScreen[j].startScreen.SetActive(true);
                                startScreen[j].playerHand.SetActive(false);
                                startScreen[j].startScreen.GetComponentInChildren<Text>().text = "Player " + (j + 1) + " is ready to Shuffle!";
                                startScreen[j].play = CustomizeCards.playerState.PressStartToBeginCards;
                                startScreen[j].DeleteCards();           //Clears the cards
                            }
                            else
                            {
                                startScreen[j].startScreen.GetComponentInChildren<Text>().text = "Press X to Start.";
                                startScreen[j].play = CustomizeCards.playerState.Wait;
                            }
                            startScreen[j].closedForBusiness.SetActive(false);

                        }

                        if (AmountOfPlayers() >= playersRequiredToStart)
                        {
                            textBanner.SetActive(true);
                        }
                    }
                }
                //Player cancels the selected card
                if (startScreen[i].play == CustomizeCards.playerState.Ready)
                {
                    startScreen[i].shuffleButton.SetActive(true);
                    startScreen[i].play = CustomizeCards.playerState.ChooseCard;
                    startScreen[i].confirmButton.SetActive(false);
                    startScreen[i].selectedCard.Deselect(); //Unselect the card you got selected

                }
                break;  //Stops this loop so you dont assign anymore screens after this one.
            }
        }
    }

    //Checks if the game can be started.
    public void CheckIfEveryoneIsReady()
    {
        int amountOfPlayers = 0;
        int amountOfReadyPlayers = 0;

        for (int i = 0; i < startScreen.Length; i++)
        {
            if (GameManager.instance.screenActive[i])
            {
                amountOfPlayers++;
                if (startScreen[i].play == CustomizeCards.playerState.PressedConfirm)
                {
                    amountOfReadyPlayers++;
                }
            }
        }

        //If all screens are ready, then show the playable scene.
        if (amountOfPlayers == amountOfReadyPlayers)
        {
            menu.SetActive(false);
            playable.SetActive(true);

            //Assigns the weapons to each player.
            for (int i = 0; i < startScreen.Length; i++)
            {
                if (GameManager.instance.screenActive[i] && startScreen[i].play == CustomizeCards.playerState.PressedConfirm)
                {
                    //                                              targetplayer - 1 because target player is a number from 1-4 but the array starts at 0.
                    GameManager.instance.prefabArray[startScreen[i].targetPlayer - 1][startScreen[i].bodyPartTranslater[startScreen[i].selectedCard.targetBodyPart]] = startScreen[i].selectedCard.GetComponent<ItemInfo>().prefab;

                    //Now since we end the first round, we set it to false. Next time the card screen is loaded, it will not wait for new controllers
                    GameManager.instance.firstRound = false;
                }
            }

            //Spawns the physical players
            GameManager.instance.amountOfPlayer = AmountOfPlayers();
            GameManager.instance.SpawnPlayers();
        }
    }

    public void PlayerPressesStart()     //Once enough players are in and one of them presses start, this gets called.
    {
        if (textBanner.activeSelf == true)
        {
            assignTarget();         //Assigns a random target for each player.
            ChangeTargetBanner();
            for (int i = 0; i < startScreen.Length; i++)
            {
                if (startScreen[i].play == CustomizeCards.playerState.PressStartToBeginCards)
                {
                    startScreen[i].play = CustomizeCards.playerState.ChooseCard;
                    startScreen[i].amountOfCards = amountOfCards;
                    textBanner.SetActive(false);
                    startScreen[i].startScreen.SetActive(false);
                    startScreen[i].playerHand.SetActive(true);
                    startScreen[i].shuffleButton.SetActive(true);
                    startScreen[i].SpawnCards(Card.cardType.all);
                }
                else
                {
                    startScreen[i].play = CustomizeCards.playerState.Disabled;
                    startScreen[i].closedForBusiness.SetActive(true);
                }
            }


        }

    }

    private void ChangeTargetBanner()           //Changes the color and the text of the banner that shows your target.
    {
        for (int i = 0; i < startScreen.Length; i++)
        {
            //Changes the banner text to say who you are.
            startScreen[i].PlayerBanner.GetComponentInChildren<Text>().text = "You are Player " + (i + 1);
            startScreen[i].PlayerBanner.GetComponent<Image>().color = GameManager.instance.playerColors[i + 1];

            if (i + 1 > 2)  // i = 3  or i =4 then the colors are too light for white letters
            {
                startScreen[i].PlayerBanner.GetComponentInChildren<Text>().color = Color.black;
            }
            else { startScreen[i].PlayerBanner.GetComponentInChildren<Text>().color = Color.white; }

            //Changes the banner text to say who your target it.
            startScreen[i].TargetBanner.GetComponentInChildren<Text>().text = "You are selecting a body part for Player " + startScreen[i].targetPlayer;
            startScreen[i].TargetBanner.GetComponent<Image>().color = GameManager.instance.playerColors[startScreen[i].targetPlayer];


            if (startScreen[i].targetPlayer > 2)  // i = 3  or i =4 then the colors are too light for white letters, so change the colors to black. It is a magic number since it will never get changed and it is just a purely cosmetic thing.
            {
                startScreen[i].TargetBanner.GetComponentInChildren<Text>().color = Color.black;
            }
            else { startScreen[i].TargetBanner.GetComponentInChildren<Text>().color = Color.white; }

            //Changes hand color
            startScreen[i].playerHand.GetComponent<Image>().color = GameManager.instance.playerColors[i + 1];
        }
    }

    //Gives all players another player as a target.
    private void assignTarget()
    {
        availablePlayers = new List<int>();
        //getting the possible targets
        for (int i = 0; i < startScreen.Length; i++)
        {
            if (GameManager.instance.screenActive[i] && GameManager.instance.playerLinks[i] != 0)      //If the screen is in use, and it has a player attached to it.
            {
                availablePlayers.Add(i + 1);
            }
        }
        bool allowed;       //Is the combination of players who got each other allowed. So a person doesn't get his self.
        do
        {
            allowed = true;
            List<int> handOutList = new List<int>(availablePlayers);
            //assigning the targets
            for (int i = 0; i < availablePlayers.Count; i++)
            {
                int value;
                //Shuffle Player numbers in the availablePlayers List.
                value = Random.Range(0, handOutList.Count);
                startScreen[i].targetPlayer = handOutList[value]; //Assigns the target player
                if (handOutList[value] == availablePlayers[i]) { allowed = false; break; }
                handOutList.RemoveAt(value);
            }
        } while (allowed == false);
    }

    //Gets the amount of player that are in the menu.
    public int AmountOfPlayers()
    {
        int amountOfPlayers = 0;

        for (int i = 0; i < GameManager.instance.screenActive.Length; i++)
        {
            if (GameManager.instance.screenActive[i] == true)
            {
                amountOfPlayers++;
            }
        }
        return amountOfPlayers;
    }
}


