using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private EquipementSystem playerEquipement;

    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private BuildSystem buildSystem;

    [SerializeField]
    private BreakMenu breakMenu;

    [Header("PARENTS TRANSFORM REFERNCES")]

    [SerializeField]
    private Transform parentSceneStructures;

    [SerializeField]
    private Transform parentSceneItems;

    [SerializeField]
    private Transform parentSceneHarvestables;

    [SerializeField]
    private Transform parentSceneEnemies;

    [SerializeField]
    private GameObject BearPrefab;

    [HideInInspector]
    public List<ObjectSaved> sceneStrucures;

    [HideInInspector]
    public List<ObjectSaved> sceneItems;

    [HideInInspector]
    public List<ObjectSaved> sceneHarvestables;

    [HideInInspector]
    public List<ObjectSaved> sceneEnemies;


    

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

            mainInventoryContent       = MainInventory.instance.getContent(),
            actionInventoryContent     = ActionInventory.instance .getContent(),
            equipementInventoryContent = EquipementInventory.instance.getContent(),

            currentHealth = playerStats.currentHealth,
            currentHunger = playerStats.currentHunger,
            currentThirst = playerStats.currentThirst,

            structures   = SaveSceneObjects(parentSceneStructures).ToArray(),
            items        = SaveSceneObjects(parentSceneItems).ToArray(),
            harvestables = SaveSceneObjects(parentSceneHarvestables).ToArray(),
            enemies      = SaveSceneObjects(parentSceneEnemies).ToArray(),
        };

        string jsonData = JsonUtility.ToJson(savedData);
        string filePath = Application.persistentDataPath + "/SavedData.json";
        
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, jsonData);

        breakMenu.loadButton.interactable = true;
        breakMenu.clearSavedDataButton.interactable = true;

        Debug.Log("Sauvegarde Terminée");
    }

    private List<ObjectSaved> SaveSceneObjects(Transform parentSceneObjects)
    {

        List<ObjectSaved> sceneObjects = new List<ObjectSaved>();

        for (int i = 0; i < parentSceneObjects.childCount; i++)
        {   if (parentSceneObjects.CompareTag("ParentSceneHarvestables"))
            {
                sceneObjects.Add( new ObjectSaved 
                {
                    prefab =   parentSceneObjects.GetChild(i).GetComponent<Harvestable>().data.prefab,
                    position = parentSceneObjects.GetChild(i).position,
                    rotation = parentSceneObjects.GetChild(i).rotation.eulerAngles,
                    scale =    parentSceneObjects.GetChild(i).localScale
                });
            }
            else if (parentSceneObjects.CompareTag("ParentSceneEnemies"))
            {
                if (!parentSceneObjects.GetChild(i).GetComponent<EnemyAI>().isDead)
                {
                    sceneObjects.Add( new ObjectSaved 
                    {
                        prefab =   BearPrefab,
                        position = parentSceneObjects.GetChild(i).position,
                        rotation = parentSceneObjects.GetChild(i).rotation.eulerAngles,
                        scale =    parentSceneObjects.GetChild(i).localScale
                    });
                }
            }
            else
            {
                sceneObjects.Add( new ObjectSaved 
                {
                    prefab =   parentSceneObjects.GetChild(i).GetComponent<Item>().data.prefab,
                    position = parentSceneObjects.GetChild(i).position,
                    rotation = parentSceneObjects.GetChild(i).rotation.eulerAngles,
                    scale =    parentSceneObjects.GetChild(i).localScale
                });
            }
        }

        return sceneObjects;
    }

    public void LoadData()
    {
        string filePath = Application.persistentDataPath + "/SavedData.json";
        string jsonData = System.IO.File.ReadAllText(filePath);

        SavedData savedData = JsonUtility.FromJson<SavedData>(jsonData);

        // Chargement des données

        playerTransform.position = savedData.playerPosition;
        playerTransform.rotation = savedData.playerRotation;

        MainInventory.instance.LoadContent(savedData.mainInventoryContent);
        ActionInventory.instance.LoadContent(savedData.actionInventoryContent);
        EquipementInventory.instance.LoadContent(savedData.equipementInventoryContent);

        playerStats.currentHealth = savedData.currentHealth;
        playerStats.currentHealth = savedData.currentHealth;
        playerStats.currentHealth = savedData.currentHealth;
        playerStats.updateHealthBarFill();

        LoadSceneObjects(savedData.structures, parentSceneStructures, sceneStrucures);;
        LoadSceneObjects(savedData.items, parentSceneItems, sceneItems);
        LoadSceneObjects(savedData.harvestables, parentSceneHarvestables, sceneHarvestables);
        LoadSceneObjects(savedData.enemies, parentSceneEnemies, sceneEnemies);

        Debug.Log("Chargement terminé");
    } 

    private bool LoadSceneObjects(ObjectSaved[] objectsSaved, Transform parentSceneObjects, List<ObjectSaved> sceneObjects) 
    {
        for (int i = 0; i < parentSceneObjects.childCount; i++)
        {
            if (parentSceneObjects.CompareTag("ParentSceneEnemies") && parentSceneObjects.GetChild(i).GetComponent<EnemyAI>().isDead)
            {

            } else 
                Destroy(parentSceneObjects.GetChild(i).gameObject);
        }

        sceneObjects.Clear();

        foreach (ObjectSaved objectSaved in objectsSaved)
        {
            sceneObjects.Add(objectSaved);
            GameObject instantiateObject = Instantiate(
                objectSaved.prefab,
                objectSaved.position,
                Quaternion.Euler(objectSaved.rotation),
                parentSceneObjects
            );
            instantiateObject.transform.localScale = objectSaved.scale;
        }

        return true;
    }
}

[Serializable]
public class SavedData // ToDo : sauvegarder camera
{
    public Vector3 playerPosition;
    public Quaternion playerRotation;

    public ItemInInventory[] mainInventoryContent;
    public ItemInInventory[] actionInventoryContent;
    public ItemInInventory[] equipementInventoryContent;

    public float currentHealth;
    public float currentHunger;
    public float currentThirst;

    public ObjectSaved[] structures;
    public ObjectSaved[] items;
    public ObjectSaved[] harvestables;
    public ObjectSaved[] enemies;
}

[Serializable]
public class ObjectSaved
{
    public GameObject prefab;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
}


