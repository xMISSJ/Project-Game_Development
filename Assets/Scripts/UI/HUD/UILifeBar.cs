using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILifeBar : MonoBehaviour {

    public float livespan = 0;
    public int playerPrefix;
    public playerStats playerStats;
    RectTransform drawHealthHud; // for player heatlh above head  

    public MasterBody mb;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(mb == null)
        {
            Destroy(gameObject);
            return;
        }
        livespan += Time.deltaTime;

        if (livespan > 1f)
        {
            playerStats.showingPlayerHealth[mb.playerID] = false;
            Destroy(gameObject);
        }
        // zorgt ervoor dat de healthbar boven de player staat
        Vector3 pos = Camera.main.WorldToScreenPoint(mb.transform.position + Vector3.up * 2);
        pos.z = 0;
        gameObject.GetComponent<RectTransform>().SetPositionAndRotation(pos, Quaternion.identity);

        drawHealthHud = transform.Find("health").GetComponent<RectTransform>(); // 1 == red

        drawHealthHud.sizeDelta = new Vector2((98 / (float)mb.basehealth) * (float)mb.health, drawHealthHud.sizeDelta.y);
        // drawHealthHud = transform.Find("healthBar").transform.Find("health").GetComponent<RectTransform>();
        drawHealthHud.sizeDelta = new Vector2((98 / (float)mb.basehealth) * (float)mb.health, drawHealthHud.sizeDelta.y); // grootte healthbar
        drawHealthHud.anchoredPosition = new Vector2(-((98 - ((98 / mb.basehealth) * (float)mb.health)) / 2), 0); // positie aanpassen
    }
}
