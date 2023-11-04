using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject 
{
    [Header("ITEM DATA")]
    public string itemName;
    public string description;
    public Sprite icon;
    public GameObject prefab;
    public bool stackable;
    public int MaxStack;
}