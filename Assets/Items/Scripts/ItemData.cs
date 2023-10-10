using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "My Game/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite visual;
    public GameObject prefab;
    public ItemType itemType;
    public EquipementType equipementType;
}

public enum ItemType
{
    Ressource,
    Equipement,
    Consumable
}

public enum EquipementType 
{
    none,
    Head,
    Chest,
    Hands,
    Legs,
    Feets
}
