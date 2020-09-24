using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrequencyController : MonoBehaviour
{
    public int[] percentage;
    public int sum = 0;

    private void Start()
    {
        foreach (int a in percentage)
        {
            sum += a;
        }
    }


    public int DecideCard(int num)
    {
        int cardID = 0;

        for (int i = 0; i < percentage.Length; i++)
        {
            num -= percentage[i];

            if (num < 0)
            {
                cardID = i;
                break;

            }

        }

        return cardID;

    }
}
