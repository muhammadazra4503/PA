using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleInGame : MonoBehaviour
{
    public int collectibleIndex; // The index of this collectible (e.g., 0 for Collectible 1)
    public string collectibleName; // The name of the collectible
    public string collectibleDescription; // The description of the collectible
    public Sprite collectibleImage; // The image of the collectible

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered by: " + other.gameObject.name);

        // Check if the player touched the collectible
        if (other.CompareTag("Player"))
        {
            // Save the unlocked collectible using PlayerPrefs (1 means unlocked)
            PlayerPrefs.SetInt("Collectible_" + collectibleIndex, 1);
            PlayerPrefs.Save();

            Debug.Log("Collectible " + (collectibleIndex + 1) + " picked up and unlocked!");

            // Notify CollectibleManager by calling the static event invoker method
            CollectibleManager.NotifyCollectiblePicked(collectibleIndex, collectibleName, collectibleDescription, collectibleImage);

            // Destroy the collectible object in the game
            Destroy(gameObject);
        }
    }
}
