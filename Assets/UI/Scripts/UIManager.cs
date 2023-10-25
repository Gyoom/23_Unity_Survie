using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] UIPanels;

    [SerializeField]
    private ThirdPersonOrbitCamBasic playerCameraScript;

    [SerializeField]
    private AimBehaviourBasic aimBehaviourBasic;

    [HideInInspector]
    public bool atleastOneOpenPanel = false;

    private float defaultHorizontalAimingSpeed;
    private float defaultVerticalAimingSpeed;

    void Start()
    {
        defaultHorizontalAimingSpeed = playerCameraScript.horizontalAimingSpeed;
        defaultVerticalAimingSpeed = playerCameraScript.verticalAimingSpeed;
    }

    void Update()
    {
        if(UIPanels.Any(panel => panel == panel.activeSelf) && aimBehaviourBasic.aim == false)
        {
            atleastOneOpenPanel = true;
            playerCameraScript.horizontalAimingSpeed = 0;
            playerCameraScript.verticalAimingSpeed = 0;
        } 
        else
        {
            atleastOneOpenPanel = false;
            playerCameraScript.horizontalAimingSpeed = defaultHorizontalAimingSpeed;
            playerCameraScript.verticalAimingSpeed = defaultVerticalAimingSpeed;
        }
    }
}
