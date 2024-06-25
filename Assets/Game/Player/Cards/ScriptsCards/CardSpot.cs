using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class CardSpot : MonoBehaviour
{
    [SerializeField] public CardType CardType;
    public bool IsCardActivated;


    [SerializeField] private Animator CardSpotAnimator;

    int countRotates = 0;
    [SerializeField] Sprite spriteCard;

    public bool HasCard;

    [SerializeField] private Card Card;
    public Card CardX;

    [SerializeField] private TextMeshProUGUI textDescCard;

    public bool IsCardSpotStucked;
    public GameObject ImageCardSpotStucked;


    public float cardPowerModifier;
    [SerializeField] private TextMeshProUGUI txtcardPowerModifier;


    



    // Start is called before the first frame update
    void Start()
    {
        HasCard = false;
        CardSpotAnimator = GetComponent<Animator>();

        switch (txtcardPowerModifier.gameObject.tag)
        {
            case "Modifier 1": cardPowerModifier = 2; txtcardPowerModifier.text = "+" + cardPowerModifier.ToString(); break; 
            case "Modifier 2": cardPowerModifier = 3; txtcardPowerModifier.text = "+" + cardPowerModifier.ToString(); break; 
            case "Modifier 3": cardPowerModifier = 4; txtcardPowerModifier.text = "+" + cardPowerModifier.ToString(); break; 
            case "Modifier 4": cardPowerModifier = 5; txtcardPowerModifier.text = "+" + cardPowerModifier.ToString(); break; 
            case "Modifier 5": cardPowerModifier = 6; txtcardPowerModifier.text = "+" + cardPowerModifier.ToString(); break; 
        }

        if (IsCardSpotStucked)
        {
            ImageCardSpotStucked.SetActive(true);
        }
        else
        {
            ImageCardSpotStucked.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private Coroutine hideTextCoroutine;

    public void InstantiateCard()
    {
        switch (CardType)
        {

            case CardType.RedWine:
                CardX = Instantiate(Card, this.transform.position, Quaternion.identity);

                CardX.transform.SetParent(this.transform);
                CardX.transform.GetChild(1).GetComponent<Image>().sprite = PowerUpsManager.instance.ImagesForButtonsCards[3];
                CardX.transform.localScale = new Vector3(1f, 1f, 0f);
                CardX.AssociatedCardSpot = this;
                HasCard = true;
                CardX.InitializePowerUp(CardType);
                textDescCard = CardX.textDesc;
                Player_Stats.instance.ListCards.Add(CardX);
                break;


            case CardType.Tequilla:
                CardX = Instantiate(Card, this.transform.position, Quaternion.identity);
                CardX.transform.SetParent(this.transform);
                CardX.transform.GetChild(1).GetComponent<Image>().sprite = PowerUpsManager.instance.ImagesForButtonsCards[2];
                CardX.AssociatedCardSpot = this;
                HasCard = true;
                CardX.InitializePowerUp(CardType);
                textDescCard = CardX.textDesc;
                Player_Stats.instance.ListCards.Add(CardX);
                break;
            case CardType.JuiceHole:
                CardX = Instantiate(Card, this.transform.position, Quaternion.identity);
                CardX.gameObject.SetActive(true);
                CardX.transform.SetParent(this.transform);
                CardX.transform.GetChild(1).GetComponent<Image>().sprite = PowerUpsManager.instance.ImagesForButtonsCards[1];
                CardX.AssociatedCardSpot = this;
                HasCard = true;
                CardX.InitializePowerUp(CardType);
                textDescCard = CardX.textDesc;
                break;
            case CardType.JuiceCollect:
                CardX = Instantiate(Card, this.transform.position, Quaternion.identity);
                CardX.gameObject.SetActive(true);
                CardX.transform.SetParent(this.transform);
                CardX.transform.GetChild(1).GetComponent<Image>().sprite = PowerUpsManager.instance.ImagesForButtonsCards[0];
                CardX.AssociatedCardSpot = this;
                HasCard = true;
                CardX.InitializePowerUp(CardType);
                textDescCard = CardX.textDesc;
                Player_Stats.instance.ListCards.Add(CardX);
                break;

            case CardType.GreenTea:
                CardX = Instantiate(Card, this.transform.position, Quaternion.identity);
                CardX.gameObject.SetActive(true);
                CardX.transform.SetParent(this.transform);
                CardX.transform.GetChild(1).GetComponent<Image>().sprite = PowerUpsManager.instance.ImagesForButtonsCards[4];
                CardX.AssociatedCardSpot = this;
                HasCard = true;
                CardX.InitializePowerUp(CardType);
                textDescCard = CardX.textDesc;
                Player_Stats.instance.ListCards.Add(CardX);
                break;

            case CardType.CoffeeCard:
                CardX = Instantiate(Card, this.transform.position, Quaternion.identity);
                CardX.gameObject.SetActive(true);
                CardX.transform.SetParent(this.transform);
                CardX.transform.GetChild(1).GetComponent<Image>().sprite = PowerUpsManager.instance.ImagesForButtonsCards[5];
                CardX.AssociatedCardSpot = this;
                HasCard = true;
                CardX.InitializePowerUp(CardType);
                textDescCard = CardX.textDesc;
                Player_Stats.instance.ListCards.Add(CardX);
                break;

            case CardType.TonicOfThePhoenixCard:
                CardX = Instantiate(Card, this.transform.position, Quaternion.identity);
                CardX.gameObject.SetActive(true);
                CardX.transform.SetParent(this.transform);
                CardX.transform.GetChild(1).GetComponent<Image>().sprite = PowerUpsManager.instance.ImagesForButtonsCards[6];
                CardX.AssociatedCardSpot = this;
                HasCard = true;
                CardX.InitializePowerUp(CardType);
                textDescCard = CardX.textDesc;
                Player_Stats.instance.ListCards.Add(CardX);
                break;
                
        }
    }

   
    public void ShowText()
    {
        if (countRotates == 0)
        {
            textDescCard.gameObject.SetActive(true);
            countRotates++;

            // Inicia a Coroutine para esconder o texto após 2 segundos
            if (hideTextCoroutine != null)
            {
                StopCoroutine(hideTextCoroutine);
            }
            hideTextCoroutine = StartCoroutine(HideTextAfterDelay(2.0f));
        }
        else if (countRotates > 0)
        {
            textDescCard.gameObject.SetActive(false);
            countRotates = 0;

            // Cancela a Coroutine se o texto foi manualmente escondido
            if (hideTextCoroutine != null)
            {
                StopCoroutine(hideTextCoroutine);
                hideTextCoroutine = null;
            }
        }
    }

    private IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        textDescCard.gameObject.SetActive(false);
        countRotates = 0;
        hideTextCoroutine = null; // Reseta a referência da Coroutine
    }
    public void RotateCardSpot()
     {

        if (countRotates == 0)
        {
            textDescCard.gameObject.SetActive(true);
            countRotates++;
        }
        else if (countRotates > 0)
        {
            textDescCard.gameObject.SetActive(false);
            countRotates = 0;
        }
        //
        //    CardSpotAnimator.SetTrigger("Rotate");
        //    StartCoroutine(WaitAnimation());


            //    CardX.transform.GetChild(0).GetComponent<Image>().sprite = null;
            //    CardX.transform.GetChild(1).gameObject.SetActive(true); 

            //}else if(countRotates > 0)
            //{
            //    CardSpotAnimator.SetTrigger("Rotate");
            //    StartCoroutine(WaitAnimation());
            //    if(CardType == CardType.RedWine) {
            //        CardX.transform.GetChild(0).GetComponent<Image>().sprite = PowerUpsManager.instance.ImagesForButtons[6];
            //    }
            //    if (CardType == CardType.Tequilla)
            //    {
            //        CardX.transform.GetChild(0).GetComponent<Image>().sprite = PowerUpsManager.instance.ImagesForButtons[7];
            //    }
            //    CardX.transform.GetChild(1).gameObject.SetActive(false);
            //    countRotates = 0;
            //}
    }

    IEnumerator WaitAnimation()
    {
        yield return new WaitForSeconds(0.3f);
    }
  
    public void UpdateChild()
    {
        if (transform.childCount > 0)
        {
            Card = transform.GetChild(0).GetComponent<Card>();
        }
        else
        {
            Card = null;
        }
    }

}
