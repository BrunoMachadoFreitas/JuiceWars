using System.Collections;
using System.Collections.Generic;
using Game.Sounds.SoundScripts;
using TMPro;
using UnityEngine;

public class Leveling : MonoBehaviour
{
    public static Leveling instance;

    public delegate void LvlChangedHandler();
    public event LvlChangedHandler OnLvlChanged;

    public int currentLvl = 1;
    public int NextLevel = 2;

    public float NextLvlExpNeed = 10;
    public float currentExp = 0;


    public bool CanDo = false;

    [SerializeField] private UnityEngine.UI.Image ExpBar;
    [SerializeField] private TextMeshProUGUI TextExp;
    [SerializeField] private TextMeshProUGUI TextLvl;
    [SerializeField] private float NextLvlMultiplier = .5f;

    [SerializeField] private GameObject CanvasPowerUps;

    private List<AudioSource> AudiosInScene = new List<AudioSource>();
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

        ExpBar = Player_Main.instance.CanvasExp.gameObject.transform.GetChild(1).GetComponent<UnityEngine.UI.Image>();
     
        OnLvlChanged += OnLvlChangedUp;
    }

    // Update is called once per frame
    void Update()
    {
        ExpBar.fillAmount = (float)currentExp / NextLvlExpNeed;
        //TextExp.text = currentExp.ToString();
        TextLvl.text = currentLvl.ToString();
        if (currentExp != NextLvlExpNeed)
        {
            CanDo = true;
        }
        if (CanDo) { 
            if (currentExp >= NextLvlExpNeed)
            {
                OnLvlChanged?.Invoke();
                CanDo = false;
            }
        }
    }
    public AudioSource sound;
    private void OnLvlChangedUp()
    {
        
        currentLvl++;
        NextLevel++;
        NextLvlExpNeed *= NextLvlMultiplier;
        currentExp = 0;
        



        RoundsManager.instance.LevelingChoose();
        SoundManager.instance.PlaySound(2);

        //Player_Stats.instance.IncreaseStatsByLevel();
        AudiosInScene.Add(sound);
        GameStateController.instance.currentGameState = GameState.Paused;
        Player_Main.instance.CanvasExp.gameObject.SetActive(false);
    }
}
