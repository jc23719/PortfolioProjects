using UnityEngine;

public class Tree : MonoBehaviour
{
    public GameObject applePrefab;
    private MiniGameUI miniGame;

    private RectTransform spawnArea;

    public void Init(MiniGameUI mg, int appleCount)
    {
        miniGame = mg;

        spawnArea = transform.Find("AppleZone").GetComponent<RectTransform>();

        for (int i = 0; i < appleCount; i++)
        {
            GameObject apple = Instantiate(applePrefab, spawnArea);
            RectTransform rt = apple.GetComponent<RectTransform>();

            float x = Random.Range(-spawnArea.rect.width / 2f, spawnArea.rect.width / 2f);
            float y = Random.Range(-spawnArea.rect.height / 2f, spawnArea.rect.height / 2f);

            rt.anchoredPosition = new Vector2(x, y);

            apple.GetComponent<Apple>().Init(miniGame);
        }
    }
}
