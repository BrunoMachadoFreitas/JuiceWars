using System.Collections;
using Game.SaveManager;
using Game.Sounds.SoundScripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlMenu : MonoBehaviour
{
    public static ControlMenu instance;

    [SerializeField] Canvas canvasMenu;


    [SerializeField] public GameObject MainMenu;
    public GameObject MainMenuX;


    [SerializeField] public GameObject MenuSettings;
    public GameObject MenuSettingsX;

    [SerializeField] public GameObject MenuAchievements;
    public GameObject MenuAchievementsX;

    public GameObject BackgroundTab1;
    public GameObject BackgroundTab2;
    public GameObject BackgroundTab3;


    [SerializeField] private GameObject GameSound;
    [SerializeField] private GameObject GameSoundX;
    

    //SOUND
    public float AudioVolumeSoundsGeneral;
    public float AudioVolumeMusic;

    [SerializeField] private Slider valueSlider;
    [SerializeField] private Slider valueSliderMusic;

    [SerializeField] private TextMeshProUGUI SoundValueOnUISettings;
    [SerializeField] private TextMeshProUGUI SoundValueMusicOnUISettings;


    [SerializeField] private SoundManager soundManager;
    public SoundManager soundManagerX;


    [SerializeField] private Toggle toggleCanPlaySounds;
    [SerializeField] private Toggle toggleCanPlayGameMusic;

    public DataInfo objectData = new DataInfo();


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
        DontDestroyOnLoad(gameObject);
        // objectData = SaveManager.instance.LoadData();
        // SaveManager.instance.objectData = objectData;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        MainMenuX = Instantiate(MainMenu);
        //GameSoundX = Instantiate(GameSound);
        MenuSettingsX = Instantiate(MenuSettings);
        MenuAchievementsX = Instantiate(MenuAchievements);
        MenuSettingsX.SetActive(false);
        MenuAchievementsX.SetActive(false);


        soundManagerX = Instantiate(soundManager, new Vector3(-487.442993f, 1.04410005f, 0), Quaternion.identity);
        
        BackgroundTab1 = MenuSettingsX.GetComponent<Transform>().GetChild(1).GetChild(2).gameObject;
        BackgroundTab2 = MenuSettingsX.GetComponent<Transform>().GetChild(1).GetChild(4).gameObject;
        BackgroundTab3 = MenuSettingsX.GetComponent<Transform>().GetChild(1).GetChild(6).gameObject;

        MainMenuX.transform.GetChild(7).GetComponent<Button>().onClick.AddListener(delegate () { startGame(); });
        MainMenuX.transform.GetChild(8).GetComponent<Button>().onClick.AddListener(delegate () { HideMainMenuShowSettings(); });
        MainMenuX.transform.GetChild(9).GetComponent<Button>().onClick.AddListener(delegate () { HideMainMenuShowAchievements(); });


        MenuSettingsX.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(delegate () { HideSettingsMenuShowMainMenu(); });
        MenuSettingsX.transform.GetChild(1).GetChild(3).GetComponent<Button>().onClick.AddListener(delegate () { OpenTab1HideOthers(); });
        MenuSettingsX.transform.GetChild(1).GetChild(5).GetComponent<Button>().onClick.AddListener(delegate () { OpenTab2HideOthers(); });
        MenuSettingsX.transform.GetChild(1).GetChild(7).GetComponent<Button>().onClick.AddListener(delegate () { OpenTab3HideOthers(); });

        valueSlider = MenuSettingsX.transform.GetChild(1).GetChild(2).GetChild(1).GetChild(0).GetComponent<Slider>();
        valueSliderMusic = MenuSettingsX.transform.GetChild(1).GetChild(2).GetChild(2).GetChild(0).GetComponent<Slider>();
        toggleCanPlaySounds = MenuSettingsX.transform.GetChild(1).GetChild(2).GetChild(3).GetChild(0).GetComponent<Toggle>();
        toggleCanPlayGameMusic = MenuSettingsX.transform.GetChild(1).GetChild(2).GetChild(4).GetChild(0).GetComponent<Toggle>();

        toggleCanPlaySounds.onValueChanged.AddListener(delegate { OnChangeValueCheckCanPlayGameSounds(); });
        toggleCanPlayGameMusic.onValueChanged.AddListener(delegate { OnChangeValueCheckCanPlayGameMusic(); });

        SoundValueOnUISettings = MenuSettingsX.transform.GetChild(1).GetChild(2).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        SoundValueMusicOnUISettings = MenuSettingsX.transform.GetChild(1).GetChild(2).GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();

        valueSlider.onValueChanged.AddListener(delegate { OnChangeValueSoundSlider(); });
        valueSliderMusic.onValueChanged.AddListener(delegate { OnChangeValueMusicSlider(); });

        valueSlider.value = objectData.SoundValue;
        valueSliderMusic.value = objectData.SoundMusicValue;

        toggleCanPlaySounds.isOn = objectData.CanPlayGameSounds;
        toggleCanPlayGameMusic.isOn = objectData.CanPlayGameMusic;

        SoundManager.instance.MainMusicX.volume = valueSliderMusic.value;
        for (int i = 0; i < SoundManager.instance.GameSoundsInGame.Count; i++)
        {
            SoundManager.instance.GameSoundsInGame[i].volume = valueSlider.value;
        }

        if (DeviceController.instance.TargetPlatform != TargetPlatform.PC)
        {
            MenuSettingsX.transform.GetChild(1).GetChild(7).gameObject.SetActive(false);
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
        if (DeviceController.instance.TargetPlatform == TargetPlatform.PC) { 
            BackgroundTab3.SetActive(true);
            BackgroundTab1.SetActive(false);
            BackgroundTab2.SetActive(false);
            MenuSettingsX.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
            SoundManager.instance.PlaySound(12);
        }
    }

    public void HideMainMenuShowSettings()
    {
        MenuSettingsX.SetActive(true);
        MainMenuX.SetActive(false);
        SoundManager.instance.PlaySound(12);
    }
    public void HideMainMenuShowAchievements()
    {
        MenuAchievementsX.SetActive(true);
        MainMenuX.SetActive(false);
        SoundManager.instance.PlaySound(12);
    }
    public void HideSettingsMenuShowMainMenu()
    {
        if (GameStateController.instance) { 
            if(GameStateController.instance.lastGameState == GameState.Playing)
            {
                MenuSettingsX.SetActive(false);
                GameStateController.instance.lastGameState = GameState.Paused;
                GameStateController.instance.currentGameState = GameState.Playing;

                GameStateController.instance.hasTriggeredPauseEvent = true;
            }
        }
        else
        {
            MenuSettingsX.SetActive(false);
            MainMenuX.SetActive(true);
            //MainMenu.SetActive(true);
        }
        SoundManager.instance.PlaySound(12);
    }

    public void startGame()
    {
        // Registra o callback para quando a cena for carregada
        //SceneManager.sceneLoaded += OnSceneLoaded;
        if (SoundManager.instance.MainMusicX.isPlaying)
        {
         
        }

        


        //Carrega a cena pelo �ndice(ou pelo nome)
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        //SceneManager.SetActiveScene(sceneJuice);

        MainMenuX.gameObject.SetActive(false);
        SoundManager.instance.PlaySound(12);

        
    }
    private IEnumerator WaitSoundEnd()
    {
       
            yield return new WaitForSeconds(10f);
       

    }
    // Callback que � chamado quando a cena � carregada
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Verifica se a cena carregada � a que estamos esperando
        if (scene.buildIndex == 1)
        {
            // Mover o objeto GameSound para a nova cena
            SceneManager.MoveGameObjectToScene(GameSound, scene);

            // Desregistra o callback para evitar chamadas repetidas
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SoundManager.instance.PlaySound(12);
        }

    }


    public void OnChangeValueSoundSlider()
    {
        AudioVolumeSoundsGeneral = valueSlider.value;

        SoundValueOnUISettings.text = Mathf.FloorToInt(valueSlider.value * 100).ToString();

        for(int i = 0; i < SoundManager.instance.GameSoundsInGame.Count; i++)
        {
            SoundManager.instance.GameSoundsInGame[i].volume = AudioVolumeSoundsGeneral;
        }
        objectData.SoundValue = AudioVolumeSoundsGeneral;
        SaveManager.SaveData();
    }
    public void OnChangeValueMusicSlider()
    {
        AudioVolumeMusic = valueSliderMusic.value;

        SoundValueMusicOnUISettings.text = Mathf.FloorToInt(valueSliderMusic.value * 100).ToString();

        
        SoundManager.instance.MainMusicX.volume = AudioVolumeMusic;

        objectData.SoundMusicValue = AudioVolumeMusic;
        SaveManager.SaveData();
    }

    public void OnChangeValueCheckCanPlayGameSounds()
    {
        if (toggleCanPlaySounds.isOn)
        {
            SoundManager.instance.canPlayGameSounds = true;
            objectData.CanPlayGameSounds = true;
            
        }
        else if (!toggleCanPlaySounds.isOn)
        {
            SoundManager.instance.canPlayGameSounds = false;
            objectData.CanPlayGameSounds = false;
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
            objectData.CanPlayGameMusic = true;
        }
        else if (!toggleCanPlayGameMusic.isOn)
        {
            SoundManager.instance.canPlayGameMusic = false;
            SoundManager.instance.MainMusicX.Stop();
            objectData.CanPlayGameMusic = false;
        }
        SaveManager.SaveData();
    }
    
}
