using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EatCardModel
{
    public string name;

    public int cal;

    public int cost;

    public bool isRare;
    public Sprite icon;

    public KIND kind;


    public int cardID;

    public EatCardModel(KIND kind, int cardID, int cost, bool isRare)
    {
        CardEntity cardEntity = null;

        this.kind = kind;


        if (kind == KIND.DISH)
        {
            cardEntity = Resources.Load<CardEntity>("CardEntityList/Dishes/Card" + cardID);
        }
        else
        {
            cardEntity = Resources.Load<CardEntity>("CardEntityList/Ingredients/Card" + cardID);
        }


        this.cardID = cardID;
        this.cost = cost;
        this.isRare = isRare;


        name = cardEntity.name;

        cal = cardEntity.cal;


        icon = cardEntity.icon;



    }
}
