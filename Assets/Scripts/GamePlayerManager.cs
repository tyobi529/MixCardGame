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
    public int cal;
    public int mixCost;
    public int necessaryCost;

    //public int defaultMixCost;

    //public void Init(List<int> cardDeck)
    //{
    //    deck = cardDeck;
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
