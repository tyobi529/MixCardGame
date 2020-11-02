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


    public NUTRIENT nutrient;
    public KIND kind;

    public int hit;

    public int red;
    public int yellow;
    public int green;


    public int special;

    public int[] partnerID;
    public int[] specialMixID;


}


public enum NUTRIENT
{
    RED,
    YELLOW,
    GREEN,


}

public enum KIND
{
    INGREDIENT,
    DISH,
    

}





