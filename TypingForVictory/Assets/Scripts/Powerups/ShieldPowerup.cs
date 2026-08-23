using UnityEngine;

// Shield – shields player from damage for a few seconds
[CreateAssetMenu(menuName = "Powerups/Support/Shield")]
public class ShieldPowerup : Powerup {
    public float duration = 4f;
    public GameObject shieldPrefab;

    private void OnEnable() {
        powerupName = "SHIELD";
        description = "Shields player from damage for a few seconds.";
        category = PowerupCategory.Support;
        energyCost = 90;
    }

    public override void Activate(GameObject player) {
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        health.EnableShield(duration);
        Debug.Log("Shield activated.");

        if (shieldPrefab != null) {
            GameObject shieldFx = Instantiate(shieldPrefab, player.transform.position, Quaternion.identity);
            Destroy(shieldFx, duration);
        }
    }
    
}