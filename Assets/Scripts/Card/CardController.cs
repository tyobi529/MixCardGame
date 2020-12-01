using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour
{
    //public CardView view; //見かけに関することを操作（view）
    //public EatCardView eatView;
    public CardModel model; //データ（model）関することを操作
                            //public CardMovement movement; //移動（movement）関することを操作


    //GameManager gameManager;
    SelectController selectController;

    private void Awake()
    {
        //view = GetComponent<CardView>();
        //movement = GetComponent<CardMovement>();

    }

    //public void Init(int cardID, int ID, bool isMix)
    public void Init(bool isDish, int cardID, int cost, bool isRare)
    {
        model = new CardModel(isDish, cardID, cost, isRare);

        //if (isDish)
        //{
        //    eatView = GetComponent<EatCardView>();
        //}
        //else
        //{
        //    view = GetComponent<CardView>();
        //}
        //view.SetCard(model);

        //gameManager = GameObject.Find("GameManager(Clone)").GetComponent<GameManager>();
        selectController = GameObject.Find("SelectController").GetComponent<SelectController>();
    }


    //public void SetView()
    //public void EatInit(KIND kind, int cardID, int cost, bool isRare)
    //{
    //    model = new CardModel(kind, cardID, cost, isRare);
    //    //view.SetEatCard(model);

    //    //gameManager = GameObject.Find("GameManager(Clone)").GetComponent<GameManager>();
    //}


    //void OnMouseDown()
    //{
    //    Debug.Log("fdff");
    //}


    public void OnCardObject()
    {

        if (!selectController.canSelect)
        {
            return;
        }

        model.isSelected = !model.isSelected;

        GetComponent<CardView>().SelectView(model.isSelected);

        selectController.SelectCard(this.GetComponent<CardController>(), model.isSelected);
    }

    public void OnEatCardObject()
    {
        if (!GameManager.instance.isMyTurn)
        {
            return;
        }

        GameManager.instance.OnDecideButton();

    }





}
