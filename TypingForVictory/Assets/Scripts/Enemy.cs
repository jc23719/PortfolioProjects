using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Enemy : MonoBehaviour {
    public char assignedLetter;
    public float speed = 2f;
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
        agent.speed = speed;

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
    }
    
    void Update() {
         if (frozen) {
            freezeTimer -= Time.deltaTime;
            if (freezeTimer <= 0f) {
                frozen = false;
                RestoreVisuals();
            }
            return;
        }

        if (player != null) {
            agent.SetDestination(player.position);
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
            Destroy(gameObject);

        }
    }

    public void Freeze(float duration) {
        frozen = true;
        freezeTimer = duration;
        ApplyFreezeVisuals();

        if (agent != null && agent != null && agent.enabled && agent.isOnNavMesh) {
            agent.ResetPath();
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
        return true;
    }

    public void RemoveRandomLetter() {
        // Not used by single-letter enemies
        Debug.Log("Enemy has no extra letters to remove.");
    }

    public void InstaKill() {
        if (audioSource != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.UnregisterAmbientSource(audioSource);
        }
        Destroy(gameObject);
    }
}

