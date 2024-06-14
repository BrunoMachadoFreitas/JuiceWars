using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger_Main : MonoBehaviour
{
    Rigidbody2D rb;

    public Transform aimTransform;

    private Animator anim;
    public GameObject closestMonster;

    float ReloadTime = 2f;
    bool reloading = false;
    float moveSpeed = 20f;

 
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (transform.parent != null)
        {
            aimTransform = transform.parent;
        }
        else
        {
        }
    }

    // Update is called once per frame
    void Update()
    {


        ReloadTime = Player_Stats.instance.RealoadTime;
        // Obter a posição atual do jogador
        Vector3 playerPosition = Player_Main.instance.transform.position;

        // Obter o raio do círculo do jogador
        float playerRadius = 4f;

        // Posição do jogador com o raio do círculo
        Vector3 playerWithRadius = playerPosition + new Vector3(playerRadius, playerRadius, 0);

        

        // Variável para armazenar a distância para o monstro mais próximo
        float closestDistance = playerRadius;

        // Verificar se existe uma instância do WaveManager
        if (WaveManager.instance != null)
        {
            // Verificar se há monstros na lista de monstros do WaveManager
            if (WaveManager.instance.Monsters.Count > 0)
            {
                // Iterar sobre todos os monstros na lista de monstros do WaveManager
                foreach (var monster in WaveManager.instance.Monsters)
                {
                    if (monster != null)
                    {
                        // Calcular a distância entre o jogador e o monstro
                        float distanceToMonster = Vector2.Distance(Player_Main.instance.transform.position, monster.transform.position);

                        // Verificar se a distância para o monstro é menor que a distância mais próxima atual
                        if (distanceToMonster < closestDistance)
                        {
                            // Atualizar o monstro mais próximo e a distância mais próxima
                            closestMonster = monster;
                            closestDistance = distanceToMonster;
                        }
                    }
                }

                if (closestMonster != null)
                {
                    Vector2 direction = (closestMonster.transform.position - transform.position).normalized;

                    // Calcula o �ngulo em graus
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                    aimTransform.transform.eulerAngles = new Vector3(0, 0, angle);

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
                    
                    if (!reloading)
                    {
                        MoveToEnemyAndDamage(aimTransform.transform.position, closestMonster.transform.position);
                    }


                }
            }

        }
    }
    private void MoveToEnemyAndDamage(Vector3 initialPosition, Vector3 targetPosition)
    {
        
        reloading = true;

        StartCoroutine(MoveAndReturn(initialPosition, targetPosition));
    }

    private IEnumerator MoveAndReturn(Vector2 initialPos, Vector2 targetPos)
    {
   

        float journeyLength = Vector2.Distance(initialPos, targetPos);
        float startTime = Time.time;

        // Move a adaga para a posição do monstro
        while (Time.time - startTime < 1f)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            rb.simulated = true;
            transform.position = Vector2.Lerp(aimTransform.transform.position, targetPos, fractionOfJourney);
           
            this.transform.parent = null;
            yield return null;
        }

        yield return new WaitForSeconds(.5f);

        // Retorna a adaga para a posição atual do parente
        startTime = Time.time;
        while (Time.time - startTime < 1f)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector2.Lerp(transform.position, aimTransform.transform.position, moveSpeed);
            rb.simulated = false;
            this.transform.parent = aimTransform.transform;
            yield return null;
        }

        reloading = false; // Define reloading como false após retornar à posição inicial
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            Player_Stats.instance.ApplyDamage(other);
        }

    }


}
