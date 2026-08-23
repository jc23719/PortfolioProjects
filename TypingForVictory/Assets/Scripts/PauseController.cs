using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseController : MonoBehaviour {
    [Header("UI")]
    public GameObject pausePanel;
    public GameObject optionMenuPanel;
    public Button resumeButton;
    public Button mainMenuButton;
    public Button quitButton;
    public Button optionButton;
    public Button saveButton;
    public TMP_Text saveButtonText;

    [Header("Options")]
    public bool useTimescalePause = true;
    public bool pauseAudio = true;

    public static bool IsPaused { get; private set; }

    void Awake() {
        if (resumeButton) resumeButton.onClick.AddListener(Resume);
        if (mainMenuButton) mainMenuButton.onClick.AddListener(GoToMainMenu);
        if (quitButton) quitButton.onClick.AddListener(QuitGame);
        if (saveButton) saveButton.onClick.AddListener(SaveGame);

        SetPaused(false);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePause();
        }
    }

    public void TogglePause() {
        SetPaused(!IsPaused);
    }

    public void Pause() => SetPaused(true);
    public void Resume() => SetPaused(false);

    private void SetPaused(bool paused) {
        if (IsPaused == paused) return;
        IsPaused = paused;

        if (pausePanel) pausePanel.SetActive(paused);

        if (useTimescalePause) Time.timeScale = paused ? 0f : 1f;

        if (pauseAudio) AudioListener.pause = paused;

        if (paused) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else {
            var selectionUI = FindFirstObjectByType<PowerupSelectionUI>();
            var typingInput = FindFirstObjectByType<TypingInput>();

            if ((selectionUI != null && selectionUI.IsActive) ||
                (typingInput != null && typingInput.gameOverPanel.activeSelf)) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                if (saveButtonText) saveButtonText.text = "Save";
            }
        }
    }

    public void GoToMainMenu() {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene("MenuScene");
    }

    public void OpenOptions() {
        pausePanel.SetActive(false);
        optionMenuPanel.SetActive(true);
    }

    public void BackToPauseMenu() {
        optionMenuPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    private void QuitGame() {
        Application.Quit();
    }

    private void SaveGame() {
        var player = GameObject.FindWithTag("Player");
        var manager = player?.GetComponent<PowerupManager>();
        var waveManager = FindFirstObjectByType<WaveManager>();

        SaveSystem.SaveGame(player?.GetComponent<PlayerHealth>(), manager, waveManager);

        if (saveButtonText) saveButtonText.text = "Saved!";
    }

}
