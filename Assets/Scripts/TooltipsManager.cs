using UnityEngine;
using System.Collections;
using TMPro;

public class TooltipsManager : MonoBehaviour
{
    public static TooltipsManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void TooltipVisible(GameObject tooltip)
    {
        tooltip.SetActive(true);
    }

    public void TooltipInvisible(GameObject tooltip)
    {
        tooltip.SetActive(false);
    }

}
