using Game.SaveManager;
using Game.Sounds.SoundScripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionDropdownHandler : MonoBehaviour
{
    public static ResolutionDropdownHandler instance;
    [SerializeField] private TMPro.TMP_Dropdown resolutionDropDown;    //check name!

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;

    //private float currentRefreshRate;
    private int currentResolutionIndex = 0;
    [HideInInspector] public Resolution resolution;

    [SerializeField] private Toggle toggleCanShowShakeCamera;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    private void Start()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();
        toggleCanShowShakeCamera.isOn = ControlMenu.instance.showSkake;
        resolutionDropDown.ClearOptions();
        //currentRefreshRate = Screen.currentResolution.refreshRate;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (!filteredResolutions.Any(x => x.width == resolutions[i].width && x.height == resolutions[i].height))  //check if resolution already exists in list
            {
                filteredResolutions.Add(resolutions[i]);  //add resolution to list if it doesn't exist yet
            }
        }

        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + " x " + filteredResolutions[i].height;
            options.Add(resolutionOption);
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentResolutionIndex;
        resolutionDropDown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        resolution = filteredResolutions[resolutionIndex];
        if (PlayerPrefs.GetInt("ScreenMode") == 0)
        {
            // Switch to 4k full
            Screen.SetResolution(resolution.width, resolution.height, true);

            //Screen.fullScreen = true;
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        if (PlayerPrefs.GetInt("ScreenMode") == 1)
        {
            Screen.SetResolution(resolution.width, resolution.height, true);
            //Screen.fullScreen = true;
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        if (PlayerPrefs.GetInt("ScreenMode") == 2)
        {
            Screen.SetResolution(resolution.width, resolution.height, false);
            //Screen.fullScreen = false;
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }


    public void OnChangeValueCheckCanShowShakeCamera()
    {
        if (toggleCanShowShakeCamera.isOn)
        {
            ControlMenu.instance.showSkake = true;
            ControlMenu.instance.objectData.ShowShakeCamera = true;
        }
        else if (!toggleCanShowShakeCamera.isOn)
        {
            
            ControlMenu.instance.showSkake = false;
            ControlMenu.instance.objectData.ShowShakeCamera = false;
        }

        SaveManager.SaveData();
    }
}
