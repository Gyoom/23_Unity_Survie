using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BreakMenu : MonoBehaviour
{
    [Header("COMPONENTS REFERENCES")]
    [SerializeField]
    private ThirdPersonOrbitCamBasic mainCameraScript;

    [SerializeField]
    private AudioMixer mainMixer;

    [SerializeField]
    private BuildSystem buildSystem;

    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private FlyBehaviour flyBehaviour;

    [Header("MAIN MENU REFERENCES")]

    [SerializeField]
    private GameObject breakMenuPanel;

    public bool breakMenuIsOpen = false;

    public Button loadButton;

    [Header("OPTIONS REFERENCES")]

    [SerializeField]
    private GameObject breakMenuOptionsPanel;

    [SerializeField]
    private Dropdown resolutionDropdown;

    [SerializeField]
    private Dropdown qualityDropdown;

    [SerializeField]
    private Toggle fullscreenToogle;

    [SerializeField]
    private Toggle cheatToogle;

    [SerializeField]
    private Slider volumeSlider;

    public Button clearSavedDataButton;

     void Start()
    {
        // Dynamic Button
        bool saveExist = System.IO.File.Exists(Application.persistentDataPath + "/SavedData.json");
        
        loadButton.interactable  = saveExist;
        clearSavedDataButton.interactable = saveExist;
        
        // Options
        // Resolutions
        Resolution[] resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>();

        int currentResolutionIndex = 0;

        for(int i = 0; i < resolutions.Length; i++)
        {
            string resolutionOption = resolutions[i].width + " x " + resolutions[i].height + " (" + resolutions[i].refreshRate + " Hz)";
            resolutionOptions.Add(resolutionOption);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Quality
        string[] qualities = QualitySettings.names;
        qualityDropdown.ClearOptions();

        List<string> qualityOptions = new List<string>();
        int currentQualityIndex = 0;

        for(int i = 0; i < qualities.Length; i++)
        {
            qualityOptions.Add(qualities[i]);

            if (i == QualitySettings.GetQualityLevel())
            {
                currentQualityIndex = i;
            }
        }

        qualityDropdown.AddOptions(qualityOptions);
        qualityDropdown.value = currentQualityIndex;
        qualityDropdown.RefreshShownValue();

        // Volume
        mainMixer.GetFloat("mainVolume", out float soundValueForSlider);
        volumeSlider.value = soundValueForSlider;

        // Fullscreen
        fullscreenToogle.isOn = Screen.fullScreen;

        // Cheat
        cheatToogle.isOn = false;

        // Option Panel
        breakMenuOptionsPanel.SetActive(false);
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

    // Main Menu Methods

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void OptionsButton()
    {
        breakMenuOptionsPanel.SetActive(!breakMenuOptionsPanel.activeSelf);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    // Options Methods

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = Screen.resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetVolume(float volume)
    {
        mainMixer.SetFloat("mainVolume", volume);
    }

    public void ClearSavedData()
    {
        System.IO.File.Delete(Application.persistentDataPath + "/SavedData.json");
        loadButton.interactable = false;
        clearSavedDataButton.interactable = false;
    }

    public void UpdateCheat(bool isCheat)
    {
        buildSystem.cheatBuild = isCheat;
        playerStats.cheatStats = isCheat;
        flyBehaviour.flyCheat = isCheat;
    }
}
