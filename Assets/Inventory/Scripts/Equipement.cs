using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Equipement : MonoBehaviour
{

    [Header("OTHER SCRIPTS REFERENCES")]

    [SerializeField]
    private ItemActionSystem itemActionSystem;

    [Header("EQUIPEMENT LIBRARY")]
    [SerializeField]
    private EquipementLibrary equipementLibrary;

    [Header("EQUIPEMENT SLOTS IMAGES")]

    [SerializeField]
    private Image headSlotImage;

    [SerializeField]
    private Image chestSlotImage;
    
    [SerializeField]
    private Image handsSlotImage;
    
    [SerializeField]
    private Image legsSlotImage;
    
    [SerializeField]
    private Image feetsSlotImage;

    [Header("EQUIPEMENT SLOTS BUTTONS")]

    [SerializeField]
    private Button headSlotButton;

    [SerializeField]
    private Button chestSlotButton;

    [SerializeField]
    private Button handsSlotButton;

    [SerializeField]
    private Button legsSlotButton;

    [SerializeField]
    private Button feetsSlotButton;

    // Données des objets équipés

    private ItemData equipedHeadItem;
    private ItemData equipedChestItem;
    private ItemData equipedHandsItem;
    private ItemData equipedLegsItem;
    private ItemData equipedFeetsItem;

    private void DisablePreviousEquipedEquipement(ItemData itemToDisable) 
    {
        if (itemToDisable == null) return;

        EquipementLibraryItem e = equipementLibrary.content.Where(elem => elem.itemData == itemToDisable).First();

        if (e != null)
        {
            for (int i = 0; i <e.elementsToDisable.Length; i++)
            {
                e.elementsToDisable[i].SetActive(true);
            }
            e.itemPrefab.SetActive(false);
        }

        Inventory.instance.AddItem(itemToDisable);

    }

    public void DesquipEquipement(EquipementType equipementType)
    {
        // Inventaire

        if (Inventory.instance.IsFull()) {
            Debug.Log("Inventaire plein");
            return;
        }

        ItemData currentItem = null;

        switch (equipementType)
            {
                case EquipementType.Head:
                    currentItem = equipedHeadItem;
                    equipedHeadItem = null;
                    headSlotImage.sprite = Inventory.instance.emptyVisualSlot;
                    break;

                case EquipementType.Chest:
                    currentItem = equipedChestItem;
                    equipedChestItem = null;
                    chestSlotImage.sprite = Inventory.instance.emptyVisualSlot;
                    break;

                case EquipementType.Hands:
                    currentItem = equipedHandsItem;
                    equipedHandsItem = null;
                    handsSlotImage.sprite = Inventory.instance.emptyVisualSlot;
                    break;

                case EquipementType.Legs:
                    currentItem = equipedLegsItem;
                    equipedLegsItem = null;
                    legsSlotImage.sprite = Inventory.instance.emptyVisualSlot;
                    break;

                case EquipementType.Feets:
                    currentItem = equipedFeetsItem;
                    equipedFeetsItem = null;
                    feetsSlotImage.sprite = Inventory.instance.emptyVisualSlot;
                    break;
            }

            Inventory.instance.AddItem(currentItem);

            // Player (objet 3D)

            EquipementLibraryItem e = equipementLibrary.content.Where(elem => elem.itemData == currentItem).First();

            if (e != null)
            {
                for (int i = 0; i <e.elementsToDisable.Length; i++)
                {
                    e.elementsToDisable[i].SetActive(true);
                }
                e.itemPrefab.SetActive(false);
            }

            Inventory.instance.RefreshContent();
    }

    public void UpdateEquipementsDesequipButtons() 
    {
        headSlotButton.onClick.RemoveAllListeners();
        headSlotButton.onClick.AddListener(delegate { DesquipEquipement(EquipementType.Head); });
        headSlotButton.gameObject.SetActive(equipedHeadItem);

        chestSlotButton.onClick.RemoveAllListeners();
        chestSlotButton.onClick.AddListener(delegate { DesquipEquipement(EquipementType.Chest); });
        chestSlotButton.gameObject.SetActive(equipedChestItem);

        handsSlotButton.onClick.RemoveAllListeners();
        handsSlotButton.onClick.AddListener(delegate { DesquipEquipement(EquipementType.Hands); });
        handsSlotButton.gameObject.SetActive(equipedHandsItem);

        legsSlotButton.onClick.RemoveAllListeners();
        legsSlotButton.onClick.AddListener(delegate { DesquipEquipement(EquipementType.Legs); });
        legsSlotButton.gameObject.SetActive(equipedLegsItem);

        feetsSlotButton.onClick.RemoveAllListeners();
        feetsSlotButton.onClick.AddListener(delegate { DesquipEquipement(EquipementType.Feets); });
        feetsSlotButton.gameObject.SetActive(equipedFeetsItem);
    }

    public void EquipAction() 
    {
        // Check la liste des équipement pouvant être ajouté (liste créer manuellement dans gameManager)
        // EquipementLibraryItem fournis l'objet à ajouter au personnage & les objets à retirer pour évter desuperposer les objets
        EquipementLibraryItem e = equipementLibrary.content.Where(elem => elem.itemData == itemActionSystem.itemCurrentlySelected).First();

        if (e != null)
        {


            switch (itemActionSystem.itemCurrentlySelected.equipementType)
            {
                case EquipementType.Head:
                    DisablePreviousEquipedEquipement(equipedHeadItem);
                    headSlotImage.sprite = itemActionSystem.itemCurrentlySelected.visual;
                    equipedHeadItem = itemActionSystem.itemCurrentlySelected;
                    break;

                case EquipementType.Chest:
                    DisablePreviousEquipedEquipement(equipedChestItem);
                    chestSlotImage.sprite = itemActionSystem.itemCurrentlySelected.visual;
                    equipedChestItem= itemActionSystem.itemCurrentlySelected;
                    break;

                case EquipementType.Hands:
                    DisablePreviousEquipedEquipement(equipedHandsItem);
                    handsSlotImage.sprite = itemActionSystem.itemCurrentlySelected.visual;
                    equipedHandsItem= itemActionSystem.itemCurrentlySelected;
                    break;

                case EquipementType.Legs:
                    DisablePreviousEquipedEquipement(equipedLegsItem);
                    legsSlotImage.sprite = itemActionSystem.itemCurrentlySelected.visual;
                    equipedLegsItem = itemActionSystem.itemCurrentlySelected;
                    break;

                case EquipementType.Feets:
                    DisablePreviousEquipedEquipement(equipedFeetsItem);
                    feetsSlotImage.sprite = itemActionSystem.itemCurrentlySelected.visual;
                    equipedFeetsItem = itemActionSystem.itemCurrentlySelected;
                    break;
            }

            for (int i = 0; i <e.elementsToDisable.Length; i++)
            {
                e.elementsToDisable[i].SetActive(false);
            }
            e.itemPrefab.SetActive(true);

            Inventory.instance.RemoveItem(itemActionSystem.itemCurrentlySelected);
        } 
        else 
        {
            Debug.Log("Equipement" + itemActionSystem.itemCurrentlySelected.itemName + " non existant dans la librairie des équipements");
        }
        itemActionSystem.closeActionPanel();
    }

}
