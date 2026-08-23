using UnityEngine;

// MSG – fires a short burst of bullets around the player
[CreateAssetMenu(menuName = "Powerups/Weapon/MSG")]
public class MSGPowerup : Powerup {
    [SerializeField] private GameObject bulletPrefab;
    public int count = 24;
    public float radius = 1.5f;

    private void OnEnable() {
        powerupName = "MSG";
        description = "Fires a short burst of bullets around the player (bullets are weak and can only damage single letter enemies).";
        category = PowerupCategory.Weapon;
        energyCost = 100;

        if (bulletPrefab == null)
            bulletPrefab = Resources.Load<GameObject>("Prefabs/BulletPrefab");
        if (icon == null)
            icon = Resources.Load<Sprite>("Icons/MSG");
    }

    public override void Activate(GameObject player) {
        Vector3 origin = player.transform.position;
        for (int i = 0; i < count; i++) {
            float angle = (360f / count) * i;
            Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0f, Mathf.Sin(angle * Mathf.Deg2Rad));
            GameObject bullet = Object.Instantiate(bulletPrefab, origin + dir * radius, Quaternion.LookRotation(dir));
        }
        Debug.Log("MSG burst fired.");
    }
}
