using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Vari�veis para controlar a intensidade e dura��o do shake
    public float shakeIntensity = 0.7f;
    public float shakeDuration = 0.5f;

    // Posi��o original da c�mera
    private Vector3 originalPosition;
   

    // Chamado para iniciar o shake
    public void ShakeCamera()
    {
        originalPosition = Camera.main.gameObject.transform.localPosition; // Salva a posi��o original da c�mera
        InvokeRepeating("DoShake", 0, 0.01f); // Come�a a chamar a fun��o DoShake repetidamente
        Invoke("StopShaking", shakeDuration); // Para o shake ap�s a dura��o especificada
    }

    // Fun��o para realizar o shake
    private void DoShake()
    {
        float shakeAmountX = Random.Range(-1f, 1f) * shakeIntensity;
        float shakeAmountY = Random.Range(-1f, 1f) * shakeIntensity;

        // Aplica o shake � posi��o da c�mera
        Camera.main.gameObject.transform.localPosition = originalPosition + new Vector3(shakeAmountX, shakeAmountY, 0);
    }

    // Fun��o para parar o shake
    private void StopShaking()
    {
        CancelInvoke("DoShake"); // Para de chamar a fun��o DoShake
        Camera.main.gameObject.transform.localPosition = originalPosition; // Retorna a c�mera � sua posi��o original
    }
}
