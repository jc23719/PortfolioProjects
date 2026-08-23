using UnityEngine;

// Ground Blast – pushes all enemies back away from player
[CreateAssetMenu(menuName = "Powerups/AOE/GroundBlast")]
public class GroundBlastPowerup : Powerup {
    [SerializeField] private GameObject blastPrefab;
    public float force = 12f;

    private void OnEnable() {
        powerupName = "GROUNDBLAST";
        description = "Pushes all enemies back away from player.";
        category = PowerupCategory.AOE;
        energyCost = 100;

        if (blastPrefab == null)
            blastPrefab = Resources.Load<GameObject>("Prefabs/GroundBlastParticles");
        if (icon == null)
            icon = Resources.Load<Sprite>("Icons/GroundBlast");
    }

    public override void Activate(GameObject player) {
        Vector3 pos = player.transform.position;

        // Spawn visual blast effect if prefab is assigned
        if (blastPrefab != null) {
            GameObject blast = Object.Instantiate(blastPrefab, pos, Quaternion.identity);
            Object.Destroy(blast, 2f); // auto-cleanup after 2 seconds
        }

        // Affect all Enemy types globally
        Enemy[] enemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy e in enemies) {
            Vector3 dir = (e.transform.position - pos).normalized;
            e.ApplyKnockback(dir * force);
        }

        WordEnemy[] wordEnemies = Object.FindObjectsByType<WordEnemy>(FindObjectsSortMode.None);
        foreach (WordEnemy we in wordEnemies) {
            Vector3 dir = (we.transform.position - pos).normalized;
            we.ApplyKnockback(dir * force);
        }
        Debug.Log("Ground Blast activated.");
    }
}