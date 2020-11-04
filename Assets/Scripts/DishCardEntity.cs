using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DishCardEntity", menuName = "Create DishCardEntity")]
public class DishCardEntity : ScriptableObject
{
    public int cardID;

    public new string name;
    public int cal;

    public Sprite icon;
    public Sprite selectIcon;


    public NUTRIENT[] nutrient;
    public DISH dish;

    public KIND kind;




}



