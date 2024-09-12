using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class StageUIManager : MonoBehaviour
{
    public Button[] levelButtons;
    public Image[] lockedImages;

    void Start()
    {
        for (int i = 1; i < levelButtons.Length; i++)
        {
            if (PlayerPrefs.GetInt("Level" + i + "Completed", 0) == 1)
            {
                levelButtons[i].interactable = true;
                lockedImages[i].enabled = false;
            }
            else
            {
                levelButtons[i].interactable = false;
                lockedImages[i].enabled = true;
            }
        }
    }

    public void LoadLevel(int levelIndex)
    {
        AudioManager.Instance.StopMusic();

        SceneManager.LoadScene("Level" + levelIndex);
    }
}


