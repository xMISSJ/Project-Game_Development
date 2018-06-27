using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class playerStats : MonoBehaviour {
    public GameObject UICoin;                   // coin that is displayed on screen when player picks up a coin
    public GameObject UIhealthBar;              // healthbar that is displayed on taking damage 
    GameObject canvas;
    public List<playerData> players;            // list of players
    public GameObject playerDataUI;             // info card with player info, always on display
    GameObject[] UIlifeBar;                     // lifebar on infocard

    [HideInInspector]
    public bool[] showingPlayerHealth;          //This keeps track of which player's health are being shown.

    // stores information about each player (mostly coins)
    public class playerData
    {
        public MasterBody masterBody;
        public GameObject UI;
        public int coins = 0;
        public int playerNr = 0;
        Text coinsText;
        RectTransform healtBar;      

        float cardTransp = 55f / 255f;
        
        Image panel;
        public playerData(MasterBody mb, GameObject ui, int receivedPlayerNummer)
        {
            masterBody = mb;
            UI = ui;
            UI.transform.Find("playerName").GetComponent<Text>().text = masterBody.name; // set name on info card on screen

            this.playerNr = receivedPlayerNummer;
            UI.GetComponent<RectTransform>().anchorMax = new Vector2(0.2f + (0.2f * playerNr), 0.2f);
            UI.GetComponent<RectTransform>().anchorMin = new Vector2(0.2f + (0.2f * playerNr), 0.2f);
       
            coinsText = UI.transform.Find("coinValue").GetComponent<Text>();
            healtBar = UI.transform.Find("healthBar").transform.Find("health").GetComponent<RectTransform>(); 

            panel = UI.transform.Find("Panel").GetComponent<Image>();
            
            // set color of info panel based on player color
            panel.color = GameManager.instance.playerColors[playerNr+1];
            panel.transform.Find("Panel background").GetComponent<Image>().color = new Color(GameManager.instance.playerColors[playerNr].r, GameManager.instance.playerColors[playerNr].g, GameManager.instance.playerColors[playerNr].b, cardTransp);
            coins = GameManager.instance.playerCoins[playerNr]; // update coins so that its equal to gameManager
            update();
        }


        // called every frame
        public void update()
        {
            // update coins in gameManager
            GameManager.instance.playerCoins[playerNr] = coins;
            //update healthbar and coins on screen
            healtBar.sizeDelta = new Vector2((98 / (float)masterBody.basehealth)* (float)masterBody.health, healtBar.sizeDelta.y);
            healtBar.anchoredPosition = new Vector2(-((98-((98/(float)masterBody.basehealth)* (float)masterBody.health))/2),0);
            coinsText.text = coins.ToString();
        }


        //add coins 
        public int addCoin(int value)
        {
            coins+= value;
            if(coins < 0)
            {
                // returns all below zero and makes coins 0 if below zero
                int returnval = coins;
                coins = 0;
                update();
                return returnval; 
            }
            update();
            return coins;
        }
    } 
    
    
    // voor adding a amount of coins to a player by name
    public void addCoin(string player, int value)
    {
        foreach (playerData p in players)
        {
            if (p == null || p.masterBody == null)  //Checks if the player still exists
            {
                continue;
            }
            // find player by name
            if (p.masterBody.name == player)
            {

                p.addCoin(value); // add coins to player
                // display coin on screen on pickup
                GameObject UICoin = Instantiate(this.UICoin, canvas.transform);     // create new coin on canvas
                UICoin.GetComponentInChildren<Text>().text = p.coins.ToString();    // change text to reflect the amound the player has
                UICoin.GetComponent<RectTransform>().anchorMin = Vector2.zero;      
                UICoin.GetComponent<RectTransform>().anchorMax = Vector2.zero;      
                Vector3 pos = Camera.main.WorldToScreenPoint(p.masterBody.transform.position + Vector3.up * 2); // position UIcoin on canvas above playerhead
                pos.z = 0;
                // set the position based on pos
                UICoin.GetComponent<RectTransform>().SetPositionAndRotation(pos, Quaternion.identity); 

                }
            
        }

    }
   
    //This code is for showing a player's healthbar above his or her head.
    public bool showHealth(GameObject player, int prefix)
    {
        //RectTransform drawHealthHud; // for player heatlh above head
        MasterBody mb = player.GetComponent<MasterBody>();

        if (canvas != null)
        {
            if (showingPlayerHealth[prefix] == false)
            {
                showingPlayerHealth[prefix] = true;         //Sets it so this script knows the players health is already being shown.

                UIlifeBar[prefix] = Instantiate(this.UIhealthBar, canvas.transform);    // add healthbar to canvas

                UIlifeBar[prefix].GetComponent<UILifeBar>().playerStats = this;         // give player info to healthbar
                UIlifeBar[prefix].GetComponent<UILifeBar>().mb = mb; // masterbody gets assigned to lifebar
                UIlifeBar[prefix].GetComponent<UILifeBar>().playerPrefix = prefix;

                UIlifeBar[prefix].GetComponent<RectTransform>().anchorMin = Vector2.zero;
                UIlifeBar[prefix].GetComponent<RectTransform>().anchorMax = Vector2.zero;

                Vector3 pos = Camera.main.WorldToScreenPoint(player.transform.position + Vector3.up * 2); // position of player relative to screen
                pos.z = 0;

                UIlifeBar[prefix].GetComponent<RectTransform>().SetPositionAndRotation(pos, Quaternion.identity);
            }
            else
            {
                UIlifeBar[prefix].GetComponent<UILifeBar>().livespan = 0;
            }
        }
        return true;
    }

    // Use this for initialization
    public void Init (GameObject[] p) {
        players = new List<playerData>();
        showingPlayerHealth = new bool[5]; //This checks wether the life bar is already displayed
        UIlifeBar = new GameObject[5]; //This keeps track of all uilifebars

        canvas = GameObject.FindGameObjectWithTag("Canvas");
        int i = 0;

        // p holds all players
        foreach (GameObject player in p)
        {
            MasterBody mb = player.GetComponent<MasterBody>();
            // add a entry in player data for each player
            playerData pd = new playerData(mb, Instantiate(playerDataUI,canvas.transform), mb.playerID);
            players.Add(pd);
            pd.coins = GameManager.instance.playerCoins[mb.playerID];
            i++;
        }
	}
	
	// Update is called once per frame
	public void update () {
        try
        {
            if (players != null)
            {
                foreach (var p in players)
                {
                    p.update();
                }
            }
        }
        catch { }
      
    }
}
