using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialController : MonoBehaviour
{
    [SerializeField] GamePlayerManager[] player = new GamePlayerManager[2];

    [SerializeField] Transform handTransform;

    [SerializeField] bool costBonus;
    [SerializeField] int maxCost;

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

        DISH dish = DISH.NONE;

        switch (specialID)
        {
            case 0:
                dish = DISH.RED;
                break;
            case 1:
                dish = DISH.YELLOW;
                break;
            case 2:
                dish = DISH.GREEN;
                break;
            case 3:
                dish = DISH.JAPANESE;
                break;
            case 4:
                dish = DISH.WESTERN;
                break;
            case 5:
                dish = DISH.CHINESE;
                break;
            default:
                if (costBonus)
                {
                    if (attacker.cost < maxCost)
                    {
                        attacker.cost++;
                    }
                }
                else
                {
                    Debug.Log("範囲外");
                }
                break;
        }

        if (dish == DISH.NONE)
        {
            return;
        }

        if (attacker.dish[0] == DISH.NONE)
        {
            attacker.dish[0] = dish;       
        }
        else if (attacker.dish[1] == DISH.NONE)
        {
            attacker.dish[1] = dish;
        }
        else
        {
            attacker.dish[0] = attacker.dish[1];
            attacker.dish[1] = dish;
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
                AddIngredientEffect(isMyTurn, strength);
                break;
            case 2:
                IncreaseMyHandCal(isMyTurn, strength);
                break;
            case 3:
                ReduceEnemyCost(defender, strength);
                break;
            case 4:
                EraseEnemyBuff(attacker, defender, strength);
                break;
            case 5:
                IncreaseMyCost(attacker, strength);
                break;
            case 6:
                EnemyCostBonus(defender, strength);
                break;
            case 7:
                MyConditionBonus(attacker, defender, strength);
                break;
            case 8:
                IngredientAttack(defender, strength);
                break;
            case 9:
                DiscardMyHand(isMyTurn, strength);
                break;
            case 10:
                ExchangeHandCard();
                break;
            case 11:
                DiscardEnemyHand(isMyTurn, strength);
                break;
            case 12:
                MixAttack(defender, strength, damageCal);
                break;
            case 13:
                HpAttack(attacker, defender, strength, damageCal);
                break;
            case 14:
                ConditionAttack(defender, strength, damageCal);
                break;
            case 15:
                Paralysis(defender, strength);
                break;
            case 16:
                Poison(defender, strength);
                break;
            case 17:
                Poison(defender, strength);
                break;
            case 18:
                DamageHealHp(attacker, strength, damageCal);
                break;
            case 19:
                HealCondition(attacker, strength);
                break;
            case 20:
                HealHp(attacker, strength);
                break;
            case 21:
                AdditionalTurn(isMyTurn, strength);
                break;
            case 22:
                NextAttack(attacker, strength);
                break;
            case 23:
                StrongerAttack(attacker, defender, strength);
                break;
            case 24:
                MultiAttack(isMyTurn, strength, damageCal);
                break;
            case 25:
                RandomDamage(isMyTurn, strength);
                break;
            case 26:
                RandomEffect(isMyTurn, strength, damageCal);
                break;
            default:
                Debug.Log("料理範囲外");
                break;
        }
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

            //cardController.view.Refresh();

        }

    }


    //素材を効果付きにする
    void AddIngredientEffect(bool isMyTurn, int strength)
    {
        Debug.Log("素材を効果付きにする" + strength);

        if (!isMyTurn)
        {
            return;
        }

        int num = 0;

        switch (strength)
        {
            case 0:
                num = 1;
                break;
            case 1:
                num = 2;
                break;
            case 2:
                num = 3;
                break;
        }


        int handCount = handTransform.childCount;

        for (int i = 0; i < handCount; i++)
        {
            if (num == 0)
            {
                return;
            }

            CardController cardController = handTransform.GetChild(i).GetComponent<CardController>();
            if (cardController.model.specialID > 5)
            {
                cardController.model.specialID = Random.Range(0, 6);
                cardController.view.Refresh(cardController.model);
                num--;
            }

        }
    }



    //自分の手札のカロリーを上げる
    void IncreaseMyHandCal(bool isMyTurn, int strength)
    {
        Debug.Log("手札のカロリーを上げる" + strength);

        if (!isMyTurn)
        {
            return;
        }

        int increaseCal = 0;

        switch (strength)
        {
            case 0:
                increaseCal = 10;
                break;
            case 1:
                increaseCal = 20;
                break;
            case 2:
                increaseCal = 50;
                break;
        }

        int handCount = handTransform.childCount;

        for (int i = 0; i < handCount; i++)
        {
            CardController cardController = handTransform.GetChild(i).GetComponent<CardController>();
            cardController.model.cal += increaseCal;

            //cardController.view.Refresh();

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



    //相手の料理効果UPを消す
    void EraseEnemyBuff(GamePlayerManager attacker, GamePlayerManager defender, int strength)
    {
        Debug.Log("相手のバフ解除" + strength);

        if (defender.dish[0] == DISH.NONE)
        {
            return;
        }

        switch (strength)
        {
            case 0:
                defender.dish[0] = DISH.NONE;
                if (defender.dish[1] != DISH.NONE)
                {
                    defender.dish[0] = defender.dish[1];
                    defender.dish[1] = DISH.NONE;
                }
                break;
            case 1:
                for (int i = 0; i < 2; i++)
                {
                    defender.dish[i] = DISH.NONE;
                }
                break;
            case 2:
                for (int i = 0; i < 2; i++)
                {
                    attacker.dish[i] = defender.dish[i];
                    defender.dish[i] = DISH.NONE;
                }
                break;
        }

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


    //自分の状態異常分ダメージ増加
    void MyConditionBonus(GamePlayerManager attacker, GamePlayerManager defender, int strength)
    {
        Debug.Log("自分の状態異常分ダメージ増加" + strength);

        int damage = (attacker.poisonCount + attacker.darkCount + attacker.paralysisCount) * 30;

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


    //素材のカロリー分の追加ダメージ
    void IngredientAttack(GamePlayerManager defender, int strength)
    {
        Debug.Log("素材のカロリー分ダメージ" + strength);

        int damage = GameManager.instance.IngredientCal();

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

    //手札交換
    void ExchangeHandCard()
    {
        Debug.Log("実装まだ");
    }

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
                GameManager.instance.ExchangeHandCard(handTransform.childCount);
                break;
        }
    }


    //前ターン相手が合成していたら追加ダメージ
    void MixAttack(GamePlayerManager defender, int strength, int damageCal)
    {

        Debug.Log("合成特攻" + strength);

        int damage = damageCal;

        if (defender.isMixed)
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

            defender.hp -= damage;
        }


    }

    //HP差があればダメージ２倍
    void HpAttack(GamePlayerManager attacker, GamePlayerManager defender, int strength, int damageCal)
    {

        Debug.Log("HP差特攻" + strength);

        //差の5分の１追加
        //ダメージを入れた後で判定になっている
        //最大100
        //int damage = (defender.hp - attacker.hp) / 5;        

        //if (damage < 0)
        //{
        //    return;
        //}
        //else if (damage > 100)
        //{
        //    damage = 100;               
        //}

        int damage = damageCal;

        int hpDifference = defender.hp - attacker.hp;

        if (hpDifference >= 500)
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

            defender.hp -= damage;
        }


    }



    //状態異常特攻
    void ConditionAttack(GamePlayerManager defender, int strength, int damageCal)
    {
        Debug.Log("状態異常特攻" + strength);

        int damage = damageCal;

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

            defender.hp -= damage;

            

        }



    }






    //相手を麻痺に
    void Paralysis(GamePlayerManager defender, int strength)
    {
        Debug.Log("麻痺付与" + strength);

        if (defender.healthCount > 0)
        {
            Debug.Log("無効");
            return;
        }

        switch (strength)
        {
            case 0:
                defender.paralysisCount = 3;
                break;
            case 1:
                defender.paralysisCount = 4;
                break;
            case 2:
                defender.paralysisCount = 9;
                break;
        }
    }

    //相手を毒に
    void Poison(GamePlayerManager defender, int strength)
    {

        Debug.Log("毒付与" + strength);

        if (defender.healthCount > 0)
        {
            Debug.Log("無効");
            return;
        }

        switch (strength)
        {
            case 0:
                defender.poisonCount = 3;
                break;
            case 1:
                defender.poisonCount = 4;
                break;
            case 2:
                defender.poisonCount = 9;
                break;
        }
    }


    //相手を暗闇に
    void Dark(GamePlayerManager defender, int strength)
    {
        Debug.Log("暗闇付与" + strength);

        if (defender.healthCount > 0)
        {
            Debug.Log("無効");
            return;
        }

        switch (strength)
        {
            case 0:
                defender.darkCount = 3;
                break;
            case 1:
                defender.darkCount = 4;
                break;
            case 2:
                defender.darkCount = 9;
                break;
        }

    }



    //与えたダメージで回復
    void DamageHealHp(GamePlayerManager attacker, int strength, int damageCal)
    {
        Debug.Log("与えたダメージで回復" + strength);

        //if (!isMyTurn)
        //{
        //    return;
        //}

        int heal = damageCal / 10;

        switch (strength)
        {
            case 0:
                heal *= 5;
                break;
            case 1:
                heal *= 8;
                break;
            case 2:
                heal = 20;
                break;
        }

        attacker.hp += heal;


        //int a = 10;

        //switch (strength)
        //{
        //    case 0:
        //        a *= Random.Range(2, 9);
        //        break;
        //    case 1:
        //        a *= Random.Range(6, 9);
        //        break;
        //    case 2:
        //        a *= 20;
        //        break;
        //}

        //int heal = damageCal * a / 100;

        //GameManager.instance.HealHp(heal);

    }



    //状態異常解除
    void HealCondition(GamePlayerManager attacker, int strength)
    {
        Debug.Log("状態異常解除" + strength);

        //attacker.poisonCount = 0;
        //attacker.darkCount = 0;
        //attacker.paralysisCount = 0;

        int healCount = 0;

        switch (strength)
        {
            case 0:
                healCount = 3;
                break;
            case 1:
                healCount = 4;
                break;
            case 2:
                healCount = 9;
                break;
        }

        attacker.poisonCount -= healCount;
        attacker.darkCount -= healCount;
        attacker.paralysisCount -= healCount;

        if (attacker.poisonCount < 0)
        {
            attacker.poisonCount = 0;
        }
        if (attacker.darkCount < 0)
        {
            attacker.darkCount = 0;
        }
        if (attacker.paralysisCount < 0)
        {
            attacker.paralysisCount = 0;
        }

    }


    //HP回復
    void HealHp(GamePlayerManager attacker, int strength)
    {
        Debug.Log("回復" + strength);

        switch (strength)
        {
            case 0:
                attacker.hp += 200;
                break;
            case 1:
                attacker.hp += 300;
                break;
            case 2:
                attacker.hp += 500;
                break;
        }
    }




    //２回行動
    void AdditionalTurn(bool isMyTurn, int strength)
    {
        Debug.Log("複数回行動" + strength);

        int additionalTurn = 0;

        switch (strength)
        {
            case 0:
                additionalTurn = 1;
                break;
            case 1:
                additionalTurn = 2;
                break;
            case 2:
                additionalTurn = 3;
                break;
        }

        GameManager.instance.additionalTurn = additionalTurn;
    }

    //次の攻撃を強くする
    void NextAttack(GamePlayerManager attacker, int strength)
    {
        Debug.Log("次の攻撃を強化");

        switch (strength)
        {
            case 0:
                attacker.nextAttack = 1;
                break;
            case 1:
                attacker.nextAttack = 2;
                break;
            case 2:
                attacker.nextAttack = 3;
                break;

        }
    }

    //試合中に使う度強くなる
    void StrongerAttack(GamePlayerManager attacker, GamePlayerManager defender, int strength)
    {
        Debug.Log("使う度強くなる" + strength);

        defender.hp -= (attacker.usedCount * 30);

        switch (strength)
        {
            case 0:
                attacker.usedCount += 1;
                break;
            case 1:
                attacker.usedCount += 2;
                break;
            case 2:
                attacker.usedCount += 3;
                break;
        }
    }





    void MultiAttack(bool isMyTurn, int strength, int damageCal)
    {
        if (!isMyTurn)
        {
            return;
        }

        Debug.Log("複数回攻撃" + strength);

        int a = 0;
        GamePlayerManager damagePlayer = player[1];

        switch (strength)
        {
            case 0:
                a = Random.Range(0, 3);
                break;
            case 1:
                a = Random.Range(1, 3);
                break;
            case 2:
                a = 3;
                break;
        }

        int damage = damageCal * a;

        GameManager.instance.AdditionalDamage(damagePlayer, damage);

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

    //ランダム効果
    void RandomEffect(bool isMyTurn, int strength, int damageCal)
    {
        Debug.Log("ランダム");

        if (!isMyTurn)
        {
            return;
        }

        int specialID = Random.Range(0, 27);

        GameManager.instance.RandomEffect(specialID, strength, damageCal);

    }



    //ここまで


    //ランダムでダメージ
    void ExchangeHp(bool isMyTurn, int strength)
    {
        if (!isMyTurn)
        {
            return;
        }

        Debug.Log("HP交換" + strength);

        int a = Random.Range(0, 100);
        bool success = false;

        switch (strength)
        {
            case 0:
                if (a < 10)
                {
                    success = true;
                    Debug.Log("成功");
                }
                break;
            case 1:
                if (a < 15)
                {
                    success = true;
                    Debug.Log("成功");
                }
                break;
            case 2:
                if (a < 30)
                {
                    success = true;
                    Debug.Log("成功");
                }
                break;
        }

        if (success)
        {
            GameManager.instance.ExchangeHp();
        }
        else
        {
            Debug.Log("失敗");
        }

        
    }







    


    //状態異常を移す
    void ShiftCondition(GamePlayerManager attacker, GamePlayerManager defender, int strength)
    {
        Debug.Log("状態異常を移す" + strength);


        if (defender.healthCount > 0)
        {
            Debug.Log("無効");
            return;
        }

        int addition = 0;

        switch (strength)
        {
            case 0:
                break;
            case 1:
                addition += 1;
                break;
            case 2:
                addition += 3;
                break;
        }

        if (attacker.poisonCount > 0)
        {
            defender.poisonCount += (attacker.poisonCount + addition);
            attacker.poisonCount = 0;
        }
        if (attacker.darkCount > 0)
        {
            defender.darkCount += (attacker.darkCount + addition);
            attacker.darkCount = 0;
        }
        if (attacker.paralysisCount > 0)
        {
            defender.paralysisCount += (attacker.paralysisCount + addition);
            attacker.paralysisCount = 0;
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
}
