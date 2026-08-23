using UnityEngine;

// Ring of Fire – removes a letter from enemies and insta kills single letter enemies
[CreateAssetMenu(menuName = "Powerups/AOE/RingOfFire")]
public class RingOfFirePowerup : Powerup {
    public float radius = 6f;
    public float duration = 3f;
    [SerializeField] private GameObject ringPrefab;

    private void OnEnable() {
        powerupName = "FIRERING";
        description = "Removes a letter from enemies and insta kills single letter enemies in that ring then disappears.";
        category = PowerupCategory.AOE;
        energyCost = 100;

        if (ringPrefab == null)
            ringPrefab = Resources.Load<GameObject>("Prefabs/RingOfFireParticles");
        if (icon == null)
            icon = Resources.Load<Sprite>("Icons/RingOfFire");
    }

    public override void Activate(GameObject player) {
        GameObject ring = Object.Instantiate(ringPrefab, player.transform);
        Object.Destroy(ring, duration);

        // Apply AoE effect immediately to all enemies in range
        Vector3 pos = ring.transform.position;
        Enemy[] enemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (var e in enemies) {
            float d = Vector3.Distance(pos, e.transform.position);
            if (d <= radius) {
                if (e.IsSingleLetter()) {
                    e.InstaKill();
                } else {
                    e.RemoveRandomLetter();
                }
            }
        }
        Debug.Log("Ring of Fire activated.");
    }
}