using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CollectibleManager : MonoBehaviour
{
    public static List<Sprite> collectedItems = new List<Sprite>();

    public GameObject[] collectibleButtons; // Array of collectible buttons in the UI
    public GameObject[] lockedImages; // Array of locked images
    public TMP_Text nameObject; // Reference to the NameObject (UI Text component)
    public TMP_Text descriptionObject; // Reference to the DescriptionObject (UI Text component)
    public Image detailImage; // Reference to the detail image (UI Image component)
    public GameObject detailImageLocked; // The "locked" placeholder image
    public GameObject collectibleDetailView; // The parent of the detail view to show/hide

    public static event System.Action<int, string, string, Sprite> OnCollectiblePicked;

    void Start()
    {
        // Initially hide the detail view
        collectibleDetailView.SetActive(false);

        bool firstCollectibleSelected = false;

        // Load the status of each collectible
        for (int i = 0; i < collectibleButtons.Length; i++)
        {
            if (PlayerPrefs.GetInt("Collectible_" + i, 0) == 1) // Check if the collectible is unlocked
            {
                UnlockCollectible(i);

                // Auto-select the first unlocked collectible
                if (!firstCollectibleSelected)
                {
                    // Grab the collectible's data
                    Button button = collectibleButtons[i].GetComponent<Button>();
                    CollectibleInGame collectibleData = button.GetComponent<CollectibleInGame>();

                    if (collectibleData != null)
                    {
                        // Update the detail view with the first unlocked collectible's data
                        UpdateDetailView(collectibleData.collectibleIndex, collectibleData.collectibleName, collectibleData.collectibleDescription, collectibleData.collectibleImage);

                        // Show the collectible detail view
                        collectibleDetailView.SetActive(true);

                        firstCollectibleSelected = true;
                    }
                }
            }
            else
            {
                LockCollectible(i);
            }
        }

        // If no collectible is unlocked, keep the detail view hidden
        if (!firstCollectibleSelected)
        {
            collectibleDetailView.SetActive(false);
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

        // Hide the locked image if necessary
        if (detailImageLocked != null)
        {
            detailImageLocked.SetActive(false);
        }

        // Ensure the detail view is visible when updating it
        collectibleDetailView.SetActive(true);
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





