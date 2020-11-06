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
    public void Init(KIND kind, int cardID, int cost, int specialID)
    {
        model = new CardModel(kind, cardID, cost, specialID);
        view.SetCard(model);

        gameManager = GameObject.Find("GameManager(Clone)").GetComponent<GameManager>();
    }


    public void MixInit(KIND kind, int cardID, int cost, int specialID)
    {
        model = new CardModel(kind, cardID, cost, specialID);
        view.SetMixCard(model);

        //gameManager = GameObject.Find("GameManager(Clone)").GetComponent<GameManager>();
    }








    public void OnField(bool isPlayer)
    {
        //gameManager.ReduceManaCost(model.cost, isPlayer);
        model.isFieldCard = true;
        //if (model.ability == ABILITY.INIT_ATTACKABLE)
        //{
        //    SetCanAttack(true);
        //}
    }


    public void OnCardObject()
    {
        if (!gameManager.isMyTurn)
        {
            return;
        }

        model.isSelected = !model.isSelected;

        gameManager.SelectCard(this.GetComponent<CardController>(), model.isSelected);
    }
   




}
