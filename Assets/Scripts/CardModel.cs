using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//カードデータとその処理
public class CardModel
{
    public string name;

    public int cal;

    public int cost;

    public bool rare;
    public Sprite icon;



    public DISH[] dish = new DISH[3];
    public KIND kind;


    public int cardID;

    public int specialID;



    public int[] partnerID;
    public int[] specialMixID;


    public bool isSelected;



    //public CardModel(int cardID, int ID, bool isMix)
    public CardModel(KIND kind, int cardID, int specialID, int cost)
    {
        CardEntity cardEntity = null;


        this.kind = kind;
        this.cardID = cardID;
        this.cost = cost;

        if (cost == 3)
        {
            rare = true;
        }
        else
        {
            rare = false;
        }

        if (kind == KIND.INGREDIENT)
        {
            cardEntity = Resources.Load<CardEntity>("CardEntityList/Ingredients/Card" + cardID);
            this.specialID = specialID;
            dish = cardEntity.dish;
            //dish = DISH.NONE;
            cal = cardEntity.cal;
            
        }
        else if (kind == KIND.DISH)
        {
            cardEntity = Resources.Load<CardEntity>("CardEntityList/Dishes/Card" + cardID);
            this.specialID = specialID;
            cal = cardEntity.cal;
            dish = cardEntity.dish;
        }

        //Debug.Log(cardID);


        //this.nutrient = cardEntity.nutrient;

        name = cardEntity.name;

        

        icon = cardEntity.icon;


        partnerID = cardEntity.partnerID;
        specialMixID = cardEntity.specialMixID;

        isSelected = false;

    }


}
