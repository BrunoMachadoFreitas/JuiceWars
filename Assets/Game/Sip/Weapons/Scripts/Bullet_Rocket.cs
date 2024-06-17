using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Rocket : MonoBehaviour
{
    float moveSpeed = 20f;
    Rigidbody2D rb;
    public Vector2 dir;
    public GameObject DangerZone;
    private float curveStrength = 0.5f;

    // Força inicial para cima
    public float initialForceUpward = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Adiciona uma força inicial para cima
        rb.velocity = Vector2.up * initialForceUpward;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 curveDir = Vector2.Lerp(dir, dir, curveStrength * Time.deltaTime);
        rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o objeto colidido é o jogador
        if (other.CompareTag("Monster") || other.CompareTag("Border"))
        {
            GameObject DangerZone1 = Instantiate(DangerZone, new Vector3(this.transform.position.x, this.transform.position.y, 0f), Quaternion.identity);
            DangerZone1.GetComponent<CircleCollider2D>().isTrigger = true;
            Destroy(this.gameObject);

           

        }
    }
}
