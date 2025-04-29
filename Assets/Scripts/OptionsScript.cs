using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class OptionsScript : MonoBehaviour
{
    public static OptionsScript instance;
    public AudioMixer mainAudioMixer;

    public Slider sfxSlider;
    public Slider musicSlider;

    private bool isInitialized = false;

    public CanvasGroup smSound, mdSound, lgSound, mtSound;
    public CanvasGroup music, mtMusic;

    private void Start()
    {
        float sfxVolume = PlayerPrefs.GetFloat("SFXVol", 0f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVol", 0f);
        sfxSlider.value = sfxVolume;
        musicSlider.value = musicVolume;


        if (!isInitialized)
        {
            isInitialized = true;
        }
        else
        {
            SetSFX(sfxVolume);
            SetMusic(musicVolume);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSFX(float volume)
    {
        mainAudioMixer.SetFloat("SFXVol", volume);
        if (volume == -80f)
        {
            mtSound.alpha = 1f;
            smSound.alpha = mdSound.alpha = lgSound.alpha = 0f;
        }
        else if (volume > -80f && volume < -50f)
        {
            smSound.alpha = 1f;
            mdSound.alpha = lgSound.alpha = mtSound.alpha = 0f;
        }
        else if (volume > -50f && volume < -20f)
        {
            smSound.alpha = mdSound.alpha = 1f;
            lgSound.alpha = mtSound.alpha = 0f;
        }
        else
        {
            mtSound.alpha = 0f;
            smSound.alpha = mdSound.alpha = lgSound.alpha = 1f;
        }
        PlayerPrefs.SetFloat("SFXVol", volume);
        PlayerPrefs.Save();
    }

    public void SetMusic(float volume)
    {
        if (volume == -35f)
        {
            mainAudioMixer.SetFloat("MusicVol", -80f);
            mtMusic.alpha = 1f;
            music.alpha = 0f;
        }
        else
        {
            mainAudioMixer.SetFloat("MusicVol", volume);
            music.alpha = 1f;
            mtMusic.alpha = 0f;
        }
        PlayerPrefs.SetFloat("MusicVol", volume);
        PlayerPrefs.Save();
    }
}
