using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatsOnUiPowerUps : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI txtLife;
    [SerializeField] TextMeshProUGUI txtCurrentLife;
    [SerializeField] TextMeshProUGUI txtSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        txtLife.text = Player_Stats.instance.Life.ToString();
        txtCurrentLife.text = Player_Stats.instance.CurrentLife.ToString();
        txtSpeed.text = Player_Stats.instance.CurrentSpeed.ToString();
    }
}
