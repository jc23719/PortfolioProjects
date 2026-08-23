using UnityEngine;

public class Bullet : MonoBehaviour {
    public float speed = 10f;
    public float lifetime = 2f;

    void Start() {
        Destroy(gameObject, lifetime);
    }

    void Update() {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) {
        Enemy e = other.GetComponent<Enemy>();
        WordEnemy we = other.GetComponent<WordEnemy>();

        if (e != null && e.IsSingleLetter()) {
            e.InstaKill();
            Destroy(gameObject);
        }
        if (we != null && we.IsSingleLetter()) {
            we.InstaKill();
            Destroy(gameObject);
        }
    }
}
