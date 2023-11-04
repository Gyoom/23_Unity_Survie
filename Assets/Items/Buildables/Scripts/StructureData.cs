using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StructureData", menuName = "My Game/New Item/Structure")]
public class StructureData : ItemData
{
    public GameObject placementPrefab;
    public StructureType structureType;

    public ItemInInventory[] ressoucesCost;
}

[Serializable]
public enum StructureType
{
    Stairs,
    Wall,
    Floor
}
