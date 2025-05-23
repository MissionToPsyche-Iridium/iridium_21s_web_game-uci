using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject arrowIndicator;
    public MenuButton[] buttons;
    private int selectedIndex = 0;

    private bool mouseActive = false;
    AudioManager sounds;

    private void Awake()
    {
        sounds = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SelectButton(selectedIndex);
        MoveArrow();
        PlayerPrefs.SetInt("MaxCurrentLevel", 0);
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        // Keyboard Navigation for up down and w s and space
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            sounds.PlaySFX(sounds.hoverSFX);
            selectedIndex = (selectedIndex - 1 + buttons.Length) % buttons.Length;
            SelectButton(selectedIndex);
            mouseActive = false;
            MoveArrow();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            sounds.PlaySFX(sounds.hoverSFX);
            selectedIndex = (selectedIndex + 1) % buttons.Length;
            SelectButton(selectedIndex);
            mouseActive = false;
            MoveArrow();
        }
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (selectedIndex == 0 || selectedIndex == 2)
            {
                buttons[selectedIndex].ClickedOption();
            }
            else if (selectedIndex == 1)
            {
                sounds.PlaySFX(sounds.generalUIButton);
                FindAnyObjectByType<BackgroundController>().ToggleOptionsPanel();

            }
        }
    }


    public void SelectButton(int index)
    {
        // Highlight the button that is getting hover over
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == index)
            {
                buttons[i].OnSelect();
            }
            else
            {
                buttons[i].OnDeselect();
            }
        }
    }

    public void MoveArrow()
    {
        // Move the arrow indicator to the selected button
        arrowIndicator.transform.position = buttons[selectedIndex].arrowPosition.position;
    }

    public void MouseHover(MenuButton button)
    {
        // Dehighlight all buttons and hover the current one
        sounds.PlaySFX(sounds.hoverSFX);
        int index = System.Array.IndexOf(buttons, button);
        SelectButton(index);
        selectedIndex = index;
        MoveArrow();
        mouseActive = true;
    }

    public void MouseExit()
    {
        // Deactivate mouse active
        if (mouseActive)
        {
            mouseActive = false;
        }
    }
}

