using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour
{
    public CardView view; //見かけに関することを操作（view）
    public CardModel model; //データ（model）関することを操作
                            //public CardMovement movement; //移動（movement）関することを操作


    GameManager gameManager;

    private void Awake()
    {
        view = GetComponent<CardView>();
        //movement = GetComponent<CardMovement>();

    }

    //public void Init(int cardID, int ID, bool isMix)
    public void Init(KIND kind, int cardID, int specialID, int cost)
    {
        model = new CardModel(kind, cardID, specialID, cost);
        //view.SetCard(model);

        gameManager = GameObject.Find("GameManager(Clone)").GetComponent<GameManager>();
    }


    public void EatInit(KIND kind, int cardID, int specialID, int cost)
    {
        model = new CardModel(kind, cardID, specialID, cost);
        //view.SetEatCard(model);

        //gameManager = GameObject.Find("GameManager(Clone)").GetComponent<GameManager>();
    }







    public void OnCardObject()
    {
        if (!gameManager.isMyTurn)
        {
            return;
        }

        model.isSelected = !model.isSelected;

        view.SelectView(model.isSelected);

        gameManager.SelectCard(this.GetComponent<CardController>(), model.isSelected);
    }
   




}
