using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("__Scene_0");
    }

    public void SettingsButton()
    {
        Debug.Log("Settings button clicked");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit button clicked");
    }
}
