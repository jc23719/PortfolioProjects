using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class EndMenu : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private Dictionary<GadgetType, int> items = new Dictionary<GadgetType, int>();

    void OnEnable()
    {
        Timer.StopTimer();
        float finalTime = Timer.GetTime();
        timerText.text = FormatTime(finalTime);
    }

    public void PlayAgain()
    {
        Timer.ResetTimer();
        Timer.StartTimer();

        if (InventoryManager.Instance != null)
        {
            foreach (GadgetType type in System.Enum.GetValues(typeof(GadgetType)))
                items[type] = 0;

            items[GadgetType.StickyWall] = 1;
            items[GadgetType.JumpPad] = 1;
            items[GadgetType.Fan] = 1;
            items[GadgetType.Plug] = 1;

            foreach (var kvp in items)
            {
                int current = InventoryManager.Instance.GetCount(kvp.Key);
                int target = kvp.Value;
                int difference = target - current;

                if (difference != 0)
                    InventoryManager.Instance.AddItem(kvp.Key, difference);
            }
        }

        SceneManager.LoadScene("GAME");
    }

    public void BackToMenu()
    {
        Timer.ResetTimer();
        SceneManager.LoadScene("MainMenu");
    }

    private string FormatTime(float t)
    {
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);
        int ms = Mathf.FloorToInt((t * 1000) % 1000);

        return $"{minutes:00}:{seconds:00}:{ms:000}";
    }
}
