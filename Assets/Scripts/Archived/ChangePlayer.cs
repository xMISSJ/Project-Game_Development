using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangePlayer : MonoBehaviour {

    public GameObject[][] prefabArray = new GameObject[5][];        //[Player Number],[ Prefab to change] XOX
    public GameObject[] defaultPrefabs;                             //The prefabs all the players start with.
    private int offset;                                             //This is so that if the object is looking for player objects, but one player doesnt exist, the for loop doesnt stop prematurely.

    // Use this for initialization

    void Start () {
        for (int i = 0; i < 5; i ++)
        {
                prefabArray[i] = (GameObject[])defaultPrefabs.Clone();
        }
	}


    //TODO arrays betere naam geven ipv array.
    public void addPrefab(int targetPlayer, GameObject prefab, int kind)
    {
        prefabArray[targetPlayer][kind] = prefab;
    }

    public void Adapt(int Playernummer)   //When level loads, apply changes to all players.
    {
            if (GameObject.Find("Player" + Playernummer) != null) //If the player object exists
            { GameObject.Find("Player" + Playernummer).GetComponent<MasterBody>().prefabArray = prefabArray[Playernummer]; }
    }
}

