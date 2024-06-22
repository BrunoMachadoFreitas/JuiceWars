using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet_MiniGun : MonoBehaviour
{
    float moveSpeed = 40f;
    Rigidbody2D rb;
    Vector2 direction;
    public float damageLvlWeapon = 0;

    private Vector3 targetPosition;
    bool willBeDestroyed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        MoveBullet();
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    public void SetTarget(Vector3 targetPos)
    {
        targetPosition = targetPos;
    }

    private void MoveBullet()
    {
        Vector3 relativePos = targetPosition - transform.position;
        float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o objeto colidido é o Monstro
        if (other.CompareTag("Monster"))
        {
            Player_Stats.instance.ApplyDamage(other);
            if (willBeDestroyed == false)
            {
                Destroy(this.gameObject, 1f);
                willBeDestroyed = true;
            }
        }

        // Verifica se o objeto colidido é uma borda
        if (other.CompareTag("Border"))
        {
            Destroy(this.gameObject);
        }
    }
}
