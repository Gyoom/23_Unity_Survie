using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    [Header("COMPONENTS REFERENCES")]
    [SerializeField]
    private ThirdPersonOrbitCamBasic playerCameraScript;

    [SerializeField]
    private AimBehaviourBasic aimBehaviourBasic;

    [Header("UI GAMEOBJECTS REFERENCES")]

    [SerializeField]
    private GameObject craftingPanel;

    [SerializeField]
    private GameObject inventoryPanel;

    [SerializeField]
    private GameObject actionInventoryPanel;

    [SerializeField]
    private GameObject statsBarsPanel;

    [SerializeField]
    private GameObject breakPanel;

    [SerializeField]
    private GameObject interactText;

    [SerializeField]
    private GameObject errorText;

    private int activeErrorCoroutine = 0;


    [HideInInspector]
    public bool atleastOneOpenPanel = false;

    private float defaultHorizontalAimingSpeed;
    private float defaultVerticalAimingSpeed;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        defaultHorizontalAimingSpeed = playerCameraScript.horizontalAimingSpeed;
        defaultVerticalAimingSpeed = playerCameraScript.verticalAimingSpeed;

        statsBarsPanel.SetActive(true);
        actionInventoryPanel.SetActive(true);

        interactText.SetActive(false);
        errorText.SetActive(false);
        breakPanel.SetActive(false);
    }

    public void PanelStatusUpdate()
    {
        // camerz freeze
        if(IsPanelOpen())
        {
            Cursor.lockState = CursorLockMode.None;
            atleastOneOpenPanel = true;
            playerCameraScript.horizontalAimingSpeed = 0;
            playerCameraScript.verticalAimingSpeed = 0;
        } 
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            atleastOneOpenPanel = false;
            playerCameraScript.horizontalAimingSpeed = defaultHorizontalAimingSpeed;
            playerCameraScript.verticalAimingSpeed = defaultVerticalAimingSpeed;
        }
    }

    public bool IsPanelOpen()
    {
        if (inventoryPanel.activeSelf || craftingPanel.activeSelf || breakPanel.activeSelf)
            return true;
        else 
            return false;
    }

    public void UpdateInteractText(string text)
    {
        if (text == "")
        {
            interactText.GetComponent<Text>().text = "";
            interactText.SetActive(false);
        }
        else
        {
            interactText.SetActive(true);
            interactText.GetComponent<Text>().text = text;
        }
    }

    public IEnumerator UpdateErrorText(string text)
    {
        
        errorText.SetActive(true);
        errorText.GetComponent<Text>().text = text;
        activeErrorCoroutine++;

        yield return new WaitForSeconds(5);
        
        activeErrorCoroutine--;

        if (activeErrorCoroutine == 0)
        {
            errorText.GetComponent<Text>().text = "";
            errorText.SetActive(true);
        }
    }
}
