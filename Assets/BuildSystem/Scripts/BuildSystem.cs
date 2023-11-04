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

    [SerializeField]
    private UIManager uIManager;

    [Header("CONFIGURATION")]

    [SerializeField]
    private Material blueMaterial;
    
    [SerializeField]
    private Material redMaterial;
    
    [SerializeField]
    private Transform rotationRef;

    [SerializeField]
    private AudioClip buildSound;

    public bool cheatBuild = false;

    [SerializeField]
    private Transform parentSceneStructure;


    private StructureData currentStructureData;

    private GameObject currentPlacementStructurePrefab;
    private Vector3 targetPosition;
    private bool canBuild;
    private bool inPlace;
    private bool systemEnabled = false;

    void Awake() 
    {
        DiseabledSystem();
    }

    void FixedUpdate() 
    {
        if (!systemEnabled)
            return;
        canBuild = currentPlacementStructurePrefab.GetComponentInChildren<CollisionDetectionEdge>().CheckConnection();
        targetPosition = grid.GetNearestPointOnGrid(transform.position);
        CheckPosition();
        RoundPlacementStructureRotation();
        UpdatePlacementStructureMaterial();
    }
    void Update()
    {

        if (uIManager.IsPanelOpen())
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0) && canBuild && inPlace && systemEnabled)
        {
            BuildStructure();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateStructure();
        }
    }

    public void DiseabledSystem()
    {
        if (systemEnabled) 
        {
            systemEnabled = false;
            Destroy(currentPlacementStructurePrefab);
        }
    }

    public void EnableSystem(StructureData newStructureItem)
    {
        systemEnabled = true;
        currentStructureData = newStructureItem;
        currentPlacementStructurePrefab = Instantiate(currentStructureData.placementPrefab);      
    }

    private void BuildStructure()
    {
        playerAudioSource.PlayOneShot(buildSound);

        Instantiate(
            currentStructureData.prefab, 
            currentPlacementStructurePrefab.transform.position, 
            currentPlacementStructurePrefab.transform.GetChild(0).rotation,
            parentSceneStructure
        );

        if (!cheatBuild)
        {
            ActionInventory.instance.RemoveItem(currentStructureData);
        }
    }

    private void UpdatePlacementStructureMaterial()
    {
        MeshRenderer placementPrefabRenderer = currentPlacementStructurePrefab.GetComponentInChildren<CollisionDetectionEdge>().meshRenderer;
        if (inPlace && canBuild)
        {
            placementPrefabRenderer.material = blueMaterial;
        }
        else 
        {
            placementPrefabRenderer.material = redMaterial;
        }
    }

    private void CheckPosition() 
    {
        inPlace = currentPlacementStructurePrefab.transform.position == targetPosition;

        if(!inPlace)
        {
            SetPosition(targetPosition);
        }
    }

    private void SetPosition(Vector3 targetPosition)
    {
        Transform placementPrefabTransform = currentPlacementStructurePrefab.transform;
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
    {   if (currentStructureData.structureType != StructureType.Wall)
        {
            currentPlacementStructurePrefab.transform.GetChild(0).transform.Rotate(0, 90, 0);
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

       currentPlacementStructurePrefab.transform.rotation = Quaternion.Euler(0, roundedRotation, 0);
    }
}
