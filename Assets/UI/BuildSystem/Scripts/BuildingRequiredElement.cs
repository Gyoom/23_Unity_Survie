using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuildingRequiredElement : MonoBehaviour
{
    [SerializeField]
    private Image slotImage;
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private Text itemCost;
    [SerializeField]
    private Color greenColor;
    [SerializeField]
    private Color redColor;
    [HideInInspector]
    private ItemInInventory ressource;
    public bool hasRessouces = false;

    public void Setup (ItemInInventory ressourceRequired) 
    {
        ressource = ressourceRequired;
        itemImage.sprite = ressourceRequired.itemData.icon; 
        itemCost.text = ressourceRequired.count.ToString();
        CheckHasRessourcesToBuild();
    }

    public void CheckHasRessourcesToBuild()
    {
        Debug.Log(MainInventory.instance);
        
        ItemInInventory[] ressourcesrequiredInInventory = MainInventory.instance.getContent().Where(elem => elem.itemData == ressource.itemData).ToArray();
        int totalRequiredItemQuantityInInventory = 0;
        foreach(ItemInInventory item in ressourcesrequiredInInventory)
        {
            totalRequiredItemQuantityInInventory += item.count;
        }

        if (ressource.count > totalRequiredItemQuantityInInventory)
        {
            hasRessouces = false;
            slotImage.color = redColor;
        }
        else
        {
            hasRessouces = true;
           slotImage.color = greenColor;
        }
    }
}
