using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    [SerializeField] Animator transition;
    public float[] levelTimes = new float[4];

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

    public void ChangeScene(string sceneToLoad)
    {
        StartCoroutine(LoadingScene(sceneToLoad));
    }

    IEnumerator LoadingScene(string scene)
    {
        transition.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(scene);
        transition.SetTrigger("Start");
    }

    public void SaveLevelTimeResult(int level, float time)
    {
        if (level < levelTimes.Length)
        {
            levelTimes[level] = time;
        }
        else
        {
            Debug.LogError("Invalid level: " + level);
        }
    }

    public float GetLevelTimeResult(int levelIndex)
    {
        return levelTimes[levelIndex];
    }
}
