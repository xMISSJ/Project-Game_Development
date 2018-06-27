using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfMatchMenuState : GameManagerState {
    // Use this for initialization
    public override void gameStateStart()
    {
        availableTransitions = new GameManager.gameState[2];
        //-----
        availableTransitions[0] = GameManager.gameState.mainMenu;
        availableTransitions[1] = GameManager.gameState.cardMenu;
        //-----
    }

    public override void gameStateUpdate()
    {
        base.gameStateUpdate();
    }
}
