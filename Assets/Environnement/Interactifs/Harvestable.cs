using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvestable : MonoBehaviour
{
    [SerializeField]
    public Ressource[] HaverstableItems;

    public bool disableKinematicOnharvest;
    public float destroyDelay;
    public Tool tool;
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

