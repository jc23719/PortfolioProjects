using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    // Stores how many of each gadget the player has
    private Dictionary<GadgetType, int> items = new Dictionary<GadgetType, int>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (FindObjectsOfType<InventoryManager>().Length > 1) { 
            Destroy(gameObject); return; 
        }
         
        DontDestroyOnLoad(gameObject);

        // Initialize all gadgets with count of 0
        foreach (GadgetType type in System.Enum.GetValues(typeof(GadgetType)))
        {
            items[type] = 0;
        }

        // Give the player one of each gadget at start
        items[GadgetType.StickyWall] = 1;
        items[GadgetType.JumpPad] = 1;
        items[GadgetType.Fan] = 1;
        items[GadgetType.Plug] = 1;
    }


    public void AddItem(GadgetType type, int amount = 1)
    {
        items[type] += amount;
        GadgetUI.Instance.RefreshUI();
    }

    public bool UseItem(GadgetType type)
    {
        if (items[type] > 0)
        {
            items[type]--;
            GadgetUI.Instance.RefreshUI();
            return true;
        }
        return false;
    }

    public int GetCount(GadgetType type)
    {
        return items[type];
    }
}
