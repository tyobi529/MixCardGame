using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayerManager : MonoBehaviour
{


    public int cost;

    public int hp;

    //public int red;
    //public int yellow;
    //public int green;

    //料理の強化
    public NUTRIENT nutrient = NUTRIENT.NONE;
    public DISH dish = DISH.NONE;


    public int poisonCount;
    public int darkCount;
    public int paralysisCount;


    public int hitUp = 0;


}
