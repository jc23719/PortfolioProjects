using UnityEngine;
using System.Collections.Generic;

public class GadgetInventory : MonoBehaviour
{
    public Dictionary<GadgetType, int> items = new Dictionary<GadgetType, int>();

    void Awake()
    {
        // Initialize inventory with zero of each gadget
        foreach (GadgetType type in System.Enum.GetValues(typeof(GadgetType)))
        {
            items[type] = 0;
        }
    }

    public void AddItem(GadgetType type, int amount = 1)
    {
        items[type] += amount;
    }

    public bool UseItem(GadgetType type)
    {
        if (items[type] > 0)
        {
            items[type]--;
            return true;
        }
        return false;
    }

    public int GetCount(GadgetType type)
    {
        return items[type];
    }
}
