using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BossEnemy : MonoBehaviour {
    public string[] bossSentences = {
        "The quick brown fox jumps over the lazy dog",
        "Sphinx of black quartz judge my vow",
        "A journey of a thousand miles begins with a single step"
    };
    public GameObject particleWavePrefab;
    
    public float particleSpawnRate = 3.5f;
    public float particleSpeed = 2.0f;
    public float particleLifetime = 8f;
    
    public Transform particleSpawnPoint;
 
    public EnemySpawner letterSpawner;
    public WordEnemySpawner wordSpawner;
    
    public GameObject bossObject;
    private int currentWaveIndex = 0;
    private string currentSentence;
    private List<string> wordsToType;
    private int currentWordIndex = 0;
    private int currentWordCharIndex = 0;
    private bool isAlive = true;
    private Coroutine attackRoutine;
    private bool isWaitingForMinions = false; 
    
    public event System.Action OnBossKilled;

    void Start() {
        if (GetComponent<UnityEngine.AI.NavMeshAgent>() != null) {
            GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        }

        if (particleSpawnPoint == null) {
            particleSpawnPoint = transform;
        }
        
        if (bossObject == null) {
            bossObject = gameObject;
        }

        // Start the first wave
        StartBossWave(0);
    }
    
    void Update() {
        if (!isAlive) return;

        // Check for typing input in Update
        foreach (char c in Input.inputString) {
            HandleTypingInput(c);
        }
    }

    private void StartBossWave(int waveIndex) {
        if (waveIndex >= bossSentences.Length) {
            isAlive = false;
            StopAllCoroutines();
            OnBossKilled?.Invoke();
            Debug.Log("Boss Defeated!");
            Destroy(bossObject, 2f); 
            return;
        }

        currentWaveIndex = waveIndex;
        currentSentence = bossSentences[waveIndex];
        
        // Split the sentence into words for individual typing
        wordsToType = currentSentence.Split(' ').ToList();
        currentWordIndex = 0;
        currentWordCharIndex = 0;
        
        Debug.Log($"Boss Wave {currentWaveIndex + 1} Started. Sentence: {currentSentence}");
        
        // Start the main attack
        if (attackRoutine != null) StopCoroutine(attackRoutine);
        attackRoutine = StartCoroutine(AttackWaveRoutine());
    }

    private void AdvanceWord() {
        currentWordIndex++;
        currentWordCharIndex = 0;
        
        if (currentWordIndex >= wordsToType.Count) {
            Debug.Log($"Sentence {currentWaveIndex + 1} complete! Spawning Minions.");
            
            isWaitingForMinions = true; 
            
            SpawnMinions();
            
            StartCoroutine(WaitForMinionClear());
        }
    }

    private void SpawnMinions() {
        if (currentWaveIndex == 0) {
            // After Sentence 1 Spawns letter enemies
            if (letterSpawner != null) {
                Debug.Log("Boss Spawning Letter Enemies...");
                letterSpawner.SpawnUniqueEnemies(5); 
            } else {
                Debug.LogError("Letter Spawner is not assigned to BossEnemy!");
            }
        } else if (currentWaveIndex == 1) {
            // After Sentence 2 Spawns word enemies
            if (wordSpawner != null) {
                Debug.Log("Boss Spawning Word Enemies...");
                wordSpawner.SpawnUniqueWordEnemies(3); 
            } else {
                Debug.LogError("Word Spawner is not assigned to BossEnemy!");
            }
        }
    }
    
    private IEnumerator WaitForMinionClear() {
        
        Debug.Log("Boss Attack Paused. Waiting for all spawned enemies (Tagged 'Enemy') to be cleared.");

        // Wait until there are no GameObjects with the "Enemy" tag remaining
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);

        Debug.Log("Minions defeated. Resuming boss attack and starting next phase.");
        
        // start the next wave/sentence
        isWaitingForMinions = false;
        StartBossWave(currentWaveIndex + 1);
    }

    public void HandleTypingInput(char c) {
        if (!isAlive || wordsToType == null || currentWordIndex >= wordsToType.Count) return;

        string currentWord = wordsToType[currentWordIndex];
        
        if (currentWordCharIndex < currentWord.Length) {
            if (char.ToLower(c) == char.ToLower(currentWord[currentWordCharIndex])) {
                // Correct character typed
                currentWordCharIndex++;
                if (currentWordCharIndex >= currentWord.Length) {
                    // Word complete
                    Debug.Log($"Word '{currentWord}' successfully typed.");
                    wordsToType[currentWordIndex] = ""; // Clear the word
                    AdvanceWord();
                }
            } else {
                // Incorrect character typed
                Debug.Log("Incorrect character typed for boss word.");
            }
        }
    }

    private string GetRandomAttackWord() {
        if (wordSpawner != null && wordSpawner.wordPool.Length > 0) {
            int index = Random.Range(0, wordSpawner.wordPool.Length);
            return wordSpawner.wordPool[index];
        }
        Debug.LogError("WordEnemySpawner not set or wordPool is empty! Defaulting to 'DODGE'");
        return "DODGE";
    }

    public string GetCurrentWord() {
        if (!isAlive || wordsToType == null || currentWordIndex >= wordsToType.Count) {
            return null;
        }
        return wordsToType[currentWordIndex];
    }

    public int GetCurrentWordProgress() {
        if (!isAlive || wordsToType == null || currentWordIndex >= wordsToType.Count) {
            return 0;
        }
        return currentWordCharIndex;
    }

    private IEnumerator AttackWaveRoutine() {
        while (isAlive) {
            if (!isWaitingForMinions) { 
                GameObject particleGO = Instantiate(
                    particleWavePrefab, 
                    particleSpawnPoint.position, 
                    Quaternion.identity
                );
                
                string attackWord = GetRandomAttackWord(); 
                
                ParticleAttack particleAttack = particleGO.AddComponent<ParticleAttack>();
                particleAttack.Initialize(attackWord, particleSpeed, particleLifetime);
                
                Rigidbody rb = particleGO.GetComponent<Rigidbody>();
                if (rb != null) {
                    // Use negative forward vector to shoot towards player
                    rb.linearVelocity = -transform.forward * particleSpeed;
                } else {
                    Debug.LogWarning("Particle Wave Prefab needs a Rigidbody component!");
                }

                yield return new WaitForSeconds(particleSpawnRate);
            } else {
                yield return null; 
            }
        }
    }
}