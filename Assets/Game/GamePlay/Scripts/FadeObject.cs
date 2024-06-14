using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class FadeObject : MonoBehaviour
{
    public static FadeObject instance;
    public float scaleFactor = 1.1f; // Fator de escala, quanto maior, mais r�pido o objeto aumentar� de tamanho
    public float maxScale = 200f; // Escala m�xima que o objeto pode alcan�ar
    public float fadeSpeed = 3f; // Velocidade de fade

    public bool fadingOut = false; // Flag para indicar se est� ocorrendo um fade out

    // Vari�veis para controlar a rota��o aleat�ria
    public float maxRotationSpeed = 90f; // Velocidade m�xima de rota��o em graus por segundo
    private Vector3 rotationAxis; // Eixo de rota��o aleat�rio
    private float rotationSpeed; // Velocidade de rota��o aleat�ria

    // Escala final que o objeto alcan�ar�
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
        // Configurar a rota��o aleat�ria
        rotationAxis = new Vector3(0, 0, Random.Range(-1f, 1f)).normalized;
        

        // Aplicar rota��o aleat�ria
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
        Vector3 targetScale = Vector3.one * maxScale; // Escala final que o objeto alcan�ar�
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, fadeSpeed * Time.deltaTime);
    }


}
