using UnityEngine;

public class BreakMenu : MonoBehaviour
{
    
    [SerializeField]
    private GameObject breakMenuPanel;

    [SerializeField]
    private GameObject breakMenuOptionsPanel;

    [SerializeField]
    private ThirdPersonOrbitCamBasic mainCameraScript;

    private bool breakMenuIsOpen = false;

    void Start()
    {
        breakMenuPanel.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            breakMenuIsOpen = !breakMenuIsOpen;
            if (!breakMenuIsOpen)
                breakMenuOptionsPanel.SetActive(false);
            breakMenuPanel.SetActive(breakMenuIsOpen);

            Time.timeScale = breakMenuIsOpen ? 0 : 1;

            mainCameraScript.enabled = !breakMenuIsOpen;

        }
    }
}
