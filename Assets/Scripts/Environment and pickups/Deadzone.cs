using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadzone : MonoBehaviour
{

    // When the player hits the deadzone (and the round cooldown is not active) the player dies 
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<MasterBody>().PlayerDies();
            Debug.Log("Deadzone collided with player");
        }
    }
}
