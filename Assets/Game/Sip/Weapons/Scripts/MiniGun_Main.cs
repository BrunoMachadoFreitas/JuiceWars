using System.Collections;
using System.Collections.Generic;
using Game.Sounds.SoundScripts;
using UnityEngine;

public class MiniGun_Main : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject closestMonster;
    public Transform aimTransform;
    private Animator anim;

    float fireRate = 0.1f; // Taxa de disparo (tempo entre os disparos)
    float nextFireTime = 0f; // Tempo até o próximo disparo
    bool reloading = false;
    bool isShooting = false;
    int bulletCount = 0;
    int bulletBag = 1; // Número de balas por rajada
    float reloadTime = 5f; // Tempo de recarga
    public float lvlUpgrade = 0f;
    public GameObject muzzle;
    public Bullet_MiniGun Bullet;
    Vector3 aimEuler;

    public AudioSource sound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInParent<Animator>();

        if (transform.parent != null)
        {
            aimTransform = transform.parent.transform.parent;
            aimEuler = aimTransform.localEulerAngles;
        }
    }

    void Update()
    {
        reloadTime = Player_Stats.instance.RealoadTime - (lvlUpgrade * 0.3f) - 0.2f;
        float closestDistance = Player_Stats.instance.Range;

        if (WaveManager.instance != null && Player_Main.instance != null)
        {
            if (WaveManager.instance.Monsters.Count > 0)
            {
                // Encontrar o monstro mais próximo
                closestMonster = GetClosestMonster();

                muzzle.SetActive(false);
                if (closestMonster && !reloading)
                {
                    if (closestMonster.GetComponent<Monster>().isBeingAttacked)
                    {
                        // Se o monstro mais próximo está sendo atacado, encontrar o próximo mais próximo
                        closestMonster = GetNextClosestMonster(closestMonster);
                    }

                    if (closestMonster != null && !closestMonster.GetComponent<Monster>().isBeingAttacked && Time.time > nextFireTime)
                    {
                        Vector2 direction = (closestMonster.transform.position - this.transform.position).normalized;
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

                        if (!isShooting)
                        {
                            StartCoroutine(FireBulletsCoroutine(closestMonster));
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

    IEnumerator FireBulletsCoroutine(GameObject monster)
    {
        isShooting = true;
        for (int i = 0; i < bulletBag; i++)
        {
            if (bulletCount >= Player_Stats.instance.maxBulletsMiniGun)
            {
                break;
            }

            FireBullet(monster);

            bulletCount++;
            yield return new WaitForSeconds(fireRate);
        }

        isShooting = false;

        if (bulletCount >= Player_Stats.instance.maxBulletsMiniGun)
        {
            StartCoroutine(Reload());
        }
    }

    void FireBullet(GameObject Monster)
    {
        muzzle.SetActive(true);
        Bullet_MiniGun BulletNew = Instantiate(Bullet, new Vector3(muzzle.transform.position.x, muzzle.transform.position.y, 0f), Quaternion.identity);
        if (BulletNew != null)
        {
            Vector3 monsterPosition = Monster.transform.position;
            BulletNew.SetTarget(monsterPosition);
            BulletNew.damageLvlWeapon = (Player_Stats.instance.Power + lvlUpgrade) / 2;
            BulletNew.SetDirection((monsterPosition - muzzle.transform.position).normalized);
        }

        Monster.GetComponent<Monster>().isBeingAttacked = true;
        anim.SetTrigger("Shoot");

        SoundManager.instance.PlaySound(4);

        StartCoroutine(ResetIsBeingAttacked(Monster));
        Invoke("ResetShootingFlag", Player_Stats.instance.VelocityMiniGun);
    }

    IEnumerator ResetIsBeingAttacked(GameObject monster)
    {
        yield return new WaitForSeconds(0.3f);
        if (monster)
            monster.GetComponent<Monster>().isBeingAttacked = false;
    }

    void ResetShootingFlag()
    {
        isShooting = false;
    }

    private IEnumerator Reload()
    {
        reloading = true;
        anim.SetTrigger("Reload"); // Opcional: adiciona animação de recarga
        StartCoroutine(RechargeCoroutine());
        yield return new WaitForSeconds(reloadTime);
        bulletCount = 0;
        reloading = false;
    }
    [SerializeField] private UnityEngine.UI.Image ImageMinigunWaiter;
    private IEnumerator RechargeCoroutine()
    {
        ImageMinigunWaiter.gameObject.SetActive(true);
        float rechargeTime = reloadTime;
        float elapsedTime = 0f;

        while (elapsedTime < rechargeTime)
        {
            elapsedTime += Time.deltaTime;
            ImageMinigunWaiter.fillAmount = Mathf.Clamp01(elapsedTime / rechargeTime);
            yield return null;
        }

        // Ao final do timer, garantir que o fillAmount esteja em 1
        ImageMinigunWaiter.fillAmount = 1f;
        ImageMinigunWaiter.gameObject.SetActive(false);
    }
}
