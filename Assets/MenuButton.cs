using UnityEngine;
using TMPro;

public class MenuButton : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    public GameObject underline;
    public float speed = 0.2f;
    public Transform arrowPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        underline.transform.localScale = new Vector3(0, 1, 1);
    }

    public void OnSelect()
    {
        buttonText.fontSize = 24f;
        buttonText.color = Color.white;
        LeanTween.scaleX(underline, 1, speed).setEaseOutExpo();
    }

    public void OnDeselect()
    {
        buttonText.fontSize = 20f;
        buttonText.color = Color.gray;
        LeanTween.scaleX(underline, 0, speed).setEaseOutExpo();
    }

    public void OnMouseEnter()
    {
        FindFirstObjectByType<MenuController>().MouseHover(this);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
