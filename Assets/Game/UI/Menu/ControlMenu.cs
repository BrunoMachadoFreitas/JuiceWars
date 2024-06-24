using System.Collections;
using Game.GamePlay.Scripts;
using Game.SaveManager;
using Game.Sounds.SoundScripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlMenu : MonoBehaviour
{
    public static ControlMenu instance;

    [SerializeField] MainMenu MainMenu;
    public SettingsPopUp MenuSettings;
    


    [SerializeField] private SoundManager soundManager;
    [HideInInspector] public SoundManager soundManagerX;


    

    public DataInfo objectData = new DataInfo();


    public SettingsPopUp SettingsMenu;


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
        objectData = SaveManager.LoadData();
        SaveManager.objectData = objectData;
    }
    // Start is called before the first frame update
    void Start()
    {


        soundManagerX = Instantiate(soundManager, new Vector3(-487.442993f, 1.04410005f, 0), Quaternion.identity);

        soundManagerX.MainMusic.volume = objectData.SoundMusicValue;

        if (DeviceController.instance.TargetPlatform != TargetPlatform.PC)
        {
            //MenuSettingsX.transform.GetChild(1).GetChild(7).gameObject.SetActive(false);
        }
     }

    // Update is called once per frame
    void Update()
    {
        
    }

  

   
    
    public void HideSettingsMenuShowMainMenu()
    {
        if (GameStateController.instance) { 
            if(GameStateController.instance.lastGameState == GameState.Playing)
            {
                Player_Main.instance.transform.GetChild(0).gameObject.SetActive(true);
                MenuSettings.gameObject.SetActive(false);
                GameStateController.instance.lastGameState = GameState.Paused;
                GameStateController.instance.currentGameState = GameState.Playing;

                GameStateController.instance.hasTriggeredPauseEvent = true;
            }
        }
        else
        {
            MenuSettings.gameObject.SetActive(false);
            MainMenu.gameObject.SetActive(true);
        }
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
            //SceneManager.MoveGameObjectToScene(GameSound, scene);

            // Desregistra o callback para evitar chamadas repetidas
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SoundManager.instance.PlaySound(12);
        }

    }


   
    
}
