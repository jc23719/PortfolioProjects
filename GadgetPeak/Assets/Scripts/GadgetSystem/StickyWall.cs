using UnityEngine;

public class StickyWall : MonoBehaviour
{
    public bool playerIsTouching = false;

    private void Start()
    {
        //Destroy(gameObject, stickDuration);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponentInParent<PlayerController>();
            if (player != null)
            {
                player.EnterStickyWall();
                playerIsTouching = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponentInParent<PlayerController>();
            if (player != null)
            {
                player.ExitStickyWall();
                playerIsTouching = false;
            }
        }
    }
}
