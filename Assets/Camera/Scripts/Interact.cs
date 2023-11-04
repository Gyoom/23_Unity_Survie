using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Interact: MonoBehaviour
{   // camera
    [SerializeField]
    private float interactRange = 2.6f;
    public InteractBehaviour playerInteractBehaviour;

    [SerializeField]
    private LayerMask interactLayers;

    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private TagInteractText[] tagInteractTexts;
    
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, interactRange, interactLayers))
        {
            string currentText = tagInteractTexts.Where(t => t.tag == hit.transform.tag).FirstOrDefault().text;

            if (hit.transform.CompareTag("Item")) 
            {   
                uiManager.UpdateInteractText(currentText);

                if (Input.GetKeyDown(KeyCode.E)) 
                {
                    playerInteractBehaviour.DoPickup(hit.transform.gameObject.GetComponent<Item>());
                }   
            }
            else if (hit.transform.CompareTag("Harvestable") && playerInteractBehaviour.CanHarvest(hit.transform.gameObject.GetComponent<Harvestable>().data)) 
            {
                uiManager.UpdateInteractText(currentText);

                if (Input.GetKeyDown(KeyCode.E)) 
                {
                    playerInteractBehaviour.DoHarvest(hit.transform.gameObject);
                }
            }
        }
        else 
            uiManager.UpdateInteractText("");
    }
}

[Serializable]
public class TagInteractText
{
    public string tag;
    public string text;
}