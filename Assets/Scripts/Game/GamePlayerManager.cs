using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayerManager : MonoBehaviour
{


    public int cost;

    public int hp;



    public int poisonCount;
    public int darkCount;
    public int paralysisCount;


    public int nextAttack = 0;


    //使う度に強くなる料理
    public bool usedDish;

    //ダメージを軽減する料理
    public int defenceDish;

}
