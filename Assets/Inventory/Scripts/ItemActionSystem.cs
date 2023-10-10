using UnityEngine;

public class ItemActionSystem : MonoBehaviour
{

    [Header("OTHER SCRIPTS REFERENCES")]

    [SerializeField]
    private Equipement equipement;

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

        switch(item.itemType)
        {
            case ItemType.Ressource:
                useItemButton.SetActive(false);
                equipItemButton.SetActive(false);
                break;
            case ItemType.Equipement:
                useItemButton.SetActive(false);
                equipItemButton.SetActive(true);
                break;
            case ItemType.Consumable:
                useItemButton.SetActive(true);
                equipItemButton.SetActive(false);
                break;
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
        print("j'uilise cet item");
        closeActionPanel();
    }

    public void EquipActionButton() {
        equipement.EquipAction();
    }
    

    public void DropActionButton() 
    {
        GameObject instantiatedItem = Instantiate(itemCurrentlySelected.prefab);
        instantiatedItem.transform.position = dropPoint.position;
        Inventory.instance.RemoveItem(itemCurrentlySelected);
        Inventory.instance.RefreshContent();
        closeActionPanel();
    }

    public void DestroyActionButton() 
    {
        Inventory.instance.RemoveItem(itemCurrentlySelected);
        Inventory.instance.RefreshContent();
        closeActionPanel();
    
    }
}
