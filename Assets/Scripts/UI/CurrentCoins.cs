using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentCoins : MonoBehaviour {
    [SerializeField]
    Text coinText;
    [SerializeField]
    CustomizeCards customizeCards;
	// Use this for initialization
	void Start () {
        UpdateText();
	}

    public void UpdateText()
    {
        coinText.text = GameManager.instance.playerCoins[customizeCards.ID].ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
