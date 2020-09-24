using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlace : MonoBehaviour, IDropHandler
{
    public enum TYPE
    {
        HAND,
        FIELD,
    }
    public TYPE type;

    public void OnDrop(PointerEventData eventData)
    {
        CardController card = eventData.pointerDrag.GetComponent<CardController>();
        if (card != null)
        {


            if (!card.movement.isDraggable)
            {
                return;
            }

            card.movement.defaultParent = this.transform;


            //if (type == TYPE.HAND)
            //{
            //    card.OnField(false);
            //}
            //else if (type == TYPE.FIELD)
            //{
            //    card.OnField(true);


            //}


            if (type == TYPE.HAND)
            {
                card.MoveToHand();
            }
            else if (type == TYPE.FIELD)
            {
                card.MoveToField();


            }






        }
    }
}
