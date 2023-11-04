using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointDetectionEdge : MonoBehaviour
{
    [SerializeField]
    private TypePoint typePoint;
    public bool connected;
    public float radius = 0.6f;
    public Collider[] hitColliders;

    public void CheckOverlap() 
    {
        connected = false;
        hitColliders = Physics.OverlapSphere(transform.position, radius); // Good practice : renvoi toute les boite de collision trouvées 

        if (hitColliders.Length > 0)
        {
            foreach (Collider collider in hitColliders)
            {
                if (collider.CompareTag("Point"))
                {
                    connected = true;
                    return;
                }
            }
        }
    }

    public void OnDisable()  // Good Practice : Se lit automatiquement quand l'objet est désactivé.
    {
        connected = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

    public enum TypePoint 
    {
        Other,
        Top,
        Bottom
    }
