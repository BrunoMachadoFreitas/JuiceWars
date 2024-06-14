using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Weapon_Main : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject closestMonster;

    GameObject lineRendererObject;
    LineRenderer lineRenderer;

    public Transform aimTransform;

    private Animator anim;


    float ReloadTime = 2f;
    bool reloading = false;

    public GameObject muzzle;

    public Bullet_Main Bullet;

    public float lvlUpgrade = 0f;

    public int Level = 0;

    private Vector3 initialPosition; // Posição inicial da arma
    //private float bounceOffset = 0f; // Offset de bounce
    //private bool isMoving = false;
    private bool Shooting = false;
    public float bounceHeight = 0.5f; // Altura máxima do bounce
    public float bounceSpeed = 2f; // Velocidade de bounce
    public float rotationSpeed = 50f; // Velocidade de rotação
    public AudioSource sound;
    // Start is called before the first frame update
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
        }

        initialPosition = transform.position;
    }

    void Update()
    {
       
        float closestDistance = Player_Stats.instance.Range - (lvlUpgrade * 0.2f);
        ReloadTime = Player_Stats.instance.RealoadTime - lvlUpgrade;
        if (WaveManager.instance)
        {
            if (WaveManager.instance.Monsters.Count > 0)
            {
               
                foreach (var monster in WaveManager.instance.Monsters)
                 {
                        if (monster)
                        {
                            float distanceToMonster = Vector2.Distance(transform.position, monster.transform.position);
                            if (distanceToMonster < closestDistance)
                            {
                                closestMonster = monster;
                                closestDistance = distanceToMonster;
                            }
                        }
                }

                if (closestMonster != null)
                {
                    Vector2 direction = (closestMonster.transform.position - this.transform.position).normalized;

                    // Calcula o �ngulo em graus
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

                    if (!reloading)
                    {
                        muzzle.SetActive(true);

                        Bullet_Main instanceBullet = Instantiate(Bullet, new Vector3(muzzle.transform.position.x, muzzle.transform.position.y, 0f), Quaternion.identity);
                        
                        if (instanceBullet) instanceBullet.dir = direction;

                        Shooting = true;
                        anim.SetTrigger("Shoot");
                        Shoot(closestMonster.transform);
                    }


                }
            }

        }
    }

    private void Shoot(Transform MonsterPos)
    {
        
        
        reloading = true;
        SoundManager.instance.PlaySound(5);
        StartCoroutine(ShootWait());
    }

    
    private IEnumerator ShootWait()
    {

        muzzle.SetActive(false);
       
        yield return new WaitForSeconds(Player_Stats.instance.VelocityPistol); // Aguarda 2 segundos

        Shooting = false;
        reloading = false;
        

    }
    void OnDestroy()
    {
        if (lineRendererObject != null)
        {
            Destroy(lineRendererObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o objeto colidido é o jogador
        if (other.CompareTag("Monster"))
        {
            Player_Stats.instance.ApplyDamage(other);
            Destroy(this.gameObject);
        }
    }


}
