using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    public BoxCollider2D nextBounds;
    public float cameraMoveTime = 1.5f;

    private bool transitioning = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (transitioning) return;
        if (!other.CompareTag("Player")) return;

        transitioning = true;

        PlayerController p = other.GetComponent<PlayerController>();
        CameraBehaviour cam = Camera.main.GetComponent<CameraBehaviour>();

        p.levelBounds = nextBounds;

        p.StartAutoWalk(3f);
        StartCoroutine(Transition(cam, p));
    }

    private System.Collections.IEnumerator Transition(CameraBehaviour cam, PlayerController p)
    {
        cam.MoveToBounds(nextBounds, cameraMoveTime);

        yield return new WaitForSeconds(cameraMoveTime);

        p.StopAutoWalk();
    }
}
