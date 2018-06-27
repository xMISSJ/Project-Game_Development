using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startingZone : MonoBehaviour {

    [SerializeField]Color selectedColor;
    [SerializeField]Color normalColor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<MeshRenderer>().material.color = selectedColor;
            GameManager.instance.readyPlayers++;
            GameManager.instance.checkIfEveryoneIsReady();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<MeshRenderer>().material.color = normalColor;
            GameManager.instance.readyPlayers--;

        }
    }
}
