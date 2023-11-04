using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EquipementInventory : AbstractInventory
{
    public static EquipementInventory instance;

    [Header("SCRIPTS REFERENCES")]
    [SerializeField]
    private EquipementSystem equipementSystem;

    void Awake() {
        instance = this;
        foreach (Transform inventorySlotParent in inventorySlotParents)
            inventorySize += inventorySlotParent.transform.childCount; 
    }

    void Start() 
    {
        InitContent();
        RefreshContent();
    }

    protected override void InitContent()
    {
        // Content
        ItemInInventory[] contentTemp = new ItemInInventory[inventorySize];

        for (int i = 0; i < contentTemp.Length; i++)
        {   
            if (i < content.Length)
            {
                contentTemp[i] =  new ItemInInventory {itemData = content[i].itemData, count = content[i].count};
                inventoryFilling++;
            }
            else 
               contentTemp[i] =  new ItemInInventory {itemData = null, count = -1}; 
        }

        content = contentTemp;

        // Slots
        slots = new EquipementSlot[inventorySize];

        int offset = 0;
        foreach (Transform inventorySlotParent in inventorySlotParents)
        {
            for (int i = 0; i < inventorySlotParent.childCount; i++)
            {
                slots[offset] = inventorySlotParent.transform.GetChild(i).GetComponent<EquipementSlot>();
                offset++;  
            }
        }
    }

    protected override bool CheckBeforeAddItem(ItemData itemDataToAdd, int indexContent)
    {
        // Check new object type
        if (itemDataToAdd.GetType() != typeof(ArmorData))
        {
            Debug.Log("Seul les armures sont autorisées dans l'equipement panel");
            return false;
        }
        if (((ArmorData) itemDataToAdd).armorType != ((EquipementSlot) slots[indexContent]).armorType)
        {
            Debug.Log("Piece d'armure incorrect");
            return false;
        }

        if (content[indexContent].itemData != null)
        {   
            if (MainInventory.instance.inventoryFilling >= MainInventory.instance.inventorySize)
            {
                Debug.Log("Inventaire plein");
                return false;
            }
            else
            {
                ItemInInventory previousItem = RemoveItem(indexContent);
                MainInventory.instance.AddItem(previousItem.itemData);
            }
        }

        return true;
    }

    public override bool AddItem(ItemInInventory itemToAdd, int itemIndex)
    {
        if(!CheckBeforeAddItem(itemToAdd.itemData, itemIndex))
            return false;
        // add in content
        content[itemIndex].itemData = itemToAdd.itemData;
        content[itemIndex].count = itemToAdd.count;

        equipementSystem.EquipArmor((ArmorData) itemToAdd.itemData);
        // add in slot
        RefreshContent();
        return true;
    }

    public override bool AddItem (ItemData armorDataToAdd)
    {
        int armorTypeSlotindex = -1;

        for (int i = 0; i < slots.Length; i++)
        {
            if (((EquipementSlot) slots[i]).armorType == ((ArmorData) armorDataToAdd).armorType)
            {
                armorTypeSlotindex = i;
                break;
            }     
        }
        
        if (armorTypeSlotindex == -1)
        {
            Debug.Log("Slot d'équipement correpondant à l'item inexistant");
            return false;
        }

        if(!CheckBeforeAddItem(armorDataToAdd, armorTypeSlotindex))
            return false;

        content[armorTypeSlotindex].itemData = armorDataToAdd;
        content[armorTypeSlotindex].count = 1;

        equipementSystem.EquipArmor((ArmorData) armorDataToAdd);
        // add in slot
        RefreshContent();
        return true;
    }
}

public enum ArmorType
{
    Head,
    Chest,
    Hands,
    Legs,
    Feets
}
