using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayerManager : MonoBehaviour
{
    public List<int> deck_cardID = new List<int>();
    public List<int> deck_cardKind = new List<int>();
    //public List<List<int>> deck = new List<List<int>>();


    //public list = new List<List<int>>();


    public int hp;

    public int red;
    public int yellow;
    public int green;

    public int mixCost;
    public int necessaryCost;

    public bool ishealth;

    public bool isPoison;
    public int poisonCount;

    public bool isDark;
    //public bool isDeadlyPoison = false;

    //追加攻撃
    public int attackUp = 0;
    //追加守備
    //public int addDefence = 0;
    public int hitUp = 0;


    //public int defaultMixCost;

    //public void Init()
    //{
    //    //heroHp = 20;
    //    mixCost = 0;
    //    //defaultMixCost = 10;
    //}

    //public void IncreaseManaCost()
    //{
    //    defaultMixCost++;
    //    mixCost = defaultMixCost;
    //}
}
