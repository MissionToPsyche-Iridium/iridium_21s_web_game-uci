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
    public float elapsedTime { get; set; }
    AudioManager sounds;


    private void Awake()
    {
        instance = this;
        sounds = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void Start()
    {
        timeCounter.text = elapsedTime.ToString("00':'00");
    }

    //Initiated in GameManager when the scene loads
    public void BeginTimer(float timer)
    {
        //timerState = true;
        elapsedTime = timer;
        StartCoroutine(UpdateTimer());
    }

    public void RestartTimer(float timeToSet)
    {
        elapsedTime = timeToSet;
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
