using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerState : MonoBehaviour
{
    protected GameManager.gameState[] availableTransitions;
    protected bool[] switchExecute;
    public virtual void gameStateStart() 
    {
       // Debug.Log("Started a new state");
        if (availableTransitions != null)
        {
            switchExecute = new bool[availableTransitions.Length];
        }
        else switchExecute = new bool[1];
        for (int i = 0; i < switchExecute.Length; i++)
        {
            switchExecute[i] = false;
        }
        //Debug.Log("Switch: " + switchExecute.Length + " | transitions: " + availableTransitions.Length);
        GameObject tempGameObject = Instantiate(GameManager.instance.dropInObject, GameObject.FindGameObjectWithTag("Canvas").transform);
        tempGameObject.GetComponent<DropIn>().moveOut();
    }

    //replaces base update, called by the gamemanager
    public virtual void gameStateUpdate()
    {
        switchScene();
    }

    //checks the array of booleans to see if any scene needs to be switched to
    protected void switchScene()
    {
        for (int i = 0; i < availableTransitions.Length; i++)
        {
            if (switchExecute[i])
            {
                GameManager.instance.SceneSwitcher(availableTransitions[i]);
                break;
            }
        }
    }
}
