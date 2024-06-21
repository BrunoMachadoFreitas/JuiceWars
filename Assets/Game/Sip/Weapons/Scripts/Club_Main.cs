using System.Collections;
using System.Collections.Generic;
using Game.Sounds.SoundScripts;
using UnityEngine;

public class Club_Main : MonoBehaviour
{
    Rigidbody2D rb;
    public Transform aimTransform;
    private Animator anim;
    public GameObject closestMonster;
    float ReloadTime = 2f;
    bool reloading = false;
    float moveSpeed = 20f;
    Vector2 targetPosPArent = Vector2.zero;

    [SerializeField] private GameObject SpriteObjectSword;
    [SerializeField] private Animator SwordAnim;
    Transform closestMonsterTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (transform.parent != null)
        {
            aimTransform = transform.parent;
        }

        closestMonsterTransform = transform.parent;
    }

    void Update()
    {
        if (Player_Main.instance) { 
            targetPosPArent = aimTransform.position;
        ReloadTime = Player_Stats.instance.RealoadTime - 3f;

        Vector3 playerPosition = Vector3.zero;

        playerPosition = Player_Main.instance.transform.position;

        float playerRadius = Player_Stats.instance.Range;
        Vector3 playerWithRadius = playerPosition + new Vector3(playerRadius, playerRadius, 0);

        float closestDistance = playerRadius;
        if (transform.parent != null)
        {
            if (Player_Main.instance)
            {
                if (WaveManager.instance != null)
                {
                    if (WaveManager.instance.Monsters.Count > 0)
                    {
                            // Encontrar o monstro mais próximo
                            closestMonster = GetClosestMonster();
                            if (closestMonster) {
                                if (closestMonster.GetComponent<Monster>().isBeingAttacked)
                                {
                                    // Se o monstro mais próximo está sendo atacado, encontrar o próximo mais próximo
                                    closestMonster = GetNextClosestMonster(closestMonster);
                                }
                            }
                            if (closestMonster != null && !closestMonster.GetComponent<Monster>().isBeingAttacked)
                            {

                                closestMonsterTransform = closestMonster.transform;
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

                                if (!reloading)
                                {
                                    Vector2 predictedPosition = PredictMonsterPosition(closestMonsterTransform);
                                    MoveToEnemyAndDamage(aimTransform.transform.position, predictedPosition);
                                }
                            }
                            else
                            {
                                closestMonster = null;
                                closestMonsterTransform = null;
                            }
                    }
                }
            }
        }
    }
    }
    GameObject GetClosestMonster()
    {
        float closestDistance = Player_Stats.instance.Range;
        GameObject closestMonster = null;

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

        return closestMonster;
    }

    GameObject GetNextClosestMonster(GameObject currentMonster)
    {
        float closestDistance = Player_Stats.instance.Range;
        GameObject nextClosestMonster = null;

        foreach (var monster in WaveManager.instance.Monsters)
        {
            if (monster != null && monster != currentMonster)
            {
                float distanceToMonster = Vector2.Distance(Player_Main.instance.transform.position, monster.transform.position);
                if (distanceToMonster < closestDistance && !monster.GetComponent<Monster>().isBeingAttacked)
                {
                    nextClosestMonster = monster;
                    closestDistance = distanceToMonster;
                }
            }
        }

        return nextClosestMonster;
    }

    private Vector2 PredictMonsterPosition(Transform monsterTransform)
    {
        // Considerar a velocidade atual do monstro para prever a posição futura
        Rigidbody2D monsterRb = monsterTransform.GetComponent<Rigidbody2D>();
        if (monsterRb != null)
        {
            // Assumindo movimento constante para prever a posição futura
            float predictionTime = 0.4f; // Ajuste conforme necessário
            Vector2 futurePosition = (Vector2)monsterTransform.position + monsterRb.velocity * predictionTime;
            return futurePosition;
        }
        else
        {
            // Se não houver Rigidbody2D, retornar a posição atual
            return monsterTransform.position;
        }
    }

    private void MoveToEnemyAndDamage(Vector3 initialPosition, Vector3 targetPosition)
    {
        reloading = true;
        closestMonster.GetComponent<Monster>().isBeingAttacked = true;
        StartCoroutine(ResetIsBeingAttacked(closestMonster.GetComponent<Monster>()));
        StartCoroutine(MoveAndReturn(initialPosition, targetPosition));
    }

    IEnumerator ResetIsBeingAttacked(Monster monster)
    {
        yield return new WaitForSeconds(ReloadTime);
        monster.isBeingAttacked = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            SoundManager.instance.PlaySound(1);
            other.gameObject.SendMessage("ApplyDamage", 8f);
        }
    }

    private IEnumerator MoveAndReturn(Vector2 initialPos, Vector2 targetPos)
    {
        float journeyLength = Vector2.Distance(initialPos, targetPos);
        float startTime = Time.time;

        while (Time.time - startTime < 0.4f)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector2.Lerp(initialPos, targetPos, fractionOfJourney);
            if(closestMonster)
            closestMonster.GetComponent<Monster>().isBeingAttacked = true;
            rb.simulated = true;
            this.transform.parent = null;
            yield return null;
        }

        startTime = Time.time;
        Vector2 initialPosition = transform.position;
        while (Time.time - startTime < 1f)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            if(aimTransform)
            transform.position = Vector2.Lerp(initialPosition, aimTransform.position, fractionOfJourney);
            if (closestMonster)
                closestMonster.GetComponent<Monster>().isBeingAttacked = false;
            rb.simulated = false;
            this.transform.parent = aimTransform;
            yield return null;
        }

        yield return new WaitForSeconds(Player_Stats.instance.VelocityClub);
        reloading = false;
    }
}
