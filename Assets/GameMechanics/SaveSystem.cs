using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    [Header("COMPONENTS REFERNCES")]
    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    private Equipement playerEquipement;

    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private BuildSystem buildSystem;

    [SerializeField]
    private MainMenu breakMenu;

    

    void Start()
    {
        if(MainMenu.loadSavedData)
        {
            LoadData();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveData();
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            LoadData();
        }
    }

    public void SaveData()
    {
        SavedData savedData = new SavedData
        {
            playerPosition = playerTransform.position,
            playerRotation = playerTransform.rotation,

            inventoryContent = Inventory.instance.getContent(),
            equipedHeadItem =   playerEquipement.GetCurrentEquipementVusual(EquipementType.Head).GetComponent<Item>().itemData,
            equipedChestItem =  playerEquipement.GetCurrentEquipementVusual(EquipementType.Chest).GetComponent<Item>().itemData,
            equipedHandsItem =  playerEquipement.GetCurrentEquipementVusual(EquipementType.Hands).GetComponent<Item>().itemData,
            equipedLegsItem =   playerEquipement.GetCurrentEquipementVusual(EquipementType.Legs).GetComponent<Item>().itemData,
            equipedFeetsItem =  playerEquipement.GetCurrentEquipementVusual(EquipementType.Feets).GetComponent<Item>().itemData,
            equipedWeaponItem = playerEquipement.GetCurrentEquipementVusual(EquipementType.Weapon).GetComponent<Item>().itemData,

            currentHealth = playerStats.currentHealth,
            currentHunger = playerStats.currentHunger,
            currentThirst = playerStats.currentThirst,

            placedStructures = buildSystem.placedStructures.ToArray(),


        };

        string jsonData = JsonUtility.ToJson(savedData);
        string filePath = Application.persistentDataPath + "/SavedData.json";
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, jsonData);

        breakMenu.loadButton.interactable = true;
        breakMenu.clearSavedDataButton.interactable = true;

        Debug.Log("Sauvegarde Terminée");
    }

    public void LoadData()
    {
        string filePath = Application.persistentDataPath + "/SavedData.json";
        string jsonData = System.IO.File.ReadAllText(filePath);

        SavedData savedData = JsonUtility.FromJson<SavedData>(jsonData);

        // Chargement des données

        playerTransform.position = savedData.playerPosition;
        playerTransform.rotation = savedData.playerRotation;

        cameraTransform = savedData.camera;

        playerEquipement.LoadEquipements(new ItemData[] 
            {
                savedData.equipedHeadItem, 
                savedData.equipedWeaponItem, 
                savedData.equipedChestItem,
                savedData.equipedHandsItem,
                savedData.equipedLegsItem,
                savedData.equipedHandsItem
            }
        );
        Inventory.instance.SetContent(savedData.inventoryContent);

        playerStats.currentHealth = savedData.currentHealth;
        playerStats.currentHealth = savedData.currentHealth;
        playerStats.currentHealth = savedData.currentHealth;
        playerStats.updateHealthBarFill();

        buildSystem.LoadStructures(savedData.placedStructures);

        Debug.Log("Chargement terminé");
    }   
}

public class SavedData // ToDo : sauvegarder les haverstables et les items + camera
{
    public Vector3 playerPosition;
    public Quaternion playerRotation;

    public Transform camera;
    public List<ItemInInventory> inventoryContent;

    public ItemData equipedWeaponItem;
    public ItemData equipedHeadItem;
    public ItemData equipedChestItem;
    public ItemData equipedHandsItem;
    public ItemData equipedLegsItem;
    public ItemData equipedFeetsItem;

    public float currentHealth;
    public float currentHunger;
    public float currentThirst;

    public PlacedStructure[] placedStructures;
}

