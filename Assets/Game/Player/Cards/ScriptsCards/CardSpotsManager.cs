using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSpotsManager : MonoBehaviour
{
    public static CardSpotsManager instance;
    public List<CardSpot> cardSpots;
    public string CardType;
    public Image ImageDelete;
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


    // Start is called before the first frame update
    void Start()
    {
        switch (CardType)
        {
            case "RedWine":  break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
