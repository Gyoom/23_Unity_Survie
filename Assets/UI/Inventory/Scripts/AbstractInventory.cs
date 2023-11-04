using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AbstractInventory : MonoBehaviour
{
    [Header("INVENTORY SYSTEM VARIABLES")]
    // Content
    [SerializeField]
    protected ItemInInventory[] content;

    [SerializeField]
    protected AbstractSlot[] slots;

    [SerializeField]
    protected Transform[] inventorySlotParents;

    [SerializeField]
    protected UIManager uIManager;

    [HideInInspector]
    public int inventorySize = 0;

    public int inventoryFilling = 0;




    // Manage Content

    protected virtual void InitContent()
    {
        // Content
        slots = new InventorySlot[inventorySize];
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
        
        int offset = 0;
        foreach (Transform inventorySlotParent in inventorySlotParents)
        {
            for (int i = 0; i < inventorySlotParent.childCount; i++)
            {
                slots[offset] = inventorySlotParent.transform.GetChild(i).GetComponent<InventorySlot>();
                offset++;  
            }
        }
    }

    public void SetContent(ItemInInventory[] newContent)
    {
        content = newContent;
        RefreshContent();
    }

    public ItemInInventory[] getContent()
    {
        return content;
    }

    public virtual void RefreshContent()
    {
        inventoryFilling = 0;
        for (int i = 0; i < inventorySize; i++)
        {
            if (content[i].itemData != null)
                inventoryFilling++;
            slots[i].Reset();
            slots[i].Configure(content[i], i);
        }
    }

    public bool IsFull()
    {
        return inventorySize == inventoryFilling;
    } 

    public void ClearContent() 
    {
        content = new ItemInInventory[inventorySize];
    }

    protected abstract bool CheckBeforeAddItem(ItemData itemDataToAdd, int indexContent);

    // Add Item
    public virtual bool AddItem(ItemInInventory itemToAdd, int itemIndex)
    {
        if(!CheckBeforeAddItem(itemToAdd.itemData, itemIndex))
            return false;
        // add in content
        content[itemIndex].itemData = itemToAdd.itemData;
        content[itemIndex].count = itemToAdd.count;
        // add in slot
        RefreshContent();
        return true;
    }

    public virtual bool AddItem (ItemData itemDataToAdd)
    {
        ItemInInventory[] itemsInInventory = content.Where(i => i != null && i.itemData == itemDataToAdd).ToArray();

        bool itemAdded = false;
        // check add in existing pile
        if (itemDataToAdd.stackable)
        {
            foreach(ItemInInventory itemInInventory in itemsInInventory)
            {
                if (itemInInventory.count < itemDataToAdd.MaxStack)
                {
                    itemInInventory.count++;
                    itemAdded = true;
                    RefreshContent();
                    break;
                }
            } 
        }
        if (!itemAdded)
            return NoExistingPile(itemDataToAdd);
        else
            return true;
    }

    protected bool NoExistingPile(ItemData itemToAdd)
    {
        for (int i = 0; i < content.Length; i++)
        {
            if (content[i].itemData == null)
            {
                if (CheckBeforeAddItem(itemToAdd, i))
                {
                    content[i].itemData = itemToAdd;
                    content[i].count = 1;
                    RefreshContent();
                    return true;
                }
                else 
                    return false;
            }
        }
        return false;
    }

    //Remove Item

    public ItemInInventory RemoveItem (ItemData item)
    {
        ItemInInventory itemInInventory = content.Where(i => i != null && i.itemData == item).FirstOrDefault();
        ItemInInventory removedItem = new ItemInInventory {itemData = itemInInventory.itemData, count = itemInInventory.count};

        if (itemInInventory != null && itemInInventory.count > 1)
        {
            itemInInventory.count--;
            RefreshContent();
            removedItem.count = 1;
        } 
        else 
        {
            itemInInventory.itemData = null;
            itemInInventory.count = -1;
            RefreshContent();
        }
        return removedItem;
    }

    public ItemInInventory RemoveItem(int itemIndex)
    {
        // empty local slot/visual
        slots[itemIndex].Reset(); 
        ItemInInventory removedItem = new ItemInInventory {itemData = content[itemIndex].itemData, count = content[itemIndex].count};
        content[itemIndex].itemData = null;
        content[itemIndex].count = -1;
        return removedItem;
    }

    public bool LoadContent(ItemInInventory[] inventoryContent)
    {
        return true;
    }
}

[Serializable]
public class ItemInInventory
{
    public ItemData itemData;
    public int count;
}
