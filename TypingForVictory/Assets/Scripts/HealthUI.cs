using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour {
    public Image[] hearts; 
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public void UpdateHearts(int currentHealth) {
        for (int i = 0; i < hearts.Length; i++) {
            hearts[i].sprite = i < currentHealth ? fullHeart : emptyHeart;
        }
    }
}
