using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Equipement : MonoBehaviour
{
    // InventoryManager
    [Header("COMPONENTS REFERENCES")]

    [SerializeField]
    private ItemActionSystem itemActionSystem;

    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private AudioSource playerAudioSource;

    [Header("EQUIPEMENT LIBRARY")]
    [SerializeField]
    private EquipementLibrary equipementLibrary;

    [Header("CONFIGURATION")]

    [SerializeField]
    private AudioClip gearupSound;

    // Données des objets équipés
    // ToDo : faire une map avec ca

    public EquipementTypeToCurrentEquipement[] equipementTypeToCurrentEquipements;

    [HideInInspector]
    public  WeaponData equipedWeaponItem;

    // Equip Action
    public void EquipAction(EquipementData equipement = null) 
    {
        EquipementData itemToEquip = equipement ? equipement : (EquipementData) itemActionSystem.itemCurrentlySelected;
        VisualLibrary itemToEquipVisualLibrary = equipementLibrary.visualLibrary.Where(elem => elem.visualPrefab.GetComponent<Item>().itemData == itemToEquip).First();

        if (itemToEquipVisualLibrary == null)
        {
            Debug.Log("Equipement" + itemToEquip.itemName + " non existant dans la librairie des équipements");
            return;
        }

        playerAudioSource.PlayOneShot(gearupSound);

        // Remove item from inventory
        Inventory.instance.RemoveItem(itemToEquip);
        Inventory.instance.RefreshContent();

        // Remove actualWeapon
        if (GetCurrentEquipementVusual(itemToEquip.equipementType))
            DisablePreviousEquipedEquipement(itemToEquip);
        
        // Remove defaut Player visuals
        equipementLibrary.EnableOrDisableDefautElement(itemToEquipVisualLibrary, true);
        
        // instantiate equipement visual 
        GameObject instantiatedEquipement = Instantiate(itemToEquipVisualLibrary.visualPrefab);
        instantiatedEquipement.transform.SetParent(itemToEquipVisualLibrary.parentVisual.transform, false);
        SetCurrentEquipementVisual(itemToEquip.equipementType, instantiatedEquipement);

        // Update inventory icon
        equipementLibrary.GetEquipementSlotImage(itemToEquip.equipementType).sprite = itemToEquip.icon;

        // update player stats
        if (itemToEquip.GetType() == typeof(ArmorData))
        {   
            playerStats.currentArmorPoint += ((ArmorData)itemToEquip).armorPoints;
        }

        itemActionSystem.closeActionPanel();
    


        void DisablePreviousEquipedEquipement(EquipementData itemToDisable) 
        {
            if (itemToDisable == null) 
                return;

            VisualLibrary visualLibrary = equipementLibrary.visualLibrary.Where(elem => elem.visualPrefab.GetComponent<Item>().itemData == itemToDisable).First();

            if (visualLibrary == null)
            {
                Debug.Log("Equipement" + itemToDisable.itemName + " non existant dans la librairie des équipements");
                return;
            }

            equipementLibrary.EnableOrDisableDefautElement(visualLibrary, true);

            if (itemToDisable.GetType() == typeof(ArmorData))
            {
                playerStats.currentArmorPoint -= ((ArmorData) itemToDisable).armorPoints;
            }

            // ToDo : destroy currentEquipement

            SetCurrentEquipementVisual(itemToDisable.equipementType, null);

            Inventory.instance.AddItem(itemToDisable);
            Inventory.instance.RefreshContent();
        }

    }

    // Desequip Action 
    public void DesquipEquipement(EquipementType equipementTypeToDesequip)
    {
        // Inventaire

        if (Inventory.instance.IsFull()) {
            Debug.Log("Inventaire plein");
            return;
        }

        playerAudioSource.PlayOneShot(gearupSound);

        GameObject currentEquipementVisual = GetCurrentEquipementVusual(equipementTypeToDesequip);
        EquipementData currentEquipementData = (EquipementData) currentEquipementVisual.GetComponent<Item>().itemData;

        // Clear Equipement slot
        Image slotImage = equipementLibrary.GetEquipementSlotImage(equipementTypeToDesequip);
        slotImage.sprite = Inventory.instance.emptyVisualSlot;
        
        // Clear equipement stats
        if (currentEquipementData.GetType() == typeof(ArmorData))
        {
            playerStats.currentArmorPoint -= ((ArmorData) currentEquipementData).armorPoints;
        }

        // Remove equipement visual
        Destroy(currentEquipementVisual);
        SetCurrentEquipementVisual(equipementTypeToDesequip, null);

        // Reenable Player default visual
        VisualLibrary e = equipementLibrary.visualLibrary.Where(elem => elem.visualPrefab.GetComponent<Item>().itemData == currentEquipementData).FirstOrDefault();
        equipementLibrary.EnableOrDisableDefautElement(e, true);

        // Inventory Update
        Inventory.instance.AddItem(currentEquipementData);
        Inventory.instance.RefreshContent();
    }

    public void UpdateEquipementsDesequipButtons() 
    {
        foreach(EquipementType equipementType in Enum.GetValues(typeof(EquipementType)))
        {
            Button slotButton = equipementLibrary.GetEquipementSlotButtom(equipementType);                    
            slotButton.onClick.RemoveAllListeners();
            slotButton.onClick.AddListener(delegate { DesquipEquipement(equipementType); });
            slotButton.gameObject.SetActive(GetCurrentEquipementVusual(equipementType) != null);
        }
    }

    public void LoadEquipements(ItemData[] savedEquipements)
    {
        Inventory.instance.ClearContent();

        foreach(EquipementType equipementType in Enum.GetValues(typeof(EquipementType))) // Good Practice : foreach sur enum
        {
            DesquipEquipement(equipementType);
        }

        foreach(EquipementData itemToEquip in savedEquipements)
        {
            if (itemToEquip)
            {
                EquipAction(itemToEquip);
            }
        }
    }

    // Accesser Methods to dictionnary

    public GameObject GetCurrentEquipementVusual(EquipementType equipementType)
    {
        return equipementTypeToCurrentEquipements.Where(e => e.type == equipementType).First().visualEquiped;
    }

    public void SetCurrentEquipementVisual(EquipementType equipementType, GameObject newEquipement)
    {
        equipementTypeToCurrentEquipements.Where(e => e.type == equipementType).First().visualEquiped = newEquipement;
    }
}

public enum EquipementType
{
    Head,
    Chest,
    Hands,
    Legs,
    Feets,
    Weapon
}

[Serializable]
public class EquipementTypeToCurrentEquipement
{
    public EquipementType type;
    public GameObject visualEquiped;
}
