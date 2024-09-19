using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject optionMenu;
    public GameObject finishUIPanel; // New: Add the Finish UI Panel
    public FinishUIManager finishUIManager; // New: Reference to the Finish UI Manager

    private bool isPaused = false;

    void Start()
    {
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
        optionMenu.SetActive(false); 
        finishUIPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused && optionMenu.activeSelf)
            {
                CloseOptionMenu(); // Close OptionMenu and return to PauseMenu
            }
            else if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        optionMenu.SetActive(false); // Ensure OptionMenu is closed when resuming
        Time.timeScale = 1f; // Resume the game
        isPaused = false;
    }

    // Method to show Option Menu
    public void OpenOptionMenu()
    {
        pausePanel.SetActive(false); // Hide PauseMenu
        optionMenu.SetActive(true);  // Show OptionMenu
        Time.timeScale = 0f; // Pause the game while OptionMenu is active
    }

    // Method to close Option Menu and return to Pause Menu
    public void CloseOptionMenu()
    {
        optionMenu.SetActive(false); // Hide OptionMenu
        pausePanel.SetActive(true);  // Show PauseMenu
    }

    // Method to show the Finish UI when the stage is complete
    public void ShowFinishUI()
    {
        Time.timeScale = 0f; // Pause the game when the finish UI is shown
        finishUIPanel.SetActive(true);

        // Call FinishUIManager to display the collected items
        finishUIManager.DisplayCollectedItemsAtFinish();
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
