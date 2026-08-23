using UnityEngine;

public class CompanionController : MonoBehaviour {
    private Transform player;
    public float speed = 3f;
    public float attackRange = 5f;
    public float fireCooldown = 0.5f;
    [SerializeField] private GameObject bulletPrefab;
    private float fireTimer = 0f;

    public void Initialize(GameObject playerObj) {
        player = playerObj.transform;
    }

    void Update() {
        if (player == null) return;

        // Orbit around player
        transform.RotateAround(player.position, Vector3.up, 50f * Time.deltaTime);

        // Target nearest single-letter enemy
        Enemy[] enemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        Enemy target = null;
        float minDist = Mathf.Infinity;
        foreach (var e in enemies) {
            if (e.IsSingleLetter()) {
                float dist = Vector3.Distance(transform.position, e.transform.position);
                if (dist < minDist) {
                    minDist = dist;
                    target = e;
                }
            }
        }

        fireTimer -= Time.deltaTime;
        if (target != null && minDist <= attackRange && fireTimer <= 0f) {
            ShootAt(target.transform);
            fireTimer = fireCooldown;
        }
    }
    private void ShootAt(Transform target) {
        Vector3 dir = (target.position - transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation(dir);

        GameObject bullet = Instantiate(bulletPrefab, transform.position, rot);
        Bullet b = bullet.GetComponent<Bullet>();
        if (b != null) {
            b.speed = 12f;
        }
    }
}
