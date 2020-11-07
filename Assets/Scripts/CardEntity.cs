using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="CardEntity", menuName ="Create CardEntity")]
public class CardEntity : ScriptableObject
{
    public new string name;
    public int cal;

    public Sprite icon;
    public Sprite selectIcon;


    //public NUTRIENT[] nutrient;
    public DISH[] dish = new DISH[3];

    public KIND kind;



    public int special;

    public int[] partnerID;
    public int[] specialMixID;


}


public enum NUTRIENT
{
    NONE,
    RED,
    YELLOW,
    GREEN,


}

public enum KIND
{
    INGREDIENT,
    DISH   
}


public enum DISH
{
    NONE,
    RED,
    YELLOW,
    GREEN,
    JAPANESE,
    WESTERN,
    CHINESE,
}



