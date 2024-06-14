using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyCollected: MonoBehaviour
{

    public int moneyForPlayer;

    [SerializeField] private Image MoneyGameObject;
    [SerializeField] private Animator AnimatorMoney;

    private void Awake()
    {
        

        DontDestroyOnLoad(transform.gameObject);
        MoneyGameObject = Player_Main.instance.CanvasInGamePlayerAux.gameObject.transform.GetChild(0).GetComponentInChildren<Image>();
        AnimatorMoney = MoneyGameObject.GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

 
    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (WaveManager.instance.isWaveStopped)
        {
            if (collision.gameObject.CompareTag("Money"))
            {
                
                Player_Main.instance.Money++;

                Destroy(collision.gameObject);
            }
        }

    }

}
