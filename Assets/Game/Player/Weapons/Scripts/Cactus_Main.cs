using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Cactus_Main : MonoBehaviour
{

    private bool isMoving = false;

    Rigidbody2D rb;

    public Transform aimTransform;

    //float reloadTime = .3f;
    bool reloading = false;
    public GameObject closestMonster;

    //[SerializeField] private int trajetoryStepCount = 20;
    //[SerializeField] private float trajetoryTimeStep = 0.1f;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Animator animCactus;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animCactus = GetComponent<Animator>();
        if (transform.parent != null)
        {
            aimTransform = transform.parent;
        }
    }

    void Update()
    {
        if (transform.parent != null)
        {
            // Calcula o vetor de direção do jogador para o inimigo
            //Vector2 directionfollow = transform.parent.position - transform.position;
            //directionfollow.Normalize();
            //// Move o inimigo na direção do jogador com a velocidade especificada
            //this.gameObject.GetComponent<Rigidbody2D>().MovePosition(this.gameObject.GetComponent<Rigidbody2D>().position + direction * 5f * Time.fixedDeltaTime);
            float closestDistance = Player_Stats.instance.Range;

            if (WaveManager.instance != null)
            {
                if (WaveManager.instance.Monsters.Count > 0)
                {
                    foreach (var monster in WaveManager.instance.Monsters)
                    {
                        if (monster != null)
                        {
                            float distanceToMonster = Vector2.Distance(Player_Main.instance.transform.position, monster.transform.position);

                            if (distanceToMonster < closestDistance)
                            {
                                closestMonster = monster;
                                closestDistance = distanceToMonster;
                            }
                        }
                    }

                    if (closestMonster != null && !closestMonster.GetComponent<Monster>().isBeingAttacked)
                    {
                        Vector2 direction = (closestMonster.transform.position - transform.position).normalized;

                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                        aimTransform.transform.eulerAngles = new Vector3(0, 0, angle);
                        this.transform.eulerAngles = aimTransform.eulerAngles;
                        Vector3 localScale = Vector3.zero;
                        if (angle > 90 || angle < -90)
                        {
                            localScale.y = -1f;
                            localScale.x = 1f;
                        }
                        else
                        {
                            localScale.y = +1f;
                            localScale.x = 1f;
                        }
                        aimTransform.transform.localScale = localScale;

                        if (!reloading && !isMoving)
                        {
                            MoveToEnemyAndDamage(aimTransform.transform.position, closestMonster.transform.position);
                        }
                    }
                }
            }
        }
    }

    private void MoveToEnemyAndDamage(Vector3 initialPosition, Vector3 targetPosition)
    {

        reloading = true;
        // Define isBeingAttacked como verdadeiro
        closestMonster.GetComponent<Monster>().isBeingAttacked = true;

        // Após um tempo, define isBeingAttacked como falso para indicar o fim do ataque
        StartCoroutine(ResetIsBeingAttacked(closestMonster.GetComponent<Monster>()));

        StartCoroutine(MoveAndReturn(initialPosition, targetPosition));
    }
    // Método para redefinir isBeingAttacked após um determinado tempo
    IEnumerator ResetIsBeingAttacked(Monster monster)
    {
        yield return new WaitForSeconds(0.3f); // Ajuste attackDuration conforme necessário

        monster.isBeingAttacked = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {

            Player_Stats.instance.ApplyDamage(other);
        }

    }



    private IEnumerator MoveAndReturn(Vector2 initialPos, Vector2 targetPos)
    {
        float journeyLength = Vector2.Distance(initialPos, targetPos);
        float startTime = Time.time;
        
        // Move o cactus em uma trajetória curva para a posição do monstro
        while (Time.time - startTime < .2f) // Tempo de duração do movimento ajustável
        {
            animCactus.SetTrigger("Attack");
            float t = (Time.time - startTime) / 2f; // Interpolação de 0 a 1 no tempo de movimento
            Vector2 currentPosition = Vector2.Lerp(initialPos, targetPos, t);
            currentPosition.y += Mathf.Sin(t * Mathf.PI) * 5f; // Adiciona uma onda senoidal para criar um arco
            transform.position = currentPosition;
            rb.simulated = true;
            this.transform.parent = null;
            yield return null;
        }

        // Retorna a adaga para a posição inicial do player
        startTime = Time.time;
        while (Time.time - startTime < 1f) // Tempo de duração do movimento de retorno ajustável
        {
            float t = (Time.time - startTime) / 1f; // Interpolação de 0 a 1 no tempo de movimento
            transform.position = Vector2.Lerp(targetPos, aimTransform.position, t); // Interpola de volta para a posição inicial
            
            this.transform.parent = aimTransform;
            rb.simulated = false;
            yield return null;
        }

        yield return new WaitForSeconds(Player_Stats.instance.VelocityCactus);

        reloading = false; // Define reloading como false após retornar à posição inicial
    }



}
