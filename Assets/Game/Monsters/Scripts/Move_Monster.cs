using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using System.Diagnostics;
public enum MonsterType
{
    Walker = 0,
    Ranger = 1
}
public class Move_Monster : MonoBehaviour
{
    public GameObject player;
    
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float CurrentMoveSpeed;
    [SerializeField] private MonsterType currentMonsterType;
    private float randomMoveInterval = 5f; // Intervalo de tempo para mudança de direção aleatória
    private float nextRandomMoveTime = 0f;

    

    //atributos
    //float life = 10f;
     // Prefab do Floating Text

    
    private bool isDamaged = false;
    

    [SerializeField] bool CanFollowPlayer = true;
    [SerializeField] bool Attacking = false;

    // Start is called before the first frame update

    //see the distance between monster and the player
    private float closestDistance = 0.6f;

    [SerializeField] private BrainBullet_Main BrainBullet;

    [SerializeField] private List<BrainBullet_Main> BulletList= new List<BrainBullet_Main>();
    [SerializeField] private Transform BulletTransform;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(Player_Main.instance)
        player = Player_Main.instance.gameObject;
       
        CurrentMoveSpeed = moveSpeed;
    }
    //private bool isStopped = false;
    // Método para alternar entre ativar e desativar o collider de dano




    // Update is called once per frame
    void Update()
    {
        if(currentMonsterType == MonsterType.Walker)
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
    private void FollowPlayerRanger()
    {
        if(GameStateController.instance.currentGameState == GameState.Playing)
        {

        
        // Calcula o vetor de direção do jogador para o inimigo
        Vector2 direction = player.transform.position - transform.position;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BallRotate"))
        {
            StartCoroutine(StopMonster());
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
            Vector2 direction = player.transform.position - transform.position;
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
