using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerupHUD : MonoBehaviour {
    public PowerupManager powerupManager;

    public GameObject aoeSlotPanel;
    public Image aoeIcon;
    public TMP_Text aoeName;
    public Slider aoeCharge;

    public GameObject supportSlotPanel;
    public Image supportIcon;
    public TMP_Text supportName;
    public Slider supportCharge;

    public GameObject weaponSlotPanel;
    public Image weaponIcon;
    public TMP_Text weaponName;
    public Slider weaponCharge;

    void Update() {
        UpdateSlot(PowerupCategory.AOE, aoeSlotPanel, aoeIcon, aoeName, aoeCharge);
        UpdateSlot(PowerupCategory.Support, supportSlotPanel, supportIcon, supportName, supportCharge);
        UpdateSlot(PowerupCategory.Weapon, weaponSlotPanel, weaponIcon, weaponName, weaponCharge);
    }


    private void UpdateSlot(PowerupCategory category, GameObject slotPanel, Image icon, TMP_Text name, Slider charge) {
        if (powerupManager == null) return;

        var slot = powerupManager.slots.Find(s => s.powerup != null && s.powerup.category == category);
        if (slot != null) {
            slotPanel.SetActive(true);
            icon.sprite = slot.powerup.icon;
            icon.enabled = true;
            name.text = slot.powerup.powerupName;
            charge.minValue = 0f;
            charge.maxValue = 100f;
            charge.value = Mathf.Clamp(slot.chargePercent, 0f, 100f);
        } else {
            slotPanel.SetActive(false);
        }
    }
}
