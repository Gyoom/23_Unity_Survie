using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{   // Inventory

    public static Inventory instance;

    [Header("OTHER SCRIPTS REFERENCES")]

    [SerializeField]
    private Equipement equipement;

    [SerializeField]
    private ItemActionSystem itemActionSystem;

    [Header("INVENTORY SYSTEM VARIABLES")]

    [SerializeField]
    private List<ItemData> content = new List<ItemData>();

    [SerializeField]
    private GameObject inventoryPanel; 

    [SerializeField]
    private Transform inventorySlotParent;

    public Sprite emptyVisualSlot;

    private bool inventoryIsOpen = false;

    const int INVENTORY_SIZE = 24;

    void Awake() {
        instance = this;
    }

    void Start() 
    {
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
        content.Add(item);
        RefreshContent();
    }

    public void RemoveItem (ItemData item) 
    {
        content.Remove(item);
        RefreshContent();
    }

    public void RefreshContent() 
    {
        for (int i = 0; i < INVENTORY_SIZE; i++)
        {
            Slot currentSlot = inventorySlotParent.GetChild(i).GetComponent<Slot>();
            currentSlot.item = null;
            currentSlot.itemVisual.sprite = emptyVisualSlot;
        }

        for (int i = 0; i < content.Count; i++)
        {
            Slot currentSlot = inventorySlotParent.GetChild(i).GetComponent<Slot>();
            currentSlot.item = content[i];
            currentSlot.itemVisual.sprite = content[i].visual;
        }

         equipement.UpdateEquipementsDesequipButtons();
    }

    public bool IsFull()
    {
        return INVENTORY_SIZE == content.Count;
    }  
}
