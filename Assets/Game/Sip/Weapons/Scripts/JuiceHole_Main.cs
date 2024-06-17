using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuiceHole_Main : MonoBehaviour
{
    public float pullForce = 1f; // For�a com a qual os inimigos ser�o puxados
    public float damagePerSecond = 1f; // Dano por segundo causado aos inimigos
    public float duration = 5f; // Dura��o do efeito
    public float orbitSpeed = .2f; // Velocidade da �rbita em torno do buraco negro

    private List<GameObject> objectsInRange = new List<GameObject>();
    private Dictionary<GameObject, Coroutine> damageCoroutines = new Dictionary<GameObject, Coroutine>();

    void Start()
    {
        StartCoroutine(BuracoNegroEffect());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            objectsInRange.Add(other.gameObject);
            Move_Monster monster = other.GetComponent<Move_Monster>();
            if (monster != null)
            {
                monster.moveSpeed = 0; // Supondo que a classe Monster tenha uma propriedade MoveSpeed
            }

            if (!damageCoroutines.ContainsKey(other.gameObject))
            {
                Coroutine damageCoroutine = StartCoroutine(ApplyDamage(other.gameObject));
                damageCoroutines.Add(other.gameObject, damageCoroutine);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            objectsInRange.Remove(other.gameObject);
            Move_Monster monster = other.GetComponent<Move_Monster>();
            if (monster != null)
            {
                monster.moveSpeed = monster.CurrentMoveSpeed; // Supondo que a classe Monster tenha um m�todo para redefinir a velocidade de movimento
                monster.transform.rotation = Quaternion.identity; // Supondo que a classe Monster tenha um m�todo para redefinir a velocidade de movimento
            }

            if (damageCoroutines.ContainsKey(other.gameObject))
            {
                StopCoroutine(damageCoroutines[other.gameObject]);
                damageCoroutines.Remove(other.gameObject);
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
            monster.TakeDamageMonsterBlackHole(damagePerSecond);
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator BuracoNegroEffect()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
