using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    public bool isWaveStopped = false;

    public float waveDuration = 10f;

    public GameObject EnemyPrefab;
    public GameObject playingArea;
    public GameObject imagetoSpawn;
    public GameObject Timerobj;
    public TextMeshProUGUI waveTimerText;
    public GameObject JuiceShieldPrefab;

    List<GameObject> ImageSpawned = new List<GameObject>();
    public List<GameObject> Monsters = new List<GameObject>();
    public List<GameObject> MoneyCollect = new List<GameObject>();
    public List<GameObject> Images = new List<GameObject>();
    public List<GameObject> ImagesWaiter = new List<GameObject>();

    [SerializeField] RectTransform canvasTransform;
    public float moveDuration = 1f;
    public bool isSpawning = false;

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

    // Variáveis de controle de dificuldade
    private int enemiesToSpawn = 1; // Quantidade inicial de inimigos a serem spawnados
    private int maxEnemiesToSpawn = 5; // Limite máximo de inimigos a serem spawnados por intervalo
    private int maxEnemiesOnScreen = 20; // Limite máximo de inimigos na tela

    // Variáveis de controle de bosses
    private int enemiesKilled = 0; // Contador de inimigos mortos
    private int bossSpawnThreshold = 100; // Número de inimigos mortos para spawnar um boss

    // Variáveis para inimigos especiais
    private float specialEnemyChance = 0.1f; // 10% de chance de spawnar um inimigo especial
    private int specialEnemyHealth = 30;

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

        StartCoroutine(SpawnJuiceShieldRoutine());
        StartCoroutine(IncreaseDifficultyOverTime()); // Inicia a corrotina para aumentar a dificuldade
    }

    void Update()
    {
        DestroyDistantEnemies();
    }

    public void HandleOnTimerSec()
    {
        if (Monsters.Count < maxEnemiesOnScreen)
        {
            SpawnEnemies();
        }
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            if (Monsters.Count >= maxEnemiesOnScreen)
                break;

            if (UnityEngine.Random.value < 0.2f) // 20% chance de spawnar um grupo de inimigos
            {
                SpawnEnemyGroup(5); // Spawna um grupo de 5 inimigos
            }
            else
            {
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        Vector3 position = GenerateRandomPosition();
        position += Player_Main.instance.transform.position;

        int randomValue = UnityEngine.Random.Range(0, monsterToSpawn.Count);
        GameObject newEnemy = Instantiate(monsterToSpawn[randomValue]);
        newEnemy.transform.position = position;

        // Verifica se deve spawnar um inimigo especial
        if (UnityEngine.Random.value < specialEnemyChance)
        {
            Monster enemyScript = newEnemy.GetComponent<Monster>();
            if (enemyScript != null)
            {
                enemyScript.life = (specialEnemyHealth); // Define a vida do inimigo especial
            }

            // Adiciona um indicador visual para distinguir o inimigo especial
            SpriteRenderer spriteRenderer = newEnemy.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.green; // Altere a cor para verde para indicar o inimigo especial
            }
        }

        Monsters.Add(newEnemy);
    }

    private void SpawnEnemyGroup(int groupSize)
    {
        for (int i = 0; i < groupSize; i++)
        {
            if (Monsters.Count >= maxEnemiesOnScreen)
                break;
            SpawnEnemy();
        }
    }

    private void SpawnBoss()
    {
        Vector3 position = GenerateRandomPosition();
        position += Player_Main.instance.transform.position;

        int randomValue = UnityEngine.Random.Range(0, monsterToSpawn.Count);

        GameObject newBoss = Instantiate(monsterToSpawn[randomValue]);
        newBoss.transform.position = position;

        // Change the color of the sprite to red to distinguish the boss
        SpriteRenderer spriteRenderer = newBoss.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
        }

        Monsters.Add(newBoss);
    }

    [SerializeField] Vector2 spawnArea;
    [SerializeField] float minSpawnDistance = 15f; // Distância mínima para spawnar inimigos
    [SerializeField] float maxSpawnDistance = 25f; // Distância máxima para spawnar inimigos

    private Vector3 GenerateRandomPosition()
    {
        Vector3 position;
        float distance = UnityEngine.Random.Range(minSpawnDistance, maxSpawnDistance);
        float angle = UnityEngine.Random.Range(0, Mathf.PI * 2);

        position.x = Mathf.Cos(angle) * distance;
        position.y = Mathf.Sin(angle) * distance;
        position.z = 0;

        return position;
    }

    private void SpawnJuiceShield()
    {
        if (Player_Main.instance != null)
        {
            Vector3 position = GenerateRandomPosition();
            position += Player_Main.instance.transform.position;

            GameObject newJuiceShield = Instantiate(JuiceShieldPrefab);
            newJuiceShield.transform.position = position;
        }
    }

    private IEnumerator SpawnJuiceShieldRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            SpawnJuiceShield();
        }
    }

    private IEnumerator IncreaseDifficultyOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f); // Aumenta a dificuldade a cada 30 segundos

            if (enemiesToSpawn < maxEnemiesToSpawn)
            {
                enemiesToSpawn++; // Incrementa a quantidade de inimigos a serem spawnados
            }
        }
    }

    private void DestroyDistantEnemies()
    {
        List<GameObject> enemiesToRemove = new List<GameObject>();

        foreach (GameObject enemy in Monsters)
        {
            if (Vector3.Distance(Player_Main.instance.transform.position, enemy.transform.position) > 100f)
            {
                enemiesToRemove.Add(enemy);
            }
        }

        foreach (GameObject enemy in enemiesToRemove)
        {
            Monsters.Remove(enemy);
            Destroy(enemy);
        }
    }

    public void OnEnemyKilled()
    {
        enemiesKilled++;
        if (enemiesKilled >= bossSpawnThreshold)
        {
            enemiesKilled = 0;
            SpawnBoss();
        }
    }
}
