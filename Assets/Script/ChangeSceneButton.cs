using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : MonoBehaviour
{
    public string sceneName;
    public int currentLevelIndex; // Set this to the current level index (e.g., 1 for Level 1, 2 for Level 2)

    public void OnButtonPress()
    {
        // Mark the current level as completed
        PlayerPrefs.SetInt("Level" + currentLevelIndex + "Completed", 1);
        PlayerPrefs.Save();

        // Load the next scene
        SceneManager.LoadScene(sceneName);
    }
}
