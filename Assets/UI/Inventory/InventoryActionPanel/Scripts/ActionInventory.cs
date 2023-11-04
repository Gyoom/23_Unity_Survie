using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class ActionInventory : AbstractInventory
{
    public static ActionInventory instance;

    private Type[] itemsTypesAllowed = {typeof(WeaponData),typeof(StructureData), typeof(ToolData)};

    [SerializeField]
    private Color selectedColor;
    [SerializeField]
    private Color unselectedColor;
    [SerializeField]
    private int slotSelected;

    [Header("SCRIPTS REFERENCES")]
    [SerializeField]
    private EquipementSystem equipSystem;

    [SerializeField]
    private BuildSystem buildSystem;

    [SerializeField]
    private CraftingSystem craftSystem;

    [SerializeField]
    private BreakMenu breakMenu;

    void Awake() {
        instance = this;
        foreach (Transform inventorySlotParent in inventorySlotParents)
            inventorySize += inventorySlotParent.transform.childCount; 
    }

    void Start()
    {
        InitContent();
        RefreshContent();
        UpdateSlotSelected(0);
    }

    void Update()
    {
        int newSlotSelected = slotSelected;
        newSlotSelected -= (int) (Input.mouseScrollDelta.y * 1f);
        if (newSlotSelected >= inventorySize) newSlotSelected = 0;
        if (newSlotSelected < 0) newSlotSelected = inventorySize - 1;
        if (slotSelected != newSlotSelected) UpdateSlotSelected(newSlotSelected);

        if (content[slotSelected].itemData != null && content[slotSelected].itemData.GetType() == typeof(StructureData))
            uIManager.UpdateInteractText("Click gauche pour poser une structure"); 
        
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

       UpdateSlotAction(slotSelected);
    }

    private void UpdateSlotSelected(int scroll)
    {
        if (craftSystem.craftPanelIsOpen || breakMenu.breakMenuIsOpen)
            return;
        
        for (int i = 0; i < content.Length; i++)
        {
            if(i == scroll) 
            {
                slots[i].gameObject.GetComponent<Image>().color = selectedColor;
                UpdateSlotAction(i);
            }
            else
                slots[i].gameObject.GetComponent<Image>().color = unselectedColor;
        }
        slotSelected = scroll;
    }

    private void UpdateSlotAction(int index)
    {
        buildSystem.DiseabledSystem();
        equipSystem.DesequipWeapon();
        equipSystem.DesequipTool();

        if (content[index].itemData != null)
        {
            if (content[index].itemData.GetType() == typeof(WeaponData))
                equipSystem.EquipWeapon((WeaponData) content[index].itemData);
            
            else if (content[index].itemData.GetType() == typeof(ToolData))
                equipSystem.EquipTool((ToolData) content[index].itemData);
            
            else if (content[index].itemData.GetType() == typeof(StructureData))
            {
                buildSystem.EnableSystem((StructureData) content[index].itemData); 
            }
        } 
        else
            equipSystem.DesequipWeapon();
    }

    protected override bool CheckBeforeAddItem(ItemData itemDataToAdd, int indexContent)
    {
        // Check new object type
        bool allowed = false;
        foreach (Type typeAllowed in itemsTypesAllowed)
        {   
            if (itemDataToAdd.GetType() == typeAllowed)
            {
                allowed = true;
                break;
            }
        }

        if (!allowed)
        {
            Debug.Log("Type d'Objet non autorisé dans le action Panel");
            return false;
        }

        // check local slot free space
        if(content[indexContent].itemData != null)
        {
            StartCoroutine(uIManager.UpdateErrorText("Slot " + indexContent + " occupé"));
            return false;
        }

        return true;
    }
}
