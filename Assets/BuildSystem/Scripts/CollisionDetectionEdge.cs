using System.Linq;
using UnityEngine;

public class CollisionDetectionEdge : MonoBehaviour
{
    [SerializeField]
    private StructureType structureType;

    [SerializeField]
    private float radius;

    [SerializeField]
    private Vector3 centerOffset;

    [SerializeField]
    private Vector3 groundOffset;

    [SerializeField]
    private Vector3 obstacleSizeOffset;

    [SerializeField]
    private Vector3 structureSizeOffset;

    [SerializeField]
    private Vector3 terrainSizeOffset;

    [SerializeField]
    PointDetectionEdge[] detectionPoints;

    public MeshRenderer meshRenderer;

    [SerializeField]
    private LayerMask structuresMask;

   public bool CheckConnection() 
   {
        Collider[] structuresColliders = Physics.OverlapBox(transform.position + centerOffset, new Vector3(structureSizeOffset.x / 2, structureSizeOffset.y / 2, structureSizeOffset.x / 2));

        Collider[] obstaclesColliders = Physics.OverlapBox(transform.position + centerOffset, new Vector3(obstacleSizeOffset.x / 2, obstacleSizeOffset.y / 2, obstacleSizeOffset.x / 2));

        Collider[] terrainColliders = Physics.OverlapBox(transform.position + groundOffset, new Vector3(terrainSizeOffset.x / 2, terrainSizeOffset.y / 2, terrainSizeOffset.x / 2));

        // check superposition sur une autre structure
        if (structuresColliders.Length > 0)
        {
            foreach(Collider collider in structuresColliders)
            {
                if (collider.tag == gameObject.tag)
                {
                    return false;
                }
            }
        }

        
        // check si la structure touche le sol et ne traverse pas d'autres objets
        if (obstaclesColliders.Length > 0)
        {   
            foreach(Collider collider in obstaclesColliders)
            {
                if (
                    !collider.CompareTag("Point") && 
                    !collider.CompareTag("Terrain") &&
                    !collider.CompareTag("Stairs") &&
                    !collider.CompareTag("Wall") &&
                    !collider.CompareTag("Floor"))
                {
                    return false;
                }
    
            }
        }

         if (terrainColliders.Length > 0)
        {   
            foreach(Collider collider in terrainColliders)
            {
                if (collider.CompareTag("Terrain"))
                {
                    return true;
                }
    
            }
            
        }
        // check si la structure est en contact avec une autre
        foreach(var point in detectionPoints)
        {
            point.CheckOverlap();
            if (point.connected) return true;
        }

        return false;
   }

   private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + centerOffset, structureSizeOffset);
        
        Gizmos.DrawWireCube(transform.position + centerOffset, obstacleSizeOffset);

        Gizmos.DrawWireCube(transform.position + groundOffset, terrainSizeOffset);
    }
}
