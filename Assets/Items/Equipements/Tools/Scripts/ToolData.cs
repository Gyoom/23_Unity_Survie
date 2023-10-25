using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ToolData", menuName = "My Game/New Item/Tool")]
public class ToolData : ItemData
{
    public GameObject visualEquipement;
    
    [Header("TOOL STATS")]
    public Vector3 positionsOffset;
    public Vector3 rotationsOffset;

    public AudioClip toolSound;
}
