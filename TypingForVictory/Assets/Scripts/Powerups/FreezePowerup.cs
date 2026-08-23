using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/AOE/Freeze")]
public class FreezePowerup : Powerup {
    [SerializeField] private GameObject freezePrefab;
    public float freezeDuration = 3f;

    private void OnEnable() {
        powerupName = "FREEZE";
        description = "Slows enemies for a short amount of time.";
        category = PowerupCategory.AOE;
        energyCost = 100;

        if (freezePrefab == null)
            freezePrefab = Resources.Load<GameObject>("Prefabs/FreezeParticles");
        if (icon == null)
            icon = Resources.Load<Sprite>("Icons/Freeze");
    }

    public override void Activate(GameObject player) {
        Vector3 pos = player.transform.position;

        // Spawn icy visual effect if prefab is assigned
        if (freezePrefab != null) {
            GameObject fx = Object.Instantiate(freezePrefab, pos, Quaternion.identity);
            Object.Destroy(fx, freezeDuration);
        }

        // Affect all Enemy types globally
        Enemy[] enemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy e in enemies) {
            e.Freeze(freezeDuration);
        }

        WordEnemy[] wordEnemies = Object.FindObjectsByType<WordEnemy>(FindObjectsSortMode.None);
        foreach (WordEnemy we in wordEnemies) {
            we.Freeze(freezeDuration);
        }

        Debug.Log("All enemies frozen for " + freezeDuration + " seconds.");
    }
}
