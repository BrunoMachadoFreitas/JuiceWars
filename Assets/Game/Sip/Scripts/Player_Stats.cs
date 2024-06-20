using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : MonoBehaviour
{
    public static Player_Stats instance;

    public float Power = 1f;
    public float LifeSteal = 0;
    public float PercentStealLife = 90;
    public float Life = 0f; // Vida total do personagem
    public float CurrentLife = 0f; // Vida atual do personagem
    public float moveSpeed = 0f;
    public float RealoadTime = 0f;
    public float Range = 0f;
    public float PlayerDodge = 0f;
    public float DropMoneyRate = 50f;
    public float VelocityMiniGun = 1f;
    public float maxBulletsMiniGun = 20f;
    public float VelocityShootGun = 1f;
    public float VelocityBoomerang = 1f;
    public float VelocityPistol = 1f;
    public float VelocityCactus = 1f;
    public float VelocityClub = 1f;
    public float spawnRate = 1f;
    public float LuckyValue = 10f;
    public float MagnetismFactor = 5f;
    
    public bool HasLuck = false;
   
    public float ExpToGive = 2;
    public float CurrentSpeed;
    public float DashSpeed;
    public float DashRechargeTime;
    public bool InDash = false;

    public bool HasWhip = false;

    // Sound
    public float GeneralVolume = 0.5f;


    [SerializeField]
    private GameObject moneyToDrop;


    [SerializeField] private GameObject CanvasSettings;
    public List<CardType> CardsActive = new List<CardType>();
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
            
        }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        GameStateController.instance.lastGameState = GameStateController.instance.currentGameState;
        GameStateController.instance.currentGameState = GameState.Paused;
        ControlMenu.instance.MenuSettingsX.SetActive(true);
    }

    public void ApplyDamage(Collider2D ToSend)
    {
        ToSend.SendMessage("TakeDamageMonster", Power);
        // Verifica se o dinheiro deve ser spawnado
        float randomValueLifeSteal = Random.Range(0f, 100f);
        if (randomValueLifeSteal < PercentStealLife)
        {
            if(CurrentLife < Life)
            {
                CurrentLife += Power;
            }
            
        }
     }
    public void ApplyDamageFromWhip(Collider2D ToSend)
    {
        ToSend.SendMessage("TakeDamageMonster", 6f);
        // Verifica se o dinheiro deve ser spawnado
        float randomValueLifeSteal = Random.Range(0f, 100f);
        if (randomValueLifeSteal < PercentStealLife)
        {
            if (CurrentLife < Life)
            {
                CurrentLife += Power;
            }

        }
    }
    public void DropMoneyCalculator(Transform MonsterTransformToDrop)
    {
        // Verifica se o dinheiro deve ser spawnado
        float randomValue = Random.Range(0f, 100f);
        if (randomValue < DropMoneyRate)
        {
            GameObject moneydrop = Instantiate(moneyToDrop, new Vector3(MonsterTransformToDrop.position.x, MonsterTransformToDrop.position.y, MonsterTransformToDrop.position.z), Quaternion.identity);
            WaveManager.instance.MoneyCollect.Add(moneydrop);


        }
    }

    public void ExpGiveCalculator()
    {
       
         Leveling.instance.currentExp += ExpToGive;
    }

    public void IncreaseStatsByLevel()
    {
        Power += Power * 0.2f;
        //LifeSteal = 0;
        //PercentStealLife = 90;
        Life += Life * 0.2f; // Vida total do personagem
        moveSpeed += moveSpeed * 0.02f;
        //RealoadTime = 0f;
        //Range = 0f;
        //PlayerDodge = 0f;
        DropMoneyRate = DropMoneyRate * 0.5f;
        //VelocityMiniGun = 1f;
        //BoomerangSpeedBack = 1f;
        //spawnRate = 1f;
    }
}
