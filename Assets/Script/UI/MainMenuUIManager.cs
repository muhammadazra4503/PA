using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    public GameObject mainMenuPanel;

    public void ShowStageMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StageMenu"); // Load the stage menu scene
    }

    // Show the options menu
    public void ShowOptionsMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("OptionMenu"); // Load the options menu scene
    }

    // Show the collectibles scene
    public void ShowCollectiblesScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("CollectiblesMenu"); // Load the collectibles scene
    }

    // Show the credits scene
    public void ShowCredits()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Credit"); // Load the credits scene
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
    }
}
