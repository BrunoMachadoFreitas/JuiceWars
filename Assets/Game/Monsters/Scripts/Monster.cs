using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Game.Sounds.SoundScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;
public enum MonsterType
{
    Walker = 0,
    Ranger = 1
}
public class Monster : MonoBehaviour
{
    //Movement
    public GameObject player;

    public float moveSpeed;
    public float CurrentMoveSpeed;
    [SerializeField] private MonsterType currentMonsterType;
    private float randomMoveInterval = 5f; // Intervalo de tempo para mudança de direção aleatória
    private float nextRandomMoveTime = 0f;






    [SerializeField] bool CanFollowPlayer = true;
    [SerializeField] bool Attacking = false;


    [SerializeField] private BrainBullet_Main BrainBullet;

    [SerializeField] private List<BrainBullet_Main> BulletList = new List<BrainBullet_Main>();
    [SerializeField] private Transform BulletTransform;



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


    Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        AnimFlash = GetComponent<Animator>();
        PlayerInitialCritalChance = PlayerCriticalChance;

        particle = transform.GetChild(2).GetComponent<ParticleSystem>();

        if (Player_Main.instance)
            player = Player_Main.instance.gameObject;

        CurrentMoveSpeed = moveSpeed;
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
        if (currentMonsterType == MonsterType.Walker)
        {
            if (CanFollowPlayer)
                FollowPlayer();
        }
        if (currentMonsterType == MonsterType.Ranger)
        {
            if (CanFollowPlayer)
                FollowPlayerRanger();
        }

        // Verifica se é hora de realizar um movimento aleatório
        if (Time.time >= nextRandomMoveTime)
        {
            // Define um novo tempo para o próximo movimento aleatório
            nextRandomMoveTime = Time.time + randomMoveInterval;

            // Gera uma nova direção de movimento aleatória
            Vector2 randomDirection = Random.insideUnitCircle.normalized;

            // Atualiza a velocidade de movimento para a direção aleatória
            this.gameObject.GetComponent<Rigidbody2D>().velocity = randomDirection * moveSpeed;
        }
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
            Vector2 pushDirection = (transform.position - other.transform.position).normalized;

            Debug.Log("Push Direction: " + pushDirection);

            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                float pushForce = 20f;
                Debug.Log("Applying push force: " + pushForce);
                rb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
                Debug.Log("Monster velocity after push: " + rb.velocity);
                
            }
            else
            {
                Debug.LogWarning("Rigidbody2D not found on monster.");
            }
        }
        if (other.CompareTag("BallRotate"))
        {
            StartCoroutine(StopMonster());
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

        if (!SoundManager.instance.GameSoundsInGame[7].isPlaying)
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
    
    private void FollowPlayerRanger()
    {
        if (GameStateController.instance.currentGameState == GameState.Playing)
        {


            // Calcula o vetor de direção do jogador para o inimigo
            direction = player.transform.position - transform.position;
            direction.Normalize();

            // Adiciona um pequeno deslocamento aleatório à direção
            float randomOffsetX = Random.Range(-1f, 1f);
            float randomOffsetY = Random.Range(-1f, 1f);
            Vector2 randomOffset = new Vector2(randomOffsetX, randomOffsetY);
            direction += randomOffset.normalized * 0.2f; // Ajuste o valor do deslocamento conforme necessário

            // Move o inimigo na direção do jogador com a velocidade especificada
            this.gameObject.GetComponent<Rigidbody2D>().MovePosition(this.gameObject.GetComponent<Rigidbody2D>().position + direction * moveSpeed * Time.fixedDeltaTime);

            // Verifica a distância entre o jogador e o monstro
            float distanceToMonster = Vector2.Distance(this.transform.position, player.transform.position);


            Vector2 directionAngle = (Player_Main.instance.transform.position - this.transform.position).normalized;

            // Calcula o �ngulo em graus
            float angle = Mathf.Atan2(directionAngle.y, directionAngle.x) * Mathf.Rad2Deg;



            Vector3 localScale = Vector3.zero;
            if (angle > 90 || angle < -90)
            {
                localScale.x = -1f;
                localScale.y = +1f;

            }
            else
            {
                localScale.x = 1f;
                localScale.y = 1f;
            }
            this.transform.localScale = localScale;
            // Verifica se a distância é menor que 4f para começar a atirar
            if (distanceToMonster < 4f)
            {

                Attacking = true;
                AttackPlayer(); // Chame a função AttackPlayer para começar o ataque

            }
            else
            {
                moveSpeed = CurrentMoveSpeed;
            }
        }

    }


    private IEnumerator StopMonster()
    {
        moveSpeed = 0f;
        yield return new WaitForSeconds(0.2f);
    }
    //private bool bulletDirectionSet = false;
    private void AttackPlayer()
    {

        moveSpeed = 0f;
        // Se não estiver atacando, comece o ataque
        if (Attacking && BulletList.Count < 1)
        {
            Attacking = false;
            // Inicie a corrotina para instanciar a bala
            StartCoroutine(SpawnBullet());

        }
    }

    private IEnumerator SpawnBullet()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        // Instancia a bala na posição do spawnPoint
        BrainBullet_Main instanceBullet = Instantiate(BrainBullet, BulletTransform.position, Quaternion.identity);

        BulletList.Add(instanceBullet);
        yield return new WaitForSeconds(1f);

        BulletList.Remove(instanceBullet);
    }
    private void FollowPlayer()
    {
        if (GameStateController.instance.currentGameState == GameState.Playing)
        {
            // Calcula o vetor de direção do jogador para o inimigo
            direction = player.transform.position - transform.position;
            direction.Normalize();

            // Adiciona um pequeno deslocamento aleatório à direção
            float randomOffsetX = Random.Range(-1f, 1f);
            float randomOffsetY = Random.Range(-1f, 1f);
            Vector2 randomOffset = new Vector2(randomOffsetX, randomOffsetY);
            direction += randomOffset.normalized * 0.2f; // Ajuste o valor do deslocamento conforme necessário

            Vector2 directionAngle = (Player_Main.instance.transform.position - this.transform.position).normalized;

            // Calcula o �ngulo em graus
            float angle = Mathf.Atan2(directionAngle.y, directionAngle.x) * Mathf.Rad2Deg;



            Vector3 localScale = Vector3.zero;
            if (angle > 90 || angle < -90)
            {
                localScale.x = 1f;
                localScale.y = 1f;
            }
            else
            {
                localScale.x = -1f;
                localScale.y = +1f;
            }
            this.transform.localScale = localScale;

            // Move o inimigo na direção do jogador com a velocidade especificada
            this.gameObject.GetComponent<Rigidbody2D>().MovePosition(this.gameObject.GetComponent<Rigidbody2D>().position + direction * moveSpeed * Time.fixedDeltaTime);

            // Verifica a distância entre o jogador e o monstro
            float distanceToMonster = Vector2.Distance(this.transform.position, player.transform.position);
            if (distanceToMonster > closestDistance)
            {
                moveSpeed = CurrentMoveSpeed;
            }
            else
            {
                moveSpeed = 0f;
            }
        }
    }



}
