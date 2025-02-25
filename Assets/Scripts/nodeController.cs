using UnityEngine;
using UnityEngine.SceneManagement;

public class NodeController : MonoBehaviour
{
    public string sceneToLoad;

    void OnMouseDown()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
