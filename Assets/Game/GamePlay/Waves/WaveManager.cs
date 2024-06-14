using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Xml.Linq;
using UnityEngine.U2D;
using UnityEngine.UI;
using System;
using System.Linq;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    public bool isWaveStopped = false;

    public delegate void MoneyEndHandler();
    //public event MoneyEndHandler OnMoneyGiven;

    public delegate void TimerSecHandler();
    //public event MoneyEndHandler OnTimerSec;

    public delegate void WaveManagerReadyHandler();
    //public event WaveManagerReadyHandler WaveManagerReady;

    public float waveDuration = 10f;

    // Prefabs e referências
    public GameObject EnemyPrefab;
    public GameObject playingArea;
    public GameObject imagetoSpawn;
    public GameObject Timerobj;
    public TextMeshProUGUI waveTimerText; // Para mostrar o tempo restante da onda
    public GameObject JuiceShieldPrefab; // Referência ao prefab do JuiceShield

    List<GameObject> ImageSpawned = new List<GameObject>();
    public List<GameObject> Monsters = new List<GameObject>();
    public List<GameObject> MoneyCollect = new List<GameObject>();
    public List<GameObject> Images = new List<GameObject>();
    public List<GameObject> ImagesWaiter = new List<GameObject>();

    [SerializeField] RectTransform canvasTransform;
    // Posição do dinheiro do jogador no canvas
    public float moveDuration = 1f; // Duração do movimento
    public bool isSpawning = false;

    //[SerializeField] int currentWave = 0;
    //[SerializeField] int currentWaveMidle = 0;
    public int currentRound = 1;

    [SerializeField] private List<GameObject> monsterToSpawn;
    float roundMultiplier = 1;

    public bool given = false;

    [SerializeField] private GameObject MoneyCollectedd;
    [SerializeField] private GameObject MoneyCollecteddAux;
    [SerializeField] private SpriteRenderer targetSprite;

    [SerializeField] private Camera Camera;

    public FadeObject FadeObjectX;
    public FadeObject FadeObjectY;
    public int MoobMultiplier = 1;
    public bool alreadyExists = false;

    List<GameObject> pickups;

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

    void Start()
    {
        playingArea = RoundsManager.instance.PlayingAreaAux;
        Timerobj.SetActive(true);

        Timer.instance.OnTimerSecond += HandleOnTimerSec;
        targetSprite = Player_Main.instance.CanvasExp.gameObject.transform.GetChild(11).GetComponent<SpriteRenderer>();
        Camera = Player_Main.instance.MainCamera;
        MoneyCollecteddAux = Instantiate(MoneyCollectedd);

        StartCoroutine(SpawnJuiceShieldRoutine()); // Inicia a corrotina para spawnar o JuiceShield
    }

    void Update()
    {
    }

    public void HandleOnTimerSec()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        Vector3 position = GenerateRandomPosition();
        position += Player_Main.instance.transform.position;

        int randomValue = UnityEngine.Random.Range(0, monsterToSpawn.Count);

        // Instancia o inimigo na posição aleatória
        GameObject newEnemy = Instantiate(monsterToSpawn[randomValue]);
        newEnemy.transform.position = position;
        Monsters.Add(newEnemy);
    }

    [SerializeField] Vector2 spawnArea;

    private Vector3 GenerateRandomPosition()
    {
        Vector3 position = new Vector3();

        float f = UnityEngine.Random.value > 0.5f ? -1f : 1f;
        if (UnityEngine.Random.value > 0.5f)
        {
            position.x = UnityEngine.Random.Range(-spawnArea.x, spawnArea.x);
        }
        else
        {
            position.y = UnityEngine.Random.Range(-spawnArea.y, spawnArea.y);
        }

        position.z = 0;
        return position;
    }

    // Método para spawnar o JuiceShield
    private void SpawnJuiceShield()
    {
        Vector3 position = GenerateRandomPosition();
        position += Player_Main.instance.transform.position;

        GameObject newJuiceShield = Instantiate(JuiceShieldPrefab);
        newJuiceShield.transform.position = position;
    }

    // Corrotina que spawna o JuiceShield a cada 5 segundos
    private IEnumerator SpawnJuiceShieldRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            SpawnJuiceShield();
        }
    }
}
