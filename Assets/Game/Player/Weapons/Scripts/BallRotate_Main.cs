using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRotate_Main : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Transform player;  // Refer�ncia ao jogador
    [SerializeField] private float distanceFromPlayer = 1.0f;  // Dist�ncia da arma ao jogador
    [SerializeField] private float pullBackDistance = 2.0f;
    private float angle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        player = Player_Main.instance.transform;
        if (player == null)
        {
            Debug.LogError("Player transform is not assigned.");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        // Incrementa o �ngulo com base na velocidade de rota��o e o tempo decorrido
        angle += rotateSpeed * Time.deltaTime;

        // Converte o �ngulo de graus para radianos
        float angleRad = angle * Mathf.Deg2Rad;

        // Calcula a nova posi��o da arma ao redor do jogador
        float x = player.position.x + Mathf.Cos(angleRad) * distanceFromPlayer;
        float y = player.position.y + Mathf.Sin(angleRad) * distanceFromPlayer;

        // Atualiza a posi��o da arma
        transform.position = new Vector3(x, y, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            // Calcula a dire��o para puxar o monstro para tr�s
            Vector2 pullDirection = (other.transform.position - player.position).normalized;

            // Calcula a nova posi��o do monstro
            Vector2 newPosition = (Vector2)other.transform.position + pullDirection * pullBackDistance;

            // Atualiza a posi��o do monstro
            other.transform.position = new Vector3(newPosition.x, newPosition.y, other.transform.position.z);

            Player_Stats.instance.ApplyDamage(other);
        }
    }
}
