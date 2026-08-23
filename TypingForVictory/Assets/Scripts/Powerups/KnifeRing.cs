using UnityEngine;

public class KnifeRing : MonoBehaviour {
    public float rotationSpeed = 100f;

    void Update() {
        // Rotate the ring around the player
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            // Try to kill either type of enemy
            Enemy e = other.GetComponent<Enemy>();
            if (e != null) e.InstaKill();

            WordEnemy we = other.GetComponent<WordEnemy>();
            if (we != null) we.InstaKill();
        }
    }
}
