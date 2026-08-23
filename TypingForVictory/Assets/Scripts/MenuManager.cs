using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public GameObject difficultyPanel;

    [Header("Scene Names")]
    public string gameSceneName = "GameScene";
    public string hardSceneName = "HardGameScene";
    public string mainMenuSceneName = "MenuScene";
    public string tutorialSceneName = "TutorialScene";

    private string selectedMode;

    public void StartTutorial() {
        // Load the tutorial scene
        if (SceneTransitionManager.Instance != null) {
            SceneTransitionManager.Instance.LoadScene(tutorialSceneName);
        } else {
            SceneManager.LoadScene(tutorialSceneName);
        }
        
        if (AudioManager.Instance != null) {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.gameMusic);
        }
    }

    void Start() {
        if (AudioManager.Instance != null) {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.menuMusic);
        }
    }

    public void SelectGameMode(string mode) {
        selectedMode = mode;
        PlayerPrefs.SetString("GameMode", mode);
        PlayerPrefs.Save();
        
        Debug.Log($"Mode selected: {mode}");
        StartSelectedGameMode();
    }

    public void StartNewGame() {
        mainMenuPanel.SetActive(false);
        if (difficultyPanel != null)
        {
            difficultyPanel.SetActive(true);
        } else {
            Debug.LogError("Difficulty Panel is not assigned in the Inspector! Defaulting to Normal Mode.");
            SelectGameMode("Normal");
            StartSelectedGameMode();
        } 
    }

    private void StartSelectedGameMode() {
        // Clear old save data
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        if (string.IsNullOrEmpty(selectedMode)) {
            selectedMode = "Normal";
        }

        string sceneToLoad = gameSceneName;
        if (selectedMode == "Hard") {
            sceneToLoad = hardSceneName;
        }

        if (SceneTransitionManager.Instance != null) {
            SceneTransitionManager.Instance.LoadScene(sceneToLoad);
        } else {
            SceneManager.LoadScene(sceneToLoad);
        }
        Debug.Log($"Starting new {selectedMode} Game! Loading {sceneToLoad}");
        
        if (AudioManager.Instance != null) {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.gameMusic);
        }
    }

    public void ResumeGame() {
        // Check if a save exists
        if (PlayerPrefs.HasKey("WaveNumber")) {
            string savedMode = PlayerPrefs.GetString("GameMode", "Normal");
            string sceneToLoad = (savedMode == "Hard") ? hardSceneName : gameSceneName;

            if (SceneTransitionManager.Instance != null) {
                SceneTransitionManager.Instance.LoadScene(sceneToLoad);
            } else {
                SceneManager.LoadScene(sceneToLoad);
            }
            Debug.Log("Resuming saved game...");

            if (AudioManager.Instance != null) {
                AudioManager.Instance.PlayMusic(AudioManager.Instance.gameMusic);
            }
        } else {
            Debug.Log("No save found. Starting new game instead.");
            selectedMode = "Normal";
            StartSelectedGameMode();
        }
    }

    public void OpenOptions() {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
        if (AudioManager.Instance != null) {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.menuMusic);
        }
    }

    public void CloseOptions() {
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void QuitGame() {
        Application.Quit();
        Debug.Log("Quit pressed!");
    }
}
