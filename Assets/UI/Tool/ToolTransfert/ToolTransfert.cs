using UnityEngine;
using UnityEngine.UI;

public class ToolTransfert : MonoBehaviour
{
    /*  
        !!! Dans la hierarchie Unity  Tooltip doit être le dernier child de Canvevas pour 
        safficher au desssus des autres

        dev : affichage du texte à revoir
    */
    [SerializeField]
    private Sprite emptySlot;

    [SerializeField]
    private LayoutElement layoutElement;

    [SerializeField]
    private RectTransform rectTransform;

    [SerializeField]
    private Image imageComponent;
    
    private ItemInInventory itemToTransfert;
    
    private AbstractSlot originalSlot;

    void Update() {

        // Desactivate toolTransfert

        if (Input.GetMouseButtonUp(0)) 
        {
            if(ToolUISystem.instance.currentPointedSlot != null)
            {
                ToolUISystem.instance.currentPointedSlot.FromToolTransfert(itemToTransfert, originalSlot);
            }
            imageComponent.sprite = emptySlot;

            ToolUISystem.instance.HideTranfert();
        }

        // update position

        Vector2 mousePosition = Input.mousePosition;

        float pivotX = mousePosition.x / Screen.width;
        float pivotY = mousePosition.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);

        transform.position = mousePosition;
    }

    public void InitItemTransfert(ItemInInventory newItemToTranfert, AbstractSlot newOriginalSlot) 
    {
        itemToTransfert = newItemToTranfert;
        originalSlot = newOriginalSlot;
        imageComponent.sprite = itemToTransfert.itemData.icon;
    } 
}