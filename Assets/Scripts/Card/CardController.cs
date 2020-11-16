using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour
{
    public CardView view; //見かけに関することを操作（view）
    public CardModel model; //データ（model）関することを操作
                            //public CardMovement movement; //移動（movement）関することを操作


    //GameManager gameManager;
    SelectController selectController;

    private void Awake()
    {
        view = GetComponent<CardView>();
        //movement = GetComponent<CardMovement>();

    }

    //public void Init(int cardID, int ID, bool isMix)
    public void Init(int cardID, int cost, bool isRare)
    {
        model = new CardModel(cardID, cost, isRare);
        //view.SetCard(model);

        //gameManager = GameObject.Find("GameManager(Clone)").GetComponent<GameManager>();
        selectController = GameObject.Find("SelectController").GetComponent<SelectController>();
    }






    public void OnCardObject()
    {
        if (!selectController.canSelect)
        {
            return;
        }

        model.isSelected = !model.isSelected;

        view.SelectView(model.isSelected);

        selectController.SelectCard(this.GetComponent<CardController>(), model.isSelected);
    }
   




}
