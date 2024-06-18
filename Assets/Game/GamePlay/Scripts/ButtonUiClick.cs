using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class ButtonUiClick : MonoBehaviour
{

    public GameObject ChooseCharacterUi;
    public GameObject RoundsManager;
    public GameObject TimerObj;
    public GameObject CanvasInGame;

    private float Life = 0;

    private float Speed = 0;
    private float ReloadTime = 0;
    private float Range = 0;
    private float Power = 0;
    private float LifeSteal = 0;
    private float PercentStealLife = 0;


    [SerializeField] private CheckDevice deviceCheck;
    [SerializeField] private GameObject CanvasMobile;
    [SerializeField] private GameObject GameStateObject;
    private GameObject GameStateObjectAux;

    [SerializeField] private Player_Main player;
    private Player_Main playerInstance;
    [SerializeField] private Canvas CanvasGameli;
    int CharacterId = 0;

    public Button ButtonBegin;

    [SerializeField] private GameObject BackgroundStats;


    [SerializeField] private TextMeshProUGUI txtLife;
    [SerializeField] private List<RuntimeAnimatorController> PlayerAnims;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ChooseCharacter()
    {
        if (!BackgroundStats.active)
        {
            BackgroundStats.SetActive(true);
        }
        CharacterId = 0;
        Life = 20f;
        Speed = 7f;
        ReloadTime = 2f;
        Range = 8f;
        Power = 1f;
        LifeSteal = 3;
        LifeSteal = PercentStealLife;

        txtLife.text = Life.ToString();
        ButtonBegin.interactable = true;
    }
    public void ChooseCharacter2()
    {
        if (!BackgroundStats.active)
        {
            BackgroundStats.SetActive(true);
        }
        CharacterId = 2;
        Life = 30f;
        Speed = 3f;
        ReloadTime = 1f;
        Range = 4f;
        Power = 1f;
        LifeSteal = 0;
        LifeSteal = PercentStealLife;

        txtLife.text = Life.ToString();
        ButtonBegin.interactable = true;
    }
    public void BeginGame()
    {
        try
        {

        
            if (CharacterId == 0)
            {
            playerInstance = Instantiate(player, new Vector3(-488.380005f, 0.0943644196f, 0f), Quaternion.identity);
            GameStateObjectAux = Instantiate(GameStateObject, new Vector3(-488.380005f, 0.0943644196f, 0f), Quaternion.identity);
            playerInstance.AnimPlayer.runtimeAnimatorController = PlayerAnims[CharacterId];

            }
            playerInstance.gameObject.SetActive(true);
            CanvasGameli.gameObject.SetActive(true);
            playerInstance.movementButtonRect = CanvasMobile.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
            
            playerInstance.GetComponent<CircleCollider2D>().radius = Range;


            Player_Stats.instance.Life = Life;
            Player_Stats.instance.CurrentLife = Life;
            Player_Stats.instance.moveSpeed = Speed;
            Player_Stats.instance.RealoadTime = ReloadTime;
            Player_Stats.instance.Range = Range;
            Player_Stats.instance.Power = Power;
            Player_Stats.instance.LifeSteal = LifeSteal;
            Player_Stats.instance.PercentStealLife = PercentStealLife;
           
            GameStateController.instance.currentGameState = GameState.Playing;
            
            RoundsManager.SetActive(true);
            Timer.instance.gameObject.SetActive(true);
            Timer.instance.ResetTimer(0f);
            
            //CanvasInGame.SetActive(true);

            ChooseCharacterUi.SetActive(false);

            //ControlMenu.instance.cards = playerInstance.transform.GetChild(0).GetChild(10).gameObject;
            //ControlMenu.instance.CardsUp();
            //ControlMenu.instance.initialPosition = playerInstance.transform.GetChild(0).GetChild(10).transform.position;
            //playerInstance.transform.GetChild(0).GetChild(11).GetComponent<Button>().onClick.AddListener(delegate () { ControlMenu.instance.CardsDown(); });
            if (deviceCheck.isTouchInterface)
            {
                CanvasMobile.SetActive(true);
                playerInstance.TargetPlatform = TargetPlatform.Android;
                CanvasMobile.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate () { Player_Stats.instance.PauseGame(); });
                
            }
            else
            {
                CanvasMobile.SetActive(false);
                playerInstance.TargetPlatform = TargetPlatform.PC;
            }
        }catch(Exception ex)
        {

        }
    }
}
