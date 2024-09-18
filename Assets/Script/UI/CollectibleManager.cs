using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CollectibleManager : MonoBehaviour
{
    public static List<Sprite> collectedItems = new List<Sprite>();

    public GameObject[] collectibleButtons;
    public GameObject[] lockedImages;
    public TMP_Text nameObject;
    public TMP_Text descriptionObject;
    public Image detailImage;
    public GameObject detailImageLocked;

    public static event System.Action<int, string, string, Sprite> OnCollectiblePicked;

    void Start()
    {
        // Load the status of each collectible
        for (int i = 0; i < collectibleButtons.Length; i++)
        {
            if (PlayerPrefs.GetInt("Collectible_" + i, 0) == 1)
            {
                UnlockCollectible(i);
            }
            else
            {
                LockCollectible(i);
            }
        }

        OnCollectiblePicked += UpdateDetailView;
    }

    void OnDestroy()
    {
        OnCollectiblePicked -= UpdateDetailView;
    }

    public static void NotifyCollectiblePicked(int index, string name, string description, Sprite image)
    {
        if (OnCollectiblePicked != null)
        {
            OnCollectiblePicked.Invoke(index, name, description, image);
        }

        collectedItems.Add(image);
    }

    public void UpdateDetailView(int index, string name, string description, Sprite image)
    {
        // Update UI elements with the provided data
        nameObject.text = name;
        descriptionObject.text = description;
        detailImage.sprite = image;

        if (detailImageLocked != null)
        {
            detailImageLocked.SetActive(false);
        }
    }

    public void UnlockCollectible(int index)
    {
        // Unlock the collectible
        lockedImages[index].SetActive(false);
        Button button = collectibleButtons[index].GetComponent<Button>();
        if (button != null)
        {
            button.interactable = true;
        }
    }

    public void LockCollectible(int index)
    {
        // Lock the collectible
        lockedImages[index].SetActive(true);
        Button button = collectibleButtons[index].GetComponent<Button>();
        if (button != null)
        {
            button.interactable = false;
        }
    }
}



