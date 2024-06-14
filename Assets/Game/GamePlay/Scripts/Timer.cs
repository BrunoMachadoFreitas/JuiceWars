using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public delegate void TimerEndHandler();
    //public event TimerEndHandler OnTimerEnd;
    public static Timer instance; // Inst�ncia est�tica para acessar o timer de outras classes
    [SerializeField] private TextMeshProUGUI timerText; // Refer�ncia ao componente TextMeshProUGUI para mostrar o tempo restante

    public float CurrentTime; // Tempo restante no timer
    private bool isRunning; // Flag para indicar se o timer est� em execu��o

    public bool Notified = false;

    public delegate void OnTimerSecondHandler();
    public event TimerEndHandler OnTimerSecond;

    public float accumulatedTime = 0f; // Tempo acumulado desde o �ltimo evento
    private const float eventInterval = 1f; 
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
    }

    // M�todo para come�ar o timer com um tempo inicial
    public void StartTimer(float startTime)
    {
        timerText = GetComponent<TextMeshProUGUI>();
        CurrentTime = startTime;
        isRunning = true;
    }

    // M�todo para resetar o timer
    public void ResetTimer(float TimeToReset)
    {
        CurrentTime = TimeToReset;
        
    }

    // M�todo para verificar se o tempo restante � igual a zero
    public bool IsTimerZero()
    {
        return CurrentTime <= 0f;
    }

    private void Update()
    {
        UpdateTimerDisplay();
        // Atualiza o tempo restante se o timer estiver em execu��o
        if (isRunning)
        {

            // Incrementa o tempo acumulado desde o �ltimo evento
            accumulatedTime += Time.deltaTime;

            // Verifica se o tempo acumulado atingiu o intervalo desejado
            if (accumulatedTime >= eventInterval && GameStateController.instance.currentGameState == GameState.Playing)
                
            {
                // Dispara o evento de timer a cada segundo
                OnTimerSecond?.Invoke();

                // Reinicia o tempo acumulado
                accumulatedTime = 0f;
            }


            CurrentTime += Time.deltaTime;
            
          
        }
    }

    // M�todo para atualizar a exibi��o do timer no TextMeshProUGUI
    private void UpdateTimerDisplay()
    {
        // Obt�m os minutos e segundos
        int minutes = Mathf.FloorToInt(CurrentTime / 60f);
        int seconds = Mathf.FloorToInt(CurrentTime % 60f);
        // Formata o tempo como texto
        string timerString = string.Format("{0}:{1:00}", minutes, seconds);
        // Atualiza o texto do TextMeshProUGUI
        timerText.text = timerString;
    }


    public bool setIsRunning()
    {
        return isRunning = true; 
    }

    public float GetTimeRemaining()
    {
        return CurrentTime; 
    }
}
