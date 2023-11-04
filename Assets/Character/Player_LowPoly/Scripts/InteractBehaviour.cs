using System.Collections;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;

public class InteractBehaviour : MonoBehaviour
{   
    // Player
    [Header("COMPONENTS REFERENCES")]

    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private MoveBehaviour playerMoveBehaviour;

    [SerializeField]
    private EquipementSystem equipSystem;

    [SerializeField]
    private AudioSource playerAudioSource;

    [Header("OTHERS")]

    [SerializeField]
    private AudioClip pickUpSound;

    [SerializeField]
    private Transform itemsParent;

    [HideInInspector]
    public bool isBusy = false;

    private Vector3 spawnItemOffset = new Vector3(0, 1, 0);

    private Item currentItem;

    private GameObject currentHarvestable;

    private ToolData currentEquipedToolData;

    // Pickup Methods
    public void DoPickup (Item item) 
    {
        if (isBusy) return;
        isBusy = true;

        if (MainInventory.instance.IsFull()) 
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
        MainInventory.instance.AddItem(currentItem.data);
        Destroy(currentItem.gameObject);
    }

    public bool CanHarvest(HarvestableData harvestableData)
    {
        if(isBusy)
            return false;

        EquipedItem equipedItem = equipSystem.GetEquipedItem(EquipableItemType.Tool);

        if (!equipedItem.equiped)
            return false;
        
        if (equipedItem.data != harvestableData.toolData)
            return false;

        currentEquipedToolData = (ToolData) equipedItem.data;
        return true;
    }

    // Harvest Methods

    public void DoHarvest(GameObject harvestable)
    {
        if (!CanHarvest(harvestable.GetComponent<Harvestable>().data))
            return;

        playerAudioSource.clip = currentEquipedToolData.useSound;
        playerAnimator.SetTrigger("Harvest");
        playerMoveBehaviour.canMove = false;
        currentHarvestable = harvestable;
    }

    // Coroutine appellée depuis l'animation Harvesting
    IEnumerator BreakHarvestable() 
    { 
        Harvestable harvestableScript  = currentHarvestable.GetComponent<Harvestable>();
        harvestableScript.gameObject.layer = LayerMask.NameToLayer("Default");

        if(harvestableScript.data.disableKinematicOnharvest) 
        {
            Rigidbody rigidbody = currentHarvestable.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            
            //rigidbody.AddForce(transform.forward * 100000);
            rigidbody.AddForceAtPosition(transform.forward * 200000, currentHarvestable.transform.GetChild(0).position);

        }
        yield return new WaitForSeconds(harvestableScript.data.destroyDelay);

        Vector3 harvestablePosition = harvestableScript.transform.position;
        Vector3 currentSpawnItemOffset = spawnItemOffset;
        Ressource[] ressources = harvestableScript.data.dropItems;

        Destroy(harvestableScript.gameObject);

        foreach (Ressource ressource in ressources)
        {
            if (Random.Range(1, 101) <= ressource.dropChance)
            {
                GameObject instantiateRessource = Instantiate(ressource.itemData.prefab, itemsParent);
                instantiateRessource.transform.position = harvestablePosition + currentSpawnItemOffset;
            }
            currentSpawnItemOffset.y += 0.2f;
        }
    }

    // Call from harvestable animation
    public void PlayHarvestingSoundEffect()
    {
        playerAudioSource.Play();
    }

    // Call from harvestable animation
    public void ReEnablePlayerMovement()
    {
        isBusy = false;
        playerMoveBehaviour.canMove = true;
    }
}
