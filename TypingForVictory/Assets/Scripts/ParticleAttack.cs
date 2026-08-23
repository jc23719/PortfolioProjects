using UnityEngine;
using System.Collections;
using TMPro;

public class ParticleAttack : MonoBehaviour {
    public string assignedWord;
    private float speed;
    public int currentIndex = 0;
    private TextMeshPro textDisplay;

    public void Initialize(string word, float moveSpeed, float lifetime) {
        assignedWord = word;
        speed = moveSpeed;

        // Start self destruct timer
        Destroy(gameObject, lifetime);

        LetterDisplay ld = GetComponentInChildren<LetterDisplay>();
    
        if (ld != null) {
            ld.UpdateTextProgress(assignedWord, currentIndex);
            ld.Refresh(); 
        } else {
            Debug.LogError("ParticleAttack failed to find LetterDisplay in children!");
        }
    }

    void Update() {
        foreach (char c in Input.inputString) {
            if (currentIndex < assignedWord.Length) {
                if (char.ToLower(c) == char.ToLower(assignedWord[currentIndex])) {
                    currentIndex++;
                    if (currentIndex >= assignedWord.Length) {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null) {
                playerHealth.TakeDamage(1); 
            }
            Destroy(gameObject);
        }
    }
}