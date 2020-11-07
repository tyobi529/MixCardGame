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

    //0には料理種
    //1,2には色をいれる
    public DISH[] dish = new DISH[3];

    public KIND kind;




}



