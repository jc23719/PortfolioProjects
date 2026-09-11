using UnityEngine;

public class Fan : MonoBehaviour
{
    public float floorForce = 3f;
    public float wallForce = 80f;

    public Vector2 pushDirection = Vector2.right;
    public bool isWallFan = false;

    public float floorImpulseInterval = 10f; // time between blasts 
    private float floorImpulseTimer = 0f;
    private bool hasPlayedFanSound = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasPlayedFanSound)
        {
            hasPlayedFanSound = true;
            SoundManager.Instance.PlaySFX(SoundManager.Instance.fanSFX);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerController pc = collision.GetComponent<PlayerController>();
        if (pc != null)
            pc.disableHorizontalControl = isWallFan; 

        Rigidbody2D rb = collision.attachedRigidbody;
        if (rb == null) return;

        if (isWallFan)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            rb.AddForce(pushDirection * wallForce, ForceMode2D.Force);
            return;
        }

        floorImpulseTimer -= Time.deltaTime; 
        if (floorImpulseTimer <= 0f) {
            rb.AddForce(pushDirection * floorForce, ForceMode2D.Impulse); 
            floorImpulseTimer = floorImpulseInterval; 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController pc = collision.GetComponent<PlayerController>();
        if (pc != null)
            pc.disableHorizontalControl = false;

        floorImpulseTimer = 0f;

        if (collision.CompareTag("Player"))
            hasPlayedFanSound = false;
    }
}
