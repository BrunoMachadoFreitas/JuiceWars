using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public CardSpot AssociatedCardSpot;
    public int MultAssociated;

    public CardType thisCardType;
    [SerializeField] public TextMeshProUGUI textDesc;
    [SerializeField] private Canvas canvas;

    Vector2 initialPosition;

    [SerializeField] private UnityEngine.UI.Image BackGround;

    private RectTransform canvasRectTransform;
    private RectTransform backgroundRectTransform;
    private RectTransform cardRectTransform;

    private CardSpotsManager backgroundScript;

    private bool isDragging = false;
    private float clickThreshold = 22f; // Threshold in seconds to differentiate between click and drag
    private float clickTime = 0f;
    private Coroutine hideTextCoroutine;

    void Start()
    {
        canvas = transform.parent.parent.parent.GetComponent<Canvas>();
        BackGround = transform.parent.parent.GetComponent<UnityEngine.UI.Image>();
        initialPosition = transform.position;

        canvasRectTransform = canvas.GetComponent<RectTransform>();
        backgroundRectTransform = BackGround.GetComponent<RectTransform>();
        cardRectTransform = GetComponent<RectTransform>();

        backgroundScript = BackGround.GetComponent<CardSpotsManager>();
        AssociatedCardSpot.HasCard = true;
    }

    public void InitializePowerUp(CardType cType)
    {
        switch (cType)
        {
            case CardType.RedWine:
                float ExpCalculator = Player_Stats.instance.ExpToGive * 0.1f;
                float ExpCalculatorFinal = (ExpCalculator * Player_Stats.instance.ExpToGive) + AssociatedCardSpot.cardPowerModifier;
                Player_Stats.instance.ExpToGive += ExpCalculatorFinal;

                thisCardType = CardType.RedWine;
                Player_Stats.instance.CardsActive.Add(this);
                textDesc.text = "More Exp";
                break;

            case CardType.Tequilla:
                float velocityCalculator = Player_Stats.instance.CurrentSpeed * 0.1f;
                Player_Stats.instance.CurrentSpeed += (velocityCalculator) + AssociatedCardSpot.cardPowerModifier;
                Player_Stats.instance.moveSpeed += (velocityCalculator) + AssociatedCardSpot.cardPowerModifier;

                thisCardType = CardType.Tequilla;
                Player_Stats.instance.CardsActive.Add(this);

                textDesc.text = "More Velocity";
                break;

            case CardType.JuiceHole:
                thisCardType = CardType.JuiceHole;
                Player_Stats.instance.CardsActive.Add(this);
                Player_Stats.instance.JuiceHoleDuration += AssociatedCardSpot.cardPowerModifier;
                Player_Main.instance.CanJuiceHole = true;
                textDesc.text = "Spawns a mini black hole at a time";
                PlayerCardsManager.instance.StartJuiceHoleCoroutine();
                break;

            case CardType.JuiceCollect:
                thisCardType = CardType.JuiceCollect;
                Player_Stats.instance.CardsActive.Add(this);
                Player_Stats.instance.MagnetismFactor += 5 + AssociatedCardSpot.cardPowerModifier;
                textDesc.text = "More magnetism";
                break;

            case CardType.GreenTea:
                thisCardType = CardType.GreenTea;
                Player_Stats.instance.CardsActive.Add(this);
                Player_Stats.instance.GreenTeaRegneration += AssociatedCardSpot.cardPowerModifier;
                Player_Main.instance.CanGreenTea = true;
                textDesc.text = "Regenerates hp over time";
                PlayerCardsManager.instance.StartGreenTeaCoroutine();
                break;

            case CardType.CoffeeCard:
                thisCardType = CardType.CoffeeCard;
                Player_Stats.instance.CardsActive.Add(this);
                Player_Stats.instance.Power += AssociatedCardSpot.cardPowerModifier;
                textDesc.text = "Increases base power stat";
                break;
            case CardType.TonicOfThePhoenixCard:
                thisCardType = CardType.TonicOfThePhoenixCard;
                Player_Stats.instance.CardsActive.Add(this);
                Player_Stats.instance.TonicOfThePhoenixActivated = true;
                textDesc.text = "You don't die the next time, card it's destroyed";
                break;
                
        }
    }

    private void switchModifier(CardType cType)
    {

        switch (cType)
        {
            case CardType.RedWine:
                Player_Stats.instance.ExpToGive = 2;
                float ExpCalculator = Player_Stats.instance.ExpToGive * 0.1f;
                float ExpCalculatorFinal = (ExpCalculator * Player_Stats.instance.ExpToGive) + AssociatedCardSpot.cardPowerModifier;
                Player_Stats.instance.ExpToGive += ExpCalculatorFinal;
                thisCardType = CardType.RedWine;
                //textDesc.text = "More Exp";
                break;

            case CardType.Tequilla:
                Player_Stats.instance.CurrentSpeed = 7;
                Player_Stats.instance.moveSpeed = 7;
                float velocityCalculator = Player_Stats.instance.CurrentSpeed * 0.1f;
                Player_Stats.instance.CurrentSpeed += (velocityCalculator) + AssociatedCardSpot.cardPowerModifier;
                Player_Stats.instance.moveSpeed += (velocityCalculator) + AssociatedCardSpot.cardPowerModifier;
                thisCardType = CardType.Tequilla;
                //textDesc.text = "More Velocity";
                break;

            case CardType.JuiceHole:
                thisCardType = CardType.JuiceHole;
                //textDesc.text = "Spawns a mini black hole at a time";
                Player_Stats.instance.JuiceHoleDuration -= AssociatedCardSpot.cardPowerModifier;
                Player_Stats.instance.damagePerSecondJuiceHole -= AssociatedCardSpot.cardPowerModifier;
                PlayerCardsManager.instance.StartJuiceHoleCoroutine();
                Player_Main.instance.CanJuiceHole = true;
                break;

            case CardType.JuiceCollect:
                thisCardType = CardType.JuiceCollect;
                Player_Stats.instance.MagnetismFactor += 5 + AssociatedCardSpot.cardPowerModifier;
                //textDesc.text = "More magnetism";
                break;

            case CardType.GreenTea:
                thisCardType = CardType.GreenTea;
                Player_Stats.instance.GreenTeaRegneration += AssociatedCardSpot.cardPowerModifier;
                Player_Main.instance.CanGreenTea = true;
                PlayerCardsManager.instance.StartGreenTeaCoroutine();
                break;

            case CardType.CoffeeCard:
                thisCardType = CardType.CoffeeCard;
                Player_Stats.instance.CardsActive.Add(this);
                Player_Stats.instance.Power -= AssociatedCardSpot.cardPowerModifier;
                //textDesc.text = "Increases base power stat";
                break;

            case CardType.TonicOfThePhoenixCard:
                thisCardType = CardType.TonicOfThePhoenixCard;
                Player_Stats.instance.CardsActive.Add(this);
                Player_Stats.instance.TonicOfThePhoenixActivated = true;
                //textDesc.text = "You don't die the next time, card it's destroyed";
                break;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        CardSpotsManager.instance.ImageDelete.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, eventData.position, canvas.worldCamera, out position);

        Vector3 newPosition = canvas.transform.TransformPoint(position);
        cardRectTransform.position = newPosition;
    }
    public void DeletedCard()
    {
        Player_Stats.instance.CardsActive.Remove(this);
        AssociatedCardSpot.HasCard = false;
        switch (thisCardType)
        {
            case CardType.RedWine:
                Player_Stats.instance.ExpToGive = 2;
                break;
            case CardType.Tequilla:
                Player_Stats.instance.CurrentSpeed = 7;
                Player_Stats.instance.moveSpeed = 7;
                break;
            case CardType.JuiceHole:
                Player_Main.instance.CanJuiceHole = false;
                break;
            case CardType.JuiceCollect:
                Player_Stats.instance.MagnetismFactor -= 5 + AssociatedCardSpot.cardPowerModifier;
                PlayerCardsManager.instance.StopJuiceHoleCoroutine();
                break;

            case CardType.CoffeeCard:
                Player_Stats.instance.Power -= AssociatedCardSpot.cardPowerModifier;
                //textDesc.text = "Increases base power stat";
                break;
            case CardType.TonicOfThePhoenixCard:
                thisCardType = CardType.TonicOfThePhoenixCard;
                Player_Stats.instance.TonicOfThePhoenixActivated = false;
                break;
        }
        Destroy(this.gameObject);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        if (IsOverDeleteArea())
        {
            DeletedCard();
            
        }
        else if (!IsWithinBackground())
        {
            StartCoroutine(MoveToPosition(this.transform, this.transform.parent.position, 0.5f));
        }
        else
        {
            if (!MoveToCardSpot())
            {
                cardRectTransform.position = this.transform.parent.position;
            }
            else
            {
                initialPosition = cardRectTransform.position;  // Update the initial position after moving
            }
        }

        CardSpotsManager.instance.ImageDelete.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isDragging && Time.time - clickTime <= clickThreshold)
        {
            textDesc.gameObject.SetActive(!textDesc.gameObject.activeSelf);

            if (textDesc.gameObject.activeSelf)
            {
                if (hideTextCoroutine != null)
                {
                    StopCoroutine(hideTextCoroutine);
                }
                hideTextCoroutine = StartCoroutine(HideTextAfterDelay(2f));
            }
        }
        clickTime = Time.time;
    }

    private IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        textDesc.gameObject.SetActive(false);
    }

    private bool IsWithinBackground()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(backgroundRectTransform, Input.mousePosition, canvas.worldCamera, out localPoint);
        return backgroundRectTransform.rect.Contains(localPoint);
    }

    private bool IsOverDeleteArea()
    {
        RectTransform deleteRectTransform = CardSpotsManager.instance.ImageDelete.GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(deleteRectTransform, Input.mousePosition, canvas.worldCamera, out localPoint);
        return deleteRectTransform.rect.Contains(localPoint);
    }

    private bool MoveToCardSpot()
    {
        foreach (CardSpot spot in backgroundScript.cardSpots)
        {
            RectTransform spotRectTransform = spot.GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(spotRectTransform, Input.mousePosition, canvas.worldCamera))
            {
                if (spot.transform.childCount > 0)
                {
                    Card otherCard = spot.transform.GetChild(0).GetComponent<Card>();

                    Transform originalParent = this.transform.parent;
                    Transform otherCardParent = otherCard.transform.parent;

                    StartCoroutine(MoveToPosition(this.transform, otherCardParent.position, 0.5f));
                    StartCoroutine(MoveToPosition(otherCard.transform, originalParent.position, 0.5f));

                    this.transform.SetParent(otherCardParent);
                    otherCard.transform.SetParent(originalParent);

                    CardSpot originalSpot = this.AssociatedCardSpot;
                    this.AssociatedCardSpot = otherCard.AssociatedCardSpot;
                    otherCard.AssociatedCardSpot = originalSpot;

                    this.AssociatedCardSpot.UpdateChild();
                    otherCard.AssociatedCardSpot.UpdateChild();

                    this.UpdateInitialPosition();
                    otherCard.UpdateInitialPosition();
                    initialPosition = otherCardParent.transform.position;
                    switchModifier(this.thisCardType);
                }
                else
                {
                    if (!spot.IsCardSpotStucked)
                    {
                        StartCoroutine(MoveToPosition(this.transform, spot.transform.position, 0.5f));
                        this.AssociatedCardSpot.HasCard = false;
                        this.transform.SetParent(spot.transform);

                        this.AssociatedCardSpot = spot;

                        spot.UpdateChild();
                        spot.HasCard = true;
                        initialPosition = spot.transform.position;
                        switchModifier(this.thisCardType);
                    }
                    else
                    {
                        StartCoroutine(MoveToPosition(this.transform, this.transform.parent.position, 0.5f));
                        this.transform.position = this.transform.parent.position;
                        initialPosition = this.transform.parent.position;
                    }
                }

                return true;
            }
        }
        return false;
    }

    private IEnumerator MoveToPosition(Transform objectTransform, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = objectTransform.position;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            objectTransform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        objectTransform.position = targetPosition;
    }

    public void UpdateInitialPosition()
    {
        initialPosition = transform.position;
    }
}
