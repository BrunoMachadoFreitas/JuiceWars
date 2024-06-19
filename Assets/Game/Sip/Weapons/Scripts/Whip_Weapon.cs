using System.Collections;
using UnityEngine;

public class Whip_Weapon : MonoBehaviour
{
    public float minWidth = 1.0f; // Largura mínima da sprite
    public float maxWidth = 2.0f; // Largura máxima da sprite
    public float stretchSpeed = 2.0f; // Velocidade do esticamento e retração
    public float intervalDuration = 1.0f; // Intervalo entre esticamentos

    private Transform spriteTransform;
    private Vector3 originalScale;
    private Vector3 originalLocalPosition;
    private bool stretchingRight = true; // Controle de direção de esticamento

    void Start()
    {
        // Assume que a sprite é o primeiro filho
        spriteTransform = transform.GetChild(0);
        if (spriteTransform != null)
        {
            originalScale = spriteTransform.localScale;
            originalLocalPosition = spriteTransform.localPosition;
            StartCoroutine(StretchAndShrink());
        }
        else
        {
            Debug.LogError("Nenhuma sprite encontrada como filha do GameObject.");
        }
    }

    private IEnumerator StretchAndShrink()
    {
        while (true)
        {
            // Estica
            yield return StartCoroutine(ChangeWidth(minWidth, maxWidth, stretchSpeed, stretchingRight));

            // Aguarda intervalo
            yield return new WaitForSeconds(intervalDuration);

            // Desestica
            yield return StartCoroutine(ChangeWidth(maxWidth, minWidth, stretchSpeed, stretchingRight));

            // Aguarda intervalo
            yield return new WaitForSeconds(intervalDuration);
        }
    }

    private IEnumerator ChangeWidth(float fromWidth, float toWidth, float speed, bool toRight)
    {
        float elapsedTime = 0f;
        Vector3 newScale = originalScale;
        Vector3 newPosition = originalLocalPosition;

        float duration = Mathf.Abs(toWidth - fromWidth) / speed;

        while (elapsedTime < duration)
        {
            float currentWidth = Mathf.Lerp(fromWidth, toWidth, elapsedTime / duration);
            newScale.x = currentWidth;
            spriteTransform.localScale = newScale;

            // Ajuste a posição para esticar na direção correta
            newPosition.x = toRight ? originalLocalPosition.x + (currentWidth - minWidth) / 2
                                    : originalLocalPosition.x - (currentWidth - minWidth) / 2;
            spriteTransform.localPosition = newPosition;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        newScale.x = toWidth;
        spriteTransform.localScale = newScale;

        // Ajuste a posição final
        newPosition.x = toRight ? originalLocalPosition.x + (toWidth - minWidth) / 2
                                : originalLocalPosition.x - (toWidth - minWidth) / 2;
        spriteTransform.localPosition = newPosition;
    }

    void Update()
    {
        // Acompanhar a posição do Player
        if (Player_Main.instance)
        {
            transform.position = Player_Main.instance.transform.position;

            // Verificar a direção do movimento do jogador
            if (Player_Main.instance.movement.x > 0 || Player_Main.instance.myInput.x > 0)
            {
                stretchingRight = true;
            }
            else if (Player_Main.instance.movement.x < 0 || Player_Main.instance.myInput.x < 0)
            {
                stretchingRight = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            Player_Stats.instance.ApplyDamageFromWhip(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
        }
    }
}
