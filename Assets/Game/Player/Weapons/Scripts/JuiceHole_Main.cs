using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuiceHole_Main : MonoBehaviour
{
    public float pullForce = 1f; // For�a com a qual os inimigos ser�o puxados
    public float orbitSpeed = .2f; // Velocidade da �rbita em torno do buraco negro
    public float spaghettificationTime = 1f; // Tempo para espaguetifica��o

    private List<GameObject> objectsInRange = new List<GameObject>();
    private Dictionary<GameObject, Coroutine> damageCoroutines = new Dictionary<GameObject, Coroutine>();
    private Dictionary<GameObject, Vector3> originalScales = new Dictionary<GameObject, Vector3>();

    void Start()
    {
        StartCoroutine(BuracoNegroEffect());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            objectsInRange.Add(other.gameObject);
            Monster monster = other.GetComponent<Monster>();
            if (monster != null)
            {
                monster.moveSpeed = 0; // Supondo que a classe Move_Monster tenha uma propriedade moveSpeed
            }

            if (!damageCoroutines.ContainsKey(other.gameObject))
            {
                Coroutine damageCoroutine = StartCoroutine(ApplyDamage(other.gameObject));
                damageCoroutines.Add(other.gameObject, damageCoroutine);
            }

            if (!originalScales.ContainsKey(other.gameObject))
            {
                originalScales.Add(other.gameObject, other.transform.localScale);
                StartCoroutine(Spaghettify(other.transform, true));
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            objectsInRange.Remove(other.gameObject);
            Monster monster = other.GetComponent<Monster>();
            if (monster != null)
            {
                monster.moveSpeed = monster.CurrentMoveSpeed; // Supondo que a classe Move_Monster tenha um m�todo para redefinir a velocidade de movimento
                monster.transform.rotation = Quaternion.identity; // Supondo que a classe Move_Monster tenha um m�todo para redefinir a rota��o
            }

            if (damageCoroutines.ContainsKey(other.gameObject))
            {
                StopCoroutine(damageCoroutines[other.gameObject]);
                damageCoroutines.Remove(other.gameObject);
            }

            if (originalScales.ContainsKey(other.gameObject))
            {
                StartCoroutine(Spaghettify(other.transform, false));
                originalScales.Remove(other.gameObject);
            }
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < objectsInRange.Count; i++)
        {
            GameObject obj = objectsInRange[i];
            if (obj != null)
            {
                Vector2 directionToCenter = (Vector2)transform.position - (Vector2)obj.transform.position;
                float distanceToCenter = directionToCenter.magnitude;
                Vector2 directionNormalized = directionToCenter.normalized;

                // Aplicar uma for�a centr�fuga para manter os objetos orbitando
                Vector2 perpendicularDirection = new Vector2(-directionNormalized.y, directionNormalized.x);
                obj.transform.position += (Vector3)(perpendicularDirection * orbitSpeed * Time.deltaTime);

                // Aplicar uma for�a reduzida em dire��o ao centro para pux�-los gradualmente
                obj.transform.position += (Vector3)(directionNormalized * pullForce * Time.deltaTime);

                // Aplicar efeito de �rbita girando em torno do buraco negro
                obj.transform.RotateAround(transform.position, Vector3.forward, orbitSpeed * Time.deltaTime);
            }
        }
    }

    IEnumerator ApplyDamage(GameObject monsterObject)
    {
        Monster monster = monsterObject.GetComponent<Monster>();
        while (monster != null)
        {
            monster.TakeDamageMonsterBlackHole(Player_Stats.instance.damagePerSecondJuiceHole);
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator BuracoNegroEffect()
    {
        yield return new WaitForSeconds(Player_Stats.instance.JuiceHoleDuration);
        Destroy(gameObject);
    }

    IEnumerator Spaghettify(Transform monsterTransform, bool isEntering)
    {
        if (monsterTransform == null)
            yield break;

        Vector3 originalScale = monsterTransform.localScale;
        Vector3 targetScale = isEntering ? new Vector3(originalScale.x * 0.5f, originalScale.y * 2f, originalScale.z) : originalScale;

        float elapsedTime = 0f;
        while (elapsedTime < spaghettificationTime)
        {
            if (monsterTransform == null)
                yield break;

            monsterTransform.localScale = Vector3.Lerp(monsterTransform.localScale, targetScale, (elapsedTime / spaghettificationTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (monsterTransform != null)
        {
            monsterTransform.localScale = targetScale;
        }
    }
}
