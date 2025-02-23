using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public static TimerScript instance;
    public Text timeCounter;
    private TimeSpan timePlaying;
    private bool timerState = false;
    public float elapsedTime;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        timeCounter.text = elapsedTime.ToString("mm':'ss");
    }

    //Initiated in GameManager when the scene loads
    public void BeginTimer()
    {
        timerState = true;
        elapsedTime = 0f;
        StartCoroutine(UpdateTimer());
    }

    public void PauseTimer()
    {
        if (timerState)
        {
            timerState = false;
        }
        else
        {
            timerState = true;
            StartCoroutine(UpdateTimer());
        }
    }

    private IEnumerator UpdateTimer()
    {
        while (timerState)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            timeCounter.text = timePlaying.ToString("mm':'ss");

            yield return null;
        }
    }
}
