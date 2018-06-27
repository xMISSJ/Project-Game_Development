using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InBetweenRoundState : GameManagerState
{
    bool displayerTracking;

    // Use this for initialization
    public override void gameStateStart()
    {
        availableTransitions = new GameManager.gameState[2];
        //-----
        availableTransitions[0] = GameManager.gameState.cardMenu;
        availableTransitions[1] = GameManager.gameState.endOfMatch;
        //-----
        base.gameStateStart();
        StartCoroutine(CalculateSwitch(5));
    }

    public override void gameStateUpdate()
    {
        if (!GameObject.Find("ScoreMeter(Clone)"))
        {
            GameManager.instance.scoreCounter.DisplayScore();
        }
    }

    private IEnumerator delayedScoreInstance(int timeToWait)
    {
        Debug.Log("Starting new thingy");
        yield return new WaitForSeconds(timeToWait);
        GameManager.instance.scoreCounter.DisplayScore();
    }


    private IEnumerator CalculateSwitch(int timetoWait)
    {

        yield return new WaitForSeconds(timetoWait);

        if (GameManager.instance.scoreCounter.MaxScoreReached())
        {
            GameManager.instance.transitionColor = Color.black;
            GameManager.instance.transitionToScene(Color.black, GameManager.gameState.endOfMatch, GameManager.musicState.fadeIn);
        }
        else
        {
            GameManager.instance.transitionColor = GameManager.instance.playerColors[1];
            GameManager.instance.transitionToScene(GameManager.instance.playerColors[1], GameManager.gameState.cardMenu, GameManager.musicState.wait);
        }
    }
}
