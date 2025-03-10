using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsBackButtonLoader : MonoBehaviour
{
    public void LoadStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
