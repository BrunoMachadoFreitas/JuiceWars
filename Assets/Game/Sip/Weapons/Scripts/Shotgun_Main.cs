using System.Collections;
using System.Collections.Generic;
using Game.Sounds.SoundScripts;
using UnityEngine;

public class Shotgun_Main : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject closestMonster;
    public Transform aimTransform;
    private Animator anim;
    [SerializeField] public Animator animParent;

    float nextFireTime = 0f; // Tempo até o próximo disparo
    bool reloading = false;
    int bulletCount = 0;
    int maxBulletCount = 3; // Limite de balas por rajada
    float reloadTime = 5f; // Tempo de recarga fixo de 5 segundos
    float lvlUpgrade = 0f;

    public GameObject muzzle;
    public Bullet_Shotgun Bullet;

    [SerializeField] private Transform[] bulletSpawnPoints; // Array de pontos de spawn das balas

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        animParent = transform.parent.GetComponent<Animator>();

        if (transform.parent != null)
        {
            aimTransform = transform.parent;
        }
    }

    void Update()
    {
        // Ajustar tempo de recarga
        reloadTime = Player_Stats.instance.RealoadTime * 1 - lvlUpgrade;

        float closestDistance = Player_Stats.instance.Range;
        if (WaveManager.instance)
        {
            if (WaveManager.instance.Monsters.Count > 0)
            {
                foreach (var monster in WaveManager.instance.Monsters)
                {
                    if (monster)
                    {
                        float distanceToMonster = Vector2.Distance(Player_Main.instance.transform.position, monster.transform.position);
                        if (distanceToMonster < closestDistance)
                        {
                            closestMonster = monster;
                            closestDistance = distanceToMonster;
                        }
                    }
                }

                if (closestMonster)
                {
                    if (closestMonster.GetComponent<Monster>().isBeingAttacked)
                    {
                        // Se o monstro mais próximo está sendo atacado, encontrar o próximo mais próximo
                        closestMonster = GetNextClosestMonster(closestMonster);
                    }


                    Vector2 direction = (closestMonster.transform.position - transform.position).normalized;

                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    aimTransform.eulerAngles = new Vector3(0, 0, angle);

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
                    aimTransform.localScale = localScale;

                    if (closestMonster != null && Time.time >= nextFireTime && !reloading)
                    {
                        
                        Shoot(closestMonster.transform);
                    }
                }
            }
        }
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
    private void Shoot(Transform MonsterPos)
    {

        

        StartCoroutine(WaitSound(MonsterPos));
        
        
        reloading = true;
        StartCoroutine(ShootWait());
    }
    private IEnumerator WaitSound(Transform MonsterPos)
    {
        SoundManager.instance.PlaySound(9);
        yield return new WaitForSeconds(1f); // Aguarda 2 segundos
        anim.SetTrigger("Shoot");
        if (MonsterPos)
        {

            Vector2 direction = (MonsterPos.transform.position - transform.position).normalized;
            foreach (var spawnPoint in bulletSpawnPoints)
            {

                // Instancia a bala na posição do spawnPoint
                Bullet_Shotgun instanceBullet = Instantiate(Bullet, spawnPoint.position, Quaternion.identity);

                // Define a direção da bala
                if (instanceBullet != null)
                {
                    instanceBullet.dir = direction; // Normaliza a direção da bala
                    bulletCount++;
                }
            }
        }
        else
        {
            SoundManager.instance.GameSoundsInGame[9].Stop();
            yield return null;
        }
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator ShootWait()
    {

        //muzzle.SetActive(false);

        yield return new WaitForSeconds(Player_Stats.instance.VelocityShootGun); // Aguarda 2 segundos

        reloading = false;


    }
    void OnDestroy()
    {
       
    }

}
