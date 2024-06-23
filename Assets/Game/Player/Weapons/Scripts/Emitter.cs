using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emitter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public int projectileCount = 10;
    public float emissionRate = 0.1f;
    public float launchForce = 5f;
    public float repeatRate = 5f;  // Intervalo de tempo para repetir a emissão

    private void Start()
    {
        ActivateEmitter();
    }

    public void ActivateEmitter()
    {
        StartCoroutine(RepeatedlyEmitProjectiles());
    }

    private IEnumerator RepeatedlyEmitProjectiles()
    {
        while (true)
        {
            yield return EmitProjectiles();
            yield return new WaitForSeconds(repeatRate);
        }
    }

    private IEnumerator EmitProjectiles()
    {
        for (int i = 0; i < projectileCount; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            float horizontalForce = Random.Range(-launchForce, launchForce);
            Vector2 force = new Vector2(horizontalForce, launchForce);
            rb.AddForce(force, ForceMode2D.Impulse);

            Destroy(projectile, 3f);  // Destrói o projetil após 3 segundos

            yield return new WaitForSeconds(emissionRate);
        }
    }
}
