using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //------------------------------------------------------------------------------------------------------------------------------------------//
    //All state-related variables.
    public static GameManager instance;
    public enum gameState { mainMenu, cardMenu, inArena, inbetweenRounds, endOfMatch, endOfMatchMenu };            //stores all possible gamestates in an enum for readability (And extra cookie points?)
    [SerializeField]
    private static gameState currentGameStateEnum = gameState.mainMenu;                                            //stores the current enum, for readability
    [SerializeField]
    private int amountOfStates = Enum.GetNames(typeof(gameState)).Length;                                          //counts the amount of states
    [SerializeField]
    private GameManagerState[] possibleStates = new GameManagerState[Enum.GetNames(typeof(gameState)).Length];     //contains the gamemanagerstates. used to switch the update of the gamemanager. 
    [SerializeField]
    private GameManagerState currentState;                                                                         //current gamestate, whose update is called every frame
    [SerializeField]
    private string[] possibleStateStrings = new string[Enum.GetNames(typeof(gameState)).Length];                   //contains the string names of every scene used. used to switch between the scenes. order MUST correspond to the order
    [SerializeField]
    private bool runStart = true;                                                                                  //of states in the possibleStates[] array. is guaranteed in this class.


    //------------------------------------------------------------------------------------------------------------------------------------------//
    //variables for keeping track of in game things

    //static public List<MasterBody> masterBodies = new List<MasterBody>();
    public ScoreCounter scoreCounter = new ScoreCounter();


    //variables for the card menu
    [SerializeField]
    public bool firstRound;                                         //Is this the first time player select each other's cards
    [SerializeField]
    public bool[] screenActive = new bool[4];                      //Which of 4 screens are currently being used
    [HideInInspector]
    public int[] playerLinks = new int[4];                                          //Keeps track of what ID game screen has which player
    //[HideInInspector]
    public int[] playerCoins = new int[4];                                          //Keeps track of the amount of coins each player has. This is like the one in playerStats, only more easily accessable.
    [SerializeField]
    public Color[] playerColors = new Color[5]              //FINAL worden, en overa
    {
        new Color32(1,1,1,1),                   //Unknown Player
        Color.blue,                             //Player 1
        Color.red,                              //Player 2
        Color.green,                            //Player 3
        Color.yellow                            //Player 4  
    };

    public GameObject[][] prefabArray = new GameObject[5][];

    //These variables have to do with the transition effects:
    public GameObject dropInObject;
    public Color transitionColor;


    //------------------------------------------------------------------------------------------------------------------------------------------//
    //All code needed to spawn the players. We do this in the gamemanager and not one of the subclasses because we want to be able to do this
    //in both the cardmenu as well as the game screen.
    public int amountOfPlayer = 0;
    public int playersAlive;
    public int[] temporaryRanking;
    public GameObject[] playersHolder;
    public GameObject[] positionHolder = new GameObject[4];             //Holds all spawn locations.
    public GameObject playerPrefab;
    public RandomWeaponAssigner randomWeaponAssigner;

    public int readyPlayers;        //Amount of players who are in the green zone.
    public bool transitionGoing = false;             //This makes it so the transition only takes place once. It can only be called once now
    [SerializeField] bool debugprinting;  //Wether we enable some debug messages to be printed.
    public string winnerText;       //Text that is shown when someone wins a round. This needs to be saved throughout the scenes.

    //All music related code
    AudioSource audioSource;
    AudioClip currentMusic;
    [SerializeField]
    AudioLowPassFilter lowPassFilter;
    [SerializeField]
    AudioHighPassFilter highPassFilter;
    public AudioClip[] possibleMusic;
    int lowBottomCutoffFrequency = 1000;
    int lowTopCutoffFrequency = 22000;
    int highBottomCutoffFrequency = 0;
    int highTopCutoffFrequency = 200;

    float pitchTop = 1.2f;
    float pitchMiddle = 1f;
    float goalPitch;                                                //The goal pitch depends on how many players are left. THe music should go faster if only last to players are left.
    float pitchBottom = 0.9f;
    float lerpTimer;
    float lerpScalar = 0.5f;                                        //This changes the speed at which the music fades in or out.


    [SerializeField]
    musicState musicTransition = musicState.fadeOut;                //Wether the lerping is happening.
    public enum musicState
    {
        fadeIn,
        fadeOut,
        wait,
        pitchUp,
        pitchDown
    }


    /*
     * This class is a singleton, a sort of static class.
     * this means that it will try to find other instances of itself and destroy them if it finds one. 
     * It is also static, meaning variables in it will be accesible from anywhere. We use this to track 
     * everything we need to know about players and the game while it is running.
     */

    void Awake()
    {
        instanceCheck();
        goalPitch = pitchMiddle;
        audioSource = GetComponent<AudioSource>();
        scoreCounter = GetComponent<ScoreCounter>();
        DontDestroyOnLoad(this);
        if (debugprinting) debug();
        scoreCounter = this.GetComponent<ScoreCounter>();

    }
    

    void Update()
    {
        if (runStart)
        {
            runStart = false;
            currentState.gameStateStart();
        }
        currentState.gameStateUpdate();
    }


    void FixedUpdate()
    {
        //Code for lerping the music. (Making it transition nicely)
        if (musicTransition != musicState.wait)
        {

            if (musicTransition == musicState.fadeIn)
            {
                lowPassFilter.cutoffFrequency = SongTransition(lowBottomCutoffFrequency, lowTopCutoffFrequency);
                highPassFilter.cutoffFrequency = SongTransition(highTopCutoffFrequency, highBottomCutoffFrequency);
                audioSource.pitch = SongTransition(pitchBottom, goalPitch);

            }
            else
            if (musicTransition == musicState.fadeOut)
            {
                lowPassFilter.cutoffFrequency = SongTransition(lowTopCutoffFrequency, lowBottomCutoffFrequency);
                highPassFilter.cutoffFrequency = SongTransition(highBottomCutoffFrequency, highTopCutoffFrequency);
                audioSource.pitch = SongTransition(goalPitch, pitchBottom);
            }
            else if (musicTransition == musicState.pitchUp)
            {
                audioSource.pitch = SongTransition(pitchBottom, goalPitch);
            }
            else if (musicTransition == musicState.pitchDown)
            {
                audioSource.pitch = SongTransition(goalPitch, pitchBottom);
            }
            lerpTimer += Time.deltaTime * lerpScalar;

            //sets the timer ready for next use.
            if (lerpTimer >= 1)
            {
                musicTransition = musicState.wait;
                lerpTimer = 0;
            }
        }
    }

    //checks if all players have locked in their choices and are ready to transition to the game
    public void checkIfEveryoneIsReady()
    {
        if (readyPlayers == amountOfPlayer && transitionGoing == false)
        {
            transitionGoing = true;
            int randomPlayer = UnityEngine.Random.Range(1, AmountOfPlayers() + 1);
            transitionToScene(playerColors[randomPlayer], gameState.inArena, musicState.fadeIn);
        }
    }

    //fades from one scene to another with a dropinObject to mask it
    public void transitionToScene(Color transitionColor, gameState goToState, musicState musicTransition)
    {
        GameObject tempGameObject = Instantiate(dropInObject, GameObject.FindGameObjectWithTag("Canvas").transform);
        this.transitionColor = transitionColor;
        tempGameObject.GetComponent<DropIn>().Drop(goToState);
        this.musicTransition = musicTransition;
    }

    //methods used to switch between scenes
    public void SceneSwitcher(gameState stateToSwitchTo)
    {
        string stateString = possibleStateStrings[(int)stateToSwitchTo];
        SceneManager.LoadScene(stateString);
        currentGameStateEnum = stateToSwitchTo;
        currentState = possibleStates[(int)stateToSwitchTo];
        runStart = true;
    }

    //debug method. prints out information to the console. only used if the debug bool is set to true
    private void debug()
    {
        Debug.Log(Enum.GetNames(typeof(gameState)).Length);
        Debug.Log(Enum.GetUnderlyingType(typeof(gameState)));
        Debug.Log((int)currentGameStateEnum);
    }
    
    //makes sure that no other instances exists. also makes sure the currentstate isn't null.
    private void instanceCheck()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            Debug.Log("Destroyed this instance of the gameManager");
        }
        else
        {
            instance = this;
            Debug.Log("This instance of the gamemanager is now the gamemanager because no other instance existed");
        }

        if (currentState == null)
        {
            currentState = possibleStates[0];
        }
    }

    //checks if any objects taggged with spawn are present and then spawns the players there
    public void SpawnPlayers()
    {
        // Fetches the name of the current scene.
        string currentScene = SceneManager.GetActiveScene().name;
        // Finding all spawn objects, by using their tag.
        positionHolder = GameObject.FindGameObjectsWithTag("Spawn");
        if (currentScene == "Endscreen")
        {
            // Sort the array so that spawn1, spawn2 enz. are in order. 
            // This because it's possible that one of the spawns are sorted in the list earlier than the other spawn.
            GameObject[] sorted = new GameObject[positionHolder.Length];
            for (int i = 1; i <= sorted.Length; i++)
            {
                foreach (GameObject ph in positionHolder)
                {
                    // This is so the players will be placed in the right order on the stages.
                    // sorted[0] here searches Spawn 1.
                    if (ph.name == "Spawn" + i)
                    {
                        sorted[i - 1] = ph;
                        break;
                    }
                }
            }
            // This is a clone so positionHolder doesn't refer to sorted.
            positionHolder = (GameObject[])sorted.Clone();

            // Sorts the players on ranking (who finishes first, second etc.).
            int[,] order = scoreCounter.PositionInRanks();

            for (int i = 0; i < order.GetLength(0); i++)
            {
                positionHolder[i] = sorted[order[i, 0]];
            }
        }
        // Holds the player gameobjects.
        playersHolder = new GameObject[amountOfPlayer];
        temporaryRanking = new int[amountOfPlayer];
        playersAlive = amountOfPlayer;
        GameObject[] players = new GameObject[amountOfPlayer];

        // Spawns every player at a certain position, which is already assigned in the GameObject array itself.
        for (int i = 0; i < amountOfPlayer; i++)
        {
            GameObject tempGameObject = players[i] = Instantiate(playerPrefab, positionHolder[i].transform.position, positionHolder[i].transform.rotation);
            tempGameObject.GetComponent<MasterBody>().playerPrefix = playerLinks[i];        // Sees what controller is part of which ID and assigns it to the player.
            tempGameObject.GetComponent<MasterBody>().playerID = i;

            // Only display countdown when not in the card menu.
            if (currentScene != "CardMenu" && currentScene != "Endscreen")tempGameObject.GetComponent<MasterBody>().StartCountDown();

            // If the weapon isn't assigned, insantiate the randomWeaponAssigner.
            if (randomWeaponAssigner != null)
            {
                randomWeaponAssigner.intantiate();
                for (int j = 0; j < 5; j++)
                {
                    // Puts the player together with random body parts.
                    tempGameObject.GetComponent<MasterBody>().prefabArray[j] = randomWeaponAssigner.randomPrefabs[i, j];
                }
            }

            tempGameObject.GetComponent<MasterBody>().prefabArray = (GameObject[])prefabArray[i].Clone();
            playersHolder[i] = tempGameObject; // Fills the playersHolder array with temGameObjects which were made above.
            tempGameObject.name = "Player" + (i + 1);
        }
    }

    //called when a player is killed. updates the amount of players alive and handles the end of the round 
    public void updatePlayer(MasterBody deadPlayer)
    {
        playersAlive -= 1;
        temporaryRanking[playersAlive] = deadPlayer.playerID;
        Debug.Log("player" + temporaryRanking[playersAlive] + " Just finished in " + playersAlive + "th place");
        //If there are two people left, speed up the music
        if (playersAlive == 2)
        {
            goalPitch = pitchTop;
            musicTransition = musicState.pitchUp;
        }
        else
        {
            goalPitch = pitchMiddle;
        }

        int winnerID = 0;
        // If there is only survivor left, loops through all the players and checks which is left alive. assigns that player as the winner.
        if (playersAlive == 1)
        {
            foreach (GameObject player in playersHolder)
            {
                if (player.GetComponent<MasterBody>().isAlive)
                {
                    winnerID = player.GetComponent<MasterBody>().playerID;
                    scoreCounter.updateScore(winnerID, "win");
                    temporaryRanking[0] = winnerID;
                    //Sets the color of the level transition:
                    transitionColor = playerColors[winnerID + 1];         //changes the color of the transitionscreen to match the color of the winning player. +1 because the first entry is 
                    break;
                }
            }
            if (transitionGoing == false)
            {
                transitionGoing = true;

                GameObject tempGameObject = Instantiate(dropInObject, GameObject.FindGameObjectWithTag("Canvas").transform);
                tempGameObject.GetComponent<DropIn>().Drop(gameState.inbetweenRounds, winnerID + 1);
                goalPitch = pitchMiddle;
                musicTransition = musicState.fadeOut;
            }
        }
    }

    //returns the amount of players currently locked into the game.
    public int AmountOfPlayers()
    {
        int amountOfPlayers = 0;

        for (int i = 0; i < screenActive.Length; i++)
        {
            if (screenActive[i] == true)
            {
                amountOfPlayers++;
            }
        }
        return amountOfPlayers;
    }

    //Music Functions we can call from anywhere
    public void SongPlay(int songNumber, bool loop)
    {
        //only change the song if it is not the same song.
        if (currentMusic != possibleMusic[songNumber])
        {
            audioSource.Stop();
            currentMusic = possibleMusic[songNumber];
            audioSource.clip = currentMusic;
            audioSource.Play();
            audioSource.loop = loop;
        }
    }

    
    public void SongPause()
    {
        audioSource.Pause();
    }
    public void SongUnPause()
    {
        audioSource.UnPause();
    }
    public void SongStop()
    {
        audioSource.Stop();
    }

    //transitions smoothly between volumes. used to make the music less loud in the menu
    private float SongTransition(float bottomCutoffFrequency, float topCutoffFrequency)
    {
        if (lerpTimer >= 1)
        {
            return topCutoffFrequency;
        }
        return Mathf.Lerp(bottomCutoffFrequency, topCutoffFrequency, lerpTimer);
    }
    public void SongTransition(musicState fadeOutState)
    {
        musicTransition = fadeOutState;
    }

}
