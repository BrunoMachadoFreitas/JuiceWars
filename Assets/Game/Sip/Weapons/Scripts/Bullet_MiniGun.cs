using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet_MiniGun : MonoBehaviour
{
    float moveSpeed = 40f;
    Rigidbody2D rb;
    public Vector2 dir;

    public float damageLvlWeapon = 0;

    public GameObject Monster;
    Vector2 direction;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Calcula o vetor de direção do jogador para o inimigo
        //if (Monster) { 
        direction = Monster.transform.position - transform.position;
        direction.Normalize();
        //}
        //else
        //{
        //    direction = new Vector3(this.transform.position.x, this.transform.position.y) - transform.position;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
       


        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    public void MoveBullet(Vector2 dir)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o objeto colidido é o Monstro
        if (other.CompareTag("Monster") )
        {
            Player_Stats.instance.ApplyDamage(other);
            Destroy(this.gameObject);
        }

        // Verifica se o objeto colidido é uma borda
        if (other.CompareTag("Border"))
        {
            Destroy(this.gameObject);
        }
    }
}
