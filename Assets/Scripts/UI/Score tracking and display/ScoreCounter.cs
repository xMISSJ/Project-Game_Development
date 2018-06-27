using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class ScoreCounter : MonoBehaviour
{
    public int numberOfPlayers, scoreTypes;
    public int maxScore = 2;
    public GameObject scoreDisplayPrefab;
    public GameObject[] scoreDisplayers;
    public int[,] playerPoints;                                                         //stores the information per player. first indice corresponds to the playerID and the second to the damagetype Int
    public int[] pointModifiers;                                                        //stores information such as the amount of points per point of damage done.

    // Use this for initialization
    void Start()
    {
        scoreTypes = 4;                                                                 //the different types of points we monitor
        pointModifiers = new int[] { 10, 20, 1, 5 };
        playerPoints = new int[4, scoreTypes + 1];                                      //Added one more than scoretypes to make room for a totalscore Field.
        scoreDisplayers = new GameObject[4];                                            //We use 4 because we have a max of 4 players. 
    }

    //used to delay the instantiation of the scoremeters. Prevents issues with the instantiation 
    public IEnumerator DelayInstance(int i)
    {
        yield return new WaitForSeconds(0.01f);
        scoreDisplayers[i].transform.localPosition = new Vector3(3 * i, 0, 0);
    }

    //takes a string and compares it to figure out what type the score is. value is multiplied by the corresponding damage type modifier and added to the player's score 
    //for that type. The score is also always added to the totalscore field.
    public void updateScore(int playernumber, string scoreType, int value = 1)
    {
        int scoreTypeInt = 0;
        scoreType = scoreType.ToLower();
        switch (scoreType)
        {
            case "kill":
                scoreTypeInt = 0;
                break;
            case "win":
                scoreTypeInt = 1;
                break;
            case "hurt":
                scoreTypeInt = 2;
                break;
            case "coin":
                scoreTypeInt = 3;
                break;
        }
        playerPoints[playernumber, scoreTypeInt] += pointModifiers[scoreTypeInt] * value;
        playerPoints[playernumber, 4] += pointModifiers[scoreTypeInt] * value;                      //Score type 4 is the total score.
        Debug.Log(playerPoints[playernumber, scoreTypeInt]);
    }

    //uses the assigned prefab (which is of the type scoremeter) to display the score of the players. also assigns all the relevant information to the instanced prefabs
    public void DisplayScore()
    {
        numberOfPlayers = GameManager.instance.amountOfPlayer;
        for (int i = 0; i < numberOfPlayers; i++)
        {
            Debug.Log("I =" + i);
            scoreDisplayers[i] = Instantiate(scoreDisplayPrefab, new Vector3(-0.5f * i, 0, 0), Quaternion.identity);
            //starts a routine in which the scoredisplayers lerp to their normal size after a short delay. this is also why the localscale is set so low upon instantiation.
            scoreDisplayers[i].GetComponent<scoreMeter>().startRoutine = true;
            scoreDisplayers[i].transform.localScale = new Vector3(1, 0.1f, 1);
            scoreDisplayers[i].GetComponent<scoreMeter>().Init();
            StartCoroutine(DelayInstance(i));
            for (int j = 0; j < scoreTypes; j++)
            {
                Debug.Log("j = " + scoreDisplayers[i].GetComponent<scoreMeter>().barSizes.Length);
                scoreDisplayers[i].GetComponent<scoreMeter>().barSizes[j] = playerPoints[i, j] * 0.05f;
                scoreDisplayers[i].GetComponent<scoreMeter>().ResizeAndMove();
            }
        }
    }

    //Returns total score
    public int PlayerPoints(int playerID)
    {
        return playerPoints[playerID, 4];
    }

    //Override if you want any other kind of score to be returned.
    public int PlayerPoints(int playerID, int scoreType)
    {
        return playerPoints[playerID, scoreType];
    }

    //Returns the highest total score
    public int HighestScore()
    {
        List<int> scores = new List<int>();
        for (int i = 0; i < GameManager.instance.AmountOfPlayers(); i++)
        {
            scores.Add(playerPoints[i, 4]);
        }
        int highestScore = 0;
        foreach (int score in scores)
        {
            if (score > highestScore)
            { highestScore = score; }
        }
        return highestScore;
    }

    //Returns the highest score in the ScoreType category.
    public int HighestScore(int scoreType)
    {
        List<int> scores = new List<int>();
        for (int i = 0; i < GameManager.instance.AmountOfPlayers(); i++)
        {
            scores.Add(playerPoints[i, scoreType]);
        }
        int highestScore = 0;
        foreach (int score in scores)
        {
            if (score > highestScore)
            { highestScore = score; }
        }
        return highestScore;
    }

    //Returns the lowest total score
    public int LowestScore()
    {
        List<int> scores = new List<int>();
        for (int i = 0; i < GameManager.instance.AmountOfPlayers(); i++)
        {
            scores.Add(playerPoints[i, 4]);
        }
        int lowestScore = HighestScore();
        foreach (int score in scores)
        {
            if (score < lowestScore)
            { lowestScore = score; }
        }
        return lowestScore;
    }

    //Returns the lowest score in the ScoreType category.
    public int LowestScore(int scoreType)
    {
        List<int> scores = new List<int>();
        for (int i = 0; i < GameManager.instance.AmountOfPlayers(); i++)
        {
            scores.Add(playerPoints[i, scoreType]);
        }
        int lowestScore = HighestScore();
        foreach (int score in scores)
        {
            if (score < lowestScore)
            { lowestScore = score; }
        }
        return lowestScore;
    }

    //Returns an array with all players and their ranks. On [0] is the player in the lead, on [1] is the player on second place. Etc
    public int[,] PositionInRanks()
    {
        int[,] rankNumber = new int[GameManager.instance.AmountOfPlayers(), 2];

        for (int i = 0; i < GameManager.instance.AmountOfPlayers(); i++)
        {
            rankNumber[i, 0] = i;
            rankNumber[i, 1] = playerPoints[i, 4];
        }
        //i resembles the player number we are checking.
        for (int i = 0; i < GameManager.instance.AmountOfPlayers(); i++)
        {
            //J is the players underneath i 
            for (int j = i; j < GameManager.instance.AmountOfPlayers(); j++)
            {
                if (rankNumber[i, 1] < rankNumber[j, 1])
                {
                    int tempScoreReminder = rankNumber[i, 1];
                    int tempPlayerReminder = rankNumber[i, 0];
                    rankNumber[i, 0] = rankNumber[j, 0];
                    rankNumber[i, 1] = rankNumber[j, 1];
                    rankNumber[j, 0] = tempPlayerReminder;
                    rankNumber[j, 1] = tempScoreReminder;
                }
            }
        }
        return rankNumber;
    }

    //Returns the average score of all players. used to determine what items players get assigned
    public float AverageScore()
    {
        int[,] ranks = new int[4, 2];
        ranks = GameManager.instance.scoreCounter.PositionInRanks();
        float averageScore = 0;

        for (int i = 0; i < GameManager.instance.AmountOfPlayers(); i++)
        {
            averageScore += ranks[i, 1];        //adds all the scores to the average score variable.
        }
        averageScore = averageScore / GameManager.instance.AmountOfPlayers();       //Divides combined score with amount of people. To get average score.
        return averageScore;
    }

    //returns whether or not the maximum score has been reached
    public bool MaxScoreReached()
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            if (playerPoints[i, 4] >= maxScore)
            {
                Debug.Log(playerPoints[i, 4]);
                return true;
            }
        }
        return false;
    }

}
