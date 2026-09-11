using UnityEngine;
using UnityEngine.EventSystems;

public class Apple : MonoBehaviour, IPointerClickHandler
{
    private MiniGameUI miniGame;

    public void Init(MiniGameUI mg)
    {
        miniGame = mg;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        miniGame.AppleCollected();
        Destroy(gameObject);
    }
}
