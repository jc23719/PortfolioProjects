using UnityEngine;
using UnityEngine.EventSystems;

public class Rat : MonoBehaviour, IPointerClickHandler
{
    private MiniGameUI miniGame;
    private RectTransform rect;
    private RectTransform container;

    public float moveSpeed = 150f;
    public float minTurnTime = 0.3f;
    public float maxTurnTime = 1.2f;

    private Vector2 direction;
    private float turnTimer;

    public void Init(MiniGameUI mg)
    {
        miniGame = mg;
        rect = GetComponent<RectTransform>();
        container = mg.ratContainer;

        PickNewDirection();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        miniGame.RatCleared();
        Destroy(gameObject);
    }

    void Update()
    {
        MoveRat();
        HandleTurning();
        KeepInsideBounds();
    }


    void MoveRat()
    {
        rect.anchoredPosition += direction * moveSpeed * Time.deltaTime;
    }

    void HandleTurning()
    {
        turnTimer -= Time.deltaTime;

        if (turnTimer <= 0f)
        {
            PickNewDirection();
        }
    }

    void PickNewDirection()
    {
        direction = Random.insideUnitCircle.normalized;
        turnTimer = Random.Range(minTurnTime, maxTurnTime);

        // Rotate sprite to face movement direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rect.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }


    void KeepInsideBounds()
    {
        Vector2 pos = rect.anchoredPosition;
        Vector2 halfSize = container.rect.size / 2f;

        bool bounced = false;

        // Left boundary
        if (pos.x < -halfSize.x)
        {
            pos.x = -halfSize.x;
            direction.x = Mathf.Abs(direction.x);
            bounced = true;
        }
        // Right boundary
        else if (pos.x > halfSize.x)
        {
            pos.x = halfSize.x;
            direction.x = -Mathf.Abs(direction.x);
            bounced = true;
        }

        // Bottom boundary
        if (pos.y < -halfSize.y)
        {
            pos.y = -halfSize.y;
            direction.y = Mathf.Abs(direction.y);
            bounced = true;
        }
        // Top boundary
        else if (pos.y > halfSize.y)
        {
            pos.y = halfSize.y;
            direction.y = -Mathf.Abs(direction.y);
            bounced = true;
        }

        if (bounced)
        {
            // Rotate to new direction after bounce
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rect.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }

        rect.anchoredPosition = pos;
    }
}
