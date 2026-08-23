using UnityEngine;

public class FreezeBehaviour : MonoBehaviour {
    public float freezeDuration = 3f;
    public float radius = 5f;

    void Start() {
        // Destroy prefab after particles finish
        Destroy(gameObject, 2f);

        // Apply freeze immediately to enemies in radius
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider hit in hits) {
            if (hit.CompareTag("Enemy")) {
                Enemy e = hit.GetComponent<Enemy>();
                if (e != null) {
                    e.Freeze(freezeDuration);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            Enemy e = other.GetComponent<Enemy>();
            if (e != null) {
                e.Freeze(freezeDuration);
            }
        }
    }
}
