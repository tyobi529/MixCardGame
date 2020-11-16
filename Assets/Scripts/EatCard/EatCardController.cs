using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatCardController : MonoBehaviour
{
    public EatCardView view; //見かけに関することを操作（view）
    public EatCardModel model; //データ（model）関することを操作
                            //public CardMovement movement; //移動（movement）関することを操作


    GameManager gameManager;

    private void Awake()
    {
        view = GetComponent<EatCardView>();

    }


    public void EatInit(KIND kind, int cardID, int cost, bool isRare)
    {
        model = new EatCardModel(kind, cardID, cost, isRare);
        //view.SetEatCard(model);

        //gameManager = GameObject.Find("GameManager(Clone)").GetComponent<GameManager>();
    }



    public void OnCardObject()
    {
        if (!GameManager.instance.isMyTurn)
        {
            return;
        }

        GameManager.instance.OnDecideButton();
    }



}
