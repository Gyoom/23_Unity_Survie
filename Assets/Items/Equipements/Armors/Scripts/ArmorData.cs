using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArmorData", menuName = "My Game/New Item/Equipement/Armor")]
public class ArmorData : EquipementData
{
    [Header("ARMOR DATA")]
    public ArmorType armorType;
    public Mesh visual;
    public Material material;
    public float armorPoints;
}
