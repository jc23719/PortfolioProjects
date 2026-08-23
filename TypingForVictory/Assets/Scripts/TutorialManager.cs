using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialManager : MonoBehaviour {
    public GameObject letterEnemyPrefab;
    public GameObject wordEnemyPrefab;
    public GameObject particleAttackerPrefab; 
    public Transform particleSpawnPoint;
    public Transform playerTarget;
    public float particleSpeed = 2f; 
    public float particleLifetime = 8f;
    public TMP_Text promptText;
    public float timeBetweenSteps = 1.5f;
    public PowerupSelectionUI selectionUI; 
    public GameObject playerObject;        
    
    public GameObject powerupHUDPanel; 
    public PowerupHUD powerupHUDScript; 
    private bool waitingForInput = false; 
    private const int KillsToFullCharge = 10; 
    
    void Start() {
        StartCoroutine(TutorialFlow());
    }

    IEnumerator TutorialFlow() {
        promptText.text = "Welcome to Typing For Victory. To survive this night you must type the text above the enemies names to kill them.";
        yield return new WaitForSeconds(timeBetweenSteps * 2);
        
        GameObject letterEnemy = Instantiate(letterEnemyPrefab, new Vector3(0, 0, 8), Quaternion.identity); 
        promptText.text = "This is an example of a single letter enemy. Type the letter 'F' to kill him.";
        yield return new WaitUntil(() => letterEnemy == null);
        
        promptText.text = "Excellent! Now press the Spacebar to continue.";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        
        GameObject wordEnemy = Instantiate(wordEnemyPrefab, new Vector3(8, 0, 0), Quaternion.identity);
        promptText.text = "Next, You are going to need to type a full word to kill this enemy. Type the word 'hello'.";
        yield return new WaitUntil(() => wordEnemy == null);
        
        promptText.text = "Great! Now, choose your Powerup. After each wave of the game you can select a powerup.";
        yield return new WaitForSeconds(timeBetweenSteps);
        
        if (selectionUI == null || playerObject == null) {
            Debug.LogError("PowerupSelectionUI or PlayerObject reference is missing in TutorialManager.");
            yield break;
        }
        
        yield return StartCoroutine(selectionUI.ShowChoicesAndWait(playerObject));

        PowerupManager manager = playerObject.GetComponent<PowerupManager>();
        
        if (manager == null || powerupHUDScript == null) {
            Debug.LogError("PowerupManager or PowerupHUDScript missing on required objects.");
            yield break;
        }
        
        // Connect HUD and Activate Panel
        powerupHUDScript.powerupManager = manager;
        if (powerupHUDPanel != null) {
            powerupHUDPanel.SetActive(true);
        }
        
        promptText.text = $"Kills charge your powerup. Defeat {KillsToFullCharge} enemies to reach full charge.";
        
        for (int i = 0; i < KillsToFullCharge; i++) {
            Vector3 safeSpawnPos = Random.insideUnitCircle.normalized * 8f;
            GameObject nextEnemy = Instantiate(letterEnemyPrefab, new Vector3(safeSpawnPos.x, 0, safeSpawnPos.y), Quaternion.identity);
            
            yield return new WaitUntil(() => nextEnemy == null);

            manager.OnEnemyKilled();
            
            yield return new WaitForEndOfFrame();
        }
        
        promptText.text = "The powerup is fully charged! Type its name to activate it.";
        
        waitingForInput = true;
        yield return new WaitUntil(() => !waitingForInput); 

        promptText.text = "Powerup Activated! Each powerup is slightly different so be strategic.";
        yield return new WaitForSeconds(timeBetweenSteps);
        
        promptText.text = "Final mechanic: The Boss launches word attacks. Type the word above the attack before it hits!";
        yield return new WaitForSeconds(timeBetweenSteps);
        
        if (particleSpawnPoint == null || playerTarget == null) {
             Debug.LogError("Particle Spawn Point or Player Target is missing for defense step.");
             yield break;
        }

        GameObject attackerInstance = Instantiate(
            particleAttackerPrefab, 
            particleSpawnPoint.position, 
            Quaternion.identity
        );
        
        ParticleAttack particleScript = attackerInstance.GetComponent<ParticleAttack>();
        if (particleScript != null) {
            particleScript.Initialize(
                "DEFENSE",
                particleSpeed,
                particleLifetime
            ); 
        }
        
        Rigidbody rb = attackerInstance.GetComponent<Rigidbody>();
        if (rb == null) {
            rb = attackerInstance.AddComponent<Rigidbody>();
        }
        rb.isKinematic = false;
        rb.useGravity = false;
        
        // Calculate direction to player and apply velocity
        Vector3 directionToPlayer = (playerTarget.position - particleSpawnPoint.position).normalized;
        rb.linearVelocity = directionToPlayer * particleSpeed;

        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("ParticleAttack").Length == 0);

        promptText.text = "Defense successful!";
        yield return new WaitForSeconds(timeBetweenSteps);
        
        promptText.text = "Tutorial complete! Loading main game...";
        yield return new WaitForSeconds(3f);
        
        GoToMainMenu();
    }

    public void OnPowerupUsedSimulated() {
        if (waitingForInput) {
            waitingForInput = false;
        }
    }

    void GoToMainMenu() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("MenuScene"); 
    }
}