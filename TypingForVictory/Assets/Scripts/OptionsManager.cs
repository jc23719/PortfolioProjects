using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour {
    public GameObject backPanel;
    public GameObject optionsPanel;
    public GameObject difficultyPanel;

    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle fullscreenToggle;

    void Update() {
        // If options panel is active and ESC is pressed, go back
        if (optionsPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape)) {
            Back();
        }
    }

    void Start() {
        musicSlider.minValue = 0f;
        musicSlider.maxValue = 1f;
        sfxSlider.minValue = 0f;
        sfxSlider.maxValue = 1f;

        // Default to middle
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        fullscreenToggle.isOn = Screen.fullScreen;

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);

        ApplySettings();
    }


    public void SetMusicVolume(float volume) {
        if (AudioManager.Instance != null) {
            float clamped = Mathf.Clamp01(volume);
            AudioManager.Instance.SetMusicVolume(clamped);
            PlayerPrefs.SetFloat("MusicVolume", volume);
        }
    }


    public void SetSFXVolume(float volume) {
        if (AudioManager.Instance != null) {
            AudioManager.Instance.SetSFXVolume(Mathf.Clamp01(volume));
            PlayerPrefs.SetFloat("SFXVolume", volume);
        }
    }


    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }

    public void Back() {
        optionsPanel.SetActive(false);
        if (difficultyPanel != null) {
            difficultyPanel.SetActive(false);
        }
        if (backPanel != null) backPanel.SetActive(true);
    }

    private void ApplySettings() {
        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);
        SetFullscreen(fullscreenToggle.isOn);
    }
}
