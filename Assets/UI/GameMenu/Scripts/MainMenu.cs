using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Button loadButton;

    [SerializeField]
    private GameObject optionsPanel;

    [SerializeField]
    private Dropdown resolutionDropdown;

    [SerializeField]
    private Dropdown qualityDropdown;

    [SerializeField]
    private Toggle fullscreenToogle;

    [SerializeField]
    private Slider volumeSlider;

    [SerializeField]
    private AudioMixer mainMixer;

    public Button clearSavedDataButton;

    

    public static bool loadSavedData;

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

        // Option Panel
        optionsPanel.SetActive(false);
    }

    // Main Menu Methods

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
    public void NewGameButton()
    {
        loadSavedData = false;
        SceneManager.LoadScene("Scene");
    }

    public void LoadButton()
    {
        loadSavedData = true;
        SceneManager.LoadScene("Scene");
    }

    public void OptionsButton()
    {
        optionsPanel.SetActive(!optionsPanel.activeSelf);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    // Options Methods

    public void setResolution(int resolutionIndex)
    {
        Resolution resolution = Screen.resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void setQuality(int qualityIndex)
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
}
