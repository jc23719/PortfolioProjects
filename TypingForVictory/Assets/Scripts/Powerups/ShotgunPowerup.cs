using UnityEngine;

// Shotgun – fires a single blast in 4 directions like a cross
[CreateAssetMenu(menuName = "Powerups/Weapon/Shotgun")]
public class ShotgunPowerup : Powerup {
    [SerializeField] private GameObject bulletPrefab;
    public int bulletsPerDirection = 12;
    public float spreadAngle = 15f;
    public float radius = 1.0f;
    public float bulletSpeed = 12f;
    public float duration = 2f;

    private void OnEnable() {
        powerupName = "SHOTGUN";
        description = "Fires a single blast in 4 directions like a cross insta killing any enemies in range (takes more energy to fill to use).";
        category = PowerupCategory.Weapon;
        energyCost = 100;

        if (bulletPrefab == null)
            bulletPrefab = Resources.Load<GameObject>("Prefabs/BulletPrefab");
        if (icon == null)
            icon = Resources.Load<Sprite>("Icons/Shotgun");
    }

    public override void Activate(GameObject player) {
        Vector3 origin = player.transform.position;

        // Base directions for the cross
        Vector3[] dirs = {
            Vector3.forward,
            Vector3.back,
            Vector3.left,
            Vector3.right
        };

        foreach (var dir in dirs) {
            // For each direction, spawn multiple bullets with angle offsets
            for (int i = 0; i < bulletsPerDirection; i++) {
                float angle = Mathf.Lerp(-spreadAngle, spreadAngle, i / (float)(bulletsPerDirection - 1));

                Quaternion rot = Quaternion.AngleAxis(angle, Vector3.up) * Quaternion.LookRotation(dir);

                // Spawn bullet
                GameObject bullet = Object.Instantiate(
                    bulletPrefab,
                    origin + dir * radius,
                    rot
                );

                // Override bullet settings
                Bullet b = bullet.GetComponent<Bullet>();
                if (b != null) {
                    b.speed = bulletSpeed;
                    b.lifetime = duration;
                }
            }
        }
        Debug.Log("Shotgun blast fired.");
    }
}