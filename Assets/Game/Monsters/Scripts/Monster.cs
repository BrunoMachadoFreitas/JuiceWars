using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

public class Monster : MonoBehaviour
{
    private bool isDamaged = false;
    
    private Rigidbody2D rb;

    [SerializeField] private Transform PopUpTransform;
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private List<SpriteRenderer> spriteRenderer = new List<SpriteRenderer>();
    [SerializeField] private float life = 6f;

    //see the distance between monster and the player
    private float closestDistance = 0.6f;
    [SerializeField] CircleCollider2D ColliderDamage;
    private bool colliderEnabled = true;

    private bool collidingPlayer = false;
    public bool isBeingAttacked = false;

    [SerializeField] private ParticleSystem particle;


    [SerializeField] private GameObject spriteFlash;

    [SerializeField] private Animator AnimFlash;
    //[SerializeField] private float hitWaitTime = 0.5f;
    [SerializeField] private float hitCounter;
    public float PlayerCriticalChance = 10f;
    public float PlayerInitialCritalChance = 0f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        AnimFlash = GetComponent<Animator>();
        PlayerInitialCritalChance = PlayerCriticalChance;

        particle = transform.GetChild(2).GetComponent<ParticleSystem>();
    }
    private void ToggleCollider()
    {
        // Verifica a distância entre o jogador e o monstro
        if (Player_Main.instance) { 
            float distanceToPlayer = Vector2.Distance(this.transform.position, Player_Main.instance.transform.position);
            if (distanceToPlayer < closestDistance)
            {
                colliderEnabled = !colliderEnabled; // Inverte o estado do collider
                ColliderDamage.enabled = colliderEnabled; // Ativa ou desativa o collider conforme o estado
                Player_Main.instance.SendMessage("TakeDamage");
                StartCoroutine(EsperaParaDarDano());
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
      
    }
    void FixedUpdate()
    {
      
    }
   
    
    // Chamado quando o player atira uma bala
    public float CriticalChanceCalculation()
    {
        // Calcula um número aleatório entre 0 e 100
        int randomValue = Random.Range(0, 100);
        if (randomValue <= PlayerCriticalChance)
        {
            return randomValue;
        }
        else
        {
            return 1f;
        }

    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Player_Main.instance.CurrentLife -= 1;
            // Inicia a repetição para ativar e desativar o collider
            InvokeRepeating("ToggleCollider", 2f, 2f);
            collidingPlayer = true;

           
        }
    
        else if (other.CompareTag("DangerZone"))
        {
            TakeDamageMonster(Player_Stats.instance.Power * 2);
        }
       

     
        else if (other.CompareTag("Turret"))
        {
            TakeDamageMonster(Player_Stats.instance.Power);
        }

        else if (other.CompareTag("Whip"))
        {
            // Calcula a direção de empurrão
            Vector2 pushDirection = (transform.position - other.transform.position).normalized;

            // Empurra o monstro para trás
            Rigidbody2D monsterRigidbody = GetComponent<Rigidbody2D>();
            if (monsterRigidbody != null)
            {
                float pushForce = 5f; // Força do empurrão
                monsterRigidbody.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
            }
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Dagger"))
        {
           collidingPlayer = false;
        }
        if (other.CompareTag("Bullet") || other.CompareTag("BulletMiniGun") || other.CompareTag("Dagger"))
        {

        }

        isDamaged = false;
    }

    private IEnumerator EsperaParaDarDano()
    {
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator WaitDeath()
    {
        yield return new WaitForSeconds(0.4f);
    }
    private IEnumerator SpriteFlash()
    {
        spriteFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
    }
    public void ApplyDamage(float damage)
    {
        TakeDamageMonster(damage);
    }

    [SerializeField] GameObject Exp;
     GameObject ExpObj;
    public void TakeDamageMonster(float damage)
    {
        life -= damage;
        isDamaged = true;

        if(AnimFlash != null)
        AnimFlash.SetTrigger("hit");

        Instantiate(floatingTextPrefab, PopUpTransform.position, Quaternion.identity);
        
        SoundManager.instance.PlaySound(7);


        floatingTextPrefab.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.FloorToInt(damage).ToString();


        if (life <= 0)
        {
            
            Player_Stats.instance.DropMoneyCalculator(this.transform);
            //Player_Stats.instance.ExpGiveCalculator();
            ExpObj = Instantiate(Exp, this.transform.position, Quaternion.identity);
            particle.Play();
            
            WaveManager.instance.Monsters.Remove(this.gameObject);
            StartCoroutine(WaitDeath());
            Player_Main.instance.EnemysKilled++;
            Destroy(this.gameObject);
        }
        else
        {
            
            isDamaged = false;
        }


    }

    public void TakeDamageMonsterBlackHole(float damage)
    {
        life -= damage;
        isDamaged = true;

        if (AnimFlash != null)
            AnimFlash.SetTrigger("hit");

        Instantiate(floatingTextPrefab, PopUpTransform.position, Quaternion.identity);

      


        floatingTextPrefab.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.FloorToInt(damage).ToString();


        if (life <= 0)
        {

            Player_Stats.instance.DropMoneyCalculator(this.transform);
            //Player_Stats.instance.ExpGiveCalculator();
            ExpObj = Instantiate(Exp, this.transform.position, Quaternion.identity);
            particle.Play();

            WaveManager.instance.Monsters.Remove(this.gameObject);
            StartCoroutine(WaitDeath());
            Player_Main.instance.EnemysKilled++;
            Destroy(this.gameObject);
        }
        else
        {

            isDamaged = false;
        }


    }




}
