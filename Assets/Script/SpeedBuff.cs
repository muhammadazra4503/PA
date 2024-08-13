using System.Collections;
using UnityEngine;
using Assets.PixelFantasy.PixelHeroes.Common.Scripts.ExampleScripts;  // Ensure correct namespace is included

public class SpeedBuff : MonoBehaviour
{
    public float buffDuration = 5f; // Duration of the buff in seconds
    public float speedMultiplier = 2f; // Speed multiplier
    public GameObject buffIndicator; // Reference to the GameObject that indicates the buff is active

    private CharacterControls playerControls; // Reference to CharacterControls script
    private bool isBuffed = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && !isBuffed)
        {
            playerControls = collision.GetComponent<CharacterControls>();
            if (playerControls != null)
            {
                StartCoroutine(ApplySpeedBuff());
            }
        }
    }

    private IEnumerator ApplySpeedBuff()
    {
        isBuffed = true;

        // Activate the buff indicator GameObject
        if (buffIndicator != null)
        {
            buffIndicator.SetActive(true);
        }

        // Increase player's speed
        float originalSpeed = playerControls.RunSpeed;
        playerControls.RunSpeed *= speedMultiplier;

        // Wait for the buff duration
        yield return new WaitForSeconds(buffDuration);

        // Reset speed to normal
        playerControls.RunSpeed = originalSpeed;

        // Deactivate the buff indicator GameObject
        if (buffIndicator != null)
        {
            buffIndicator.SetActive(false);
        }

        // Destroy the buff object after use
        Destroy(gameObject);
    }
}
