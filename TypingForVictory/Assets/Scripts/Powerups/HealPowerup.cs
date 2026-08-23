using UnityEngine;

// Heal – heals player for 2 hearts
[CreateAssetMenu(menuName = "Powerups/Support/Heal")]
public class HealPowerup : Powerup {
    public int hearts = 2;

    private void OnEnable() {
        powerupName = "HEAL";
        description = "Heals player for 2 hearts.";
        category = PowerupCategory.Support;
        energyCost = 80;
    }

    public override void Activate(GameObject player) {
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        if (health == null) {
            Debug.LogError("HealPowerup: No PlayerHealth found on " + player.name);
        } else {
            Debug.Log("HealPowerup: Found PlayerHealth on " + player.name);
            health.Heal(hearts);
        }
    }
}