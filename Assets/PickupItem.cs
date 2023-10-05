using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{   // camera
    [SerializeField]
    private float PickupRange = 2.6f;
    public PickupBehaviour playerPickupBehaviour;
    
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, PickupRange)){
            if (hit.transform.CompareTag("Item")) {
                
                if (Input.GetKeyDown(KeyCode.E)) {
                    playerPickupBehaviour.DoPickup(hit.transform.gameObject.GetComponent<Item>());
                }
            }
        }
    }
}
