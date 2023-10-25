using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HarvestableData", menuName = "My Game/New Harvestable")]
public class HarvestableData : ScriptableObject
{
    public GameObject harvestablePrefab;

    public Ressource[] dropItems;

    public bool disableKinematicOnharvest;
    public float destroyDelay;
    public ToolData toolData;
}

[Serializable]
public class Ressource
{
    public ItemData itemData;
    [Range(0,100)]
    public int dropChance;
}

public enum Tool
{
    Pickaxe,
    axe
}
