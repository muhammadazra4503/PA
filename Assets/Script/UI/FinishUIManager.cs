using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishUIManager : MonoBehaviour
{
    public GameObject collectibleImagePrefab; // A prefab for the collectible image (for dynamic instantiation)
    public Transform collectibleListParent; // The parent where collectibles will be displayed (CollectibleList container)

    private void Start()
    {
        // Hide collectible list initially if needed
        collectibleListParent.gameObject.SetActive(false);
    }

    // Method to show all collected items
    public void ShowCollectedItems(List<Sprite> collectedSprites)
    {
        collectibleListParent.gameObject.SetActive(true);

        foreach (Sprite collectedSprite in collectedSprites)
        {
            // Instantiate a new collectible image for each collected sprite
            GameObject newCollectibleImage = Instantiate(collectibleImagePrefab, collectibleListParent);
            Image imageComponent = newCollectibleImage.GetComponent<Image>();

            if (imageComponent != null && collectedSprite != null)
            {
                imageComponent.sprite = collectedSprite;
            }
        }
    }

    public void DisplayCollectedItemsAtFinish()
    {
        ShowCollectedItems(CollectibleManager.collectedItems);
    }
}

