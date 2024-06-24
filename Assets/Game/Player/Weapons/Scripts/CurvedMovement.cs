using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CurvedMovement : MonoBehaviour
{
    public float verticalSpeed = 5f;
    public float horizontalSpeedRange = 2f;
    public float maxHeight = 4f;  // altura máxima de 4 unidades
    public float extraFallHeight = 10f;  // altura extra para descer
    public float duration = 10f;  // duração do movimento de subida e descida

    private Vector2 horizontalDirection;
    private float startTime;
    private float startY;


    public float knockbackForce = 5f;
    void Start()
    {
        startY = transform.position.y;
        float horizontalSpeed = Random.Range(-horizontalSpeedRange, horizontalSpeedRange);
        horizontalDirection = new Vector2(horizontalSpeed, 0);
        startTime = Time.time;
        Destroy(gameObject, duration);  // Destrói o objeto após a duração
    }

    void Update()
    {
        float elapsed = Time.time - startTime;
        float normalizedTime = elapsed / duration;  // Normaliza o tempo para o intervalo de 0 a 1 durante a duração total

        if (normalizedTime <= 0.5f)
        {
            // Fase de subida
            float verticalOffset = Mathf.Sin(normalizedTime * Mathf.PI) * maxHeight;
            Vector2 movement = new Vector2(horizontalDirection.x * Time.deltaTime, (startY + verticalOffset) - transform.position.y);
            transform.Translate(movement);
        }
        else if (normalizedTime <= 1f)
        {
            // Fase de descida
            float verticalOffset = Mathf.Sin((1 - normalizedTime) * Mathf.PI) * (maxHeight + extraFallHeight);
            Vector2 movement = new Vector2(horizontalDirection.x * Time.deltaTime, (startY + verticalOffset - extraFallHeight) - transform.position.y);
            transform.Translate(movement);
        }
        else
        {
            // Após o término da descida, manter apenas o movimento horizontal
            transform.Translate(horizontalDirection.x * Time.deltaTime, 0, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            ApplyKnockback(other);
            Player_Stats.instance.ApplyDamage(other);
        }
    }

    void ApplyKnockback(Collider2D monster)
    {
        Rigidbody2D monsterRb = monster.GetComponent<Rigidbody2D>();
        if (monsterRb != null)
        {
            monster.GetComponent<Monster>().moveSpeed = 0;
            // Aplica uma força contrária na direção horizontal ao monstro
            Vector2 knockbackDirection = horizontalDirection.normalized;
            monsterRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }
}