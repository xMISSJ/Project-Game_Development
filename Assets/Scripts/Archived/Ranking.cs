using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Ranking : MonoBehaviour
{
    /* -------------------------------------------------------------------------
     * made obsolete by inbetweenroundstate and the scorecounter being updated
     * -------------------------------------------------------------------------*/



    //public Text winnerText;
    //public GameObject testObject;
    //public Transform[] stagePositions;
    //static int[] ranking = new int[] { 3, 4, 2, 1 };
    //[SerializeField]
    //private Text[] podiumtext = new Text[4];
    //float height;

    //private string winnerString = "The winner of the round is player: ";
    //bool testLoop = true;

    //// Use this for initialization
    //void Start()
    //{
    //    height = 3f;
    //    // Fetches the text from the scene.
    //    if (winnerText == null)
    //    {
    //        winnerText = GameObject.Find("WinnerText").GetComponent<Text>();
    //        Debug.Log("Toot");
    //    }
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (testLoop)
    //        RankingDisplay(ranking);
    //    testLoop = false;


    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        SceneManager.LoadScene("Playtest");
    //    }
    //}

    //public void RankingDisplay(int[] rankingSystem)
    //{
    //    winnerText.text = winnerString + rankingSystem[0];
    //    for (int i = 0; i < rankingSystem.Length; i++)
    //    {
    //        if (podiumtext[i] != null)
    //        {
    //            podiumtext[i].text = (i + 1) + ". " + rankingSystem[i];
    //        }
    //        Debug.Log(rankingSystem[i]);
    //        Instantiate(testObject, stagePositions[i].position + new Vector3(0, height * ((7.8f - i) * 0.07f), 0), Quaternion.identity);
    //    }
    //}



    //public static void setPlayerRanking(int[] rankingToSet)
    //{
    //    ranking = rankingToSet;
    //}
}