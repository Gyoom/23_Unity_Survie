using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EquipementLibrary : MonoBehaviour
{
    public List<VisualLibrary> visualLibrary = new List<VisualLibrary>();

    public List<SlotLibrary> slotLibrary = new List<SlotLibrary>();

    public Image GetEquipementSlotImage(EquipementType equipementType)
    {
        return slotLibrary.Where(e => e.type == equipementType).First().slotImage;         
    }

    public Button GetEquipementSlotButtom(EquipementType equipementType)
    {
        return slotLibrary.Where(e => e.type == equipementType).First().slotButton;              
    }

    public void EnableOrDisableDefautElement(VisualLibrary visualLibrary, bool boolean)
    {
        for (int i = 0; i <visualLibrary.elementsToDisable.Length; i++)
        {
            visualLibrary.elementsToDisable[i].SetActive(boolean);
        }
    }
}

[Serializable]
public class VisualLibrary 
{
    public GameObject visualPrefab;
    public GameObject parentVisual;

    public GameObject[] elementsToDisable;
}

[Serializable]
public class SlotLibrary
{
    public EquipementType type;

    public Image slotImage;

    public Button slotButton;

}