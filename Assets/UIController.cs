using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public Slider _musicSlider;
    public TextMeshProUGUI musicPercentText; // Reference to the TMP text
    public Button increaseVolumeButton; // Button for increasing volume
    public Button decreaseVolumeButton; // Button for decreasing volume

    private const float volumeStep = 0.05f; // 5% increment/decrement
    private const float minVolume = 0f;
    private const float maxVolume = 1f;

    private void Start()
    {
        // Load the saved volume into the slider and update text
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f); // Default volume is 1 (max)
        _musicSlider.value = savedVolume;
        UpdateMusicPercentText(savedVolume);

        // Set initial volume based on saved preferences
        AudioManager.Instance.MusicVolume(savedVolume);

        // Add listener to handle slider value changes
        _musicSlider.onValueChanged.AddListener(delegate { OnMusicSliderChanged(); });

        // Add listeners to buttons
        increaseVolumeButton.onClick.AddListener(IncreaseVolume);
        decreaseVolumeButton.onClick.AddListener(DecreaseVolume);
    }

    public void OnMusicSliderChanged()
    {
        // Update the music volume and text when the slider value changes
        AudioManager.Instance.MusicVolume(_musicSlider.value);
        UpdateMusicPercentText(_musicSlider.value);
    }

    private void UpdateMusicPercentText(float value)
    {
        // Convert slider value to percentage and update the TMP text
        int percent = Mathf.RoundToInt(value * 100); // Assuming slider max value is 1
        musicPercentText.text = percent + "%";
    }

    public void IncreaseVolume()
    {
        // Increase the slider value by 5%, clamping to maxVolume (1)
        _musicSlider.value = Mathf.Clamp(_musicSlider.value + volumeStep, minVolume, maxVolume);
        OnMusicSliderChanged();
    }

    public void DecreaseVolume()
    {
        // Decrease the slider value by 5%, clamping to minVolume (0)
        _musicSlider.value = Mathf.Clamp(_musicSlider.value - volumeStep, minVolume, maxVolume);
        OnMusicSliderChanged();
    }
}
