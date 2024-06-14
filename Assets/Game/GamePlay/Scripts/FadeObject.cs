using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class FadeObject : MonoBehaviour
{
    public static FadeObject instance;
    public float scaleFactor = 1.1f; // Fator de escala, quanto maior, mais rápido o objeto aumentará de tamanho
    public float maxScale = 200f; // Escala máxima que o objeto pode alcançar
    public float fadeSpeed = 3f; // Velocidade de fade

    public bool fadingOut = false; // Flag para indicar se está ocorrendo um fade out

    // Variáveis para controlar a rotação aleatória
    public float maxRotationSpeed = 90f; // Velocidade máxima de rotação em graus por segundo
    private Vector3 rotationAxis; // Eixo de rotação aleatório
    private float rotationSpeed; // Velocidade de rotação aleatória

    // Escala final que o objeto alcançará
    Vector3 targetScaleOut = Vector3.zero;
    public float value = 1f;


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

    private void Start()
    {
        // Configurar a rotação aleatória
        rotationAxis = new Vector3(0, 0, Random.Range(-1f, 1f)).normalized;
        

        // Aplicar rotação aleatória
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

        FadeIn();
    }

    private void Update()
    {

    }
    public void FadeOut()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScaleOut, fadeSpeed * Time.deltaTime);
    }
    public void FadeIn()
    {
        Vector3 targetScale = Vector3.one * maxScale; // Escala final que o objeto alcançará
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, fadeSpeed * Time.deltaTime);
    }


}
