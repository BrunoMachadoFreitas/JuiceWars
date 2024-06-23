using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket_Main : MonoBehaviour
{
    Rigidbody2D rb;

    public Transform aimTransform;
    private Animator anim;

    float ReloadTime = 1f;
    bool reloading = false;

    public Bullet_Rocket Bullet;

    private GameObject[] enemies; // Lista de inimigos disponíveis

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (transform.parent != null)
        {
            aimTransform = transform.parent;
        }
        else
        {
            aimTransform = transform; // Se não houver um pai, use o próprio transform
        }

        StartCoroutine(FindEnemiesRoutine());
    }

    IEnumerator FindEnemiesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // Atualiza a lista de inimigos a cada segundo
            enemies = GameObject.FindGameObjectsWithTag("Monster");
        }
    }

    void Update()
    {
        ReloadTime = Player_Stats.instance.RealoadTime + 3f;
        if (enemies != null && enemies.Length > 0 && !reloading)
        {
            GameObject randomEnemy = enemies[Random.Range(0, enemies.Length)];
            Vector2 direction = Vector2.zero;
            if (randomEnemy != null) { 
             direction = (randomEnemy.transform.position - transform.position).normalized;
            }
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            aimTransform.eulerAngles = new Vector3(0, 0, angle);

            Vector3 localScale = Vector3.one;
            if (angle > 90 || angle < -90)
            {
                localScale.y = -1f;
            }
            else
            {
                localScale.y = +1f;
            }
            aimTransform.localScale = localScale;

            Bullet_Rocket instanceBullet = Instantiate(Bullet, transform.position, Quaternion.identity);
            if (instanceBullet) instanceBullet.dir = direction;

            reloading = true;
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(ReloadTime);
        reloading = false;
    }

    void OnDestroy()
    {
        // Cleanup, se necessário
    }
}
