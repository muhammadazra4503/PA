using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleButton : MonoBehaviour
{
    public int collectibleIndex; // The index of the collectible this button represents
    public string collectibleName;
    public string collectibleDescription;
    public Sprite collectibleImage;

    public void OnButtonClick()
    {
        // Notify the CollectibleManager to update the detail view
        CollectibleManager.NotifyCollectiblePicked(collectibleIndex, collectibleName, collectibleDescription, collectibleImage);
    }
}

