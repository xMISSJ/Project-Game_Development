using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InArenaState : GameManagerState {
    playerStats Ps;
    ItsTheFinalCountdown finalCD;

    // Use this for initialization
    public override void gameStateStart()
    {
        availableTransitions = new GameManager.gameState[1];
        //-----
        availableTransitions[0] = GameManager.gameState.inbetweenRounds;

        GameManager.instance.firstRound = false;
              
        //-----
        base.gameStateStart();

        GameManager.instance.SpawnPlayers();
        Ps = gameObject.GetComponent<playerStats>();

        Ps.Init(GameManager.instance.playersHolder);


        // countdown round related
        finalCD = gameObject.GetComponent<ItsTheFinalCountdown>();
        finalCD.StartRoundCooldown();
    }


    // Update is called once per frame
    public override void gameStateUpdate()
    {
        base.gameStateUpdate();
        Ps.update();
    }
}
