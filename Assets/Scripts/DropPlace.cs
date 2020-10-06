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

        //相手の手札やフィールドには移動できない
        //if ((type == TYPE.PLAYERHAND || type == TYPE.PLAYERFIELD) && card.model.playerID == 2)
        //{
        //    card.MoveToHand();
        //    return;
        //}
        //if ((type == TYPE.ENEMYHAND || type == TYPE.ENEMYFIELD) && card.model.playerID == 1)
        //{
        //    card.MoveToHand();
        //    return;
        //}

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




        if (!card.movement.isDraggable)
        {
            return;
        }

        if (card != null)
        {
            if (GameManager.instance.isAttackButton)
            {
                if (transform.childCount == 1)
                {
                    //transform.GetChild(0).GetComponent<CardController>().MoveToHand();
                    //card.MoveToHand();
                    //return;
                    //card.MoveToHand();
                    return;
                }
            }

            else if (GameManager.instance.isMixButton)
            {
                //アイテムカードはおけない
                if (card.model.kind == 2)
                {
                    //card.MoveToHand();
                    return;
                }

                if (transform.childCount == 2)
                {
                    //transform.GetChild(1).GetComponent<CardController>().MoveToHand();
                    //card.MoveToHand();
                    //return;

                    //card.MoveToHand();
                    return;
                }
            }

            //if (!card.movement.isDraggable)
            //{
            //    return;
            //}

            //Debug.Log("aa");

            card.movement.defaultParent = this.transform;


            //if (GameManager.)
            //{
            //    uiManager.ShowExpectDamage();
            //}
            //else if (type == TYPE.FIELD)
            //{
            //    card.OnField(true);


            //}


            //if (type == TYPE.PLAYERHAND || type == TYPE.ENEMYHAND)
            //{
            //    card.MoveToHand();
            //}
            //else if (type == TYPE.PLAYERFIELD || type == TYPE.ENEMYFIELD)
            //{
            //    card.MoveToField();


            //}






        }
    }
}
