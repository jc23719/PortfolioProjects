using UnityEngine;

public class TypingInput : MonoBehaviour {
    public GameObject gameOverPanel;
    private PowerupManager powerupManager;
    public TutorialManager tutorialManager;

    void Start() {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            powerupManager = player.GetComponent<PowerupManager>();
        }
    }


    void Update()
    {
        foreach (char c in Input.inputString)
        {
            Enemy[] enemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            foreach (Enemy e in enemies)
            {
                if (char.ToLower(c) == char.ToLower(e.assignedLetter))
                {
                    if (powerupManager != null) {
                        powerupManager.OnEnemyKilled();
                    }
                    Destroy(e.gameObject);
                    break;
                }
            }
            if (powerupManager != null) {
            foreach (var slot in powerupManager.slots) {
                if (slot.powerup == null) continue;

                string name = slot.powerup.powerupName.ToLower();

                if (char.ToLower(c) == name[slot.progressIndex]) {
                    slot.progressIndex++;
                    if (slot.progressIndex == name.Length) {
                        if (slot.chargePercent >= slot.powerup.energyCost) {
                            slot.powerup.Activate(GameObject.FindGameObjectWithTag("Player"));
                            slot.chargePercent = 0;
                            Debug.Log($"{slot.powerup.powerupName} activated!");
                            if (tutorialManager != null) {
                                tutorialManager.OnPowerupUsedSimulated();
                            }
                        }
                        slot.progressIndex = 0; // reset after activation
                    }
                } else {
                    slot.progressIndex = 0; // reset if wrong letter
                }
            }
        }
        }
    

    }
    
    public void TriggerGameOver() {
        if (gameOverPanel != null) {
            gameOverPanel.SetActive(true);
        }

        // Freeze gameplay
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


}