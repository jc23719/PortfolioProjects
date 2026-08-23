using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PowerupDatabase", menuName = "Game/Powerup Database")]
public class PowerupDatabase : ScriptableObject {
    public List<Powerup> allPowerups;
}