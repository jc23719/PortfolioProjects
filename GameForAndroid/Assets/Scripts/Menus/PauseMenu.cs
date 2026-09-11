using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pausePanel;
    public GameObject optionsPanel;

    public Button resumeButton;
    public Button exitButton;
    public Button optionsButton;
    public Button openMenuButton;
    public Button redoButton;
    public Button resetToStartButton;
    public Button backButton;
    public Button toggleInventoryButton;
    public GameObject inventoryPanel;
    private Dictionary<GadgetType, int> items = new Dictionary<GadgetType, int>();

    private bool isPaused = false;
    private bool inventoryVisible = true;

    void Start()
    {
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);

        resumeButton.onClick.AddListener(ResumeGame);
        exitButton.onClick.AddListener(ExitToMainMenu);
        optionsButton.onClick.AddListener(OpenOptions);
        openMenuButton.onClick.AddListener(TogglePause);
        redoButton.onClick.AddListener(RedoLevel);
        resetToStartButton.onClick.AddListener(ResetToStart);
        backButton.onClick.AddListener(BackToPauseMenu);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
            SoundManager.Instance.PlaySFX(SoundManager.Instance.pauseSFX);
    }

    public void ToggleInventory()
    {
        inventoryVisible = !inventoryVisible;
        inventoryPanel.SetActive(inventoryVisible);
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        optionsPanel.SetActive(false);
        Timer.PauseTimer();
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
        Timer.ResumeTimer();
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OpenOptions()
    {
        pausePanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        optionsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f; 
        Time.timeScale = 1f;
        Timer.ResetTimer();
        Timer.StartTimer();

        if (InventoryManager.Instance != null)
        {
            foreach (GadgetType type in System.Enum.GetValues(typeof(GadgetType)))
                items[type] = 0;

            items[GadgetType.StickyWall] = 1;
            items[GadgetType.JumpPad] = 1;
            items[GadgetType.Fan] = 1;
            items[GadgetType.Plug] = 1;

            foreach (var kvp in items)
            {
                int current = InventoryManager.Instance.GetCount(kvp.Key);
                int target = kvp.Value;
                int difference = target - current;

                if (difference != 0)
                    InventoryManager.Instance.AddItem(kvp.Key, difference);
            }
        }
        SceneManager.LoadScene("MAINMENU");
    }

    public void RedoLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (InventoryManager.Instance != null)
        {
            string scene = SceneManager.GetActiveScene().name;
            foreach (GadgetType type in System.Enum.GetValues(typeof(GadgetType)))
            {
                items[type] = 0;
            }

            if (scene == "GAME")
            {
                items[GadgetType.StickyWall] = 1;
                items[GadgetType.JumpPad] = 1;
                items[GadgetType.Fan] = 1;
                items[GadgetType.Plug] = 1;
            }
            else if (scene == "ICEGAME")
            {
                items[GadgetType.StickyWall] = 2;
                items[GadgetType.JumpPad] = 2;
                items[GadgetType.Fan] = 1;
                items[GadgetType.Plug] = 1;
            }
            else if (scene == "LAVAGAME")
            {
                items[GadgetType.StickyWall] = 2;
                items[GadgetType.JumpPad] = 1;
                items[GadgetType.Fan] = 2;
                items[GadgetType.Plug] = 1;
            }

            foreach (var kvp in items)
            {
                int current = InventoryManager.Instance.GetCount(kvp.Key);
                int target = kvp.Value;
                int difference = target - current;

                if (difference != 0)
                    InventoryManager.Instance.AddItem(kvp.Key, difference);
            }
        }
    }

    public void ResetToStart()
    {
        Time.timeScale = 1f;
        Timer.ResetTimer();
        Timer.StartTimer();

        if (InventoryManager.Instance != null)
        {
            foreach (GadgetType type in System.Enum.GetValues(typeof(GadgetType)))
                items[type] = 0;

            items[GadgetType.StickyWall] = 1;
            items[GadgetType.JumpPad] = 1;
            items[GadgetType.Fan] = 1;
            items[GadgetType.Plug] = 1;

            foreach (var kvp in items)
            {
                int current = InventoryManager.Instance.GetCount(kvp.Key);
                int target = kvp.Value;
                int difference = target - current;

                if (difference != 0)
                    InventoryManager.Instance.AddItem(kvp.Key, difference);
            }
        }

        SceneManager.LoadScene("GAME");
    }
}
