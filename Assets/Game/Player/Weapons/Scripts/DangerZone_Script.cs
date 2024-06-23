using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZone_Script : MonoBehaviour
{

    public bool Exploded = false;
    private CircleCollider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<CircleCollider2D>();
        StartCoroutine(Explode());
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(0.5f); // Aguarda 2 segundos
        Exploded = true;
        collider.enabled = true;
        yield return new WaitForSeconds(0.2f);
        Destroy(this.gameObject);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o objeto colidido é o jogador
        
    }
}
