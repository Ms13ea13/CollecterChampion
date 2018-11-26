using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetResolution : MonoBehaviour
{
    int activeScreenResolutionIndex;

    public GameObject optionPanelHolder;
    public Toggle[] resolutionToggles;
    public int[] screenWidths;

    void Start()
    {
        activeScreenResolutionIndex = PlayerPrefs.GetInt("Screen resolution index");
        bool isFullscreen = (PlayerPrefs.GetInt("Fullscreen") == 1) ? true : false;

        for (int i = 0; i < resolutionToggles.Length; i++)
        {
            resolutionToggles[i].isOn = i == activeScreenResolutionIndex;
        }

        SetFullScreen(isFullscreen);
    }

    public void SetScreenResolution(int index)
    {
        if (resolutionToggles[index].isOn)
        {
            activeScreenResolutionIndex = index;
            float aspectRatio = 16 / 9f;
            Screen.SetResolution(screenWidths[index], (int)(screenWidths[index] / aspectRatio), false);
            PlayerPrefs.SetInt("Screen resolution index", activeScreenResolutionIndex);
            PlayerPrefs.Save();
        }
    }

    public void SetFullScreen(bool isFullscreen)
    {
        for (int i = 0; i < resolutionToggles.Length; i++)
        {
            resolutionToggles[i].interactable = !isFullscreen;
        }

        if (isFullscreen)
        {
            Resolution[] allResolutions = Screen.resolutions;
            Resolution maxResolution = allResolutions[allResolutions.Length - 1];
            Screen.SetResolution(maxResolution.width, maxResolution.height, true);
        }
        else
        {
            SetScreenResolution(activeScreenResolutionIndex);
        }

        PlayerPrefs.SetInt("Fullscreen", (isFullscreen) ? 1 : 0);
        PlayerPrefs.Save();
    }
}
