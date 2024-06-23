using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet_Main : MonoBehaviour
{
    float moveSpeed = 40f;
    Rigidbody2D rb;
    public Vector2 dir;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);
    }

    public void MoveBullet(Vector2 dir)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o objeto colidido é o jogador
        if (other.CompareTag("Monster"))
        {
            Player_Stats.instance.ApplyDamage(other);
                  
            Destroy(this.gameObject);
        }

        if (other.CompareTag("Border"))
        {
            
            Destroy(this.gameObject);
        }
    }
}
