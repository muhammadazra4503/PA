using UnityEngine;
using UnityEngine.UI;

public class DisableUpButton : MonoBehaviour
{
    public Button upButton;

    void Start()
    {
        // Nonaktifkan klik pada button
        upButton.onClick.RemoveAllListeners();
        upButton.interactable = false;
    }
}
