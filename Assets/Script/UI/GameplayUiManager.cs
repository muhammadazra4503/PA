using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject optionMenu;

    private bool isPaused = false;

    void Start()
    {
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
        optionMenu.SetActive(false); // Ensure OptionMenu is hidden at start
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
}
