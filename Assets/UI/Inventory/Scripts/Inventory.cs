using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading;

public class Inventory : MonoBehaviour
{   // Inventory

    public static Inventory instance;

    [Header("SCRIPTS REFERENCES")]

    [SerializeField]
    private Equipement equipement;

    [SerializeField]
    private ItemActionSystem itemActionSystem;

    [SerializeField]
    private CraftingSystem craftingSystem;

    [SerializeField]
    private BuildSystem buildSystem;

    [Header("INVENTORY SYSTEM VARIABLES")]

    [SerializeField]
    private List<ItemInInventory> content = new List<ItemInInventory>();

    [SerializeField]
    private GameObject inventoryPanel; 

    [SerializeField]
    private Transform inventorySlotParent;

    public Sprite emptyVisualSlot;

    private bool inventoryIsOpen = false;

    private int INVENTORY_SIZE = 24;

    void Awake() {
        instance = this;
        INVENTORY_SIZE = inventorySlotParent.transform.childCount;
    }

    void Start() 
    {
        inventoryPanel.SetActive(false);
        instance = this;
        RefreshContent();
        CloseInventory();
    }

    void Update () 
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryIsOpen)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }    
    }

    private void OpenInventory() 
    {
        inventoryPanel.SetActive(true);
        inventoryIsOpen = true;
    }

    public void CloseInventory() 
    {
        inventoryPanel.SetActive(false);
        itemActionSystem.actionPanel.SetActive(false);
        TooltipSystem.instance.Hide();
        inventoryIsOpen = false;
    }

    public void AddItem (ItemData item) 
    {
        ItemInInventory[] itemsInInventory = content.Where(i => i.itemData == item).ToArray();

        bool itemAdded = false;

        if (itemsInInventory.Length > 0 && item.stackable)
        {
            foreach(ItemInInventory itemInInventory in itemsInInventory)
            {
                if (itemInInventory.count < item.MaxStack)
                {
                    itemInInventory.count++;
                    itemAdded = true;
                    break;
                }
            } 

            if(!itemAdded)
                content.Add(new ItemInInventory{ itemData = item, count = 1 });
        }
        else 
        {
            content.Add(new ItemInInventory{ itemData = item, count = 1 });
        }
        RefreshContent();
    }

    public void RemoveItem (ItemData item) // ToDo, prend un nombre en param
    {
        ItemInInventory itemInInventory = content.Where(i => i.itemData == item).FirstOrDefault();
        if (itemInInventory != null && itemInInventory.count > 1)
        {
            itemInInventory.count--;
        } 
        else 
        {
            content.Remove(itemInInventory);
        }
        RefreshContent();
    }

    public bool IsFull()
    {
        return INVENTORY_SIZE == content.Count;
    } 

    public List<ItemInInventory> getContent()
    {
        return content;
    }

    public void SetContent(List<ItemInInventory> newContent)
    {
        content = newContent;
        RefreshContent();
    }

    public void RefreshContent() 
    {
        for (int i = 0; i < INVENTORY_SIZE; i++)
        {
            Slot currentSlot = inventorySlotParent.GetChild(i).GetComponent<Slot>();
            currentSlot.item = null;
            currentSlot.itemVisual.sprite = emptyVisualSlot;
            currentSlot.countText.enabled = false;
        }

        for (int i = 0; i < content.Count; i++)
        {
            Slot currentSlot = inventorySlotParent.GetChild(i).GetComponent<Slot>();
            currentSlot.item = content[i].itemData;
            currentSlot.itemVisual.sprite = content[i].itemData.icon;

            if (currentSlot.item.stackable)
            {
                currentSlot.countText.enabled = true;
                currentSlot.countText.text = content[i].count.ToString();
            }
        }

        craftingSystem.UpdateDisplayedRecipes();
        equipement.UpdateEquipementsDesequipButtons();
        buildSystem.UpdateDisplayCosts();
    }

    public void ClearContent()
    {
        content.Clear();
    }

}

[Serializable]
public class ItemInInventory
{
    public ItemData itemData;
    public int count;
}
