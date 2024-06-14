using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Turret_Main : MonoBehaviour
{

    public Transform turretLaunch; // Ponto de lançamento da torreta
    private LineRenderer lineRenderer;
    private GameObject targetMonster; // Monstro alvo atual
    private Vector3 currentTargetPosition; // Posição atual do alvo
    private Vector3 targetPosition; // Posição do alvo
    float ReloadTime = 2f;
    public Transform aimTransform;
    public GameObject GoDamageTurret;

    bool reloading = false;

    [SerializeField] private Texture2D texture;
    // Start is called before the first frame update
    void Start()
    {

        if (transform.parent != null)
        {
            aimTransform = transform.parent;
        }
        else
        {
            Debug.LogError("Este GameObject não tem um pai na hierarquia.");
        }
        // Inicializa o componente LineRenderer
        lineRenderer = GetComponent<LineRenderer>();
        // Define a cor inicial da linha (opcional)
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;

        // Define a largura inicial da linha (opcional)
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        lineRenderer.material.SetTexture("_MainText", texture);

        // Inicializa a posição atual do alvo como a posição do ponto de lançamento da torreta
        currentTargetPosition = turretLaunch.position;

        
    }
    void Update()
    {
        if (!reloading)
        {
            // Verifica se há monstros disponíveis
            if (WaveManager.instance.Monsters != null && WaveManager.instance.Monsters.Count > 0)
            {
                // Escolhe um novo monstro aleatório
                targetMonster = WaveManager.instance.Monsters[Random.Range(0, WaveManager.instance.Monsters.Count)];
                if(targetMonster != null) { 
                    Vector2 direction = (targetMonster.transform.position - transform.position).normalized;

                    // Calcula o ângulo em graus
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                    aimTransform.eulerAngles = new Vector3(0, 0, angle);

                    Vector3 localScale = Vector3.zero;
                    if (angle > 90 || angle < -90)
                    {
                        localScale.y = -1f;
                        localScale.x = 1f;
                    }
                    else
                    {
                        localScale.y = +1f;
                        localScale.x = 1f;
                    }
                    aimTransform.localScale = localScale;
                }
                // Define a nova posição do alvo
                targetPosition = targetMonster.transform.position;

            
               // Escolhe um monstro aleatório
               targetMonster = WaveManager.instance.Monsters[Random.Range(0, WaveManager.instance.Monsters.Count)];

               // Define a posição do monstro alvo
               targetPosition = targetMonster.transform.position;
            

                // Define os pontos inicial e final da linha
                Vector3 startPoint = turretLaunch.position;
                Vector3 endPoint = targetPosition;

                // Define os pontos inicial e final da linha no LineRenderer
                lineRenderer.SetPosition(0, startPoint);
                lineRenderer.SetPosition(1, endPoint);

                Shoot();
            }
        }

        
            

            
        
    }

    private void Shoot()
    {
        currentTargetPosition = Vector3.MoveTowards(currentTargetPosition, targetPosition, 5f * Time.deltaTime);
        // Define a nova posição do objeto de dano
        GoDamageTurret.transform.position = targetPosition;
        reloading = true;
        StartCoroutine(ShootWait());
    }

    private IEnumerator ShootWait()
    {
        yield return new WaitForSeconds(0.2f);
        lineRenderer.enabled = false;
        GoDamageTurret.SetActive(false);
        yield return new WaitForSeconds(ReloadTime); // Aguarda o tempo de recarregamento
        reloading = false;

        lineRenderer.enabled = true;
        GoDamageTurret.SetActive(true);
    }
    
}

