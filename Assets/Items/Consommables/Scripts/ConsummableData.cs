using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConsummableData", menuName = "My Game/New Item/Consummable")]
public class ConsummableData : ItemData
{
    [Header("CONSUMMABLE STATS")]
    public ConsumableEffect[] consumableEffects;
}

[Serializable]
public class ConsumableEffect
{
    public ConsumableTarget consumableTarget;
    public float consumableValue;
}

public enum ConsumableTarget
{
    health,
    hunger,
    thirst
}
