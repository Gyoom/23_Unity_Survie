using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "My Game/Item Data")]
public class ItemData : ScriptableObject
{
    public string name;
    public Sprite visual;
    public GameObject prefab;
}
