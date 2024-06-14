using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BrainBullet_Main : MonoBehaviour
{
    float moveSpeed = 15f;
    Rigidbody2D rb;
    Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = Player_Main.instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player_Main.instance) { 
        // Calcula a direção em direção ao jogador
        Vector2 direction = (Player_Main.instance.transform.position - transform.position).normalized;

        // Move a bala em direção ao jogador
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Border") || other.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}
