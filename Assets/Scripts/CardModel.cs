using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//カードデータとその処理
public class CardModel
{
    public string name;
    //public int hp;
    //public int at;
    //public int de;
    public int cal;

    //public int cost;
    public Sprite icon;
    public Sprite selectIcon;

    //public ABILITY ability;
    //public SPELL spell;
    //public NUTRIENT[] nutrient = new NUTRIENT[2];
    public DISH[] dish = new DISH[3];
    public KIND kind;


    //public int cardKind;
    public int cardID;

    //public int playerID;
    public int specialID;

    //public bool isMix;

    public bool isSpecialMix;

    public int[] partnerID;
    public int[] specialMixID;


    public bool isSelected;
    //public int selectNum;



    //public CardModel(int cardID, int ID, bool isMix)
    public CardModel(KIND kind, int cardID, int specialID)
    {
        CardEntity cardEntity = null;


        this.kind = kind;
        this.cardID = cardID;


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
        selectIcon = cardEntity.selectIcon;


        partnerID = cardEntity.partnerID;
        specialMixID = cardEntity.specialMixID;

        isSelected = false;

    }


}
