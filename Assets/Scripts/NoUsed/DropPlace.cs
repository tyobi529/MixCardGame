using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlace : MonoBehaviour, IDropHandler
{
    [SerializeField] MixController mixController;

    [SerializeField] Transform basicField;
    //[SerializeField] Transform additionalField;

    //[SerializeField] UIManager uiManager;

    public enum TYPE
    {
        HAND,
        FIELD,
        BASICFIELD,
        ADDITIONALFIELD,
    }
    public TYPE type;

    public void OnDrop(PointerEventData eventData)
    {


        CardController card = eventData.pointerDrag.GetComponent<CardController>();


        if (type == TYPE.ADDITIONALFIELD)
        {
            if (basicField.childCount == 0)
                return;

            if (card.model.red > 0 && basicField.GetChild(0).GetComponent<CardController>().model.red > 0)
            {
                return;
            }
            if (card.model.yellow > 0 && basicField.GetChild(0).GetComponent<CardController>().model.yellow > 0)
            {
                return;
            }
            if (card.model.green > 0 && basicField.GetChild(0).GetComponent<CardController>().model.green > 0)
            {
                return;
            }

        }

        if (type == TYPE.BASICFIELD || type == TYPE.ADDITIONALFIELD)
        {
            if (transform.childCount == 1)
            {
                return;
            }


        }




        //if (!card.movement.isDraggable)
        //{
        //    return;
        //}

        //if (card != null)
        //{
        //    if (GameManager.instance.isAttackButton)
        //    {
        //        if (transform.childCount == 1)
        //        {

        //            return;
        //        }
        //    }

        //    else if (GameManager.instance.isMixButton)
        //    {
        //        if (transform.childCount == 2)
        //        {

        //            return;
        //        }
        //    }






        //}
    }
}
