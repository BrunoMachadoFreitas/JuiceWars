using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Variáveis para controlar a intensidade e duração do shake
    public float shakeIntensity = 0.7f;
    public float shakeDuration = 0.5f;

    // Posição original da câmera
    private Vector3 originalPosition;
   

    // Chamado para iniciar o shake
    public void ShakeCamera()
    {
        originalPosition = Camera.main.gameObject.transform.localPosition; // Salva a posição original da câmera
        InvokeRepeating("DoShake", 0, 0.01f); // Começa a chamar a função DoShake repetidamente
        Invoke("StopShaking", shakeDuration); // Para o shake após a duração especificada
    }

    // Função para realizar o shake
    private void DoShake()
    {
        float shakeAmountX = Random.Range(-1f, 1f) * shakeIntensity;
        float shakeAmountY = Random.Range(-1f, 1f) * shakeIntensity;

        // Aplica o shake à posição da câmera
        Camera.main.gameObject.transform.localPosition = originalPosition + new Vector3(shakeAmountX, shakeAmountY, 0);
    }

    // Função para parar o shake
    private void StopShaking()
    {
        CancelInvoke("DoShake"); // Para de chamar a função DoShake
        Camera.main.gameObject.transform.localPosition = originalPosition; // Retorna a câmera à sua posição original
    }
}
