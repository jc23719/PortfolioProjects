using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public GameObject optionsPanel;

    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        // Slider ranges
        musicSlider.minValue = 0f;
        musicSlider.maxValue = 1f;
        sfxSlider.minValue = 0f;
        sfxSlider.maxValue = 1f;

        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        ApplySettings();
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }

    public void SetMusicVolume(float volume)
    {
        float v = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("MusicVolume", v);

        if (SoundManager.Instance != null)
            SoundManager.Instance.SetMusicVolume(v);
    }

    public void SetSFXVolume(float volume)
    {
        float v = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("SFXVolume", v);

        if (SoundManager.Instance != null)
            SoundManager.Instance.SetSFXVolume(v);
    }

    private void ApplySettings()
    {
        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);
    }
}
