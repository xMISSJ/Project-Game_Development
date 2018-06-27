using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfMatchState : GameManagerState {

    float timeToSwitch;
    // Use this for initialization
    public override void gameStateStart()
    {
        availableTransitions = new GameManager.gameState[1];
        //-----
        availableTransitions[0] = GameManager.gameState.endOfMatchMenu;
        //-----
        base.gameStateStart();
        GameManager.instance.SpawnPlayers();
        timeToSwitch = 5;
    }

    // Update is called once per frame
    public override void gameStateUpdate()
    {
       
        timeToSwitch -= Time.deltaTime;
        Debug.Log(timeToSwitch);
        if (timeToSwitch <0)
        {
            Debug.Log("hi");
            GameManager.instance.transitionColor = GameManager.instance.playerColors[Random.Range(0, GameManager.instance.playerColors.Length)];
            GameManager.instance.transitionToScene(GameManager.instance.playerColors[1], GameManager.gameState.cardMenu, GameManager.musicState.fadeOut);
            GameManager.instance.playersHolder = new GameObject[4];
            GameManager.instance.firstRound = true;
            GameManager.instance.screenActive = new bool[4];
            GameManager.instance.playersHolder = new GameObject[4];
            GameManager.instance.amountOfPlayer = 0;
            GameManager.instance.playersAlive = 0;
            GameManager.instance.readyPlayers = 0;



        }

        base.gameStateUpdate();
    }



}

