using UnityEngine;
using System.Collections.Generic;

public static class SaveSystem {
    // Save everything
    public static void SaveGame(PlayerHealth player, PowerupManager powerupManager, WaveManager waveManager) {
        // Save wave
        if (waveManager != null) {
            PlayerPrefs.SetInt("WaveNumber", waveManager.GetCurrentWave());
            Debug.Log($"Saved WaveNumber = {waveManager.GetCurrentWave()}");
        }

        // Save powerups
        if (powerupManager != null) {
            int powerupIndex = 0;
            foreach (var slot in powerupManager.slots) {
                if (slot.powerup != null) {
                    PlayerPrefs.SetString($"Powerup{powerupIndex}", slot.powerup.powerupName);
                    PlayerPrefs.SetInt($"Charge{powerupIndex}", slot.chargePercent);
                    powerupIndex++;
                }
            }
            PlayerPrefs.SetInt("PowerupCount", powerupIndex);
            Debug.Log($"Saved {powerupIndex} powerups");

            Enemy[] letterEnemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            WordEnemy[] wordEnemies = Object.FindObjectsByType<WordEnemy>(FindObjectsSortMode.None);

            PlayerPrefs.SetInt("LetterEnemyCount", letterEnemies.Length);
            for (int letterIndex = 0; letterIndex < letterEnemies.Length; letterIndex++) {
                Vector3 pos = letterEnemies[letterIndex].transform.position;
                PlayerPrefs.SetFloat($"LetterEnemy{letterIndex}_X", pos.x);
                PlayerPrefs.SetFloat($"LetterEnemy{letterIndex}_Y", pos.y);
                PlayerPrefs.SetFloat($"LetterEnemy{letterIndex}_Z", pos.z);
                PlayerPrefs.SetString($"LetterEnemy{letterIndex}_Letter", letterEnemies[letterIndex].assignedLetter.ToString());
            }

            PlayerPrefs.SetInt("WordEnemyCount", wordEnemies.Length);
            for (int wordIndex = 0; wordIndex < wordEnemies.Length; wordIndex++) {
                Vector3 pos = wordEnemies[wordIndex].transform.position;
                PlayerPrefs.SetFloat($"WordEnemy{wordIndex}_X", pos.x);
                PlayerPrefs.SetFloat($"WordEnemy{wordIndex}_Y", pos.y);
                PlayerPrefs.SetFloat($"WordEnemy{wordIndex}_Z", pos.z);
                PlayerPrefs.SetString($"WordEnemy{wordIndex}_Word", wordEnemies[wordIndex].assignedWord);
            }


            
            PlayerPrefs.SetInt("PowerupCount", powerupIndex);
            Debug.Log($"Saved {powerupIndex} powerups");
        }

        PlayerPrefs.Save();
    }

    // Load everything
    public static void LoadGame(GameObject player, PowerupManager powerupManager, WaveManager waveManager) {
        // Restore wave
        if (waveManager != null && PlayerPrefs.HasKey("WaveNumber")) {
            int savedWave = PlayerPrefs.GetInt("WaveNumber");
            waveManager.SetCurrentWave(savedWave, resumeMidWave: true);
            Debug.Log($"Loaded WaveNumber = {savedWave}");
        }


        // Restore powerups
        if (powerupManager != null) {
            powerupManager.slots.Clear();
            int count = PlayerPrefs.GetInt("PowerupCount", 0);
            for (int i = 0; i < count; i++) {
                string name = PlayerPrefs.GetString($"Powerup{i}");
                int charge = PlayerPrefs.GetInt($"Charge{i}");

                Powerup p = FindPowerupByName(name);
                if (p != null) {
                    powerupManager.slots.Add(new PowerupManager.PowerupSlot {
                        powerup = p,
                        chargePercent = charge,
                        progressIndex = 0
                    });
                }
            }
            powerupManager.RefreshUI();
            Debug.Log($"Loaded {count} powerups");
        }
    }

    // Wipe everything
    public static void ClearSave() {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Save data cleared");
    }

    private static Powerup FindPowerupByName(string name) {
        Powerup[] all = Resources.LoadAll<Powerup>("");
        foreach (var p in all) {
            if (p.powerupName == name) return p;
        }
        Debug.LogWarning($"Powerup {name} not found in Resources!");
        return null;
    }

}
