using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float launchForce = 20f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponentInParent<PlayerController>();

            if (player != null)
            {
                player.LaunchFromJumpPad(launchForce);
            }
        }
    }
}
