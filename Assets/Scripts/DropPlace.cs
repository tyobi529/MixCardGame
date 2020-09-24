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
            if (type == TYPE.HAND)
            {
                //Debug.Log(card.movement.siblingIndex);
                //card.transform.SetSiblingIndex(card.movement.siblingIndex);
                return;
            }

            if (!card.movement.isDraggable)
            {
                return;
            }

            //if (card.movement.defaultParent == this.transform)
            //{
            //    return;
            //}

            card.movement.defaultParent = this.transform;
            //if (card.model.isFieldCard)
            //{
            //    return;
            //}

            //コストが少なければおけない
            //if (this.transform.childCount == 1)
            //{
            //    if (GameManager.instance.playerID == 1 && GameManager.instance.player.mixCost < 2)
            //    {
            //        card.MoveToHand();
            //        return;
            //    }
            //    if (GameManager.instance.playerID == 2 && GameManager.instance.enemy.mixCost < 2)
            //    {
            //        card.MoveToHand();
            //        return;
            //    }
            //}


            card.OnField(true);

            //相手側と同期する
            card.MoveToField();
        }
    }
}
