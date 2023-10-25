using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Recip : MonoBehaviour
{
    // prefab recip
    [SerializeField]
    private Image craftableItemImage;
    [SerializeField]
    private GameObject elementReqiredPrefab;
    [SerializeField]
    private Transform elementsRequiredParent;
    [SerializeField]
    private Button craftButton;
    [SerializeField]
    private Sprite canBuildIcon;
    [SerializeField]
    private Sprite cantBuildIcon;
    [SerializeField]
    private Color missingColor;
    [SerializeField]
    private Color availableColor;

    private RecipData currentRecip;

    public void Configure(RecipData recip) 
    {
        currentRecip = recip;
        // initialise le slot de l'item à crafter
        craftableItemImage.sprite = recip.craftableItem.icon;
        craftableItemImage.transform.parent.GetComponent<Slot>().item = recip.craftableItem; // good practice

        bool canCraft = true;

        foreach(ItemInInventory itemInRecip in recip.requiredItems)
        {
            // récupère les composants du slot de l'item courant 
            GameObject requiredItem = Instantiate(elementReqiredPrefab, elementsRequiredParent);
            ItemData requiredItemData = itemInRecip.itemData;

            Slot requiredItemSlot = requiredItem.GetComponent<Slot>();
            Image requiredItemImage = requiredItemSlot.itemVisual;
            Text requiredItemCountText = requiredItemSlot.countText;


            // check si l'item existe dans l'inventaire
            ItemInInventory[] itemsInInventory = Inventory.instance.getContent().Where(i => i.itemData == requiredItemData).ToArray();
            int totalRequiredItemQuantityInInventory = 0;

            foreach(ItemInInventory itemInInventory in itemsInInventory)
            {
                totalRequiredItemQuantityInInventory += itemInInventory.count;
            }

            if (totalRequiredItemQuantityInInventory >= itemInRecip.count)
            {
                requiredItemImage.color = availableColor;
            } 
            else 
            {
                canCraft = false;
                requiredItemImage.color = missingColor;
            }

            // initialise le slot de l'item courant
            requiredItemSlot.item = requiredItemData;
            requiredItemImage.sprite = requiredItemData.icon;

            if (requiredItemData.stackable)
            {
                requiredItemCountText.enabled = true;
                requiredItemCountText.text = itemInRecip.count.ToString();
            }
        }


        // initialise le bouton de la recette
        craftButton.image.sprite = canCraft ? canBuildIcon : cantBuildIcon;
        craftButton.enabled = canCraft;

        ResizeElementRequiredParent();
    }

    private void ResizeElementRequiredParent() 
    {
        Canvas.ForceUpdateCanvases();
        elementsRequiredParent.GetComponent<ContentSizeFitter>().enabled = false;
        elementsRequiredParent.GetComponent<ContentSizeFitter>().enabled = true;
    }

    public void CraftItem()
    {
        // le check se fait à l'activation/désactivation du bouton craft
        
        foreach(ItemInInventory itemInInventory in currentRecip.requiredItems)
        {   
            for (int i = 0; i < itemInInventory.count; i++)
            {
                Inventory.instance.RemoveItem(itemInInventory.itemData);
            }   
        }
        Inventory.instance.AddItem(currentRecip.craftableItem);
    }


}
