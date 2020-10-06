using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="CardEntity", menuName ="Create CardEntity")]
public class CardEntity : ScriptableObject
{
    public new string name;
    //public int hp;
    public int cal;
    //public int de;

    //public int cost;
    public Sprite icon;
    //public ABILITY ability;
    //public SPELL spell;
    //public KIND kind;
    public int kind;
    //public NUTRITION nutrtion;
    public int red;
    public int yellow;
    public int green;

    //public int spellNum;

    public int special;

    //public string[] partnerName;
    public int[] partnerID;
    public int[] specialMixID;

    bool isSpecialMix;

}

//public enum SPECIAL
//{
//    NONE,
//    POISON1,
//    POISON2,
//    HEAL1,
//    HEAL2,

    
//}





