using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    public CameraBehaviour cam;
    public BoxCollider2D respawnLevelBounds;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.position = RespawnPoint.current.position;

            //cam.SetBounds(respawnLevelBounds);
            //cam.MoveToBounds(respawnLevelBounds, 0.5f);
        }
    }
}
