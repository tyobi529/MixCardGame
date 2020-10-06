using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayerManager : MonoBehaviour
{
    public List<int> deck_cardID = new List<int>();
    public List<int> deck_cardKind = new List<int>();
    //public List<List<int>> deck = new List<List<int>>();


    //public list = new List<List<int>>();


    public int heroHp;

    public int heroRed;
    public int heroYellow;
    public int heroGreen;

    public int mixCost;
    public int necessaryCost;

    //０：不健康、１：普通、２：健康
    public int health = 2;
    public bool isPoison = false;
    public bool isDeadlyPoison = false;


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
