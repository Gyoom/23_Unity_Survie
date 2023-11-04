using System;
using UnityEngine;

public class ItemActionSystem : MonoBehaviour
{

    [Header("OTHER SCRIPTS REFERENCES")]

    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private Transform parentSceneItems;

    [Header("ACTION PANEL SYSTEM VARIABLES")]

    public GameObject actionPanel;

    [SerializeField]
    private Transform dropPoint;

    [SerializeField]
    private GameObject useItemButton; 

    [SerializeField]
    private GameObject equipItemButton;

    [SerializeField]
    private GameObject dropItemButton;

    [SerializeField]
    private GameObject destroyItemButton;

    [HideInInspector]
    public ItemData itemDataCurrentlySelected;

    [HideInInspector]
    public AbstractInventory itemInventory;

    public void OpenActionPanel(ItemData item, Vector3 slotPosition, AbstractInventory newItemInventory) 
    {
        itemDataCurrentlySelected = item;

        if (item == null) 
        {
            actionPanel.SetActive(false);
            return;
        }

        if(item.GetType() == typeof(RessourceData)) 
        {
            useItemButton.SetActive(false);
            equipItemButton.SetActive(false);
        }
        else if(item.GetType().IsSubclassOf(typeof(EquipementData))) 
        {
            useItemButton.SetActive(false);
            equipItemButton.SetActive(true);
        }

        else if(item.GetType() == typeof(ConsummableData)) 
        {
            useItemButton.SetActive(true);
            equipItemButton.SetActive(false);
        }

        itemInventory = newItemInventory;
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
        itemInventory.RemoveItem(itemDataCurrentlySelected);
        CloseActionPanel();
    }

    public void EquipActionButton() {
        if((itemDataCurrentlySelected.GetType() == typeof(WeaponData)) || 
            (itemDataCurrentlySelected.GetType() ==typeof(ToolData)))
        {
             if(ActionInventory.instance.AddItem(itemDataCurrentlySelected))
                itemInventory.RemoveItem(itemDataCurrentlySelected);
        }
        if (itemDataCurrentlySelected.GetType() == typeof(ArmorData))
        {
            if(EquipementInventory.instance.AddItem(itemDataCurrentlySelected))
                itemInventory.RemoveItem(itemDataCurrentlySelected);
        }

        CloseActionPanel();
    }
    

    public void DropActionButton() 
    {
        GameObject instantiatedItem = Instantiate(itemDataCurrentlySelected.prefab, parentSceneItems);
        instantiatedItem.transform.position = dropPoint.position;
        instantiatedItem.transform.SetParent(parentSceneItems, false);
        itemInventory.RemoveItem(itemDataCurrentlySelected);
        CloseActionPanel();
    }

    public void DestroyActionButton() 
    {
        itemInventory.RemoveItem(itemDataCurrentlySelected);
        CloseActionPanel();
    
    }
}
