using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    ///PLACED ALL OF THIS IN THE GAMEMANAGER.

/*
    public int playersAlive;
    public int[] temporaryRanking;
    GameObject[] playersHolder;
    public GameObject playerPrefab;
    public GameObject[] positionHolder;

    // TEMPORARY FOR PLAYTESTING
    public RandomWeaponAssigner randomWeaponAssigner;
    //

    float height;

    bool winner;

    // Use this for initialization
    public void SpawnPlayer()
    {
        height = 3f;
        //stagePositions = new Transform[positionHolder.Length];
        playersHolder = new GameObject[positionHolder.Length];
        temporaryRanking = new int[positionHolder.Length];
        playersAlive = positionHolder.Length;
        GameObject[] players = new GameObject[positionHolder.Length];
        // Spawns every player at a certain position, which is already assigned in the GameObject array itself.
        for (int i = 0; i < positionHolder.Length; i++)
        {

            GameObject tempGameObject = players[i] = Instantiate(playerPrefab, positionHolder[i].transform.position, positionHolder[i].transform.rotation);
            tempGameObject.GetComponent<MasterBody>().playerPrefix = (i + 1);

            if (randomWeaponAssigner != null)
            {
                randomWeaponAssigner.intantiate();
                for (int j = 0; j < 5; j++)
                {
                    tempGameObject.GetComponent<MasterBody>().prefabArray[j] = randomWeaponAssigner.randomPrefabs[i, j];
                }
            }
            playersHolder[i] = tempGameObject; // Fills the playersHolder array with temGameObjects which were made above.
            tempGameObject.name = "Player" + (i + 1);
        }
        if (GetComponent<playerStats>() != null)        //If this object contains the player stats script (It doesnt in the card menu, thats why it has to check.
        {
            gameObject.GetComponent<playerStats>().Init(players);
        }
    }


    public void updatePlayer(MasterBody deadPlayer)
    {
        playersAlive -= 1;
        temporaryRanking[playersAlive] = deadPlayer.playerPrefix;
        Debug.Log("player" + temporaryRanking[playersAlive] + " Just finished in " + playersAlive + "th place");
        // If there is only survivor left, then there is a winner.
        if (playersAlive == 1)
        {
            foreach (GameObject player in playersHolder)
            {
                if (player != null)
                {
                    if (player.GetComponent<MasterBody>().isAlive)
                    {
                        temporaryRanking[0] = player.GetComponent<MasterBody>().playerPrefix;

                        Debug.Log("player" + temporaryRanking[0] + " Just finished in 0th place");
                        break;
                    }
                }
            }
            Ranking.setPlayerRanking(temporaryRanking);
            SceneManager.LoadScene("InBetweenRound_JL");

        }
    }*/
}
