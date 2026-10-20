using Game.scripts;
using UnityEngine;
using UnityEngine.EventSystems;

public enum PlaceType
{
    SelfHand,
    EnemyHand,
    SelfField,
    EnemyField
}

public class DropPlaceScr : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public PlaceType Type;
    
    public void OnDrop(PointerEventData eventData)  
    {
        if (Type != PlaceType.SelfField)
            return;
        
        CardMovementScr cardMovement = eventData.pointerDrag.GetComponent<CardMovementScr>();
        if (cardMovement && GameManagerScr.G.CanHaveMoreCardInEitherPlace(PlaceType.SelfField))
        {
            var cardInfoScr = cardMovement.GetComponent<CardInfoScr>();
            GameManagerScr.G.PlayerFieldCards.Add(cardInfoScr);
            GameManagerScr.G.PlayerHandCards.Remove(cardInfoScr);
            cardMovement.defaultParent = transform;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || Type != PlaceType.SelfField)
            return;
        var cart = eventData.pointerDrag.GetComponent<CardMovementScr>();
        if (cart)
            cart.defaultTempCardParent = transform;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;
        var card = eventData.pointerDrag.GetComponent<CardMovementScr>();
        if (card && card.defaultTempCardParent == transform)
            card.defaultTempCardParent = card.defaultParent;

    }
}
