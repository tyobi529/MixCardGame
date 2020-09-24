using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayerManager : MonoBehaviour
{
    public List<int> deck = new List<int>();

    public int heroHp;
    public int mixCost;
    public int defaultMixCost;

    public void Init(List<int> cardDeck)
    {
        deck = cardDeck;
        //heroHp = 20;
        mixCost = 0;
        //defaultMixCost = 10;
    }

    public void IncreaseManaCost()
    {
        defaultMixCost++;
        mixCost = defaultMixCost;
    }
}
