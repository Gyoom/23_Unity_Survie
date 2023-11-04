using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipementSlot : AbstractSlot
{
    public ArmorType armorType;

    [SerializeField]
    private Image slotBackground;

    [SerializeField]
    private Sprite slotEmptyImageBackground;
    
    [SerializeField]
    private Sprite slotFillImageBackground;

    public override void Configure(ItemInInventory newItem, int newIndex) 
    {   
        index = newIndex; 
        if (newItem.itemData == null)
        {
            isActualyUsed = false;
        }
        else 
        {
            data = newItem.itemData;
            visual.sprite = newItem.itemData.icon;
            slotBackground.sprite = slotFillImageBackground;

            if (data.stackable)
            {
                countText.enabled = true;
                countText.text = newItem.count.ToString();
            }
            isActualyUsed = true;
        }
    }

    public override void Reset()
    {
        data = null;
        visual.sprite = emptySprite;
        slotBackground.sprite = slotEmptyImageBackground;

        isActualyUsed = false;
    }
}
