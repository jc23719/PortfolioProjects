using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class PowerupSelectionUI : MonoBehaviour {
    public GameObject panel;
    public Button aoeButton;
    public Button supportButton;
    public Button weaponButton;

    public Image aoeIcon;
    public Image supportIcon;
    public Image weaponIcon;

    public TMP_Text aoeTitle;
    public TMP_Text supportTitle;
    public TMP_Text weaponTitle;

    public TMP_Text aoeDescription;
    public TMP_Text supportDescription;
    public TMP_Text weaponDescription;

    public Button cancelButton;
    public PowerupDatabase database;

    public List<Powerup> allPowerups;

    private Powerup aoeChoice, supportChoice, weaponChoice;
    private PowerupManager targetManager;
    private bool picked;
    public bool IsActive => panel.activeSelf;

    public IEnumerator ShowChoicesAndWait(GameObject player) {
        targetManager = player.GetComponent<PowerupManager>();
        if (targetManager == null) {
            Debug.LogError("Player missing PowerupManager component!");
            yield break;
        }

        PopulateChoices();
        BindButtons();

        panel.SetActive(true);
        picked = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Wait until one is chosen
        yield return new WaitUntil(() => picked);

        panel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void PopulateChoices() {
        aoeChoice = RandomFromCategory(PowerupCategory.AOE);
        supportChoice = RandomFromCategory(PowerupCategory.Support);
        weaponChoice = RandomFromCategory(PowerupCategory.Weapon);

        SetSlotUI(aoeIcon, aoeTitle, aoeDescription, aoeChoice);
        SetSlotUI(supportIcon, supportTitle, supportDescription, supportChoice);
        SetSlotUI(weaponIcon, weaponTitle, weaponDescription, weaponChoice);
    }

    private Powerup RandomFromCategory(PowerupCategory category) {
        var list = database.allPowerups.Where(p => p.category == category).ToList();
        if (list.Count == 0) {
            Debug.LogWarning($"No powerups found for category {category}");
            return null;
        }
        return list[Random.Range(0, list.Count)];
    }

    private void SetSlotUI(Image icon, TMP_Text title, TMP_Text desc, Powerup choice) {
        if (choice == null) {
            title.text = "None";
            desc.text = "No powerup available";
            icon.enabled = false;
            return;
        }
        title.text = choice.powerupName;
        desc.text = choice.description;
        icon.enabled = true;
        icon.sprite = choice.icon;
    }

    private void BindButtons() {
        aoeButton.onClick.RemoveAllListeners();
        supportButton.onClick.RemoveAllListeners();
        weaponButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();

        aoeButton.onClick.AddListener(() => Pick(aoeChoice));
        supportButton.onClick.AddListener(() => Pick(supportChoice));
        weaponButton.onClick.AddListener(() => Pick(weaponChoice));
        cancelButton.onClick.AddListener(CancelSelection);
    }

    private void Pick(Powerup choice) {
        if (choice == null) return;
        targetManager.SwapPowerup(choice.category, choice);
        Debug.Log($"Picked/swapped powerup: {choice.powerupName}");
        picked = true;
    }


    private void CancelSelection() {
        Debug.Log("Selection cancelled.");
        picked = true; // exit the coroutine
        panel.SetActive(false);
    }
}
