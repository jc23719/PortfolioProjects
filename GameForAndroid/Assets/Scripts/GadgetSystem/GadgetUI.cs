using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GadgetUI : MonoBehaviour
{
    public static GadgetUI Instance;
    //public InventoryManager inventory;

    [Header("UI Elements")]
    public TextMeshProUGUI stickyWallCount;
    public TextMeshProUGUI jumpPadCount;
    public TextMeshProUGUI fanCount;
    public TextMeshProUGUI plugCount;

    void Start() {
        RefreshUI();
    }


    void Awake() { 
        Instance = this;
    }

    public void RefreshUI() {
        stickyWallCount.text = InventoryManager.Instance.GetCount(GadgetType.StickyWall).ToString(); 
        jumpPadCount.text = InventoryManager.Instance.GetCount(GadgetType.JumpPad).ToString(); 
        fanCount.text = InventoryManager.Instance.GetCount(GadgetType.Fan).ToString(); 
        plugCount.text = InventoryManager.Instance.GetCount(GadgetType.Plug).ToString();
    }
}
