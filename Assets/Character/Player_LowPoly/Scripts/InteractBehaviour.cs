using System.Collections;
using UnityEngine;
using System.Linq;

public class InteractBehaviour : MonoBehaviour
{   // Player
    [Header("COMPONENTS REFERENCES")]
    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private MoveBehaviour playerMoveBehaviour;

    [SerializeField]
    private Equipement equipementSystem;

    [SerializeField]
    private EquipementLibrary equipementLibrary;

    [SerializeField]
    private AudioSource playerAudioSource;

    [Header("TOOLS CONFIGURATION")]
    [SerializeField]
    private Transform toolTransfomParent;

    // [SerializeField]
    // private GameObject pickAxeVisual;

    // [SerializeField]
    // private GameObject axeVisual;

    [Header("OTHERS")]

    [SerializeField]
    private AudioClip pickUpSound;

    [SerializeField]
    private Transform itemsParent;

    [HideInInspector]
    public bool isBusy = false;

    private Vector3 spawnItemOffset = new Vector3(0, 1, 0);

    private Item currentItem;

    private Harvestable currentHarvestable;

    private ToolData currentTool;

    private GameObject toolCurentlyUsed;

    // Pickup Methods
    public void DoPickup (Item item) 
    {
        if (isBusy) return;
        isBusy = true;

        if (inventory.IsFull()) 
        {
            Debug.Log("L'inventaire est plein, je ne peux pas ramasser : " + item.name);
            return;
        }

        currentItem = item;
        
        playerAnimator.SetTrigger("Pickup");
        playerMoveBehaviour.canMove = false;

    }
    // Pickup - Call from Animation
    public void AddItemToInventory()
    {
        playerAudioSource.PlayOneShot(pickUpSound);
        inventory.AddItem(currentItem.itemData);
        Destroy(currentItem.gameObject);
    }

    // Harvest Methods

    public void DoHarvest(Harvestable haverstable)
    {
        if (isBusy) return;
        isBusy = true;

        currentTool = haverstable.harvestableData.toolData;
        EnabledToolGameObject();

        playerAnimator.SetTrigger("Harvest");
        playerMoveBehaviour.canMove = false;
        currentHarvestable = haverstable;
    }

    // coroutine appellée depuis l'animation Harvesting
    IEnumerator BreakHarvestable() 
    { 
        Harvestable harvestable  = currentHarvestable;
        harvestable.gameObject.layer = LayerMask.NameToLayer("Default");

        if(harvestable.harvestableData.disableKinematicOnharvest) 
        {
            Rigidbody rigidbody = harvestable.gameObject.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.AddForce(transform.forward * 800, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(harvestable.harvestableData.destroyDelay);

        foreach (Ressource ressource in harvestable.harvestableData.dropItems)
        {
            if (UnityEngine.Random.Range(1, 101) <= ressource.dropChance)
            {
                GameObject instantiateRessource = Instantiate(ressource.itemData.prefab, itemsParent);
                instantiateRessource.transform.position = harvestable.transform.position + spawnItemOffset;
            }
        }

        Destroy(harvestable.gameObject);
    }

    private void EnabledToolGameObject(bool enabled = true) 
    {
        // activer / desactiver l'arme équipée le temps de la récolte 
        GameObject actualWeapon = equipementSystem.GetCurrentEquipementVusual(EquipementType.Weapon);
        if (actualWeapon != null)
        {
            actualWeapon.SetActive(!enabled);
        }

        if (enabled)
        {    
            toolCurentlyUsed = Instantiate(
                currentTool.visualEquipement, 
                currentTool.positionsOffset, 
                Quaternion.Euler(currentTool.rotationsOffset)
            );
            toolCurentlyUsed.transform.SetParent(toolTransfomParent, false);
            playerAudioSource.clip = currentTool.toolSound; 

        }
        else 
        {
            Destroy(toolCurentlyUsed);
            toolCurentlyUsed = null;
        }
    }
    // call from animation
    public void PlayHarvestingSoundEffect()
    {
        playerAudioSource.Play();
    }

    // Various Methods
    public void ReEnablePlayerMovement()
    {
        isBusy = false;
        playerMoveBehaviour.canMove = true;
        EnabledToolGameObject(false);
    }
}
