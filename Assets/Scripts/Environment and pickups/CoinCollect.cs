using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCollect : MonoBehaviour {

    [SerializeField]
    AudioClip sndCoin;
    public playerStats ps;
    int coinValue = 1;
    GameObject[] players;

	// Use this for initialization
	void Start () {
        players = GameObject.FindGameObjectsWithTag("Player");
        
    }

    // Update is called once per frame
    void Update () {
        foreach (GameObject p in players)
        {
            if(p == null)
            {
                continue;
            }
            float dist = Vector3.Distance(p.transform.position, transform.position); // middle position from coin and player, CR: Jelmer
            if (dist<4f)
            {
                p.GetComponent<MasterBody>().playSound(sndCoin);
                GameManager.instance.scoreCounter.updateScore(p.GetComponent<MasterBody>().playerID, "coin", 1);
                ps.addCoin(p.name, coinValue);
                Destroy(gameObject);           
            }
        }
	}

}
