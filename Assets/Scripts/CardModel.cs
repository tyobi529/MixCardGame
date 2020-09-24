using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//カードデータとその処理
public class CardModel
{
    public string name;
    //public int hp;
    public int at;
    public int de;
    //public int cost;
    public Sprite icon;
    //public ABILITY ability;
    //public SPELL spell;
    public KIND kind;
    public int spellNum;



    //public bool isAlive;
    //public bool canAttack;
    public bool isFieldCard;
    //public bool isPlayerCard;

    public int playerID;
   


    public CardModel(int cardID, int ID)
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card" + cardID);
        name = cardEntity.name;
        //hp = cardEntity.hp;
        at = cardEntity.at;
        de = cardEntity.de;

        //cost = cardEntity.cost;
        icon = cardEntity.icon;
        //ability = cardEntity.ability;
        //spell = cardEntity.spell;
        kind = cardEntity.kind;
        spellNum = cardEntity.spellNum;


        //isAlive = true;
        //isPlayerCard = isPlayer;

        playerID = ID;

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
