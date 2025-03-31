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
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        // Keyboard Navigation
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
            buttons[selectedIndex].ClickedOption();
        }
    }


    public void SelectButton(int index)
    {    
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
        arrowIndicator.transform.position = buttons[selectedIndex].arrowPosition.position;
    }

    public void MouseHover(MenuButton button)
    {
        sounds.PlaySFX(sounds.hoverSFX);
        int index = System.Array.IndexOf(buttons, button);
        SelectButton(index);
        selectedIndex = index;
        MoveArrow();
        mouseActive = true;
    }

    public void MouseExit()
    {
        if (mouseActive)
        {
            mouseActive = false;
        }
    }
}

