using UnityEngine;

// Companion – slowly targets incoming single letter enemies
[CreateAssetMenu(menuName = "Powerups/Support/Companion")]
public class CompanionPowerup : Powerup {
    public GameObject companionPrefab;
    public float lifetime = 10f;
    public float spawnRadius = 2f;

    private void OnEnable() {
        powerupName = "COMPANION";
        description = "Slowly targets incoming single letter enemies.";
        category = PowerupCategory.Support;
        energyCost = 100;

        if (companionPrefab == null)
            companionPrefab = Resources.Load<GameObject>("Prefabs/CompanionPrefab");
        if (icon == null)
            icon = Resources.Load<Sprite>("Icons/Companion");
    }

    public override void Activate(GameObject player) {
        // Pick a random angle around the player
        float angle = Random.Range(0f, 360f);
        Vector3 offset = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            0f,
            Mathf.Sin(angle * Mathf.Deg2Rad)
        ) * spawnRadius;

        // Spawn companion at offset position
        GameObject comp = Object.Instantiate(
            companionPrefab,
            player.transform.position + offset,
            Quaternion.identity
        );

        comp.GetComponent<CompanionController>().Initialize(player);

        Object.Destroy(comp, lifetime);
        Debug.Log("Companion summoned.");
    }
}