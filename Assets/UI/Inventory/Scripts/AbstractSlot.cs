using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class AbstractSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector]
    public ItemData data;
    public int index;
    public Image visual;

    [SerializeField]
    protected Sprite emptySprite;

    [SerializeField]
    protected AbstractInventory inventoryScriptParent;

    public bool isActualyUsed = false;

    protected bool pointed = false;

    [SerializeField]
    protected ItemActionSystem itemActionSystem;

    public Text countText;

    protected void Update() 
    {   
        if (pointed)
        {
            if (Input.GetMouseButtonDown(0) && isActualyUsed)
            {
                itemActionSystem.CloseActionPanel();
                ToolUISystem.instance.ShowTranfert(inventoryScriptParent.getContent()[index], this);
            }
            if (Input.GetMouseButtonDown(1))
		        itemActionSystem.OpenActionPanel(data, transform.position, inventoryScriptParent);
        }
    }

    public virtual void Configure(ItemInInventory newItem, int newIndex) 
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

            if (data.stackable)
            {
                countText.enabled = true;
                countText.text = newItem.count.ToString();
            }
            isActualyUsed = true;
        }
    }

    public virtual void Reset()
    {
       
        countText.enabled = false;
        data = null;
        visual.sprite = emptySprite;

        isActualyUsed = false;
    }

    public virtual ItemInInventory RemoveItem()
    {
        return inventoryScriptParent.RemoveItem(index);
    }

    public void  FromToolTransfert(ItemInInventory newItem, AbstractSlot previousSlot) 
    {
        if (inventoryScriptParent.AddItem(newItem, index))
            previousSlot.RemoveItem();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {   
        pointed = true;
        ToolUISystem.instance.UpdateCurrentPointedSlot(this);
        if (data != null && !ToolUISystem.instance.isToolTransfertActive) {
            ToolUISystem.instance.ShowTip(data.description, data.itemName);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointed = false;
        ToolUISystem.instance.HideTip();
        ToolUISystem.instance.UpdateCurrentPointedSlot();
    }
}
