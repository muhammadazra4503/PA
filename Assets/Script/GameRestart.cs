using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestart : MonoBehaviour
{
    public float fallThreshold = -10f; // Y position threshold for falling

    void Update()
    {
        // Check if the player's Y position is below the fall threshold
        if (transform.position.y < fallThreshold)
        {
            RestartGame();
        }
    }

    void RestartGame()
    {
        Time.timeScale = 1f; // Resume the game (in case it was paused)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }
}
