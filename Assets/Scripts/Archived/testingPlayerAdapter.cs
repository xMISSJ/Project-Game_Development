using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testingPlayerAdapter : MonoBehaviour {

    public GameObject[][] prefabArray = new GameObject[5][];       //[Player Number],[ Prefab to change]
    public GameObject[] defaultPrefabs;      //The prefabs all the players start with.
    private int offset;                      //This is so that if the object is looking for player objects, but one player doesnt exist, the for loop doesnt stop prematurely.

                                             // Use this for initialization
    void Start () {

        for (int i = 0; i < 5; i++)
        {
            prefabArray[i] = (GameObject[])defaultPrefabs.Clone();
        }

        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Player").Length + offset; i++)    //For each player in the scene
        {
            if (GameObject.Find("Player") != null) //If the player object exists
            { GameObject.Find("Player").GetComponent<MasterBody>().prefabArray = prefabArray[i]; }
            else
            {
                offset++;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
