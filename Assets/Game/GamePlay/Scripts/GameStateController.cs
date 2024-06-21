using System.Collections;
using System.Collections.Generic;
using Game.Sounds.SoundScripts;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum GameState
{
    Started,
    Loading,
    Playing,
    Paused,
    Completed,
    Ended
}
public class GameStateController : MonoBehaviour
{
    public static GameStateController instance;
    public GameState currentGameState;
    public GameState lastGameState;
    public Scene sceneGame;

    // Evento para carregar a cena "Menu"
    public event System.Action OnGameEnded;
    public event System.Action OnGamePaused;

    private bool hasTriggeredEndEvent = false;
    public bool hasTriggeredPauseEvent = false;



    [SerializeField] private GameObject CanvasEnd;
    [SerializeField] private GameObject CanvasSettings;

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

    // Start is called before the first frame update
    void Start()
    {
        currentGameState = GameState.Playing;
        sceneGame = SceneManager.GetActiveScene();

        // Inscreve o método LoadMenuScene no evento OnGameEnded
        OnGameEnded += LoadCanvasEnd;
        OnGamePaused += LoadCanvasGamePaused;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentGameState)
        {
            case GameState.Started: break;
            case GameState.Paused:
              
                Time.timeScale = 0f;

                
                break;
            case GameState.Playing: Time.timeScale = 1f; break;
            case GameState.Ended:
                hasTriggeredEndEvent = false;
                if (!hasTriggeredEndEvent)
                {
                    OnGameEnded?.Invoke();
                }
                break;
        }

        //// Verifica se o botão "Pause" foi pressionado
        //if (Input.GetKeyDown("Pause") && currentGameState != GameState.Paused)
        //{
        //    currentGameState = GameState.Paused;
        //}
    }

    // Função para carregar a cena "Menu"
    private void LoadCanvasEnd()
    {
        if (GameObject.Find("CanvasEnd(Clone)") == null)
        {
            Instantiate(CanvasEnd);
            CanvasEnd.SetActive(true);
            Destroy(Player_Main.instance.gameObject);
            for(int i = 0; i < WaveManager.instance.Monsters.Count; i++)
            {
                Destroy(WaveManager.instance.Monsters[i].gameObject);
            }
            if (SoundManager.instance.GameSoundsInGame[8].isPlaying)
            {
                SoundManager.instance.GameSoundsInGame[8].Stop();
            }
            hasTriggeredEndEvent = true;
        }
       
    }

    private void LoadCanvasGamePaused()
    {
        if (GameObject.Find("CanvasSettings(Clone)") == null && !hasTriggeredPauseEvent)
        {
            
            hasTriggeredPauseEvent = true;
        }

    }
    

    public void LoadSceneMenu()
    {
        
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        CanvasEnd.SetActive(false);
        ControlMenu.instance.MainMenuX.SetActive(true);
    }

    // Função para verificar se uma cena está carregada
    private bool IsSceneLoaded(string sceneName)
    {
        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();

            if (currentScene.name == sceneName)
            {
                return true;
            }
        
        return false;
    }
}


