using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Player_Main : MonoBehaviour
{
    //Variavel para controlo da plaforma
    public TargetPlatform TargetPlatform;


    // Implementação dos membros da interface


    public static Player_Main instance;

    [SerializeField]
    private TextMeshProUGUI textMoney;

    // Variáveis para controlar a intensidade e duração do shake
    public float shakeIntensity = 0.0001f;
    public float shakeDuration = 0.1f;

    // Posição original da câmera
    private Vector3 originalPosition;

    [SerializeField] public Animator AnimPlayer;
    Rigidbody2D rb; // Referência ao Rigidbody2D do personagem

    public Vector2 myInput; // Vetor para armazenar o movimento mobile
    public Vector2 movement; // Vetor para armazenar o movimento

    public BoxCollider2D ColliderDamage;
    public bool HasLuck = false;

    private float closestDistance = 1.8f;
    public List<GameObject> WeaponsSpots = new List<GameObject>();
    public List<GameObject> Weapons = new List<GameObject>();


    [SerializeField] private bool UsedMobile = false;


    public bool CanMov = true;

    


    //Instanciar canvas UI
    public Canvas CanvasInGamePlayer;
    public Canvas CanvasInGamePlayerAux;
    public Canvas CanvasExp;
    public Camera MainCamera;
    public Camera MainCameraAux;


    //UI
    public int EnemysKilled = 0;
    [SerializeField] private TextMeshProUGUI TextEnemysKilled;
    [SerializeField] private UnityEngine.UI.Image HealthBar;
    [SerializeField] private TextMeshProUGUI TextHealth;
    public int Money = 0;
    public Transform PopUpTransform;
    public GameObject floatingTextPrefab;






    //Shield
    private bool ActiveJuice = false;
    [SerializeField] private GameObject JuiceShieldChild;
    [SerializeField] private GameObject Whip;
    [SerializeField] private GameObject ImageLowLife;

    //DASH
    [SerializeField] private GameObject SpriteDash;
    [SerializeField] private GameObject spriteDash;
    [SerializeField] private GameObject TrailDash;
    [SerializeField] private UnityEngine.UI.Image ImageDash;

    [SerializeField] private RectTransform dashButtonRect; 
    public RectTransform movementButtonRect;


    public GameObject juiceHolePrefab; // Referência ao prefab do JuiceHole
    public GameObject currentJuiceHole;

    public GameObject WhipPrefab; // Referência ao prefab do JuiceHole
    private GameObject currentWhip;

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
        AnimPlayer = GetComponent<Animator>(); // Obtém o componente Rigidbody2D do personagem

        //CanvasInGamePlayerAux =  Instantiate(CanvasInGamePlayer, new Vector2(), Quaternion.identity);
        MainCameraAux = Instantiate(MainCamera, this.transform.position, Quaternion.identity);
        
    }



    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtém o componente Rigidbody2D do personagem
        ColliderDamage = GetComponent<BoxCollider2D>(); // Obtém o componente Rigidbody2D do personagem


        // Inicializa a vida atual com o valor da vida total
        Player_Stats.instance.CurrentLife = Player_Stats.instance.Life;
        Player_Stats.instance.PlayerDodge = 0f;
        Player_Stats.instance.CurrentSpeed = Player_Stats.instance.moveSpeed;

        if (Player_Stats.instance.HasWhip)
        {
            currentWhip = Instantiate(WhipPrefab, this.transform.position, Quaternion.identity);
            currentWhip.transform.position = new Vector3(0, 0);
            currentWhip.transform.SetParent(this.transform, false);
        }

        //Instantiate(Whip, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
    }
   

    void Update()
    {


        textMoney.text = Money.ToString();
        TextEnemysKilled.text = EnemysKilled.ToString();

        if (Player_Stats.instance.HasLuck && !isCoroutineLuckRunning)
        {
            isCoroutineLuckRunning = true;
            InvokeRepeating("LuckCoroutine", 0f, 5f);
        }

    }
    private bool isCoroutineLuckRunning = false;
    private IEnumerator LuckCoroutine()
    {
        int randomValue = UnityEngine.Random.Range(0, 100);

        // Verifica se o valor aleatório é maior que 50 para aplicar o efeito
        if (randomValue < Player_Stats.instance.LuckyValue)
        {
            for (int i = 0; i < WaveManager.instance.Monsters.Count; i++)
            {
                if (WaveManager.instance.Monsters[i] != null)
                {
                    int choosedEnemy = UnityEngine.Random.Range(0, WaveManager.instance.Monsters.Count);
                    Destroy(WaveManager.instance.Monsters[choosedEnemy]);
                    WaveManager.instance.Monsters.RemoveAt(choosedEnemy);
                    break;
                }
            }
        }
        isCoroutineLuckRunning = false;
        randomValue = 0;

        yield return null; // Pode ser alterado conforme a necessidade
    }
    private void MovePlayerKeyboard()
    {
        Vector2 lastMovement = Vector2.right;
        UsedMobile = false;
        // Input do jogador nas direções horizontal e vertical
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Define o vetor de movimento baseado no input
        movement = new Vector2(moveX, moveY).normalized;

        if (movement == Vector2.zero) TargetPlatform = TargetPlatform.PC;



        // Aplica a escala de acordo com a última direção de movimento
        Vector3 localScale = transform.localScale;
        if (movement.x < 0)
        {
            localScale.x = 0.9916076f; // Vira para a direita
            localScale.y = 1.23547f;

        }
        else
        {
            localScale.x = -0.9916076f; // Vira para a esquerda
            localScale.y = 1.23547f;
        }
        VectorCurrentPlatform = movement;
        transform.GetChild(2).GetComponent<SpriteRenderer>().transform.localScale = localScale;
    }

   

    public void MovePlayer(InputAction.CallbackContext value)
    {
        TargetPlatform = TargetPlatform.Android;
        myInput = value.ReadValue<Vector2>();
        Vector2 lastMovement = Vector2.right;
        if (myInput == Vector2.zero) TargetPlatform = TargetPlatform.Android;

        if (myInput != Vector2.zero)
        {
            lastMovement = movement;
        }

        Vector3 localScale = transform.localScale;
        if (lastMovement.x < 0)
        {
            localScale.x = -0.9916076f;
            localScale.y = 1.23547f;
        }
        else
        {
            localScale.x = 0.9916076f;
            localScale.y = 1.23547f;
        }
        VectorCurrentPlatform = myInput;
        transform.GetChild(2).GetComponent<SpriteRenderer>().transform.localScale = localScale;
    }

    Vector2 VectorCurrentPlatform = Vector2.zero;
    void FixedUpdate()
    {



        if (CanMov)
        {
            if (TargetPlatform == TargetPlatform.PC)
                MovePlayerKeyboard();


            //if (TargetPlatform == TargetPlatform.Android)
            //{
                rb.MovePosition(rb.position + VectorCurrentPlatform * Player_Stats.instance.CurrentSpeed * Time.fixedDeltaTime);
            //}
            //else
            //{

            //    rb.MovePosition(rb.position + movement * Player_Stats.instance.CurrentSpeed * Time.fixedDeltaTime);
            //}
        }
        else
        {
            AnimPlayer.SetBool("Moving", false);
        }



        if (movement != Vector2.zero || myInput != Vector2.zero)
        {
            AnimPlayer.SetBool("Moving", true);


        }
        else
        {
            AnimPlayer.SetBool("Moving", false);
        }

        if (Player_Stats.instance.CurrentLife <= 10)
        {
            if (!SoundManager.instance.GameSoundsInGame[8].isPlaying)
            {
                SoundManager.instance.GameSoundsInGame[8].Play();
                ImageLowLife.SetActive(true);
            }
        }
        else
        {
            if (SoundManager.instance.GameSoundsInGame[8].isPlaying)
            {
                ImageLowLife.SetActive(false);
                SoundManager.instance.GameSoundsInGame[8].Stop();
            }
        }

        if (Player_Stats.instance.CurrentLife <= 0)
        {
            Player_Stats.instance.CurrentLife = 0;
        }
        else if (Player_Stats.instance.CurrentLife > Player_Stats.instance.Life)
        {
            Player_Stats.instance.CurrentLife = Player_Stats.instance.Life;
        }
        HealthBar.fillAmount = Mathf.Clamp(Player_Stats.instance.CurrentLife / Player_Stats.instance.Life, 0, 1);
        //if(Player_Stats.instance.InDash && ImageDash.fillAmount < 1)

        if(movement != Vector2.zero || myInput != Vector2.zero)
        {
            if(!SoundManager.instance.GameSoundsInGame[11].isPlaying)
            SoundManager.instance.PlaySound(11);
        }
        else
        {
            SoundManager.instance.GameSoundsInGame[11].Stop();
        }
    }


    public void DashTrigger()
    {
        if ((movement != Vector2.zero && !Player_Stats.instance.InDash) || (myInput != Vector2.zero && !Player_Stats.instance.InDash))
        {
            Player_Stats.instance.InDash = true;

            spriteDash = Instantiate(SpriteDash, this.transform.position, Quaternion.identity);
            spriteDash.transform.localScale = new Vector3(.5f, .5f, 0f);

            TrailDash.SetActive(true);

            GetComponent<BoxCollider2D>().enabled = false;

            Player_Stats.instance.CurrentSpeed = Player_Stats.instance.DashSpeed;

            Invoke("PosDash", 0.1f);
        }
    }

    public void PosDash()
    {
        Player_Stats.instance.CurrentSpeed = Player_Stats.instance.moveSpeed;
        Destroy(spriteDash);
        TrailDash.SetActive(false);
        GetComponent<BoxCollider2D>().enabled = true;
        Invoke("EndDash", Player_Stats.instance.DashRechargeTime);
        StartCoroutine(DashRechargeCoroutine());
    }

    private IEnumerator DashRechargeCoroutine()
    {
        float rechargeTime = Player_Stats.instance.DashRechargeTime;
        float elapsedTime = 0f;

        while (elapsedTime < rechargeTime)
        {
            elapsedTime += Time.deltaTime;
            ImageDash.fillAmount = Mathf.Clamp01(elapsedTime / rechargeTime);
            yield return null;
        }

        // Ao final do timer, garantir que o fillAmount esteja em 1
        ImageDash.fillAmount = 1f;
        Player_Stats.instance.InDash = false; // Permitir dash novamente
    }
    public void EndDash()
    {
        Player_Stats.instance.InDash = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (!WaveManager.instance.isWaveStopped)
        {
            if (collision.gameObject.CompareTag("Money"))
            {
                Money++;
                WaveManager.instance.MoneyCollect.Remove(collision.gameObject);
                Destroy(collision.gameObject);
                Destroy(collision.gameObject.transform.parent.gameObject);
            }
            if (collision.gameObject.CompareTag("Exp"))
            {
                Player_Stats.instance.ExpGiveCalculator();
                Destroy(collision.gameObject);
                //Destroy(collision.gameObject.transform.parent.gameObject);
            }

            if (!ActiveJuice)
            {

                if (collision.gameObject.CompareTag("Monster"))
                {
                    float dodgeValue = DodgeCalculation();
                    if (dodgeValue > Player_Stats.instance.PlayerDodge)
                    {
                        Player_Stats.instance.CurrentLife -= 2;
                        GameObject floatingTextObject = Instantiate(floatingTextPrefab, PopUpTransform.position, Quaternion.identity);
                        floatingTextObject.transform.SetParent(PopUpTransform);
                        // Configura o texto do Floating Text com o número do dano infligido
                        TextMeshPro floatingText = floatingTextObject.GetComponentInChildren<TextMeshPro>();
                        floatingText.color = Color.white;
                        floatingText.text = "OUCH!";
                        if (Player_Stats.instance.CurrentLife <= 0f)
                        {
                            GameStateController.instance.currentGameState = GameState.Ended;
                        }
                        ShakeCamera();
                    }
                    else
                    {
                    }
                }
                if (collision.gameObject.CompareTag("MonsterBrainBullet"))
                {
                    float dodgeValue = DodgeCalculation();
                    if (dodgeValue > Player_Stats.instance.PlayerDodge)
                    {
                        Player_Stats.instance.CurrentLife -= 2;
                        GameObject floatingTextObject = Instantiate(floatingTextPrefab, PopUpTransform.position, Quaternion.identity);
                        floatingTextObject.transform.SetParent(PopUpTransform);
                        // Configura o texto do Floating Text com o número do dano infligido
                        TextMeshPro floatingText = floatingTextObject.GetComponentInChildren<TextMeshPro>();
                        floatingText.color = Color.white;
                        floatingText.text = "OUCH!";
                        if (Player_Stats.instance.CurrentLife <= 0f)
                        {
                            GameStateController.instance.currentGameState = GameState.Ended;
                        }
                        ShakeCamera();
                    }
                    else
                    {
                    }
                }

            }
            if (collision.gameObject.CompareTag("JuiceShield"))
            {
                ActiveJuice = true;
                JuiceShieldChild.SetActive(true);
                StartCoroutine(DeactivateJuiceShieldAfterDelay(5f)); // Inicia a coroutine para desativar após 5 segundos

            }

        }
    }

    private IEnumerator DeactivateJuiceShieldAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Espera pelo tempo especificado
        ActiveJuice = false; // Desativa o Juice
        JuiceShieldChild.SetActive(false); // Desativa o objeto
    }
    public float DodgeCalculation()
    {
        // Calcula um número aleatório entre 0 e 100
        int randomValue = UnityEngine.Random.Range(0, 100);
        if (randomValue <= Player_Stats.instance.PlayerDodge)
        {
            return randomValue;
        }
        else
        {
            return 1f;
        }

    }

    private void TakeDamage()
    {
        if (!ActiveJuice)
        {


            float dodgeValue = DodgeCalculation();
            if (dodgeValue > Player_Stats.instance.PlayerDodge)
            {
                Player_Stats.instance.CurrentLife -= 1f;
                GameObject floatingTextObject = Instantiate(floatingTextPrefab, PopUpTransform.position, Quaternion.identity);
                floatingTextObject.transform.SetParent(PopUpTransform);
                // Configura o texto do Floating Text com o número do dano infligido
                TextMeshPro floatingText = floatingTextObject.GetComponentInChildren<TextMeshPro>();
                floatingText.color = Color.white;
                floatingText.text = "OUCH!";
                ShakeCamera();
            }
            else
            {
            }
        }

    }

    // Chamado para iniciar o shake
    public void ShakeCamera()
    {
        if (transform.position != Vector3.zero)
        {


            originalPosition = Camera.main.gameObject.transform.localPosition; // Salva a posição original da câmera
            InvokeRepeating("DoShake", 0, shakeIntensity); // Começa a chamar a função DoShake repetidamente
            Invoke("StopShaking", shakeDuration); // Para o shake após a duração especificada

        }
    }

    // Função para realizar o shake
    private void DoShake()
    {
        float shakeAmountX = UnityEngine.Random.Range(-1f, 1f) * shakeIntensity;
        float shakeAmountY = UnityEngine.Random.Range(-1f, 1f) * shakeIntensity;

        // Aplica o shake à posição da câmera
        Camera.main.gameObject.transform.localPosition = originalPosition + new Vector3(shakeAmountX, shakeAmountY, 0);
    }

    // Função para parar o shake
    private void StopShaking()
    {
        CancelInvoke("DoShake"); // Para de chamar a função DoShake
        Camera.main.gameObject.transform.localPosition = originalPosition; // Retorna a câmera à sua posição original
    }

    public bool CanJuiceHole = false;

   
}
