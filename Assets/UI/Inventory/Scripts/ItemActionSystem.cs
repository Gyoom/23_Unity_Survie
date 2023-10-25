using System;
using UnityEngine;

public class ItemActionSystem : MonoBehaviour
{

    [Header("OTHER SCRIPTS REFERENCES")]

    [SerializeField]
    private Equipement equipement;

    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private Transform itemsParent;

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
    public ItemData itemCurrentlySelected;

    public void OpenActionPanel(ItemData item, Vector3 slotPosition) 
    {
        itemCurrentlySelected = item;

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

        actionPanel.transform.position = slotPosition;
        actionPanel.SetActive(true);
    }

    public void closeActionPanel() {
        actionPanel.SetActive(false);
        itemCurrentlySelected = null;
    }

    public void UseActionButton() 
    {
        playerStats.consumeItem(((ConsummableData) itemCurrentlySelected).consumableEffects);
        Inventory.instance.RemoveItem(itemCurrentlySelected);
        closeActionPanel();
    }

    public void EquipActionButton() {
        equipement.EquipAction();
    }
    

    public void DropActionButton() 
    {
        GameObject instantiatedItem = Instantiate(itemCurrentlySelected.prefab, itemsParent);
        instantiatedItem.transform.position = dropPoint.position;
        Inventory.instance.RemoveItem(itemCurrentlySelected);
        closeActionPanel();
    }

    public void DestroyActionButton() 
    {
        Inventory.instance.RemoveItem(itemCurrentlySelected);
        closeActionPanel();
    
    }
}
