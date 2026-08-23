using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class PowerupManager : MonoBehaviour {
    [System.Serializable]
    public class PowerupSlot {
        public Powerup powerup;
        public int chargePercent;
        public int progressIndex = 0;
    }

    public List<PowerupSlot> slots = new List<PowerupSlot>();

    public int chargePerKill = 5;
    public int maxCharge = 100;

    [Header("UI References")]
    public Image[] slotIcons;
    public TMP_Text[] slotLabels; 
    public TMP_Text[] slotCharges; 

    public void OnEnemyKilled() {
        foreach (var slot in slots) {
            if (slot.powerup != null) {
                slot.chargePercent = Mathf.Min(slot.chargePercent + chargePerKill, maxCharge);
            }
        }
        RefreshUI();
    }

    public bool TryActivateByWord(string typedWord, GameObject player) {
        foreach (var slot in slots) {
            if (slot.powerup != null &&
                slot.powerup.powerupName.Equals(typedWord, System.StringComparison.OrdinalIgnoreCase)) {

                if (slot.chargePercent >= slot.powerup.energyCost) {
                    slot.powerup.Activate(player);
                    slot.chargePercent = 0; // reset after use
                    Debug.Log($"{slot.powerup.powerupName} activated!");
                    RefreshUI();
                    return true;
                } else {
                    Debug.Log($"{slot.powerup.powerupName} not ready ({slot.chargePercent}%).");
                }
            }
        }
        return false;
    }

    public void AddPowerup(Powerup p) {
        if (p == null) return;
        slots.Add(new PowerupSlot { powerup = p, chargePercent = 0 });
        Debug.Log($"Added powerup: {p.powerupName}");
        RefreshUI();
    }

    public void SwapPowerup(PowerupCategory category, Powerup newPowerup) {
        if (newPowerup == null) return;

        var slot = slots.Find(s => s.powerup != null && s.powerup.category == category);
        if (slot != null) {
            Debug.Log($"Swapping {slot.powerup.powerupName} with {newPowerup.powerupName}");
            slot.powerup = newPowerup;
            slot.chargePercent = 0;
        } else {
            AddPowerup(newPowerup);
        }
        RefreshUI();
    }

    public void RefreshUI() {
        for (int i = 0; i < slotIcons.Length; i++) {
            if (i < slots.Count && slots[i].powerup != null) {
                slotIcons[i].sprite = slots[i].powerup.icon;
                slotIcons[i].enabled = true;

                if (slotLabels != null && i < slotLabels.Length) {
                    slotLabels[i].text = slots[i].powerup.powerupName;
                }
                if (slotCharges != null && i < slotCharges.Length) {
                    slotCharges[i].text = $"{slots[i].chargePercent}%";
                }
            } else {
                slotIcons[i].enabled = false;
                if (slotLabels != null && i < slotLabels.Length) {
                    slotLabels[i].text = "";
                }
                if (slotCharges != null && i < slotCharges.Length) {
                    slotCharges[i].text = "";
                }
            }
        }
    }
}
