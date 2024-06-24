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
        float minDistance = 10.0f; // Distância mínima do player
        float maxDistance = 20.0f; // Distância máxima do player

        while (Player_Main.instance.CanJuiceHole)
        {
            if (Player_Main.instance.currentJuiceHole == null)
            {
                Vector3 playerPosition = Player_Main.instance.transform.position;

                // Gera uma posição aleatória dentro do raio especificado
                Vector3 randomPosition = GetRandomPositionAround(playerPosition, minDistance, maxDistance);

                Player_Main.instance.currentJuiceHole = Instantiate(Player_Main.instance.juiceHolePrefab, randomPosition, Quaternion.identity);
            }
            yield return new WaitForSeconds(5f);
        }
    }

    private Vector2 GetRandomPositionAround(Vector3 center, float minRadius, float maxRadius)
    {
        // Gera um ângulo aleatório
        float angle = UnityEngine.Random.Range(0f, 2f * Mathf.PI);

        // Gera uma distância aleatória dentro do intervalo especificado
        float distance = UnityEngine.Random.Range(minRadius, maxRadius);

        // Calcula a posição x e z com base no ângulo e na distância
        float x = center.x + distance * Mathf.Cos(angle);

        // Mantém a mesma posição y
        float y = center.y;

        return new Vector2(x, y);
    }
}
