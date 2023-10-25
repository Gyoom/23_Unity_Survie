using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "My Game/New Item/Equipement/Weapon")]
public class WeaponData : EquipementData
{

    [Header("WEAPON STATS")]
    public Vector3 positionsOffset;
    public Vector3 rotationsOffset;
    public float minDamagePoints;
    public float maxDamagePoints;
}
