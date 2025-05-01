using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    [SerializeField] string sceneToLoad;
    [SerializeField] Animator transition;

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

    public void ChangeScene()
    {
        transition.SetTrigger("End");
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
