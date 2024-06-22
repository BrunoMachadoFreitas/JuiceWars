using Game.SaveManager;
using Game.Sounds.SoundScripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopUp : MonoBehaviour
{
    //SOUND
    public float AudioVolumeSoundsGeneral;
    public float AudioVolumeMusic;

    [SerializeField] private Slider valueSlider;
    [SerializeField] private Slider valueSliderMusic;

    [SerializeField] private Toggle toggleCanPlaySounds;
    [SerializeField] private Toggle toggleCanPlayGameMusic;

    [SerializeField] private TextMeshProUGUI SoundValueOnUISettings;
    [SerializeField] private TextMeshProUGUI SoundValueMusicOnUISettings;


    public GameObject BackgroundTab1;
    public GameObject BackgroundTab2;
    public GameObject BackgroundTab3;
    public Image TabDisplay;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {


        valueSlider.value = ControlMenu.instance.objectData.SoundValue;
        valueSliderMusic.value = ControlMenu.instance.objectData.SoundMusicValue;

        toggleCanPlaySounds.isOn = ControlMenu.instance.objectData.CanPlayGameSounds;
        toggleCanPlayGameMusic.isOn = ControlMenu.instance.objectData.CanPlayGameMusic;

        SoundManager.instance.MainMusicX.volume = valueSliderMusic.value;
        for (int i = 0; i < SoundManager.instance.GameSoundsInGame.Count; i++)
        {
            SoundManager.instance.GameSoundsInGame[i].volume = valueSlider.value;
        }
        this.gameObject.SetActive(false);

        if(DeviceController.instance.TargetPlatform != TargetPlatform.PC)
        {
            TabDisplay.gameObject.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenTab1HideOthers()
    {
        BackgroundTab1.SetActive(true);
        BackgroundTab2.SetActive(false);
        BackgroundTab3.SetActive(false);
        SoundManager.instance.PlaySound(12);
    }

    public void OpenTab2HideOthers()
    {
        BackgroundTab2.SetActive(true);
        BackgroundTab1.SetActive(false);
        BackgroundTab3.SetActive(false);
        SoundManager.instance.PlaySound(12);
    }

    public void OpenTab3HideOthers()
    {
        if (DeviceController.instance.TargetPlatform == TargetPlatform.PC)
        {
            BackgroundTab3.SetActive(true);
            BackgroundTab1.SetActive(false);
            BackgroundTab2.SetActive(false);
            //MenuSettingsX.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
            SoundManager.instance.PlaySound(12);
        }
    }
    public void OnChangeValueSoundSlider()
    {
        AudioVolumeSoundsGeneral = valueSlider.value;

        SoundValueOnUISettings.text = Mathf.FloorToInt(valueSlider.value * 100).ToString();

        for (int i = 0; i < SoundManager.instance.GameSoundsInGame.Count; i++)
        {
            SoundManager.instance.GameSoundsInGame[i].volume = AudioVolumeSoundsGeneral;
        }
        ControlMenu.instance.objectData.SoundValue = AudioVolumeSoundsGeneral;
        SaveManager.SaveData();
    }
    public void OnChangeValueMusicSlider()
    {
        AudioVolumeMusic = valueSliderMusic.value;

        SoundValueMusicOnUISettings.text = Mathf.FloorToInt(valueSliderMusic.value * 100).ToString();


        SoundManager.instance.MainMusicX.volume = AudioVolumeMusic;

        ControlMenu.instance.objectData.SoundMusicValue = AudioVolumeMusic;
        SaveManager.SaveData();
    }

    public void OnChangeValueCheckCanPlayGameSounds()
    {
        if (toggleCanPlaySounds.isOn)
        {
            SoundManager.instance.canPlayGameSounds = true;
            ControlMenu.instance.objectData.CanPlayGameSounds = true;

        }
        else if (!toggleCanPlaySounds.isOn)
        {
            SoundManager.instance.canPlayGameSounds = false;
            ControlMenu.instance.objectData.CanPlayGameSounds = false;
        }

        SaveManager.SaveData();

    }
    public void OnChangeValueCheckCanPlayGameMusic()
    {
        if (toggleCanPlayGameMusic.isOn)
        {
            SoundManager.instance.canPlayGameMusic = true;
            if (!SoundManager.instance.MainMusicX.isPlaying)
            {
                SoundManager.instance.MainMusicX.Play();
            }
            ControlMenu.instance.objectData.CanPlayGameMusic = true;
        }
        else if (!toggleCanPlayGameMusic.isOn)
        {
            SoundManager.instance.canPlayGameMusic = false;
            SoundManager.instance.MainMusicX.Stop();
            ControlMenu.instance.objectData.CanPlayGameMusic = false;
        }
        SaveManager.SaveData();
    }
}
