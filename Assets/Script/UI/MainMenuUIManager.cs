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

    public void ExitGame()
    {
        // Check if we are running in the Unity editor
        #if UNITY_EDITOR
        // If in the editor, stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // If not in the editor, quit the application
        Application.Quit();
        #endif
    }

}
