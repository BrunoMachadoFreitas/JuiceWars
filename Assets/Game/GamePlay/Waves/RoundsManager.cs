using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static WaveManager;

public class RoundsManager : MonoBehaviour
{
    public static RoundsManager instance;
    [SerializeField] GameObject waveManagerPrefab;
    private WaveManager currentWaveManager;
  

    public PowerUpsManager powerUpsManagerPrefab;

    public GameObject PowerUpsChoose;
    public GameObject newwaveManagerPrefab;
    public bool isChoosing = false;

    public int currentRound = 1;


    public GameObject PlayingArea;
    public GameObject PlayingAreaAux;
    

    public int CurrentTimeRound = 10;


    [SerializeField]private GameObject TextCountRounds;
    [SerializeField]private GameObject TextTime;
    [SerializeField]public Canvas CanvasMobile;
    public FadeObject FadeObjectX;


    public delegate void WaveManagerReadyHandler();

    

    [SerializeField] private GameObject TimerGameObject;




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

        PlayingAreaAux = Instantiate(PlayingArea, new Vector3(-487.442993f, 1.04410005f, 0), Quaternion.identity);
        PlayingAreaAux.transform.localScale = new Vector3(10f, 10f, 0f);

    }
    // Start is called before the first frame update
    void Start()
    {
        instantiateWaveManager();
        
        
        Timer.instance.StartTimer(0f);
    }


    public GameObject instantiateWaveManager()
    {
        newwaveManagerPrefab = Instantiate(waveManagerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        // Instancia o WaveManager inicial com uma duração de onda de 10 segundos
        newwaveManagerPrefab.gameObject.SetActive(true);
        
        newwaveManagerPrefab.GetComponent<WaveManager>().isWaveStopped = false;
        isChoosing = false;
     
        Timer.instance.OnTimerSecond += newwaveManagerPrefab.GetComponent<WaveManager>().HandleOnTimerSec;
        
        return newwaveManagerPrefab;
    }
    // Método que lida com o evento OnTimerEnd
    public void LevelingChoose()
    {
        StartCoroutine(WaitWaveManager());
        PowerUpsChoose.SetActive(true);

        isChoosing = true;
        
        powerUpsManagerPrefab.gameObject.SetActive(true);

        for (int i = 0; i < PowerUpsManager.instance.PowerUpObject.Count; i++)
        {
            PowerUpsManager.instance.RandomizePowerUpsForButtons(i);
        }


        PowerUpsChoose.GetComponent<Animator>().SetTrigger("GoButton");
        CanvasMobile.gameObject.SetActive(false);
    }

    IEnumerator WaitWaveManager()
    {
        yield return new WaitForSeconds(2f);
    }
    // Update is called once per frame
    void Update()
    {
   
            
        
    }

    
}
