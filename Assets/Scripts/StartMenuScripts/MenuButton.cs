using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    public GameObject underline;
    public float speed = 0.2f;
    public Transform arrowPosition;
    [SerializeField] string sceneToLoad;
    AudioManager sounds;

    private void Awake()
    {
        sounds = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        underline.transform.localScale = new Vector3(0, 1, 1);
    }

    public void OnSelect()
    {
        // Hover over the button highlight
        buttonText.fontSize = 30f;
        buttonText.color = Color.white;
        LeanTween.scaleX(underline, 1, speed).setEaseOutExpo();
    }

    public void OnDeselect()
    {
        // Hover out the button highlight
        buttonText.fontSize = 28f;
        buttonText.color = Color.gray;
        LeanTween.scaleX(underline, 0, speed).setEaseOutExpo();
    }

    public void OnMouseEnter()
    {
        // Activate the mouse hover/ button highlight
        FindFirstObjectByType<MenuController>().MouseHover(this);
    }

    public void ClickedOption()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            sounds.PlaySFX(sounds.generalUIButton);
            //SceneManager.LoadScene(sceneToLoad);
            SceneController.instance.ChangeScene(sceneToLoad);
        }
    }
}
