using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AutoScroll : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float scrollSpeed = 0.1f; // Adjust the speed as needed
    public float startDelay = 1f;    // Delay before starting auto-scroll
    public GameObject storyPanel;    // Reference to the StoryPanel
    private bool isUserScrolling = false;
    private bool scrollComplete = false;

    private void Start()
    {
        // Set the scroll position at the top without snapping
        scrollRect.verticalNormalizedPosition = 1f;

        // Pause the game and show the StoryPanel
        Time.timeScale = 0f;
        storyPanel.SetActive(true);

        // Start the coroutine to initialize and start auto-scrolling
        StartCoroutine(StartAutoScrollAfterDelay(startDelay));
    }

    private void Update()
    {
        // If the auto-scroll is not complete and the user isn't scrolling manually
        if (!isUserScrolling && !scrollComplete)
        {
            // Auto-scroll down
            scrollRect.verticalNormalizedPosition -= scrollSpeed * Time.unscaledDeltaTime;

            // Ensure it doesn't scroll past the limit
            if (scrollRect.verticalNormalizedPosition <= 0f)
            {
                scrollRect.verticalNormalizedPosition = 0f;
                scrollComplete = true;
                StartCoroutine(EndStoryAndStartGame());
            }
        }
    }

    private IEnumerator StartAutoScrollAfterDelay(float delay)
    {
        // Wait for the specified delay before starting auto-scroll
        yield return new WaitForSecondsRealtime(delay);  // Use unscaled time because the game is paused
    }

    private IEnumerator EndStoryAndStartGame()
    {
        // Optionally wait a moment before closing the story panel
        yield return new WaitForSecondsRealtime(1f); // Adjust the wait time as needed

        // Close the StoryPanel and resume the game
        storyPanel.SetActive(false);
        Time.timeScale = 1f;  // Resume the game
    }

    // Detect if the user is scrolling manually
    private void OnScrollValueChanged(Vector2 value)
    {
        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            isUserScrolling = true;
        }
        else
        {
            isUserScrolling = false;
        }
    }
}




