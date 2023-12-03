using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    // Game Manager
    [SerializeField]
    private RecipData[] availableRecipes;
    // une liste plutôt qu'une array pour une liste dynamique dans le jeu
    [SerializeField]
     private GameObject recipUIPrefab;
    [SerializeField]
     private Transform recipesParent;
    [SerializeField]
     private KeyCode openPanelCraftInput;

    [SerializeField]
    protected UIManager uIManager;

    [SerializeField]
    private GameObject craftingPanel;

    [HideInInspector]
    public bool craftPanelIsOpen;

    public bool craftCheat = false;

    void Start()
    {   
        craftingPanel.SetActive(false);
        craftPanelIsOpen = false;
    }

    void Update() 
    {
        if (Input.GetKeyDown(openPanelCraftInput))
        {
            if (!craftPanelIsOpen)
            {
                openPanel();
            }
            else
            {
                ClosePanel();
            }
        }  
    }

    private void openPanel() 
    {
        craftingPanel.SetActive(true);
        craftPanelIsOpen = true;
        UpdateDisplayedRecipes();
        uIManager.PanelStatusUpdate();
    }

    public void ClosePanel() 
    {
        craftingPanel.SetActive(false);
        craftPanelIsOpen = false;
        uIManager.PanelStatusUpdate();
    }

    public void UpdateDisplayedRecipes()
    {
        foreach(Transform child in recipesParent)
        {
            Destroy(child.gameObject);
        }

        foreach(RecipData recipData in availableRecipes)
        {
            GameObject currentRecip = Instantiate(recipUIPrefab, recipesParent);
            currentRecip.GetComponent<Recip>().Configure(recipData, craftCheat);

        }
    }
}
