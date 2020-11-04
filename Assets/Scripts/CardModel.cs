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

    public int cost;

    //public int cost;
    public Sprite icon;
    public Sprite selectIcon;

    //public ABILITY ability;
    //public SPELL spell;
    public NUTRIENT[] nutrient = new NUTRIENT[2];
    public DISH dish;
    public KIND kind;

    public bool isFieldCard;
    //public bool isPlayerCard;

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

    public int deadLine;


    //public CardModel(int cardID, int ID, bool isMix)
    public CardModel(KIND kind, int cardID, int cost, int specialID)
    {
        CardEntity cardEntity = null;

        //FrequencyController frequencyController = GameObject.Find("FrequencyController").GetComponent<FrequencyController>();

        this.kind = kind;
        this.cardID = cardID;

        this.cost = cost;


        if (kind == KIND.INGREDIENT)
        {
            cardEntity = Resources.Load<CardEntity>("CardEntityList/Ingredients/Card" + cardID);
            //hit = 100;
            //special = -1;
            this.specialID = specialID;
            dish = DISH.NONE;
            
        }
        else if (kind == KIND.DISH)
        {
            cardEntity = Resources.Load<CardEntity>("CardEntityList/Dishes/Card" + cardID);
            this.specialID = cardEntity.special;
            cal = cardEntity.cal;
            dish = cardEntity.dish;
        }

        //Debug.Log(cardID);


        this.nutrient = cardEntity.nutrient;

        name = cardEntity.name;

        

        icon = cardEntity.icon;
        selectIcon = cardEntity.selectIcon;


        isFieldCard = false;


        partnerID = cardEntity.partnerID;
        specialMixID = cardEntity.specialMixID;

        isSelected = false;

        cost = 0;
        deadLine = 5;
    }


}
