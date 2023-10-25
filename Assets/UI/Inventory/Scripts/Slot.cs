using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector]
    public ItemData item;
    public Image itemVisual;
    public Text countText;

    [Header("OPTIONNAL : ")]
    [SerializeField]
    private ItemActionSystem itemActionSystem;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null) {
            TooltipSystem.instance.Show(item.description, item.itemName);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.instance.Hide();
    }

    public void ClickOnSlot()
    {
        if (itemActionSystem)
            itemActionSystem.OpenActionPanel(item, transform.position);
    }
}
