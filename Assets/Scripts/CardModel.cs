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
    //public ABILITY ability;
    //public SPELL spell;
    //public KIND kind;
    //0:攻撃　１：防御　３：スペル
    public int kind;
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
    public int special;

    public bool isMix;

    public bool isSpecialMix;

    public int[] partnerID;
    public int[] specialMixID;





    //public CardModel(int cardID, int ID, bool isMix)
    public CardModel(int kind, int cardID, bool isMix)
    {
        CardEntity cardEntity = null;

        FrequencyController frequencyController = GameObject.Find("FrequencyController").GetComponent<FrequencyController>();

        this.kind = kind;
        this.cardID = cardID;

        if (kind == 0)
        {
            cardEntity = Resources.Load<CardEntity>("CardEntityList/Attack/Card" + cardID);
        }
        else if (kind == 1)
        {
            cardEntity = Resources.Load<CardEntity>("CardEntityList/Defence/Card" + cardID);

        }
        else if (kind == 2)
        {
            cardEntity = Resources.Load<CardEntity>("CardEntityList/Spell/Card" + cardID);

        }
        else if (kind == 3)
        {
            cardEntity = Resources.Load<CardEntity>("CardEntityList/Special/Card" + cardID);
            isSpecialMix = true;
        }




        name = cardEntity.name;

        cal = cardEntity.cal;
        //if (kind == 1)
        //{
        //    cal = DecideDefenceCal();
        //}

        icon = cardEntity.icon;
        //ability = cardEntity.ability;
        //spell = cardEntity.spell;
        this.kind = cardEntity.kind;
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

        special = cardEntity.special;

        partnerID = cardEntity.partnerID;
        specialMixID = cardEntity.specialMixID;

        this.isMix = isMix;



    }


    //int DecideDefenceCal()
    //{
    //    float num = Random.Range(0f, 2f);
    //    int cal = (int)(50f * Mathf.Pow(2.0f, num));
    //    return cal;
    //}


}
