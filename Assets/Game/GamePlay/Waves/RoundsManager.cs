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
    //public event WaveManagerReadyHandler WaveManagerReady;

    [SerializeField] private SoundManager soundManager;
     public SoundManager soundManagerX;

    [SerializeField] private GameObject TimerGameObject;
    //[SerializeField] private GameObject MapController;
    //[SerializeField] private GameObject MapControllerX;


    //[SerializeField] private GameObject Chunck;
    //[SerializeField] private GameObject ChunckX;



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
        soundManagerX = Instantiate(soundManager, new Vector3(-487.442993f, 1.04410005f, 0), Quaternion.identity);
        //MapControllerX = Instantiate(MapController, new Vector3(-487.442993f, 1.04410005f, 0), Quaternion.identity);
        //ChunckX = Instantiate(Chunck, new Vector3(-487.442993f, 1.04410005f, 0), Quaternion.identity);
        
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
        
       
     
        //newwaveManagerPrefab.SetActive(false); 


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
