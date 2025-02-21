using UnityEngine;
using UnityEngine.SceneManagement;

public class NodeController : MonoBehaviour
{
    [Tooltip("Name of the mini-game scene to load when this node is clicked.")]
    public string sceneToLoad;

    void OnMouseDown()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
