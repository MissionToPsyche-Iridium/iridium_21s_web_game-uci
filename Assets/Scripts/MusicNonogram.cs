using UnityEngine;

public class MusicNonogram : MonoBehaviour
{
    public AudioSource[] audioSources;
    public AudioClip currentClip;
    public double nextTime, duration;
    public int toggle;

    void Update()
    {
        if (AudioSettings.dspTime > nextTime - 1)
        {
            audioSources[toggle].clip = currentClip;
            audioSources[toggle].PlayScheduled(nextTime);
            
            duration = (double)currentClip.samples / currentClip.frequency;
            nextTime = nextTime + duration;
            toggle = 1 - toggle;
        }
    }

    public void SetClip(AudioClip clip)
    {
        currentClip = clip;
    }
}
