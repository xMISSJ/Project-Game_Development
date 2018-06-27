using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    //we gaan timer niet als superklasse gebruiken. 
    protected float timeLeft;
    protected bool timerRunning;
    protected bool timerEnded = false;

    // Update is called once per frame
    protected void TimerRunning()
    {
        if (timerRunning == true)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime * 10;
            }
            if (timeLeft <= 0)
            {
                TimerEnd();
            }
        }
    }

    protected void TimerStart(float time)
    {
        if (timerRunning == false)
        {
            timeLeft = time;
            timerRunning = true;
            timerEnded = false;
        }
    }
    protected void TimerPause()
    {
        timerRunning = false;
    }
    protected void TimerEnd()
    {
        timeLeft = 0;
        timerRunning = false;
        timerEnded = true;
    }
}
