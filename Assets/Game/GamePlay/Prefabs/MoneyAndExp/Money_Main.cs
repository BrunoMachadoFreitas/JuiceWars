using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money_Main : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameStateController.instance.currentGameState == GameState.Playing)
        {

            Vector2 directionAngle = (Player_Main.instance.transform.position - this.transform.position).normalized;
            // Verifica a distância entre o jogador e o monstro
            float distanceToPlayer = Vector2.Distance(this.transform.position, Player_Main.instance.transform.position);
            if (distanceToPlayer < 2f)
            {
                rb.velocity = directionAngle * 10f;
            }

        }
    }


}
