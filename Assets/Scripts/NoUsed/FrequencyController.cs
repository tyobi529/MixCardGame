using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrequencyController : MonoBehaviour
{
    public int[] attackCardPercentage;
    public int[] defenceCardPercentage;
    public int[] spellCardPercentage;


    public int[] kindPercentage;
    int kindSum;

    public int attackSum = 0;
    public int defenceSum = 0;
    int spellSum = 0;
    //public int kindNum = 0;

    private void Start()
    {
        foreach (int a in kindPercentage)
        {
            kindSum += a;
        }

        foreach (int a in attackCardPercentage)
        {
            attackSum += a;
        }
        foreach (int a in defenceCardPercentage)
        {
            defenceSum += a;
        }
        foreach (int a in spellCardPercentage)
        {
            spellSum += a;
        }

        //kindNum = attackCardPercentage.Length + defenceCardPercentage.Length + spellCardPercentage.Length;
    }


    public int DecideCardKind()
    {
        int kind = -1;

        int num = Random.Range(0, kindSum);

        for (int i = 0; i < kindPercentage.Length; i++)
        {
            num -= kindPercentage[i];

            if (num < 0)
            {
                kind = i;
                break;
            }


        }

        return kind;
    }

    public int DecideCardID(int kind)
    {


        int cardID = -1;


        if (kind == 0)
        {
            int num = Random.Range(0, attackSum);

            for (int i = 0; i < attackCardPercentage.Length; i++)
            {
                num -= attackCardPercentage[i];

                if (num < 0)
                {
                    cardID = i;
                    break;
                }

               
            }

        }
        else if (kind == 1)
        {
            int num = Random.Range(0, defenceSum);

            for (int i = 0; i < defenceCardPercentage.Length; i++)
            {
                num -= defenceCardPercentage[i];

                if (num < 0)
                {
                    cardID = i;
                    break;
                }


            }

        }
        else
        {
            int num = Random.Range(0, spellSum);

            for (int i = 0; i < spellCardPercentage.Length; i++)
            {
                num -= spellCardPercentage[i];

                if (num < 0)
                {
                    cardID = i;
                    break;
                }


            }
        }

        return cardID;

    }
}
