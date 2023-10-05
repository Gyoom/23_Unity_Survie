using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{   // Inventory
    [SerializeField]
    private List<ItemData> content = new List<ItemData>();

    public void AddItem (ItemData item) 
    {
        content.Add(item);
    }
}
