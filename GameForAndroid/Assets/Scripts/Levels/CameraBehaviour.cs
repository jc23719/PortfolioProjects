using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform target;
    public BoxCollider2D bounds;

    private float minX, maxX, minY, maxY;

    private bool manualMove = false;
    private Vector3 manualTarget;
    private float manualSpeed = 3f;

    void Update()
    {
        if (manualMove)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(manualTarget.x, manualTarget.y, transform.position.z),
                manualSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, manualTarget) < 0.05f)
                manualMove = false;

            return;
        }

        Bounds b = bounds.bounds;

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, b.min.x, b.max.x);
        pos.y = Mathf.Clamp(pos.y, b.min.y, b.max.y);

        transform.position = pos;

    }


    public void MoveToBounds(BoxCollider2D newBounds, float duration)
    {
        bounds = newBounds;
        manualMove = true;
        manualTarget = newBounds.bounds.center;
        manualSpeed = Vector3.Distance(transform.position, manualTarget) / duration;
    }

    public void SetBounds(BoxCollider2D newBounds)
    {
        bounds = newBounds;
    }
}
