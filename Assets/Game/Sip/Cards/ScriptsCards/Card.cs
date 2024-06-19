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
        canvas = transform.parent.parent.GetComponent<Canvas>();
        BackGround = transform.parent.parent.GetChild(10).GetComponent<UnityEngine.UI.Image>();
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
                Player_Stats.instance.CardsActive.Add(thisCardType);
                textDesc.text = "More Exp";
                break;

            case CardType.Tequilla:
                Player_Stats.instance.CurrentSpeed = 10 + AssociatedCardSpot.cardPowerModifier;
                Player_Stats.instance.moveSpeed = 10 + AssociatedCardSpot.cardPowerModifier;
                thisCardType = CardType.Tequilla;
                Player_Stats.instance.CardsActive.Add(thisCardType);
                textDesc.text = "More Velocity";
                break;

            case CardType.JuiceHole:
                thisCardType = CardType.JuiceHole;
                Player_Stats.instance.CardsActive.Add(thisCardType);
                Player_Main.instance.CanJuiceHole = true;
                textDesc.text = "Spawns a mini black hole at a time";
                PlayerCardsManager.instance.StartJuiceHoleCoroutine();
                //if (juiceHoleCoroutine != null)
                //{
                //    StopCoroutine(juiceHoleCoroutine);
                //}
                //StartCoroutine(JuiceHoleCoroutine());
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
                textDesc.text = "More Exp";
                break;

            case CardType.Tequilla:
                Player_Stats.instance.CurrentSpeed = 8;
                Player_Stats.instance.moveSpeed = 8;
                Player_Stats.instance.CurrentSpeed = 10 + AssociatedCardSpot.cardPowerModifier; ;
                Player_Stats.instance.moveSpeed = 10 + AssociatedCardSpot.cardPowerModifier;
                thisCardType = CardType.Tequilla;
                textDesc.text = "More Velocity";
                break;

            case CardType.JuiceHole:
                thisCardType = CardType.JuiceHole;
                Player_Stats.instance.CardsActive.Add(thisCardType);
                textDesc.text = "Spawns a mini black hole at a time";

                PlayerCardsManager.instance.StartJuiceHoleCoroutine();

                break;
        }
    }
   
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, eventData.position, canvas.worldCamera, out position);

        Vector3 newPosition = canvas.transform.TransformPoint(position);
        cardRectTransform.position = newPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        if (!IsWithinBackground())
        {
            //this.transform.position = this.transform.parent.position;
            // Move the cards to their new parents smoothly
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

    private bool MoveToCardSpot()
    {
        foreach (CardSpot spot in backgroundScript.cardSpots)
        {
            RectTransform spotRectTransform = spot.GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(spotRectTransform, Input.mousePosition, canvas.worldCamera))
            {

                if (spot.transform.childCount > 0)
                {
                    // If the spot already has a card, swap positions
                    Card otherCard = spot.transform.GetChild(0).GetComponent<Card>();

                    // Store the original parents
                    Transform originalParent = this.transform.parent;
                    Transform otherCardParent = otherCard.transform.parent;

                    // Move the cards to their new parents smoothly
                    StartCoroutine(MoveToPosition(this.transform, otherCardParent.position, 0.5f));
                    StartCoroutine(MoveToPosition(otherCard.transform, originalParent.position, 0.5f));

                    // Swap the parents after the move
                    this.transform.SetParent(otherCardParent);
                    otherCard.transform.SetParent(originalParent);

                    // Update associated spots
                    CardSpot originalSpot = this.AssociatedCardSpot;
                    this.AssociatedCardSpot = otherCard.AssociatedCardSpot;
                    otherCard.AssociatedCardSpot = originalSpot;

                    // Update the child references in the CardSpots
                    this.AssociatedCardSpot.UpdateChild();
                    otherCard.AssociatedCardSpot.UpdateChild();

                    // Update initial positions
                    this.UpdateInitialPosition();
                    otherCard.UpdateInitialPosition();
                    initialPosition = otherCardParent.transform.position;
                    switchModifier(this.thisCardType);
                }
                else
                {
                    if (!spot.IsCardSpotStucked)
                    {
                        // Move to the empty spot smoothly
                        StartCoroutine(MoveToPosition(this.transform, spot.transform.position, 0.5f));
                        this.AssociatedCardSpot.HasCard = false;
                        this.transform.SetParent(spot.transform);

                        this.AssociatedCardSpot = spot;

                        // Update the child reference
                        spot.UpdateChild();
                        spot.HasCard = true;
                        initialPosition = spot.transform.position;
                        switchModifier(this.thisCardType);
                    }
                    else
                    {
                        // Move to the empty spot smoothly
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