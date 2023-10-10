using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractBehaviour : MonoBehaviour
{   // Player
    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private MoveBehaviour playerMoveBehaviour;

    [SerializeField]
    private GameObject pickAxeVisual;

    [SerializeField]
    private GameObject axeVisual;


    private Vector3 spawnItemOffset = new Vector3(0, 1, 0);

    private Item currentItem;

    private Harvestable currentHarvestable;

    private Tool currentTool;


    public void DoPickup (Item item) 
    {
        if (inventory.IsFull()) 
        {
            Debug.Log("L'inventaire est plein, je ne peux pas ramasser : " + item.name);
            return;
        }

        currentItem = item;
        
        playerAnimator.SetTrigger("Pickup");
        playerMoveBehaviour.canMove = false;

    }

    public void DoHarvest(Harvestable haverstable)
    {
        currentTool = haverstable.tool;
        EnabledToolGameObjectFromEnum();

        playerAnimator.SetTrigger("Harvest");
        playerMoveBehaviour.canMove = false;
        currentHarvestable = haverstable;
    }

    // coroutine appellée depuis l'animation Harvesting
    IEnumerator BreakHarvestable() 
    {   
        if(currentHarvestable.disableKinematicOnharvest) 
        {
            Rigidbody rigidbody = currentHarvestable.gameObject.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.AddForce(new Vector3(750, 750, 0), ForceMode.Impulse);
        }

        yield return new WaitForSeconds(currentHarvestable.destroyDelay);

        foreach (Ressource ressource in currentHarvestable.HaverstableItems)
        {
            if (UnityEngine.Random.Range(1, 101) <= ressource.dropChance)
            {
                GameObject instantiateRessource = Instantiate(ressource.itemData.prefab);
                instantiateRessource.transform.position = currentHarvestable.transform.position + spawnItemOffset;
            }
        }
        {
            
        }
        Destroy(currentHarvestable.gameObject);
    }

    public void AddItemToInventory()
    {
        inventory.AddItem(currentItem.itemData);
        Destroy(currentItem.gameObject);
    }

    public void ReEnablePlayerMovement()
    {
        playerMoveBehaviour.canMove = true;
        EnabledToolGameObjectFromEnum();
    }

    private void EnabledToolGameObjectFromEnum() 
    {
        switch (currentTool)
        {
            case Tool.Pickaxe:
                pickAxeVisual.SetActive(!pickAxeVisual.activeSelf);
                break;
            case Tool.axe:
                axeVisual.SetActive(!axeVisual.activeSelf);
                break;

        }
    }
}
