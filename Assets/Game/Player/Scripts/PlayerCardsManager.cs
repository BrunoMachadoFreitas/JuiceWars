using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCardsManager : MonoBehaviour
{
    public static PlayerCardsManager instance;
    private Coroutine juiceHoleCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void StartJuiceHoleCoroutine()
    {
        StartCoroutine(JuiceHoleCoroutine());
    }

    public void StopJuiceHoleCoroutine()
    {
        StopCoroutine(JuiceHoleCoroutine());
    }

    public IEnumerator JuiceHoleCoroutine()
    {
        float minDistance = 10.0f; // Dist�ncia m�nima do player
        float maxDistance = 20.0f; // Dist�ncia m�xima do player

        while (Player_Main.instance.CanJuiceHole)
        {
            if (Player_Main.instance.currentJuiceHole == null)
            {
                Vector3 playerPosition = Player_Main.instance.transform.position;

                // Gera uma posi��o aleat�ria dentro do raio especificado
                Vector3 randomPosition = GetRandomPositionAround(playerPosition, minDistance, maxDistance);

                Player_Main.instance.currentJuiceHole = Instantiate(Player_Main.instance.juiceHolePrefab, randomPosition, Quaternion.identity);
            }
            yield return new WaitForSeconds(5f);
        }
    }

    private Vector2 GetRandomPositionAround(Vector3 center, float minRadius, float maxRadius)
    {
        // Gera um �ngulo aleat�rio
        float angle = UnityEngine.Random.Range(0f, 2f * Mathf.PI);

        // Gera uma dist�ncia aleat�ria dentro do intervalo especificado
        float distance = UnityEngine.Random.Range(minRadius, maxRadius);

        // Calcula a posi��o x e z com base no �ngulo e na dist�ncia
        float x = center.x + distance * Mathf.Cos(angle);

        // Mant�m a mesma posi��o y
        float y = center.y;

        return new Vector2(x, y);
    }
}
