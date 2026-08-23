using UnityEngine;

public class GameInitializer : MonoBehaviour {
    void Start() {
        var player = GameObject.FindWithTag("Player");
        var powerupManager = player?.GetComponent<PowerupManager>();
        var waveManager = FindFirstObjectByType<WaveManager>();

        // Restore save data
        SaveSystem.LoadGame(player, powerupManager, waveManager);

        // Restore health if saved
        var health = player.GetComponent<PlayerHealth>();
        if (health != null && PlayerPrefs.HasKey("PlayerHealth")) {
            int savedHealth = PlayerPrefs.GetInt("PlayerHealth", health.maxHealth);
            health.RestoreHealth(savedHealth);
        }

        if (powerupManager != null) {
            powerupManager.RefreshUI();
        }
    }
}
