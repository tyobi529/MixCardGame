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

    public int strength;


    public CardModel(bool isDish, int cardID, int cost, bool isRare)
    {
        CardEntity cardEntity = null;

        if (isDish)
        {
            cardEntity = Resources.Load<CardEntity>("CardEntityList/Dishes/Card" + cardID);
        }
        else
        {
            cardEntity = Resources.Load<CardEntity>("CardEntityList/Ingredients/Card" + cardID);
            this.cost = cost;
            this.isRare = isRare;
            partnerID = cardEntity.partnerID;
            specialMixID = cardEntity.specialMixID;
        }


        this.cardID = cardID;

        name = cardEntity.name;

        cal = cardEntity.cal;

        this.kind = cardEntity.kind;

        icon = cardEntity.icon;

        isSelected = false;

    }


}
