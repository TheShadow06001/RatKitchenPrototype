using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_OptionsMenu : MonoBehaviour
{
    public TMP_Dropdown DisplayModeDropdown;
    public TMP_Dropdown ResolutionDropdown;
    public TMP_Dropdown QualityDropdown;
    public Toggle VSyncToggle;
    public GameObject OptionsMenu;

    private void Start()
    {
        SyncOptions();
        DisplayModeDropdown.onValueChanged.AddListener(SetDisplayMode);
        ResolutionDropdown.onValueChanged.AddListener(SetResolution);
        QualityDropdown.onValueChanged.AddListener(SetQuality);
        VSyncToggle.onValueChanged.AddListener(SetVSync);
    }

    public void CloseOptionsMenu() //Return to Main Menu
    {
        OptionsMenu.SetActive(false);
    }

    public void SyncOptions()
    {
        //Display Mode
        var FSM = Screen.fullScreenMode;
        var DropMenu = FSM == FullScreenMode.ExclusiveFullScreen ? 0 :
            FSM == FullScreenMode.FullScreenWindow ? 1 :
            FSM == FullScreenMode.Windowed ? 2 : 0;
        DisplayModeDropdown.SetValueWithoutNotify(DropMenu);

        //Resolution
        var resIndex = 0;
        if (Screen.width == 1920 && Screen.height == 1080)
            resIndex = 1;
        else if (Screen.width == 1280 && Screen.height == 720)
            resIndex = 2;
        ResolutionDropdown.SetValueWithoutNotify(resIndex);

        //Quality
        QualityDropdown.SetValueWithoutNotify(QualitySettings.GetQualityLevel());
        //VSync
        VSyncToggle.SetIsOnWithoutNotify(QualitySettings.vSyncCount > 0);
    }

    public void SetDisplayMode(int i)
    {
        var mode = i == 0 ? FullScreenMode.ExclusiveFullScreen :
            i == 1 ? FullScreenMode.FullScreenWindow :
            i == 2 ? FullScreenMode.Windowed : FullScreenMode.ExclusiveFullScreen;
        Screen.fullScreenMode = mode;
        PlayerPrefs.SetInt("DisplayMode", i);
    }

    public void SetResolution(int i)
    {
        var width = i == 0 ? 2560 :
            i == 1 ? 1920 :
            i == 2 ? 1280 : 1920;
        var height = i == 0 ? 1440 :
            i == 1 ? 1080 :
            i == 2 ? 720 : 1080;
        Screen.SetResolution(width, height, Screen.fullScreenMode);
        PlayerPrefs.SetInt("Resolution", i);
    }

    public void SetQuality(int i)
    {
        QualitySettings.SetQualityLevel(i);
        PlayerPrefs.SetInt("Quality", i);
    }

    public void SetVSync(bool on)
    {
        QualitySettings.vSyncCount = on ? 1 : 0;
        PlayerPrefs.SetInt("VSync", on ? 1 : 0);
    }
}