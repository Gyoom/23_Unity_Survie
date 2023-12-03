using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MainInventory : AbstractInventory
{   // Inventory

    public static MainInventory instance;

    [SerializeField]
    private GameObject inventoryPanel; 
    
    [Header("SCRIPTS REFERENCES")]

    [SerializeField]
    private ItemActionSystem itemActionSystem;

    [SerializeField]
    private CraftingSystem craftingSystem;

    private bool inventoryIsOpen = false;

    void Awake() {
        instance = this;
        foreach (Transform inventorySlotParent in inventorySlotParents)
            inventorySize += inventorySlotParent.transform.childCount;   
    }

    void Start() 
    {
        InitContent();
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
        uIManager.PanelStatusUpdate();
    }

    public void CloseInventory() 
    {
        inventoryPanel.SetActive(false);
        itemActionSystem.actionPanel.SetActive(false);
        ToolUISystem.instance.HideTip();
        ToolUISystem.instance.HideTranfert();
        inventoryIsOpen = false;
         uIManager.PanelStatusUpdate();
    }

    public override void RefreshContent() 
    {
        inventoryFilling = 0;
        for (int i = 0; i < inventorySize; i++)
        {
            if (content[i].itemData != null)
                inventoryFilling++;
            slots[i].Reset();
            slots[i].Configure(content[i], i);
        }

        craftingSystem.UpdateDisplayedRecipes();
        //equipement.UpdateEquipementsDesequipButtons();
        //buildSystem.UpdateDisplayCosts();
    }

    protected override bool CheckBeforeAddItem(ItemData itemDataToAdd, int indexContent)
    {
        // check local slot free space
        if(content[indexContent].itemData != null)
        {
            StartCoroutine(uIManager.UpdateErrorText("Slot " + indexContent + " occupé"));
            return false;
        }

        return true;
    }
}


