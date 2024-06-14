using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Main : MonoBehaviour
{

    public string CardType;
    // Start is called before the first frame update
    void Start()
    {
        switch (CardType)
        {
            case "RedWineCard": Player_Stats.instance.ExpToGive += (Player_Stats.instance.ExpToGive * 0.1f) + Player_Stats.instance.ExpToGive; break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
