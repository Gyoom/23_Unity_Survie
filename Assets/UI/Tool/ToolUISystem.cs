using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolUISystem : MonoBehaviour
{

    public static ToolUISystem instance;

    [SerializeField]
    private Tooltip tooltip;

    [SerializeField]
    private ToolTransfert toolTransfert;

    [HideInInspector]
    public bool isToolTransfertActive = false;

    [HideInInspector]
    public AbstractSlot currentPointedSlot;

    private void Awake()
    {
        instance = this;
    }

    public void ShowTip(string content, string header = "")
    {
        tooltip.SetText(content, header);
        tooltip.gameObject.SetActive(true);
    }

    public void ShowTranfert(ItemInInventory itemToTransfert, AbstractSlot originalSlot)
    {
        toolTransfert.gameObject.SetActive(true);
        isToolTransfertActive = true;
        toolTransfert.InitItemTransfert(itemToTransfert, originalSlot);;
    }

    public void HideTip()
    {
        tooltip.gameObject.SetActive(false);
    }

    public void HideTranfert()
    {
        toolTransfert.gameObject.SetActive(false);
        isToolTransfertActive = false;
    }

    public void UpdateCurrentPointedSlot(AbstractSlot newSlot = null)
    {
        currentPointedSlot =  newSlot;
    }

}
