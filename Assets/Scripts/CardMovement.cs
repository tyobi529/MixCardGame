using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform defaultParent;

    //現在の子要素内での位置
    //public int siblingIndex;

    public bool isDraggable = false;


    //[SerializeField] MixController mixController;


    public void OnBeginDrag(PointerEventData eventData)
    {
        CardController card = GetComponent<CardController>();

        //相手のカードは動かせない
        //if (card.model.playerID != GameManager.instance.playerID)
        //{
        //    return;
        //}



        //isDraggable = true;



        ////合成コストが足りない
        //if (GameManager.instance.playerID == 1)
        //{
        //    if (GameManager.instance.playerFieldTransform.childCount == 1)
        //    {
        //        if (GameManager.instance.player.mixCost < 2)
        //        {
        //            return;
        //        }
        //    }
        //}
        //else
        //{
        //    if (GameManager.instance.enemyFieldTransform.childCount == 1)
        //    {
        //        if (GameManager.instance.enemy.mixCost < 2)
        //        {
        //            return;
        //        }
        //    }

        //}

        //if (card.model.playerID == GameManager.instance.playerID && GameManager.instance.isMyTurn && !card.model.isFieldCard)
        //{
        //    isDraggable = true;
        //}
        //else if (card.model.playerID == GameManager.instance.playerID && GameManager.instance.isMyTurn && card.model.isFieldCard)
        //{
        //    isDraggable = true;
        //}
        //else
        //{
        //    isDraggable = false;
        //}


        if (!isDraggable)
        {
            return;
        }

        defaultParent = transform.parent;
        //順番を保存
        //siblingIndex = transform.GetSiblingIndex();
        transform.SetParent(defaultParent.parent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable)
        {
            return;
        }

        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable)
        {
            return;
        }

        //if (defaultParent == transform.parent)
        //{
        //    Debug.Log("aa");
        //    transform.SetParent(defaultParent, false);
        //    return;
        //}

        transform.SetParent(defaultParent, false);

        //if (defaultParent == GameManager.instance.playerHandTransform || defaultParent == GameManager.instance.enemyHandTransform)
            //transform.SetSiblingIndex(siblingIndex);

        GetComponent<CanvasGroup>().blocksRaycasts = true;

        //カードの並び替え
        GameManager.instance.LineUpCard(defaultParent);


        //合成予測
        //if (defaultParent.GetComponent<DropPlace>().type == DropPlace.TYPE.BASICFIELD ||
        //    defaultParent.GetComponent<DropPlace>().type == DropPlace.TYPE.ADDITIONALFIELD)
        //{
        //}
        if (GameManager.instance.attackID == GameManager.instance.playerID)
        {
            MixController.instance.ExpectMix();

        }
        else
        {
            UIManager.instance.ShowExpectDamage();
        }


    }


    public void MoveToField(Transform field)
    {
        //一度親をCanvasに変更する
        transform.SetParent(defaultParent.parent);
        //DOTweenでカードをフィールドに移動
        transform.DOMove(field.position, 0.25f);

        defaultParent = field;
        transform.SetParent(defaultParent);
    }




    //public IEnumerator MoveToField(Transform field)
    //{
    //    //一度親をCanvasに変更する
    //    transform.SetParent(defaultParent.parent);
    //    //DOTweenでカードをフィールドに移動
    //    transform.DOMove(field.position, 0.25f);
    //    yield return new WaitForSeconds(0.25f);

    //    defaultParent = field;
    //    transform.SetParent(defaultParent);
    //}

    public IEnumerator MoveToTarget(Transform target)
    {
        //現在の位置と並びを取得
        Vector3 currentPosition = transform.position;
        int siblingIndex = transform.GetSiblingIndex();

        //一度親をCanvasに変更する
        transform.SetParent(defaultParent.parent);
        //DOTweenでカードをTargetに移動
        transform.DOMove(target.position, 0.25f);
        yield return new WaitForSeconds(0.25f);

        //元の位置に戻る
        transform.DOMove(currentPosition, 0.25f);
        yield return new WaitForSeconds(0.25f);
        transform.SetParent(defaultParent);
        transform.SetSiblingIndex(siblingIndex);




    }






    void Start()
    {
        defaultParent = transform.parent;
    }
}
