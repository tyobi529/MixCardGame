using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialController : MonoBehaviour
{
    public GamePlayerManager player;
    public GamePlayerManager enemy;

    public void IngredientEffect(int specialID, bool isMyTurn, int damageCal)
    {
        GamePlayerManager attacker;

        if (isMyTurn)
        {
            attacker = player;
        }
        else
        {
            attacker = enemy;
        }

        switch (specialID)
        {
            case 0:
                attacker.nutrient = NUTRIENT.RED;
                break;
            case 1:
                attacker.nutrient = NUTRIENT.YELLOW;
                break;
            case 2:
                attacker.nutrient = NUTRIENT.GREEN;
                break;
            case 3:
                attacker.dish = DISH.JAPANESE;
                break;
            case 4:
                attacker.dish = DISH.WESTERN;
                break;
            case 5:
                attacker.dish = DISH.CHINESE;
                break;
            default:
                Debug.Log("範囲外");
                break;

        }
    }

    //料理のカロリーをプラス
    //void AddDishCal(GamePlayerManager attacker)
    //{
    //    Debug.Log("料理のカロリー+");
    //    attacker.addDishCal += 100;
    //}

    ////料理の効果をプラス
    //void AddDishEffect(GamePlayerManager attacker)
    //{
    //    attacker.addDishEffect++;
    //}

    //料理の効果
    public void DishEffect(int specialID, bool isMyTurn, int damageCal)
    {
        Debug.Log("料理効果");
    }

    //相手を毒に
    void Poison(bool isMyTurn, int damageCal)
    {
        if (damageCal == 0)
        {
            return;
        }

        Debug.Log("毒付与");

        if (isMyTurn)
        {
            enemy.poisonCount = 3;
        }
        else
        {
            player.poisonCount = 3;
        }
    }

    //相手を暗闇に
    void Dark(bool isMyTurn, int damageCal)
    {
        if (damageCal == 0)
        {
            return;
        }

        Debug.Log("暗闇付与");

        if (isMyTurn)
        {
            enemy.darkCount = 3;
        }
        else
        {
            player.darkCount = 3;
        }
    }

    //状態異常解除
    void RecoverCondition(bool isMyTurn)
    {
        Debug.Log("状態異常解除");

        if (isMyTurn)
        {
            player.poisonCount = 0;
            player.darkCount = 0;
        }
        else
        {
            enemy.poisonCount = 0;
            enemy.darkCount = 0;
        }
    }





}
