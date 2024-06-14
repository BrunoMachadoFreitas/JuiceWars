using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun_Main : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject closestMonster;
    public Transform aimTransform;
    private Animator anim;
    [SerializeField] public Animator animParent;

    float fireRate = 0.1f; // Taxa de disparo (tempo entre os disparos)
    float nextFireTime = 0f; // Tempo até o próximo disparo
    bool reloading = false;
    int bulletCount = 0;
    int maxBulletCount = 3;
    int shotsPerBurst = 3; // Número de balas por rajada
    //float timeBetweenShots = 2f; // Intervalo de tempo entre as balas na rajada
    float reloadTime = 1.5f;
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
        else
        {
        }
    }

    void Update()
    {
        // Escala o tempo de recarga usando uma função exponencial
        if(Player_Stats.instance.RealoadTime >= 1)
        {
            reloadTime = Player_Stats.instance.RealoadTime - lvlUpgrade;
        }
        else
        {
            reloadTime = Player_Stats.instance.RealoadTime * 1  - lvlUpgrade - .5f;
        }
       


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
                        anim.SetTrigger("Shoot");
                        StartCoroutine(ShootBurst(direction)); // Dispara uma rajada
                    }

                }
            }
        }
    }

    private IEnumerator ShootBurst(Vector2 direction)
    {

        if(bulletCount < shotsPerBurst)
        {

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
        
        StartCoroutine(ReloadIfNeeded()); // Inicia a rotina de recarga se necessário

        yield return new WaitForSeconds(reloadTime); // Aguarda o intervalo entre as balas na rajada

        nextFireTime = Time.time + fireRate; // Atualiza o tempo do próximo disparo
    }

    private IEnumerator ReloadIfNeeded()
    {
        if (bulletCount >= maxBulletCount)
        {
            // Inicia a recarga se a contagem de balas atingiu o máximo
            yield return StartCoroutine(Reload());
            
        }
    }

    private IEnumerator Reload()
    {
        anim.ResetTrigger("Shoot");
        reloading = true;
        yield return new WaitForSeconds(Player_Stats.instance.VelocityShootGun);
        bulletCount = 0;
        reloading = false;
    }

    void OnDestroy()
    {

    }

   
}
