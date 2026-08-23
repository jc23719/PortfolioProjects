using UnityEngine;

public enum PowerupCategory { AOE, Support, Weapon }

[CreateAssetMenu(menuName = "Powerups/Powerup Base")]
public class Powerup : ScriptableObject {
    public string powerupName;
    public string description;
    public Sprite icon;
    public PowerupCategory category;
    public int energyCost = 100; // how much charge is needed

    public virtual void Activate(GameObject player) {
        Debug.Log($"Activated {powerupName}");
    }
}

