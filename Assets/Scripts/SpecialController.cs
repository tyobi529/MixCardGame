using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialController : MonoBehaviour
{
    public GamePlayerManager player;
    public GamePlayerManager enemy;

    public void IngredientEffect(int specialID, bool isMyTurn)
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
    public void DishEffect(int specialID, int strength, bool isMyTurn)
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

    }

    //相手を毒に
    void Poison(GamePlayerManager defender, int strength)
    {

        Debug.Log("毒付与" + strength);

        defender.poisonCount = strength + 2;

    }

    //相手を暗闇に
    void Dark(GamePlayerManager defender, int strength)
    {
        Debug.Log("暗闇付与" + strength);

        defender.darkCount = strength + 2;

    }


    //相手を麻痺に
    void Paralysis(GamePlayerManager defender, int strength)
    {
        Debug.Log("麻痺付与" + strength);

        defender.paralysisCount = strength + 2;
    }

    //状態異常解除
    void RecoverCondition(GamePlayerManager attacker, int strength)
    {
        Debug.Log("状態異常解除" + strength);

        attacker.poisonCount = 0;
        attacker.darkCount = 0;
        attacker.paralysisCount = 0;
    }



    //自分にダメージ
    void DamageMyself(GamePlayerManager attacker, int strength)
    {
        Debug.Log("自分にダメージ" + strength);

        attacker.hp -= (50 * (2 - strength));
    }


    //相手のコストを下げる
    void ReduceCost(GamePlayerManager defender, int strength)
    {
        Debug.Log("コストを下げる" + strength);

        defender.cost -= (strength + 1);
        if (defender.cost < 0)
        {
            defender.cost = 0;
        }
    }

    //状態異常を移す
    void ShiftStatus(GamePlayerManager attacker, GamePlayerManager defender, int strength)
    {
        Debug.Log("状態異常を移す" + strength);

        defender.poisonCount = attacker.poisonCount + strength;
        defender.darkCount = attacker.darkCount + strength;
        defender.paralysisCount = attacker.paralysisCount + strength;

        attacker.poisonCount = 0;
        attacker.darkCount = 0;
        attacker.paralysisCount = 0;

    }

    //回復
    void Heal(GamePlayerManager attacker, int strength)
    {
        Debug.Log("回復" + strength);

        attacker.hp += 50 * (strength + 1);

    }

    //攻撃で回復
    void DamageHeal(GamePlayerManager attacker, int strength, int damage)
    {
        Debug.Log("攻撃で回復" + strength);

        attacker.hp += (damage / 2) * (strength + 1);

    }

    //食材の入れ替え
    void ChangeIngredient(int strength)
    {
        Debug.Log("食材入れ替え" + strength);


    }

}
