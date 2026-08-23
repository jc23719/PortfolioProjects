using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {
    public int maxHealth = 3;
    private int currentHealth;
    public AudioClip hitSound;
    public TypingInput TypingInput;
    private AudioSource hitAudioSource;
    public HealthUI healthUI;
    private bool shieldActive = false;


    void Start() {
        currentHealth = maxHealth;
        hitAudioSource = transform.Find("HitAudio").GetComponent<AudioSource>();
    }

    public void TakeDamage(int amount){
        if (shieldActive){
            Debug.Log("Shield absorbed damage");
            return;
        }

        currentHealth -= amount;
        Debug.Log("Player took damage! Hearts left: " + currentHealth);
        healthUI.UpdateHearts(currentHealth);

        if (hitAudioSource != null && hitSound != null)
        {
            hitAudioSource.PlayOneShot(hitSound);
        }

        if (currentHealth <= 0)
        {
            Debug.Log("Player is dead!");
            Death();
        }
    }
    
    void Death()
    {
        TypingInput.TriggerGameOver();

        gameObject.SetActive(false);
    }

    public void PlayAgain() {
        Time.timeScale = 1f;
        SaveSystem.ClearSave();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RestoreHealth(int value) {
        currentHealth = Mathf.Clamp(value, 0, maxHealth);
        healthUI.UpdateHearts(currentHealth);
    }

    public void EnableShield(float duration) {
        if (!shieldActive) {
            shieldActive = true;
            Debug.Log("Shield enabled!");
            Invoke(nameof(DisableShield), duration);
        }
    }

    private void DisableShield() {
        shieldActive = false;
        Debug.Log("Shield expired.");
    }

    public void Heal(int hearts) {
        int before = currentHealth;
        currentHealth = Mathf.Min(currentHealth + hearts, maxHealth);
        Debug.Log($"Heal() called. Before: {before}, After: {currentHealth}");
        healthUI.UpdateHearts(currentHealth);
    }
}
