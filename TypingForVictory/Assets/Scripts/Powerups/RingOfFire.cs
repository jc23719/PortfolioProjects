using UnityEngine;

public class RingOfFire : MonoBehaviour {
    public float duration = 3f;

    void Start() {
        Destroy(gameObject, duration);
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            Enemy e = other.GetComponent<Enemy>();
            if (e != null) e.InstaKill();

            WordEnemy we = other.GetComponent<WordEnemy>();
            if (we != null) we.RemoveRandomLetter();
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.CompareTag("Enemy")) {
            Enemy e = other.GetComponent<Enemy>();
            if (e != null) e.Freeze(1f);
        }
    }
}
