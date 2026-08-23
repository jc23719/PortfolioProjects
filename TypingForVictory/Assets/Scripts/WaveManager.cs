using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class WaveManager : MonoBehaviour {

    private bool IsHardMode {
        get {
            return SceneManager.GetActiveScene().name == "HardGameScene"; 
        }
    }

    public EnemySpawner normalSpawner;
    public WordEnemySpawner wordSpawner;
    public int maxWaves = 10;
    private int currentWave = 1;
    public PowerupSelectionUI selectionUI;
    public GameObject Player;
    public bool restoreFromSave = false;
    public BossEnemy bossPrefab;
    public Transform bossSpawnLocation;
    public GameObject youWinUIPanel;

    public TextMeshProUGUI waveText;
    public float waveDisplayDuration = 2.0f;
    public float waveFadeDuration = 0.5f;

    void Start() {
        if (!restoreFromSave) {
            StartCoroutine(ProgressWaves());
        } else {
            Debug.Log("WaveManager: restoreFromSave active skipping automatic wave spawn");
        }
    }

    private int GetSpawnMultiplier() {
        // Hard mode spawns 4 additional enemies for each enemy type
        return IsHardMode ? 4 : 0;
    }

    IEnumerator ProgressWaves() {
        int extraSpawnCount = GetSpawnMultiplier();

        while (currentWave <= maxWaves) { 
            int wordCount = 0;
            int normalCount = 0;

            if (currentWave < 10 && !restoreFromSave) {
                yield return StartCoroutine(DisplayWaveText(currentWave, waveDisplayDuration, waveFadeDuration));
            }
            if (currentWave < 10) {
                int normalBase = 10 + (currentWave - 1) * 5;
                int wordBase = (currentWave >= 2) ? (currentWave - 1) * 4 : 0;

                normalCount = normalBase + extraSpawnCount;
                wordCount = wordBase + extraSpawnCount;

                if (!restoreFromSave) {

                    normalSpawner.SpawnUniqueEnemies(normalCount);
                    wordSpawner.SpawnUniqueWordEnemies(wordCount);

                    Debug.Log($"Wave {currentWave} started: {normalCount} normal, {wordCount} word enemies");
                }
                
                yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);
            
            } 
            
            else if (currentWave == 10) {
                Debug.Log("BOSS WAVE STARTED!");
                yield return StartCoroutine(DisplayWaveText(currentWave, 3.0f, 0.75f, "BOSS WAVE"));
                
                BossEnemy boss = Instantiate(bossPrefab, bossSpawnLocation.position, Quaternion.identity);
                
                if (boss != null) {
                    boss.letterSpawner = this.normalSpawner; 
                    boss.wordSpawner = this.wordSpawner;     
                }

                bool bossIsDead = false;
                boss.OnBossKilled += () => {
                    bossIsDead = true;
                    if (youWinUIPanel != null)
                    {
                        youWinUIPanel.SetActive(true);
                        Cursor.lockState = CursorLockMode.None; // unlock cursor
                        Cursor.visible = true;
                    }
                };
                yield return new WaitUntil(() => bossIsDead);
                
                Debug.Log("Boss successfully defeated!");
            }
            
            if (currentWave < maxWaves && selectionUI != null) {
                yield return selectionUI.ShowChoicesAndWait(Player);
            }
            
            currentWave++;

            if (restoreFromSave) {
                restoreFromSave = false;
            }
        }

        Debug.Log("All waves complete or Boss Defeated!");
    }

    private IEnumerator DisplayWaveText(int waveNumber, float displayDuration, float fadeDuration, string overrideText = null) {
        if (waveText == null) yield break;

        waveText.text = overrideText ?? $"WAVE {waveNumber}";
        waveText.gameObject.SetActive(true);

        CanvasGroup waveGroup = waveText.GetComponent<CanvasGroup>();
        if (waveGroup == null)
        {
            waveGroup = waveText.gameObject.AddComponent<CanvasGroup>();
        }

        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            waveGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            yield return null;
        }
        waveGroup.alpha = 1f;

        yield return new WaitForSeconds(displayDuration);

        time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            waveGroup.alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            yield return null;
        }
        waveGroup.alpha = 0f;

        waveText.gameObject.SetActive(false);
    }

    public int GetCurrentWave() {
        return currentWave;
    }


    public void SetCurrentWave(int wave, bool resumeMidWave = false) {
        currentWave = wave;
        Debug.Log($"Wave restored to {wave}");

        StopAllCoroutines();

        if (resumeMidWave) {
            restoreFromSave = true;
            // Respawn saved enemies
            RestoreEnemiesForWave(wave);
            StartCoroutine(ProgressWaves());
        } else {
            restoreFromSave = false;
            // Start wave from beginning
            StartCoroutine(ProgressWaves());
        }
    }

    // Called when loading a save
    private void RestoreEnemiesForWave(int wave) {
        // Clear any existing enemies
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
            Destroy(enemy);
        }

        // Restore letter enemies
        int letterCount = PlayerPrefs.GetInt("LetterEnemyCount", 0);
        for (int letterIndex = 0; letterIndex < letterCount; letterIndex++) {
            float x = PlayerPrefs.GetFloat($"LetterEnemy{letterIndex}_X");
            float y = PlayerPrefs.GetFloat($"LetterEnemy{letterIndex}_Y");
            float z = PlayerPrefs.GetFloat($"LetterEnemy{letterIndex}_Z");
            char letter = PlayerPrefs.GetString($"LetterEnemy{letterIndex}_Letter")[0];

            GameObject prefab = Resources.Load<GameObject>("Prefabs/Enemy0");
            var enemy = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity)
                .GetComponent<Enemy>();
            enemy.assignedLetter = letter;
        }

        // Restore word enemies
        int wordCount = PlayerPrefs.GetInt("WordEnemyCount", 0);
        for (int wordIndex = 0; wordIndex < wordCount; wordIndex++) {
            float x = PlayerPrefs.GetFloat($"WordEnemy{wordIndex}_X");
            float y = PlayerPrefs.GetFloat($"WordEnemy{wordIndex}_Y");
            float z = PlayerPrefs.GetFloat($"WordEnemy{wordIndex}_Z");
            string word = PlayerPrefs.GetString($"WordEnemy{wordIndex}_Word");

            GameObject prefab = Resources.Load<GameObject>("Prefabs/WordEnemy");
            var wordEnemy = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity)
                .GetComponent<WordEnemy>();
            wordEnemy.assignedWord = word;
        }
    }

}

