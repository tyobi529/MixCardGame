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
    public DISH[] dish = new DISH[2];

    public int poisonCount;
    public int darkCount;
    public int paralysisCount;
    public int healthCount;


    //前ターンに合成したかどうか
    public bool isMixed = false;

    //使う度に強くなる料理
    public int usedCount = 0;


}
