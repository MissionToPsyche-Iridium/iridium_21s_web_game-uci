using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsBackButtonLoader : MonoBehaviour
{
    AudioManager sounds;

    private void Awake()
    {
        sounds = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void LoadStartMenu()
    {
        sounds.PlaySFX(sounds.generalUIButton);
        SceneManager.LoadScene("StartMenu");
    }
}
