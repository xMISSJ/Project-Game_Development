using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuState : GameManagerState {

    // Use this for initialization

    public override void gameStateStart()
    {
        availableTransitions = new GameManager.gameState[1];
        //-----
        availableTransitions[0] = GameManager.gameState.inArena;
        //-----
        base.gameStateStart();
    }

    // Update is called once per frame
    void Update () {
		 //no scene, link buttons to the variables to trigger a transition
	}
}
