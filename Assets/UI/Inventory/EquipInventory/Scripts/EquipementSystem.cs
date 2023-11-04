using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using Unity.VisualScripting;
using System.Collections.Generic;

public class EquipementSystem : MonoBehaviour
{
    // InventoryManager
    [Header("COMPONENTS REFERENCES")]

    [SerializeField]
    private ItemActionSystem itemActionSystem;

    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private AudioSource playerAudioSource;

    [Header("CONFIGURATION")]

    [SerializeField]
    private AudioClip gearupSound;

    [Header("EQUIPEMENT LIBRARY")]
    public List<EquipedItem> equipedItems;

    [SerializeField]
    private Transform weaponAndToolVisualParent;

    public bool EquipItem(ItemData itemDataToEquip)
    {   
        if (itemDataToEquip.GetType().IsSubclassOf(typeof(EquipementData)))
        {
            playerAudioSource.PlayOneShot(gearupSound);
            
            // Call method 
            if (itemDataToEquip.GetType() == typeof(WeaponData))
                return EquipWeapon((WeaponData) itemDataToEquip);
            
            if (itemDataToEquip.GetType() == typeof(ToolData))
                return EquipTool((ToolData) itemDataToEquip);
        
            if (itemDataToEquip.GetType() == typeof(ArmorData))
                return EquipArmor((ArmorData) itemDataToEquip);
        } 

        return false;
    }

    public bool EquipWeapon(WeaponData weaponToEquip)
    {
        EquipedItem equipedWeapon = GetEquipedItem(EquipableItemType.Weapon);
       
        DesequipWeapon();

        // Instantiate Prefab
        GameObject instantiatedEquipement = Instantiate(weaponToEquip.visual);
        instantiatedEquipement.transform.position = weaponToEquip.positionsOffset;
        instantiatedEquipement.transform.rotation = Quaternion.Euler(weaponToEquip.rotationsOffset);

        instantiatedEquipement.transform.SetParent(weaponAndToolVisualParent, false);
        // Update locale list
        equipedWeapon.gameobjectVisual = instantiatedEquipement;
        equipedWeapon.data = weaponToEquip;
        equipedWeapon.equiped = true;
        
        return true;
    }

    public bool EquipTool(ToolData toolToEquip)
    {
        EquipedItem equipedTool = GetEquipedItem(EquipableItemType.Tool);
        DesequipTool();
        DesequipWeapon();

        // Instantiate Prefab
        GameObject instantiatedEquipement = Instantiate(toolToEquip.visual);
        instantiatedEquipement.transform.position = toolToEquip.positionsOffset;
        instantiatedEquipement.transform.rotation = Quaternion.Euler(toolToEquip.rotationsOffset);
        
        instantiatedEquipement.transform.SetParent(weaponAndToolVisualParent, false);
        // Update locale list
        equipedTool.gameobjectVisual = instantiatedEquipement;
        equipedTool.data = toolToEquip;
        equipedTool.equiped = true;

        return true;
    }

    public bool EquipArmor(ArmorData armorToEquip)
    {
        EquipedItem equipedArmor = GetEquipedItem(armorToEquip.armorType);
        
        DesequipArmor(armorToEquip.armorType);
    
        foreach(GameObject visual in equipedArmor.defaultVisuals)
            visual.SetActive(false);
        
        SkinnedMeshRenderer meshRenderer =  equipedArmor.gameobjectVisual.GetComponent<SkinnedMeshRenderer>();
        meshRenderer.sharedMesh = armorToEquip.visual;
        meshRenderer.material = armorToEquip.material;
        playerStats.currentArmorPoint += armorToEquip.armorPoints;
        
        equipedArmor.equiped = true;
        return true;
    }

    public bool DesequipWeapon()
    {
        EquipedItem equipedWeapon = GetEquipedItem(EquipableItemType.Weapon);
        if (!equipedWeapon.equiped)
            return false;

        Destroy(equipedWeapon.gameobjectVisual);
        equipedWeapon.gameobjectVisual = null;
        equipedWeapon.data = null;
        equipedWeapon.equiped = false; 
           

        return true;
    }

    public bool DesequipTool()
    {
        EquipedItem equipedTool = GetEquipedItem(EquipableItemType.Tool);
        if (!equipedTool.equiped)
            return false;
             
        Destroy(equipedTool.gameobjectVisual);
        equipedTool.gameobjectVisual = null;
        equipedTool.data = null;
        equipedTool.equiped = false; 
           
        return true;
    } 

    public bool DesequipArmor(ArmorType armorToDesequip)
    {
        EquipedItem equipedArmor = GetEquipedItem(armorToDesequip);
        if (!equipedArmor.equiped)
            return false;
        
        SkinnedMeshRenderer meshRenderer =  equipedArmor.gameobjectVisual.GetComponent<SkinnedMeshRenderer>();
        meshRenderer.sharedMesh = null;
        meshRenderer.material = null;
        playerStats.currentArmorPoint -= ((ArmorData) equipedArmor.data).armorPoints;

        foreach(GameObject visual in equipedArmor.defaultVisuals)
            visual.SetActive(true);

        equipedArmor.equiped = false; 
        return true;     
    }

    public EquipedItem GetEquipedItem(EquipableItemType equipableItemType)
    {
        return equipedItems.Where(e => e.equipementType == equipableItemType).FirstOrDefault();
    }

    public EquipedItem GetEquipedItem(ArmorType equipementSlotType)
    {
        return equipedItems.Where(e => ((EquipableItemType)(int) e.equipementType).ToString() == ((EquipableItemType) (int) equipementSlotType).ToString()).First();
    }

    public void LoadEquipements(ItemData[] savedEquipements)//  ToDo
    {
        MainInventory.instance.ClearContent();

        foreach(ArmorType equipementType in Enum.GetValues(typeof(ArmorType)))
        {
            
        }

        foreach(EquipementData itemToEquip in savedEquipements)
        {
            if (itemToEquip)
            {
                
            }
        }
    }
        



}

[Serializable]
public enum EquipableItemType
{
    Head,
    Chest,
    Hands,
    Legs,
    Feets,
    Weapon,
    Tool
}

[Serializable]
public class EquipedItem
{
    public bool equiped;
    public EquipableItemType equipementType;
    public EquipementData data;
    public GameObject gameobjectVisual;
    public GameObject[] defaultVisuals;
}
