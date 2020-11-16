using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//カードデータとその処理
public class CardModel
{
    public string name;

    public int cal;

    public int cost;

    public bool isRare;
    public Sprite icon;



    public KIND kind;


    public int cardID;

    public int[] partnerID;
    public int[] specialMixID;


    public bool isSelected;



    public CardModel(int cardID, int cost, bool isRare)
    {
        //CardEntity cardEntity = null;
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Ingredients/Card" + cardID);


        this.cardID = cardID;
        this.cost = cost;
        this.isRare = isRare;




        //if (kind == KIND.DISH)
        //{
        //    cardEntity = Resources.Load<CardEntity>("CardEntityList/Dishes/Card" + cardID);
        //    cal = cardEntity.cal;
        //}
        //else
        //{
        //    cardEntity = Resources.Load<CardEntity>("CardEntityList/Ingredients/Card" + cardID);
        //    cal = cardEntity.cal;

        //}


        name = cardEntity.name;

        cal = cardEntity.cal;

        kind = cardEntity.kind;

        icon = cardEntity.icon;


        partnerID = cardEntity.partnerID;
        specialMixID = cardEntity.specialMixID;

        isSelected = false;

    }


}
