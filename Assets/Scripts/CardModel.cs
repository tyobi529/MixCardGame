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
    public NUTRITION nutrition;

    public int spellNum;



    //public bool isAlive;
    //public bool canAttack;
    public bool isFieldCard;
    //public bool isPlayerCard;

    //public int cardKind;
    public int cardID;

    //public int playerID;

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





        //CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card" + cardID);
        name = cardEntity.name;
        //hp = cardEntity.hp;
        //at = cardEntity.at;
        //de = cardEntity.de;
        cal = cardEntity.cal;
        //cost = cardEntity.cost;
        icon = cardEntity.icon;
        //ability = cardEntity.ability;
        //spell = cardEntity.spell;
        this.kind = cardEntity.kind;
        nutrition = cardEntity.nutrtion;

        spellNum = cardEntity.spellNum;

        isFieldCard = false;
        //isAlive = true;
        //isPlayerCard = isPlayer;

        //this.cardKind = cardKind;

        //playerID = ID;

        partnerID = cardEntity.partnerID;
        specialMixID = cardEntity.specialMixID;

        this.isMix = isMix;

        //isSpecialMix = cardEntity.isSpecialMix;

        //partnerName = cardEntity.partnerName;
        //specialMixID = cardEntity.specialMixID;

        //for (int i = 0; i < cardEntity.partnerName.Length)
        //{
            
        //}
        //int i = 0;
        //foreach (string name in cardEntity.partnerName)
        //{
        //    this.partnerName[i] = name;
        //    i++;
        //}

        

    }

    //void Damage(int dmg)
    //{
    //    hp -= dmg;
    //    if (hp <= 0)
    //    {
    //        hp = 0;
    //        //isAlive = false;
    //    }
    //}

    //public void Attack(CardController card)
    //{
    //    card.model.Damage(at);
    //}
}
