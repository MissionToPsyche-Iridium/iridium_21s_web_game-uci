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
        if (volume == -40f)
        {
            mainAudioMixer.SetFloat("SFXVol", -80f);
            mtSound.alpha = 1f;
            smSound.alpha = mdSound.alpha = lgSound.alpha = 0f;
        }
        else if (volume > -40f && volume < -25f)
        {
            mainAudioMixer.SetFloat("SFXVol", volume);
            smSound.alpha = 1f;
            mdSound.alpha = lgSound.alpha = mtSound.alpha = 0f;
        }
        else if (volume >= -25f && volume < -15f)
        {
            mainAudioMixer.SetFloat("SFXVol", volume);
            smSound.alpha = mdSound.alpha = 1f;
            lgSound.alpha = mtSound.alpha = 0f;
        }
        else
        {
            mainAudioMixer.SetFloat("SFXVol", volume);
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

    public void MuteSFS()
    {
        float sfxVolume = PlayerPrefs.GetFloat("SFXVol", 0f);
        if (sfxVolume > -40f)
        {
            sfxSlider.value = -40f;
            SetSFX(-40f);
        }
        else
        {
            sfxSlider.value = 0f;
            SetSFX(0f);
        }
    }

    public void MuteMusic()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVol", 0f);
        if (musicVolume > -35f)
        {
            musicSlider.value = -35f;
            SetMusic(-35f);
        }
        else
        {
            musicSlider.value = 0f;
            SetMusic(0f);
        }
    }
}
