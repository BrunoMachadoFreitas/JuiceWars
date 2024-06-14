using System.Collections;
using UnityEngine;

public class Whip_Main : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private BoxCollider2D boxCollider;
    private Vector3 whipStartPos;
    private Vector3 whipEndPos;
    public float whipLength = 10f; // Comprimento do chicote (10 unidades)
    public float whipSpeed = .7f; // Velocidade de esticamento do chicote
    public float pauseDuration = .4f; // Duração da pausa entre os movimentos
    public Transform WhipPos;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // Inicializa o tamanho e a posição do BoxCollider2D
        boxCollider.size = new Vector2(0f, 0.1f); // Inicialmente colapsado
        boxCollider.offset = Vector2.zero;

        // Começa a corrotina de animação
        StartCoroutine(WhipAnimation());
    }

    IEnumerator WhipAnimation()
    {
        while (true)
        {
            // Esticar o chicote
            yield return StartCoroutine(StretchWhip());
            // Pausa entre os movimentos
            yield return new WaitForSeconds(pauseDuration);
            // Recolher o chicote
            yield return StartCoroutine(RetractWhip());
            // Pausa entre os movimentos
            yield return new WaitForSeconds(pauseDuration * 2f);
        }
    }

    IEnumerator StretchWhip()
    {
        float elapsedTime = 0f;

        while (elapsedTime < whipSpeed)
        {
            // Atualiza a posição do jogador durante a animação
            whipStartPos = WhipPos.position;
            whipEndPos = whipStartPos + Vector3.right * whipLength;

            float t = elapsedTime / whipSpeed;
            Vector3 currentPosition = Vector3.Lerp(whipStartPos, whipEndPos, t);

            lineRenderer.SetPosition(0, whipStartPos);
            lineRenderer.SetPosition(1, currentPosition);

            // Atualiza o BoxCollider2D
            float currentLength = Vector3.Distance(whipStartPos, currentPosition);
            boxCollider.size = new Vector2(currentLength, 0.1f);
            boxCollider.offset = new Vector2(currentLength / 2, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        lineRenderer.SetPosition(0, whipStartPos);
        lineRenderer.SetPosition(1, whipEndPos);

        // Ajuste final do BoxCollider2D
        boxCollider.size = new Vector2(whipLength + 17, 0.1f);
        boxCollider.offset = new Vector2(whipLength, 0);
    }

    IEnumerator RetractWhip()
    {
        float elapsedTime = 0f;

        while (elapsedTime < whipSpeed)
        {
            // Atualiza a posição do jogador durante a animação
            whipStartPos = WhipPos.position;
            whipEndPos = whipStartPos + Vector3.right * whipLength;

            float t = elapsedTime / whipSpeed;
            Vector3 currentPosition = Vector3.Lerp(whipEndPos, whipStartPos, t);

            lineRenderer.SetPosition(0, whipStartPos);
            lineRenderer.SetPosition(1, currentPosition);

            // Atualiza o BoxCollider2D
            float currentLength = Vector3.Distance(whipStartPos, currentPosition);
            boxCollider.size = new Vector2(currentLength, 0.1f);
            boxCollider.offset = new Vector2(currentLength / 2, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        lineRenderer.SetPosition(0, whipStartPos);
        lineRenderer.SetPosition(1, whipStartPos);

        // Ajuste final do BoxCollider2D
        boxCollider.size = new Vector2(0f, 0.1f); // Colapsado
        boxCollider.offset = Vector2.zero;
    }

    void Update()
    {
        // Atualiza a posição inicial e final do chicote
        whipStartPos = WhipPos.position;
        whipEndPos = whipStartPos + Vector3.right * whipLength;

        // Calcula o vetor de direção do jogador para o inimigo
        Vector2 direction = Player_Main.instance.transform.position - transform.position;
        direction.Normalize();
        // Move o inimigo na direção do jogador com a velocidade especificada
        this.gameObject.GetComponent<Rigidbody2D>().MovePosition(this.gameObject.GetComponent<Rigidbody2D>().position + direction * 5f * Time.fixedDeltaTime);

        this.transform.position = WhipPos.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            Player_Stats.instance.ApplyDamage(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
        }
    }
}
