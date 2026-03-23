using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject MainMenuRoot;
    [SerializeField] private GameObject ControlsMenu;

    private void Awake()
    {
        if (MainMenuRoot == null)
        {
            MainMenuRoot = GameObject.Find("MainMenu");
        }

        if (ControlsMenu == null)
        {
            ControlsMenu = GameObject.Find("ControlsMenu");
        }
    }

    private void Start()
    {
        SetMenuState(showMainMenu: true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("__Scene_0");
    }

    public void ControlsButton()
    {
        Debug.Log("Controls button clicked");
        SetMenuState(showMainMenu: false);
    }
    
    public void backToMenu()
    {
        SetMenuState(showMainMenu: true);
    }

    private void SetMenuState(bool showMainMenu)
    {
        if (MainMenuRoot == null)
        {
            Debug.LogWarning("MainMenuRoot is not assigned on MainMenu.");
        }
        else
        {
            MainMenuRoot.SetActive(showMainMenu);
        }

        if (ControlsMenu == null)
        {
            Debug.LogWarning("ControlsMenu is not assigned on MainMenu.");
        }
        else
        {
            ControlsMenu.SetActive(!showMainMenu);
        }
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit button clicked");
    }
}
