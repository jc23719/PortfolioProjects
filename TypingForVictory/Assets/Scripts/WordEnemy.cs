using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WordEnemy : MonoBehaviour {
    public string assignedWord;
    public float speed = 2f;
    private int currentIndex = 0;
    private Transform player;
    private UnityEngine.AI.NavMeshAgent agent;
    private AudioSource audioSource;
    private bool frozen = false;
    private float freezeTimer = 0f;
    private Renderer[] renderers;
    private List<Color[]> originalColors = new List<Color[]>();

    void Start() {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed=speed;

        audioSource = GetComponent<AudioSource>();
        
        if (audioSource != null && AudioManager.Instance != null) {
            AudioManager.Instance.RegisterAmbientSource(audioSource);
            
            audioSource.volume = AudioManager.Instance.sfxSource.volume; 
            
            if (audioSource.clip != null && !audioSource.isPlaying) {
                audioSource.Play();
            }
        }

        renderers = GetComponentsInChildren<Renderer>()
            .Where(r => r.materials.Any(m => m.HasProperty("_Color")))
            .ToArray();
        
        foreach (var r in renderers) {
            if (r.materials != null) {
                Color[] colors = new Color[r.materials.Length];
                for (int i = 0; i < r.materials.Length; i++) {
                    if (r.materials[i].HasProperty("_Color")) {
                        colors[i] = r.materials[i].color;
                    }
                }
                originalColors.Add(colors);
            }
        }

        LetterDisplay display = GetComponentInChildren<LetterDisplay>();
        if (display != null) display.Refresh();
    }

    void Update() {
        if (frozen) {
            freezeTimer -= Time.deltaTime;
            if (freezeTimer <= 0f) {
                frozen = false;
                RestoreVisuals();
            }
            return; // skip movement while frozen
        }
        
        if (player != null&& agent != null && agent.enabled && agent.isOnNavMesh) {
            agent.SetDestination(player.position);
        }
        // Typing logic
        foreach (char c in Input.inputString) {
            if (char.ToLower(c) == char.ToLower(assignedWord[currentIndex])) {
                currentIndex++;
                if (currentIndex >= assignedWord.Length) {
                    Destroy(gameObject);
                }
            } else {
                currentIndex = 0; // resets on wrong input
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null) {
                playerHealth.TakeDamage(1);
            }

            if (audioSource != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.UnregisterAmbientSource(audioSource);
            }    
            Destroy(gameObject); // Removes the enemy after dealing damage
        }
    }

    public void Freeze(float duration) {
        frozen = true;
        freezeTimer = duration;
        ApplyFreezeVisuals();

        if (agent != null && agent.isOnNavMesh && agent.enabled) {
            agent.ResetPath(); // only safe if agent is valid
        }
    }

    private void ApplyFreezeVisuals() {
        foreach (var r in renderers) {
            for (int i = 0; i < r.materials.Length; i++) {
                if (r.materials[i].HasProperty("_Color")) {
                    r.materials[i].color = Color.cyan;
                }
            }
        }
    }

    private void RestoreVisuals() {
        for (int ri = 0; ri < renderers.Length; ri++) {
            var r = renderers[ri];
            for (int mi = 0; mi < r.materials.Length; mi++) {
                if (r.materials[mi].HasProperty("_Color")) {
                    r.materials[mi].color = originalColors[ri][mi];
                }
            }
        }
    }

    public void ApplyKnockback(Vector3 force) {
    StartCoroutine(KnockbackRoutine(force));
    }

    private IEnumerator KnockbackRoutine(Vector3 force) {
        agent.enabled = false;
        float duration = 0.2f;
        float timer = 0f;
        while (timer < duration) {
            transform.position += force * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        agent.enabled = true;
    }

    public bool IsSingleLetter() {
        return assignedWord.Length == 1;
    }

    public void RemoveRandomLetter() {
        if (assignedWord.Length > 1) {
            int index = Random.Range(0, assignedWord.Length);
            assignedWord = assignedWord.Remove(index, 1);
            Debug.Log($"WordEnemy lost a letter, now: {assignedWord}");

            LetterDisplay display = GetComponentInChildren<LetterDisplay>();
            if (display != null) display.Refresh();
        } else {
            InstaKill();
        }
    }

    public void InstaKill() {
        if (audioSource != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.UnregisterAmbientSource(audioSource);
        }
        Destroy(gameObject);
    }
}
