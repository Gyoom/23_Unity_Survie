using UnityEngine;


[CreateAssetMenu(fileName = "RecipData", menuName = "My Game/New Recip")]
public class RecipData : ScriptableObject
{
    public ItemData craftableItem;
    public ItemInInventory[] requiredItems;
}
