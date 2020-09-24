using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="CardEntity", menuName ="Create CardEntity")]
public class CardEntity : ScriptableObject
{
    public new string name;
    //public int hp;
    public int at;
    public int de;

    //public int cost;
    public Sprite icon;
    //public ABILITY ability;
    //public SPELL spell;
    public KIND kind;
    public int spellNum;

}

public enum KIND
{
    ATTACK,
    DEFENCE,
    SPELL,
    
}


