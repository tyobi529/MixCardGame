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
    public NUTRIENT nutrient;
    public KIND kind;

    public int hit;

    //0:攻撃　１：防御　３：スペル
    //public int kind;
    //public NUTRITION nutrition;
    public int red;
    public int yellow;
    public int green;

    //public int spellNum;



    //public bool isAlive;
    //public bool canAttack;
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


    public bool isSelect;
    //public int selectNum;


    public int cost;
    public int deadLine;


    //public CardModel(int cardID, int ID, bool isMix)
    public CardModel(KIND kind, int cardID, int specialID)
    {
        CardEntity cardEntity = null;

        //FrequencyController frequencyController = GameObject.Find("FrequencyController").GetComponent<FrequencyController>();

        this.kind = kind;
        this.cardID = cardID;


        if (kind == KIND.INGREDIENT)
        {
            cardEntity = Resources.Load<CardEntity>("CardEntityList/Ingredients/Card" + cardID);
            hit = 100;
            //special = -1;
            this.specialID = specialID;
        }
        else if (kind == KIND.DISH)
        {
            cardEntity = Resources.Load<CardEntity>("CardEntityList/Dishes/Card" + cardID);
            hit = cardEntity.hit;
            this.specialID = cardEntity.special;
        }

        //Debug.Log(cardID);
        this.nutrient = cardEntity.nutrient;



        name = cardEntity.name;

        cal = cardEntity.cal;
        //if (kind == 1)
        //{
        //    cal = DecideDefenceCal();
        //}

        icon = cardEntity.icon;
        selectIcon = cardEntity.selectIcon;

        //ability = cardEntity.ability;
        //spell = cardEntity.spell;
        //this.kind = cardEntity.kind;
        //nutrition = cardEntity.nutrtion;

        red = cardEntity.red;
        yellow = cardEntity.yellow;
        green = cardEntity.green;

        //spellNum = cardEntity.spellNum;


        isFieldCard = false;
        //isAlive = true;
        //isPlayerCard = isPlayer;

        //this.cardKind = cardKind;

        //playerID = ID;

        //special = cardEntity.special;

        partnerID = cardEntity.partnerID;
        specialMixID = cardEntity.specialMixID;

        //this.isMix = isMix;

        isSelect = false;
        //selectNum = 0;

        cost = 0;
        deadLine = 5;
    }


}
