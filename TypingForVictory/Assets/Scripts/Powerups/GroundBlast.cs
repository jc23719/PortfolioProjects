using UnityEngine;

public class GroundblastBehaviour : MonoBehaviour {
    [Header("Blast settings")]
    public float maxRadius = 6f;
    public float expandDuration = 0.3f;
    public float lingerDuration = 0.2f; // optional small window after expansion
    public bool instaKillSingleLetter = true;

    private SphereCollider trigger;
    private float timer;
    private float totalDuration;
    private Vector3 startScale;

    void Awake() {
        trigger = GetComponent<SphereCollider>();
        if (trigger == null) {
            Debug.LogWarning("GroundblastBehaviour requires a SphereCollider (Is Trigger).");
        }
        startScale = transform.localScale;
        totalDuration = expandDuration + lingerDuration;
    }

    void OnEnable() {
        timer = 0f;
        // Start small
        trigger.radius = 0.1f;
        transform.localScale = startScale;
    }

    void Update() {
        timer += Time.deltaTime;

        // Expand collider over time
        float t = Mathf.Clamp01(timer / expandDuration);
        float currentRadius = Mathf.Lerp(0.1f, maxRadius, t);
        trigger.radius = currentRadius;

        float visualScale = currentRadius / 0.5f;
        transform.localScale = new Vector3(visualScale, 1f, visualScale);

        // Destroy after total duration
        if (timer >= totalDuration) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Enemy")) return;

        // Try WordEnemy first for letter removal
        WordEnemy we = other.GetComponent<WordEnemy>();
        if (we != null) {
            we.RemoveRandomLetter();
            return;
        }

        // Fallback: standard Enemy
        Enemy e = other.GetComponent<Enemy>();
        if (e != null) {
            if (instaKillSingleLetter && e.IsSingleLetter()) {
                e.InstaKill();
            } else {
                e.ApplyKnockback((other.transform.position - transform.position).normalized * 4f);
            }
        }
    }
}
