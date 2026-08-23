using UnityEngine;

// Knife – a circle of knives surrounds the player
[CreateAssetMenu(menuName = "Powerups/Weapon/Knife")]
public class KnifePowerup : Powerup {
    [SerializeField]private GameObject knifeRingPrefab;
    public float duration = 4f;

    private void OnEnable() {
        powerupName = "KNIFE";
        description = "A circle of knives will surround the player killing enemies that touch them (only lasts for a short amount of time).";
        category = PowerupCategory.Weapon;
        energyCost = 100;

        if (knifeRingPrefab == null)
            knifeRingPrefab = Resources.Load<GameObject>("Prefabs/KnifeRingPrefab");
        if (icon == null)
            icon = Resources.Load<Sprite>("Icons/Knife");
    }

    public override void Activate(GameObject player) {
        GameObject ring = Object.Instantiate(knifeRingPrefab, player.transform);
        Object.Destroy(ring, duration);
        Debug.Log("Knife ring activated.");
    }
}
