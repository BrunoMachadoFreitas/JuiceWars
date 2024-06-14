using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ChoosePowerUpUiUiAndStartRound : MonoBehaviour
{
    
    //private float currentTimeRound = 10f;

    

    [SerializeField] TextMeshProUGUI textLife;
    [SerializeField] TextMeshProUGUI textSpeed;
    [SerializeField] TextMeshProUGUI textReloadTime;
    [SerializeField] TextMeshProUGUI textRange;
    [SerializeField] TextMeshProUGUI textLifeSteal;

    public GameObject FadeObjectX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textLife.text = Player_Stats.instance.Life.ToString();
        textSpeed.text = Player_Stats.instance.moveSpeed.ToString();
        textReloadTime.text = Player_Stats.instance.RealoadTime.ToString();
        textRange.text = Player_Stats.instance.Range.ToString();
        textLifeSteal.text = Player_Stats.instance.LifeSteal.ToString();
    }

   

   
}
