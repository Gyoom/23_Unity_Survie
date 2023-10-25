using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;


public class BuildSystem : MonoBehaviour // ToDo :Pourquoi pas un system avec raycast ?
{
    [Header("COMPONENTS REFERENCES")]
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private AudioSource playerAudioSource;

    [Header("CONFIGURATION")]

    [SerializeField]
    private Structure[] structures;

    [SerializeField]
    private Material blueMaterial;
    
    [SerializeField]
    private Material redMaterial;
    
    [SerializeField]
    private Transform rotationRef;

    [SerializeField]
    private AudioClip buildSound;

    [SerializeField]
    private bool cheatBuild = false;

    [SerializeField]
    private Transform placedStructureParent;

    [Header("UI REFERENCES")]

    [SerializeField]
    private Transform buildSystemPanel;

    [SerializeField]
    private GameObject buildingRequiredElement;

    private Structure currentStructure;
    private Vector3 targetPosition;
    private bool canBuild;
    private bool inPlace;
    private bool systemEnabled = false;

    [HideInInspector]
    public List<PlacedStructure> placedStructures;

    void Awake() 
    {
        currentStructure = structures.First();
        DiseabledSystem();
    }

    void FixedUpdate() 
    {
        if (!systemEnabled)
            return;
        canBuild = currentStructure.placementPrefab.GetComponentInChildren<CollisionDetectionEdge>().CheckConnection();
        targetPosition = grid.GetNearestPointOnGrid(transform.position);
        CheckPosition();
        RoundPlacementStructureRotation();
        UpdatePlacementStructureMaterial();
    }
    void Update() // ToDo : Menu de construction minecraft
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (currentStructure.structureType == StructureType.Stairs && systemEnabled)
                DiseabledSystem();
            else
                ChangeStructureType(GetStructureByType(StructureType.Stairs));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {   
            if (currentStructure.structureType == StructureType.Wall && systemEnabled)
                DiseabledSystem();
            else
                ChangeStructureType(GetStructureByType(StructureType.Wall));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (currentStructure.structureType == StructureType.Floor && systemEnabled)
                DiseabledSystem();
            else
                ChangeStructureType(GetStructureByType(StructureType.Floor));
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && canBuild && inPlace && systemEnabled && (hasAllRessouces() || cheatBuild))
        {

            BuildStructure();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateStructure();
        }
    }

    private void BuildStructure()
    {
        playerAudioSource.PlayOneShot(buildSound);

        Instantiate(
            currentStructure.instantiatePrefab, 
            currentStructure.placementPrefab.transform.position, 
            currentStructure.placementPrefab.transform.GetChild(0).rotation,
            placedStructureParent
        );

        placedStructures.Add(
            new PlacedStructure 
            {
                prefab = currentStructure.instantiatePrefab, 
                position = currentStructure.placementPrefab.transform.position, 
                rotation = currentStructure.placementPrefab.transform.GetChild(0).rotation.eulerAngles
            }
        );

        if (!cheatBuild)
        {
            foreach(ItemInInventory item in currentStructure.ressoucesCost)
            {
                for (int i = 0; i < item.count; i++)
                {
                    Inventory.instance.RemoveItem(item.itemData);
                }
            }
        }
    }

    public void UpdateDisplayCosts()
    {
        foreach (Transform child in buildSystemPanel)  
        {
            child.GetComponent<BuildingRequiredElement>().CheckHasRessourcesToBuild();
        }
    }

    private bool hasAllRessouces()
    {
        BuildingRequiredElement[] requiredElementsScripts = GameObject.FindObjectsOfType<BuildingRequiredElement>();
        return requiredElementsScripts.All(requiredElement => requiredElement.hasRessouces);
    }

    private void DiseabledSystem()
    {
        systemEnabled = false;
        currentStructure.placementPrefab.SetActive(false);
        buildSystemPanel.gameObject.SetActive(false);
    }

    private void UpdatePlacementStructureMaterial()
    {
        MeshRenderer placementPrefabRenderer = currentStructure.placementPrefab.GetComponentInChildren<CollisionDetectionEdge>().meshRenderer;
        if (inPlace && canBuild)
        {
            placementPrefabRenderer.material = blueMaterial;
        }
        else 
        {
            placementPrefabRenderer.material = redMaterial;
        }
    }

    private void ChangeStructureType(Structure newStructure)
    {
        systemEnabled = true;
        buildSystemPanel.gameObject.SetActive(true);
        currentStructure = newStructure;
        foreach(Structure structure in structures)
        {
            structure.placementPrefab.SetActive(structure.structureType == currentStructure.structureType);
        }

        foreach (Transform child in buildSystemPanel) // Good Practice : foreach sur un transform pour obtenir ses objets  
        {
            Destroy(child.gameObject);
        }

        foreach(ItemInInventory requiredRessource in currentStructure.ressoucesCost)
        {
            GameObject buildingRequiredElementGO = Instantiate(buildingRequiredElement, buildSystemPanel);
            buildingRequiredElementGO.GetComponent<BuildingRequiredElement>().Setup(requiredRessource);
            
        }

    }

    private Structure GetStructureByType(StructureType structureType)
    {
        return structures.Where(s => s.structureType == structureType).FirstOrDefault();
    }

    private void CheckPosition() 
    {
        inPlace = currentStructure.placementPrefab.transform.position == targetPosition;

        if(!inPlace)
        {
            SetPosition(targetPosition);
        }
    }

    private void SetPosition(Vector3 targetPosition)
    {
        Transform placementPrefabTransform = currentStructure.placementPrefab.transform;
        Vector3 positionVelocity = Vector3.zero; 

        if (Vector3.Distance(placementPrefabTransform.position, targetPosition) > 10)
        {
            placementPrefabTransform.position = targetPosition;
        } 
        else
        {
            Vector3 newTargetPosition = Vector3.SmoothDamp(placementPrefabTransform.position, targetPosition, ref positionVelocity, 0, 15000);
            // Good practice : deplacement progressif de l'objet d'un point A à B
            placementPrefabTransform.position = newTargetPosition;
        } 
    }

    private void RotateStructure() 
    {   if (currentStructure.structureType != StructureType.Wall)
        {
            currentStructure.placementPrefab.transform.GetChild(0).transform.Rotate(0, 90, 0);
        }
    }

    private void RoundPlacementStructureRotation()
    {
        float yAngle = rotationRef.localEulerAngles.y;
        int roundedRotation = 0;
        if (yAngle > -45 && yAngle <= 45)
        {
            roundedRotation = 0;
        }
        else if (yAngle > 45 && yAngle <= 135)
        {
            roundedRotation = 90;
        }
        else if (yAngle > 135 && yAngle <= 225)
        {
            roundedRotation = 180;
        }
        else if (yAngle > 225 && yAngle <= 315)
        {
            roundedRotation = 270;
        }

        currentStructure.placementPrefab.transform.rotation = Quaternion.Euler(0, roundedRotation, 0);
    }

    public void LoadStructures(PlacedStructure[] structureToLoad)
    { 
        foreach (Transform child in placedStructureParent.transform)
        {
            Destroy(child.gameObject);
        }
        placedStructures.Clear();
        foreach (PlacedStructure structure in structureToLoad)
        {
            placedStructures.Add(structure);
            Instantiate(
                structure.prefab,
                structure.position,
                Quaternion.Euler(structure.rotation),
                placedStructureParent
            );
        }
    }
}

[Serializable]
public class Structure // ToDo : créer une Data 
{
    public GameObject placementPrefab;
    public GameObject instantiatePrefab;
    public StructureType structureType;

    public ItemInInventory[] ressoucesCost;
}

[Serializable]
public class PlacedStructure
{
    public GameObject prefab;
    public Vector3 position;
    public Vector3 rotation;
}

public enum StructureType 
{
  Stairs,
  Wall,
  Floor  
}
