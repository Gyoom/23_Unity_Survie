using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "My Game/New Item/Equipement/Weapon")]
public class WeaponData : EquipementData
{

    [Header("WEAPON DATA")]

    public GameObject visual;
    public float minDamagePoints;
    public float maxDamagePoints;
}
