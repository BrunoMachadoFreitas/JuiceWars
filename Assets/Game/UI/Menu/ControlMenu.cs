using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private TextMeshProUGUI SoundValueOnUISettings;

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
    }
    // Start is called before the first frame update
    void Start()
    {
        objectData = DataManagment.instance.LoadData();
        MainMenuX = Instantiate(MainMenu);
        //GameSoundX = Instantiate(GameSound);
        MenuSettingsX = Instantiate(MenuSettings);
        MenuAchievementsX = Instantiate(MenuAchievements);
        MenuSettingsX.SetActive(false);
        MenuAchievementsX.SetActive(false);



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
        SoundValueOnUISettings = MenuSettingsX.transform.GetChild(1).GetChild(2).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        valueSlider.onValueChanged.AddListener(delegate { OnChangeValueSoundSlider(); });
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
    }

    public void OpenTab2HideOthers()
    {
        BackgroundTab2.SetActive(true);
        BackgroundTab1.SetActive(false);
        BackgroundTab3.SetActive(false);
    }

    public void OpenTab3HideOthers()
    {
        BackgroundTab3.SetActive(true);
        BackgroundTab1.SetActive(false);
        BackgroundTab2.SetActive(false);
        MenuSettingsX.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
    }

    public void HideMainMenuShowSettings()
    {
        MenuSettingsX.SetActive(true);
        MainMenuX.SetActive(false);
    }
    public void HideMainMenuShowAchievements()
    {
        MenuAchievementsX.SetActive(true);
        MainMenuX.SetActive(false);
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
        
    }

    public void startGame()
    {
        // Registra o callback para quando a cena for carregada
        //SceneManager.sceneLoaded += OnSceneLoaded;

        // Carrega a cena pelo índice (ou pelo nome)
        SceneManager.LoadScene(1, LoadSceneMode.Single);

        //SceneManager.SetActiveScene(sceneJuice);

        MainMenuX.gameObject.SetActive(false);
    }

    // Callback que é chamado quando a cena é carregada
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Verifica se a cena carregada é a que estamos esperando
        if (scene.buildIndex == 1)
        {
            // Mover o objeto GameSound para a nova cena
            SceneManager.MoveGameObjectToScene(GameSound, scene);

            // Desregistra o callback para evitar chamadas repetidas
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }


    public void OnChangeValueSoundSlider()
    {
        AudioVolumeSoundsGeneral = valueSlider.value;

        SoundValueOnUISettings.text = Mathf.FloorToInt(valueSlider.value * 100).ToString();

        DataManagment.instance.objectData.SoundValue = AudioVolumeSoundsGeneral;
        DataManagment.instance.SaveData();
    }

    // Posição inicial das cartas
    public Vector3 initialPosition;
    // Referência ao GameObject UI Image
    public GameObject cards;

    // Função que inicia o movimento das cartas para cima
    public void CardsUp()
    {
        // Inicia a coroutine que move as cartas para cima
        StartCoroutine(MoveCardsUp());
    }

    // Coroutine que espera 5 segundos e move as cartas para cima
    private IEnumerator MoveCardsUp()
    {
        // Espera 5 segundos
        yield return new WaitForSeconds(5f);

        // Pega a posição atual das cartas
        Vector3 currentPosition = cards.transform.position;

        // Define a nova posição para onde as cartas irão se mover (aumentando o valor de Y)
        Vector3 newPosition = new Vector3(currentPosition.x, currentPosition.y + 120, currentPosition.z);

        // Move as cartas para a nova posição
        cards.transform.position = newPosition;
        Player_Main.instance.transform.GetChild(0).GetChild(11).gameObject.SetActive(true);
        Player_Main.instance.transform.GetChild(0).GetChild(11).GetComponent<Button>().onClick.AddListener(delegate () {CardsDown(); });
    }

    // Função que move as cartas de volta para a posição inicial
    public void CardsDown()
    {
        // Inicia a coroutine que move as cartas de volta para a posição inicial
        StartCoroutine(MoveCardsDown());
    }

    // Coroutine que move as cartas de volta para a posição inicial
    private IEnumerator MoveCardsDown()
    {
        // Aguarda 5 segundos
        yield return new WaitForSeconds(5f);
        Player_Main.instance.transform.GetChild(0).GetChild(11).gameObject.SetActive(false);
        // Move as cartas para a posição inicial
        cards.transform.position = initialPosition;
        // Aguarda 5 segundos
        yield return new WaitForSeconds(5f);

        // Chama CardsUp para mover as cartas para cima novamente
        CardsUp();
    }
}
