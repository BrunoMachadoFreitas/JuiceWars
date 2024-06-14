using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGun_Main : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject closestMonster;
    public Transform aimTransform;
    private Animator anim;

    float fireRate = 0.1f; // Taxa de disparo (tempo entre os disparos)
    float nextFireTime = 1.5f; // Tempo até o próximo disparo
    bool reloading = false;
    bool isShooting = false;
    int bulletCount = 0;
    float reloadTime = 5f;
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

                    if (closestMonster != null && !closestMonster.GetComponent<Monster>().isBeingAttacked)
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

                        FireBullet(closestMonster);

                        bulletCount++;
                        StartCoroutine(WaitForNextShot());
                        if (closestMonster == null)
                        {
                            muzzle.SetActive(false);
                            StartCoroutine(Reload());
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

    void FireBullet(GameObject Monster)
    {
        if (!isShooting)
        {
            isShooting = true;
            muzzle.SetActive(true);
            Bullet_MiniGun BulletNew = Instantiate(Bullet, new Vector3(muzzle.transform.position.x, muzzle.transform.position.y, 0f), Quaternion.identity);
            if (BulletNew != null)
            {
                BulletNew.Monster = Monster;
                BulletNew.damageLvlWeapon = (Player_Stats.instance.Power + lvlUpgrade) / 2;
            }

            Monster.GetComponent<Monster>().isBeingAttacked = true;
            anim.SetTrigger("Shoot");

            SoundManager.instance.PlaySound(4);


            StartCoroutine(ResetIsBeingAttacked(Monster));
            Invoke("ResetShootingFlag", Player_Stats.instance.VelocityMiniGun);
        }
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

    IEnumerator WaitForNextShot()
    {
        yield return new WaitForSeconds(Player_Stats.instance.RealoadTime);

        nextFireTime = Time.time + fireRate;
    }

    private IEnumerator Reload()
    {
        reloading = true;
        yield return new WaitForSeconds(reloadTime);
        bulletCount = 0;
        reloading = false;
    }
}
