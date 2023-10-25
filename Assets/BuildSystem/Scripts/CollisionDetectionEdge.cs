using System.Linq;
using UnityEngine;

public class CollisionDetectionEdge : MonoBehaviour
{
    [SerializeField]
    private float radius;

    [SerializeField]
    private Vector3 centerOffset;

    private Collider[] hitColliders;

    [SerializeField]
    PointDetectionEdge[] detectionPoints;

    public MeshRenderer meshRenderer;

   public bool CheckConnection() 
   {

        hitColliders = Physics.OverlapSphere(transform.position + centerOffset, radius);

        if (hitColliders.Length > 0)
        {

            if (hitColliders.Any(collider => !collider.CompareTag("Terrain"))) // évite de construire des structures à l'infini et au même endroit et à traver des gamesobjects existants
            {
                return false;
            }

        }
        foreach(var point in detectionPoints)
        {
            point.CheckOverlap();
            if (point.connected) return true;
        }

        return false;
   }

   private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + centerOffset, radius);
    }
}
