using System;
using UnityEngine;

public class ItemActionSystem : MonoBehaviour
{

    [Header("OTHER SCRIPTS REFERENCES")]

    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private Transform parentSceneItems;

    [SerializeField]
    private EquipementSystem equipementSystem;

    [Header("ACTION PANEL SYSTEM VARIABLES")]

    public GameObject actionPanel;

    [SerializeField]
    private Transform dropPoint;

    [SerializeField]
    private GameObject useItemButton; 

    [SerializeField]
    private GameObject equipItemButton;

    [SerializeField]
    private GameObject unequipItemButton;

    [SerializeField]
    private GameObject dropItemButton;

    [SerializeField]
    private GameObject destroyItemButton;

    [HideInInspector]
    public ItemData itemDataCurrentlySelected;

    [HideInInspector]
    public AbstractInventory currentItemInventory;

    public void OpenActionPanel(ItemData item, Vector3 slotPosition, AbstractInventory newItemInventory) 
    {
        itemDataCurrentlySelected = item;

        if (item == null) 
        {
            actionPanel.SetActive(false);
            return;
        }

        useItemButton.SetActive(false);
        equipItemButton.SetActive(false);
        unequipItemButton.SetActive(false);

 
        if(item.GetType().IsSubclassOf(typeof(EquipementData))) 
        {   
            if (newItemInventory.GetType() == typeof(EquipementInventory) || 
                newItemInventory.GetType() == typeof(ActionInventory))
                unequipItemButton.SetActive(true);
            else
                equipItemButton.SetActive(true);
        }
        else if(item.GetType() == typeof(ConsummableData)) 
            useItemButton.SetActive(true);

        currentItemInventory = newItemInventory;
        actionPanel.transform.position = slotPosition;
        actionPanel.SetActive(true);
    }

    public void CloseActionPanel() {
        actionPanel.SetActive(false);
        itemDataCurrentlySelected = null;
    }

    public void UseActionButton() 
    {
        playerStats.consumeItem(((ConsummableData) itemDataCurrentlySelected).consumableEffects);
        currentItemInventory.RemoveItem(itemDataCurrentlySelected);
        CloseActionPanel();
    }

    public void EquipActionButton() {
        if((itemDataCurrentlySelected.GetType() == typeof(WeaponData)) || 
            (itemDataCurrentlySelected.GetType() ==typeof(ToolData)))
        {
             if(ActionInventory.instance.AddItem(itemDataCurrentlySelected))
                currentItemInventory.RemoveItem(itemDataCurrentlySelected);
        }
        if (itemDataCurrentlySelected.GetType() == typeof(ArmorData))
        {
            if(EquipementInventory.instance.AddItem(itemDataCurrentlySelected))
                currentItemInventory.RemoveItem(itemDataCurrentlySelected);
        }

        CloseActionPanel();
    }

    public void UnequipActionButton() {
        if(MainInventory.instance.AddItem(itemDataCurrentlySelected))
        {
            if(itemDataCurrentlySelected.GetType() == typeof(WeaponData) || 
                itemDataCurrentlySelected.GetType() == typeof(ToolData))
            {
                    currentItemInventory.RemoveItem(itemDataCurrentlySelected);
                
            }
            else if (itemDataCurrentlySelected.GetType() == typeof(ArmorData))
            {
                    currentItemInventory.RemoveItem(itemDataCurrentlySelected);    
            }
        }

        CloseActionPanel();
    }
    

    public void DropActionButton() 
    {
        GameObject instantiatedItem = Instantiate(itemDataCurrentlySelected.prefab, parentSceneItems);
        instantiatedItem.transform.position = dropPoint.position;
        instantiatedItem.transform.SetParent(parentSceneItems, false);
        currentItemInventory.RemoveItem(itemDataCurrentlySelected);
        CloseActionPanel();
    }

    public void DestroyActionButton() 
    {
        currentItemInventory.RemoveItem(itemDataCurrentlySelected);
        CloseActionPanel();
    
    }
}
