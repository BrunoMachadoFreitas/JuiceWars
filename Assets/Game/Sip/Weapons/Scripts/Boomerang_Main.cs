using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Boomerang_Main : MonoBehaviour
{
    Rigidbody2D rb;

    public Transform aimTransform;

    private Animator anim;

    float ReloadTime = 2f;
    bool reloading = false;
    float moveSpeed = 12f;

    private CircleCollider2D circleCollider;
    GameObject closestMonster;
    [SerializeField] private GameObject trailObject;

    private Animator BoomerangAnimator;

    public int lvlUpgrade = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        BoomerangAnimator = GetComponent<Animator>();
        trailObject.GetComponent<TrailRenderer>().emitting = false;
        if (transform.parent != null)
        {
            aimTransform = transform.parent;
        }
        else
        {
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {

            Player_Stats.instance.ApplyDamage(other);
        }

    }

    // Update is called once per frame
    void Update()
    {
        float playerRadius = Player_Stats.instance.Range;

        // Variável para armazenar a distância para o monstro mais próximo
        float closestDistance = playerRadius;
        ReloadTime = Player_Stats.instance.RealoadTime;
        if(Player_Main.instance != null)
        {
            if (WaveManager.instance)
            {
                if (!reloading && WaveManager.instance.Monsters.Count > 0)
                {
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
                    if (closestMonster)
                    {
                    
                            MoveToEnemyAndDamage(closestMonster);
                    
                    }

                }

            }

        }

        if(GameStateController.instance.currentGameState == GameState.Paused)
        {
            transform.SetParent(aimTransform);
        }
    }



    void MoveToEnemyAndDamage(GameObject monster)
    {
        reloading = true;
        Vector2 initialPosition = transform.position;
        Vector2 targetPosition = monster.transform.position - (monster.transform.position - Player_Main.instance.transform.position).normalized * 0.5f;

        StartCoroutine(MoveAndReturn(initialPosition, targetPosition));
    }

    private IEnumerator MoveAndReturn(Vector2 initialPos, Vector2 targetPos)
    {
        // Armazena a posição atual do parent
        Vector2 parentPosition = transform.parent.position;

        float journeyLength = Vector2.Distance(initialPos, targetPos);
        float startTime = Time.time;
        
        // Move a adaga para a posição do monstro
        while (Time.time - startTime < 1f)
        {
            BoomerangAnimator.SetTrigger("Shoot");
            trailObject.GetComponent<TrailRenderer>().emitting = true;
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            rb.simulated = true;
            if(aimTransform)
            transform.position = Vector2.Lerp(aimTransform.transform.position, targetPos, fractionOfJourney);
            this.transform.parent = null;
            circleCollider.enabled = true;
            this.gameObject.transform.Rotate(0f, 0f, 4f);
            this.gameObject.transform.Translate(0f, 2f, 0f);
           
            yield return null;
        }


        // Retorna a adaga para a posição inicial do player
        startTime = Time.time;
        //Vector2 initialPosition = transform.position; // Salva a posição inicial da adaga
        while (Time.time - startTime < 1f)
        {
            trailObject.GetComponent<TrailRenderer>().emitting = false;
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            if (aimTransform)
                transform.position = Vector2.Lerp(this.transform.position, aimTransform.position, fractionOfJourney); // Interpola gradualmente para a posição inicial
            rb.simulated = false;
            this.transform.parent = aimTransform;
            yield return new WaitForSeconds(Player_Stats.instance.VelocityBoomerang);
        }

        reloading = false; // Define reloading como false após retornar à posição inicial
    }

}
