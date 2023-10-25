using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArmorData", menuName = "My Game/New Item/Equipement/Armor")]
public class ArmorData : EquipementData
{
    [Header("ARMOR STATS")]
    public float armorPoints;
}
