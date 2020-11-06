using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialController : MonoBehaviour
{
    [SerializeField] GamePlayerManager[] player = new GamePlayerManager[2];

    [SerializeField] Transform handTransform;


    public void IngredientEffect(int specialID, bool isMyTurn)
    {
        GamePlayerManager attacker;
        

        if (isMyTurn)
        {
            attacker = player[0];
        }
        else
        {
            attacker = player[1];
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



    //料理の効果
    public void DishEffect(int specialID, int strength, bool isMyTurn, int damageCal)
    {
        GamePlayerManager attacker;
        GamePlayerManager defender;

      

        if (isMyTurn)
        {
            attacker = player[0];
            defender = player[1];
        }
        else
        {
            attacker = player[1];
            defender = player[0];
        }

        switch (specialID)
        {
            case 0:
                ReduceEnemyCost(defender, strength);
                break;
            case 1:
                DamageMyself(attacker, strength);
                break;
            case 2:
                IncreaseMyCost(attacker, strength);
                break;
            case 3:
                StatusAttack(defender, strength);
                break;
            case 4:
                EnemyCostBonus(defender, strength);
                break;
            case 6:
                DiscardMyHand(isMyTurn, strength);
                break;
            case 7:
                ReduceEnemyHandCal(isMyTurn, strength);
                break;
            case 11:
                DiscardEnemyHand(isMyTurn, strength);
                break;
            case 12:
                DamageHealHp(isMyTurn, strength, damageCal);
                break;
            case 15:
                Paralysis(defender, strength);
                break;
            case 18:
                Dark(defender, strength);
                break;
            case 22:
                RandomDamage(isMyTurn, strength);
                break;
            case 24:
                Poison(defender, strength);
                break;
            case 25:
                HealHp(attacker, strength);
                break;
            default:
                Debug.Log("料理範囲外");
                break;
        }
    }


    //相手のコストを下げる
    void ReduceEnemyCost(GamePlayerManager defender, int strength)
    {
        Debug.Log("コスト減少" + strength);

        switch (strength)
        {
            case 0:
                defender.cost -= 1;
                break;
            case 1:
                defender.cost -= 2;
                break;
            case 2:
                defender.cost -= 3;
                break;
        }

    }


    //自分にダメージ
    void DamageMyself(GamePlayerManager attacker, int strength)
    {
        Debug.Log("自分にダメージ" + strength);

        int damage = 0;

        switch (strength)
        {
            case 0:
                damage = 100;
                break;
            case 1:
                damage = 80;
                break;
            case 2:
                damage = 0;
                break;
        }

        attacker.hp -= damage;
    }

    //状態異常の時追加ダメージ
    void StatusAttack(GamePlayerManager defender, int strength)
    {
        Debug.Log("状態異常特攻" + strength);

        int damage = 100;

        if (defender.poisonCount > 0 || defender.darkCount > 0 || defender.paralysisCount > 0)
        {
            Debug.Log("特攻あり");

            switch (strength)
            {
                case 0:                    
                    break;
                case 1:
                    damage *= 2;
                    break;
                case 2:
                    damage *= 3;
                    break;
            }
        }

        defender.hp -= damage;


    }

    //自分のコスト増加
    void IncreaseMyCost(GamePlayerManager attacker, int strength)
    {
        Debug.Log("コスト増加" + strength);

        switch (strength)
        {
            case 0:
                attacker.cost += 1;
                break;
            case 1:
                attacker.cost += 2;
                break;
            case 2:
                attacker.cost += 3;
                break;
        }
    }

    //相手のコスト分ダメージ増加
    void EnemyCostBonus(GamePlayerManager defender, int strength)
    {
        Debug.Log("相手のコスト分ダメージ増加" + strength);

        int damage = defender.cost * 30;

        switch (strength)
        {
            case 0:                
                break;
            case 1:
                damage *= 2;
                break;
            case 2:
                damage *= 3;
                break;
        }

        defender.hp -= damage;
    }

    //相手の手札のカロリーを下げる
    void ReduceEnemyHandCal(bool isMyTurn, int strength)
    {
        Debug.Log("手札のカロリーを下げる" + strength);

        if (isMyTurn)
        {
            return;
        }

        int reduceCal = 0;

        switch (strength)
        {
            case 0:
                reduceCal = 10;
                break;
            case 1:
                reduceCal = 20;
                break;
            case 2:
                reduceCal = 50;
                break;
        }

        int handCount = handTransform.childCount;

        for (int i = 0; i < handCount; i++)
        {
            CardController cardController = handTransform.GetChild(i).GetComponent<CardController>();
            cardController.model.cal -= reduceCal;
            if (cardController.model.cal <= 0)
            {
                cardController.model.cal = 1;
            }

            cardController.view.Refresh(cardController.model);

        }

    }


    //元素材のCal分ダメージ
    //void IngredientsAttack(GamePlayerManager attacker, int strength)
    //{
    //    Debug.Log("素材分追加ダメージ" + strength);



    //    switch (strength)
    //    {
    //        case 0:
    //            attacker.hp -= 100;
    //            break;
    //        case 1:
    //            attacker.hp -= 80;
    //            break;
    //        case 2:
    //            attacker.hp -= 0;
    //            break;
    //    }

    //}


    //相手のカードを捨てる
    void DiscardEnemyHand(bool isMyTurn, int strength)
    {
        Debug.Log("相手のカードを捨てる" + strength);

        if (isMyTurn)
        {
            return;
        }

        switch (strength)
        {
            case 0:
                GameManager.instance.ExchangeHandCard(1);
                break;
            case 1:
                GameManager.instance.ExchangeHandCard(2);
                break;
            case 2:
                Debug.Log(handTransform.childCount);
                GameManager.instance.ExchangeHandCard(handTransform.childCount);
                break;
        }
    }

    //与えたダメージで回復
    void DamageHealHp(bool isMyTurn, int strength, int damageCal)
    {
        Debug.Log("与えたダメージで回復" + strength);

        if (!isMyTurn)
        {
            return;
        }

        int a = 10;

        switch (strength)
        {
            case 0:
                a *= Random.Range(2, 9);
                break;
            case 1:
                a *= Random.Range(6, 9);
                break;
            case 2:
                a *= 20;
                break;
        }

        int heal = damageCal * a / 100;

        GameManager.instance.HealHp(heal);

    }


    //自分のカードを捨てる
    void DiscardMyHand(bool isMyTurn, int strength)
    {
        Debug.Log("自分のカードを捨てる" + strength);

        if (!isMyTurn)
        {
            return;
        }

        switch (strength)
        {
            case 0:
                GameManager.instance.ExchangeHandCard(1);
                break;
            case 1:
                GameManager.instance.ExchangeHandCard(2);
                break;
            case 2:
                GameManager.instance.ExchangeHandCard(handTransform.childCount);
                break;
        }
    }

    //HP回復
    void HealHp(GamePlayerManager attacker, int strength)
    {
        Debug.Log("回復" + strength);

        switch (strength)
        {
            case 0:
                attacker.hp += 50;
                break;
            case 1:
                attacker.hp += 100;
                break;
            case 2:
                attacker.hp += 300;
                break;
        }
    }


    //ランダムでダメージ
    void RandomDamage(bool isMyTurn, int strength)
    {
        if (!isMyTurn)
        {
            return;
        }

        Debug.Log("ランダムダメージ" + strength);

        int a = Random.Range(0, 100);
        GamePlayerManager damagePlayer = null;

        switch (strength)
        {
            case 0:
                if (a < 50)
                {
                    damagePlayer = player[0];
                }
                else
                {
                    damagePlayer = player[1];
                }
                break;
            case 1:
                if (a < 30)
                {
                    damagePlayer = player[0];
                }
                else
                {
                    damagePlayer = player[1];
                }
                break;
            case 2:
                if (a < 10)
                {
                    damagePlayer = player[0];
                }
                else
                {
                    damagePlayer = player[1];
                }
                break;
        }

        GameManager.instance.AdditionalDamage(damagePlayer, 100);
    }


    //相手を毒に
    void Poison(GamePlayerManager defender, int strength)
    {

        Debug.Log("毒付与" + strength);

        switch (strength)
        {
            case 0:
                defender.poisonCount = 3;
                break;
            case 1:
                defender.poisonCount = 4;
                break;
            case 2:
                defender.poisonCount = 10;
                break;
        }
    }

    //相手を暗闇に
    void Dark(GamePlayerManager defender, int strength)
    {
        Debug.Log("暗闇付与" + strength);

        switch (strength)
        {
            case 0:
                defender.darkCount = 3;
                break;
            case 1:
                defender.darkCount = 4;
                break;
            case 2:
                defender.darkCount = 10;
                break;
        }

    }


    //相手を麻痺に
    void Paralysis(GamePlayerManager defender, int strength)
    {
        Debug.Log("麻痺付与" + strength);

        switch (strength)
        {
            case 0:
                defender.paralysisCount = 3;
                break;
            case 1:
                defender.paralysisCount = 4;
                break;
            case 2:
                defender.paralysisCount = 10;
                break;
        }
    }

    //状態異常解除
    void RecoverCondition(GamePlayerManager attacker, int strength)
    {
        Debug.Log("状態異常解除" + strength);

        attacker.poisonCount = 0;
        attacker.darkCount = 0;
        attacker.paralysisCount = 0;
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


}
