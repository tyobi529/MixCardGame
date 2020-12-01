using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialController : MonoBehaviour
{
    [SerializeField] GamePlayerManager[] player = new GamePlayerManager[2];

    [SerializeField] Transform handTransform;

    [SerializeField] bool costBonus;
    [SerializeField] int maxCost;


    [SerializeField] MessageController messageController;


    int damageCal = 0;
    int strength = 0;
    string effectMessage = null;


    [SerializeField] UIManager uiManager;

    //料理の効果
    public IEnumerator DishEffect(int cardID, int strength, int damageCal, bool isMyTurn)
    {
        GamePlayerManager attacker;
        GamePlayerManager defender;

        this.damageCal = damageCal;
        this.strength = strength;

        //Debug.Log("damageCal  " + damageCal);

        effectMessage = null;


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



        //ランダム効果
        if (cardID == 2)
        {
            effectMessage = "ランダムで効果が発生する！";
            messageController.EffectMessage(effectMessage);
            yield return new WaitForSeconds(2);
            RandomEffect(isMyTurn);
            //StopAllCoroutines();
            yield break;
        }


        //攻撃できなくても効果が出るもの
        switch (cardID)
        {
            case 3:
                IncreaseMyHandCost(isMyTurn);
                effectMessage = "食材のコストが上昇した！";
                break;
            case 4:
                IncreaseMyHandCal(isMyTurn);
                effectMessage = "食材のカロリーが上昇した！";
                break;
            case 5:
                MakeRareIngredient(isMyTurn);
                effectMessage = "食材がレアに変わった！";
                break;
            case 7:
                IncreaseMyCost(attacker, isMyTurn);
                effectMessage = "合成コストが上昇した！";
                break;
            case 8:
                NextAttack(attacker);
                effectMessage = "次に作る料理のカロリーが上昇する！";
                break;
            case 12:
                HpAttack(attacker, defender);
                break;
            case 13:
                ConditionAttack(defender, isMyTurn);
                break;
            case 14:
                CostAttack(attacker, defender, isMyTurn);
                break;
            case 18:
                HealCondition(attacker, defender, isMyTurn);
                break;
            case 20:
                HealHp(attacker, isMyTurn);
                effectMessage = "HPが回復した！";
                break;
            case 21:
                OneDefence(attacker);
                effectMessage = "次ターンのダメージを軽減する";
                break;
            case 22:
                AdditionalTurn(isMyTurn);
                break;
            case 23:
                SecondAttack(attacker);
                break;
            case 24:
                TurnBonus();
                break;
            case 25:
                MyConditionBonus(attacker, isMyTurn);
                break;
            case 26:
                EnemyCostBonus(defender, isMyTurn);
                break;
            default:
                Debug.Log("料理範囲外");
                break;
        }

        //効果テキスト
        if (effectMessage != null)
        {
            messageController.EffectMessage(effectMessage);
            effectMessage = null;
            yield return new WaitForSeconds(2);
        }


        //「あなたの攻撃」テキスト
        messageController.AttackText(isMyTurn);
        yield return new WaitForSeconds(1);


        if (this.damageCal > 0)
        {
            //ダメージテキスト
            defender.hp -= this.damageCal;
            uiManager.ShowDamageText(this.damageCal, isMyTurn);
            yield return new WaitForSeconds(2);

            switch (cardID)
            {
                case 1:
                    GamblingAttack();
                    break;
                case 2:
                    RandomDamage(isMyTurn);
                    break;
                case 6:
                    ReduceEnemyCost(defender, isMyTurn);
                    effectMessage = "合成コストが減少した！";
                    break;
                case 9:
                    ReduceEnemyHandCost(isMyTurn);
                    effectMessage = "食材のコストが減少した！";
                    break;
                case 10:
                    ReduceEnemyHandCal(isMyTurn);
                    effectMessage = "食材のカロリーが減少した！";
                    break;
                case 11:
                    StealRareIngredient(isMyTurn);
                    break;
                case 15:
                    Paralysis(defender);
                    if (isMyTurn)
                    {
                        effectMessage = "相手を麻痺にした！";
                    }
                    else
                    {
                        effectMessage = "麻痺になってしまった！";
                    }
                    break;
                case 16:
                    Dark(defender);
                    if (isMyTurn)
                    {
                        effectMessage = "相手を暗闇にした！";
                    }
                    else
                    {
                        effectMessage = "暗闇になってしまった！";
                    }
                    break;
                case 17:
                    Poison(defender);
                    if (isMyTurn)
                    {
                        effectMessage = "相手を毒にした！";
                    }
                    else
                    {
                        effectMessage = "毒になってしまった！";
                    }
                    break;
                case 19:
                    DamageHealHp(attacker, isMyTurn);
                    effectMessage = "HPが回復した！";
                    break;
                default:
                    break;
            }
        }
        else
        {
            //麻痺テキスト
            messageController.ParalysisText();
            yield return new WaitForSeconds(2);
        }

        //効果テキスト
        if (effectMessage != null)
        {
            messageController.EffectMessage(effectMessage);
            effectMessage = null;
            yield return new WaitForSeconds(2);
        }


        messageController.messagePanel.SetActive(false);

        GameManager.instance.ChangeTurn();

    }


    void GamblingAttack()
    {
        Debug.Log("確率で1になる");

        if (damageCal == 1)
        {
            effectMessage = "ダメージが１になってしまった！";
        }
    }


    void IncreaseMyHandCost(bool isMyTurn)
    {
        Debug.Log("手札のコストを上げる" + strength);

        if (!isMyTurn)
        {
            return;
        }

        int increaseCost = 0;

        switch (strength)
        {
            case 0:
                increaseCost = 1;
                break;
            case 1:
                increaseCost = 2;
                break;
            case 2:
                increaseCost = 3;
                break;
        }

        int handCount = handTransform.childCount;

        for (int i = 0; i < handCount; i++)
        {
            CardController cardController = handTransform.GetChild(i).GetComponent<CardController>();
            cardController.model.cost += increaseCost;

            //cardController.view.Refresh(cardController.model);
            cardController.GetComponent<CardView>().Refresh(cardController.model);

        }

        //return "手札のコストがあがった！";

    }

    //相手の手札の素材のコストを下げる
    void ReduceEnemyHandCost(bool isMyTurn)
    {
        Debug.Log("手札のコストを下げる" + strength);

        if (isMyTurn)
        {
            return;
        }

        int reduceCost = 0;

        switch (strength)
        {
            case 0:
                reduceCost = 1;
                break;
            case 1:
                reduceCost = 2;
                break;
            case 2:
                reduceCost = 3;
                break;
        }

        int handCount = handTransform.childCount;

        for (int i = 0; i < handCount; i++)
        {
            CardController cardController = handTransform.GetChild(i).GetComponent<CardController>();
            cardController.model.cost -= reduceCost;
            if (cardController.model.cost < 0)
            {
                cardController.model.cost = 0;
            }

            //cardController.view.Refresh(cardController.model);
            cardController.GetComponent<CardView>().Refresh(cardController.model);

        }

    }


    void MakeRareIngredient(bool isMyTurn)
    {
        Debug.Log("手札１枚をレア食材にする" + strength);

        if (!isMyTurn)
        {
            return;
        }

        int num = -1;

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

        List<int> cardIndex = new List<int>();

        Debug.Log("残り手札" + handCount);

        for (int i = 0; i < handCount; i++)
        {
            CardController cardController = handTransform.GetChild(i).GetComponent<CardController>();

            if (!cardController.model.isRare)
            {
                cardIndex.Add(i);
            }
        }


        if (cardIndex.Count == 0)
        {
            return;
        }

        for (int i = 0; i < num; i++)
        {           
            int index = UnityEngine.Random.Range(0, cardIndex.Count);

            CardController cardController = handTransform.GetChild(cardIndex[index]).GetComponent<CardController>();

            cardController.model.isRare = true;
            cardController.GetComponent<CardView>().Refresh(cardController.model);

            cardIndex.Remove(index);

            if (cardIndex.Count == 0)
            {
                break;
            }


        }

        if (num == 3)
        {
            GameManager.instance.rareDraw = true;
        }

    }


    //自分の手札のカロリーを上げる
    void IncreaseMyHandCal(bool isMyTurn)
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
                increaseCal = 50;
                break;
            case 1:
                increaseCal = 70;
                break;
            case 2:
                increaseCal = 100;
                break;
        }

        int handCount = handTransform.childCount;

        for (int i = 0; i < handCount; i++)
        {
            CardController cardController = handTransform.GetChild(i).GetComponent<CardController>();
            cardController.model.cal += increaseCal;

            //cardController.view.Refresh(cardController.model);
            cardController.GetComponent<CardView>().Refresh(cardController.model);

        }

    }


    //相手の手札のカロリーを下げる
    void ReduceEnemyHandCal(bool isMyTurn)
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
                reduceCal = 15;
                break;
            case 1:
                reduceCal = 30;
                break;
            case 2:
                reduceCal = 999;
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

            //cardController.view.Refresh(cardController.model);
            cardController.GetComponent<CardView>().Refresh(cardController.model);

        }

    }



    //相手のコストを下げる
    void ReduceEnemyCost(GamePlayerManager defender, bool isMyTurn)
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

        if (defender.cost < 0)
        {
            defender.cost = 0;
        }

        StartCoroutine(uiManager.ShowCost(!isMyTurn));


    }






    //自分のコスト増加
    void IncreaseMyCost(GamePlayerManager attacker, bool isMyTurn)
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

        if (attacker.cost > 6)
        {
            attacker.cost = 6;
        }

        StartCoroutine(uiManager.ShowCost(isMyTurn));

    }


    void TurnBonus()
    {
        Debug.Log("経過ターン分ダメージ増加");

        int addDamage = GameManager.instance.turnCount * 5;

        switch (strength)
        {
            case 0:
                break;
            case 1:
                addDamage /= 2;
                addDamage *= 3;
                break;
            case 2:
                addDamage *= 2;
                break;
        }
        

        damageCal += addDamage;

        effectMessage = "時間がたつほどダメージが増加する！";
    }

    //相手のコスト分ダメージ増加
    void EnemyCostBonus(GamePlayerManager defender, bool isMyTurn)
    {
        Debug.Log("相手のコスト分ダメージ増加" + strength);

        int addDamage = defender.cost * 30;

        switch (strength)
        {
            case 0:
                break;
            case 1:
                addDamage /= 2;
                addDamage *= 3;
                break;
            case 2:
                addDamage *= 2;
                break;
        }


        Debug.Log("追加ダメージ" + addDamage);
        damageCal += addDamage;

        if (isMyTurn)
        {
            effectMessage = "あいてのコスト分カロリーが上昇する";
        }
        else
        {
            effectMessage = "あなたのコスト分カロリーが上昇する";
        }
    }


    //自分の状態異常分ダメージ増加
    void MyConditionBonus(GamePlayerManager attacker, bool isMyTurn)
    {
        Debug.Log("自分の状態異常分ダメージ増加" + strength);

        int addDamage = (attacker.poisonCount + attacker.darkCount + attacker.paralysisCount) * 50;

        switch (strength)
        {
            case 0:
                break;
            case 1:
                addDamage /= 2;
                addDamage *= 3;
                break;
            case 2:
                addDamage *= 2;
                break;
        }

        Debug.Log("追加ダメージ" + addDamage);

        damageCal += addDamage;

        if (isMyTurn)
        {
            effectMessage = "あなたの状態異常の数だけカロリーが上昇する";
        }
        else
        {
            effectMessage = "あいての状態異常の数だけカロリーが上昇する";
        }


    }


    //素材のカロリー分の追加ダメージ
    //void IngredientBonus(GamePlayerManager defender)
    //{
    //    Debug.Log("素材のカロリー分ダメージ" + strength);

    //    int damage = GameManager.instance.IngredientCal();

    //    switch (strength)
    //    {
    //        case 0:
    //            break;
    //        case 1:
    //            damage *= 2;
    //            break;
    //        case 2:
    //            damage *= 3;
    //            break;
    //    }

    //    defender.hp -= damage;

    //}

    ////自分のカードを捨てる
    //void DiscardMyHand(bool isMyTurn)
    //{
    //    Debug.Log("自分のカードを捨てる" + strength);

    //    if (!isMyTurn)
    //    {
    //        return;
    //    }

    //    switch (strength)
    //    {
    //        case 0:
    //            GameManager.instance.ExchangeHandCard(1);
    //            break;
    //        case 1:
    //            GameManager.instance.ExchangeHandCard(2);
    //            break;
    //        case 2:
    //            GameManager.instance.ExchangeHandCard(4);
    //            break;
    //    }
    //}


    ////相手のカードを捨てる
    //void DiscardEnemyHand(bool isMyTurn)
    //{
    //    Debug.Log("相手のカードを捨てる" + strength);

    //    if (isMyTurn)
    //    {
    //        return;
    //    }

    //    switch (strength)
    //    {
    //        case 0:
    //            GameManager.instance.ExchangeHandCard(1);
    //            break;
    //        case 1:
    //            GameManager.instance.ExchangeHandCard(2);
    //            break;
    //        case 2:
    //            GameManager.instance.ExchangeHandCard(4);
    //            break;
    //    }
    //}


    //相手のレア素材を奪う
    void StealRareIngredient(bool isMyTurn)
    {
        Debug.Log("レア素材を奪う" + strength);

        if (isMyTurn)
        {
            effectMessage = "あいてのレア食材をうばった！";
        }
        else
        {
            effectMessage = "レア素材をうばわれた！";
        }

        if (isMyTurn)
        {
            return;
        }

        int num = -1;
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

        GameManager.instance.StealRareIngredient(num);
    }



    //HP差があればダメージ２倍
    void HpAttack(GamePlayerManager attacker, GamePlayerManager defender)
    {

        Debug.Log("HP差特攻" + strength);


        int hpDifference = defender.hp - attacker.hp;
        Debug.Log(hpDifference);

        if (hpDifference > 0)
        {
            Debug.Log("特攻あり");

            damageCal /= 2;

            switch (strength)
            {
                case 0:
                    damageCal *= 3;
                    break;
                case 1:
                    damageCal *= 4;
                    break;
                case 2:
                    damageCal *= 5;
                    break;
            }

            effectMessage = "HP差によりダメージが増加する！";
        }


    }

    void CostAttack(GamePlayerManager attacker, GamePlayerManager defender, bool isMyTurn)
    {
        Debug.Log("コスト差特攻" + strength);

        int costDifference = defender.cost - attacker.cost;

        if (costDifference > 3)
        {
            Debug.Log("特攻あり");

            damageCal /= 2;

            switch (strength)
            {
                case 0:
                    damageCal *= 3;
                    break;
                case 1:
                    damageCal *= 4;
                    break;
                case 2:
                    damageCal *= 5;
                    break;
            }

            effectMessage = "コスト差によりダメージが増加する！";

        }
    }



    //状態異常特攻
    void ConditionAttack(GamePlayerManager defender, bool isMyTurn)
    {
        Debug.Log("状態異常特攻" + strength);


        if (defender.poisonCount > 0 || defender.darkCount > 0 || defender.paralysisCount > 0)
        {
            Debug.Log("特攻あり");

            damageCal /= 2;

            switch (strength)
            {
                case 0:
                    damageCal *= 3;
                    break;
                case 1:
                    damageCal *= 4;
                    break;
                case 2:
                    damageCal *= 5;
                    break;
            }

            if (isMyTurn)
            {
                effectMessage = "あいてが状態異常なのでダメージが増加する！";
            }
            else
            {
                effectMessage = "あなたが状態異常なのでダメージが増加する！";
            }
            
        }



    }






    //相手を麻痺に
    void Paralysis(GamePlayerManager defender)
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
                defender.paralysisCount = 5;
                break;
        }


    }

    //相手を毒に
    void Poison(GamePlayerManager defender)
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
                defender.poisonCount = 5;
                break;
        }
    }


    //相手を暗闇に
    void Dark(GamePlayerManager defender)
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
                defender.darkCount = 5;
                break;
        }

    }



    //与えたダメージで回復
    void DamageHealHp(GamePlayerManager attacker, bool isMyTurn)
    {
        Debug.Log("与えたダメージで回復" + strength);


        int healCal = damageCal / 10;

        switch (strength)
        {
            case 0:
                healCal *= 5;
                break;
            case 1:
                healCal *= 10;
                break;
            case 2:
                healCal *= 15;
                break;
        }

        attacker.hp += healCal;

        uiManager.ShowHealText(healCal, isMyTurn);

    }



    //状態異常解除
    void HealCondition(GamePlayerManager attacker, GamePlayerManager defender, bool isMyTurn)
    {
        Debug.Log("状態異常解除" + strength);


        switch (strength)
        {
            case 0:
                break;
            case 1:
                defender.poisonCount += attacker.poisonCount;
                defender.darkCount += attacker.darkCount;
                defender.paralysisCount += attacker.paralysisCount;
                break;
            case 2:
                if (attacker.poisonCount > 0)
                {
                    defender.poisonCount += attacker.poisonCount;
                    defender.poisonCount += 3;
                }
                if (attacker.darkCount > 0)
                {
                    defender.darkCount += attacker.darkCount;
                    defender.darkCount += 3;
                }
                if (attacker.paralysisCount > 0)
                {
                    defender.paralysisCount += attacker.paralysisCount;
                    defender.paralysisCount += 3;
                }
                break;
        }

        attacker.poisonCount = 0;
        attacker.darkCount = 0;
        attacker.paralysisCount = 0;

        if (isMyTurn)
        {
            if (strength == 0)
            {
                effectMessage = "状態異常が治った！";
            }
            else
            {
                effectMessage = "状態異常があいてにうつった！";
            }
        }
        else
        {
            if (strength == 0)
            {
                effectMessage = "あいての状態異常が治った！";
            }
            else
            {
                effectMessage = "状態異常をうつされた！";
            }
        }
    }


    //HP回復
    void HealHp(GamePlayerManager attacker, bool isMyTurn)
    {
        Debug.Log("回復" + strength);

        int healCal = 0;

        switch (strength)
        {
            case 0:
                healCal = 200;
                break;
            case 1:
                healCal = 250;
                break;
            case 2:
                healCal = 300;
                break;
        }

        attacker.hp += healCal;

        uiManager.ShowHealText(healCal, isMyTurn);

    }

    //次ターンのダメージを軽減する
    void OneDefence(GamePlayerManager attacker)
    {
        Debug.Log("次ターンのダメージを軽減する");

        switch (strength)
        {
            case 0:
                attacker.defenceDish = 1;
                break;
            case 1:
                attacker.defenceDish = 2;
                break;
            case 3:
                attacker.defenceDish = 3;
                break;
        }
    }



    //２回行動
    void AdditionalTurn(bool isMyTurn)
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

        if (isMyTurn)
        {
            effectMessage = "行動回数が" + additionalTurn + "増えた！";
        }
        else
        {
            effectMessage = "あいての行動回数が" + additionalTurn + "増えた！";
        }
    }

    //次の攻撃を強くする
    void NextAttack(GamePlayerManager attacker)
    {
        Debug.Log("次の攻撃を強化");

        switch (strength)
        {
            case 0:
                attacker.nextAttack = 3;
                break;
            case 1:
                attacker.nextAttack = 4;
                break;
            case 2:
                attacker.nextAttack = 5;
                break;

        }
    }

    //試合中に使う度強くなる
    //void StrongerAttack(GamePlayerManager attacker, GamePlayerManager defender)
    //{
    //    Debug.Log("使う度強くなる" + strength);

    //    defender.hp -= (attacker.usedCount * 50);

    //    switch (strength)
    //    {
    //        case 0:
    //            attacker.usedCount += 1;
    //            break;
    //        case 1:
    //            attacker.usedCount += 2;
    //            break;
    //        case 2:
    //            attacker.usedCount += 4;
    //            break;
    //    }
    //}





    //void MultiAttack(bool isMyTurn, int damageCal)
    //{
    //    if (!isMyTurn)
    //    {
    //        return;
    //    }

    //    Debug.Log("複数回攻撃" + strength);

    //    int a = -1;
    //    int additionalNum = 0;
    //    GamePlayerManager damagePlayer = player[1];

    //    switch (strength)
    //    {
    //        case 0:
    //            a = Random.Range(0, 4);
    //            //01
    //            if (a <= 1)
    //            {
    //                additionalNum = 0;
    //            }
    //            //2
    //            else if (a <= 2)
    //            {
    //                additionalNum = 1;
    //            }
    //            //3
    //            else
    //            {
    //                additionalNum = 2;
    //            }
    //            break;
    //        case 1:
    //            a = Random.Range(0, 3);
    //            //01
    //            if (a <= 1)
    //            {
    //                additionalNum = 1;
    //            }
    //            //2
    //            else if (a <= 2)
    //            {
    //                additionalNum = 2;
    //            }
    //            break;
    //        case 2:
    //            additionalNum = 3;
    //            break;
    //    }

    //    int damage = damageCal * additionalNum;

    //    GameManager.instance.AdditionalDamage(damagePlayer, damage);

    //}







    //ランダムでダメージ
    void RandomDamage(bool isMyTurn)
    {
        effectMessage = "ランダムでどちらかに大ダメージ！";

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

    //２回目以降強くなる
    void SecondAttack(GamePlayerManager attacker)
    {
        Debug.Log("２回目以降なら強くなる");

        if (!attacker.usedDish)
        {
            attacker.usedDish = true;
            effectMessage = "次に食べた時カロリーが上昇する！";
            return;
        }
        else
        {
            int calUp = 0;
            switch (strength)
            {
                case 0:
                    calUp = 100;
                    break;
                case 1:
                    calUp = 150;
                    break;
                case 2:
                    calUp = 200;
                    break;
            }

            effectMessage = "食べたことがあるため、カロリーが上昇する！";
            damageCal += calUp;

        }


    }

    //ランダム効果
    void RandomEffect(bool isMyTurn)
    {
        Debug.Log("ランダム");

        if (!isMyTurn)
        {
            return;
        }

        int cardID = Random.Range(0, 27);

        GameManager.instance.RandomEffect(cardID, strength, damageCal);

    }


    //自分にダメージ
    //void DamageMyself(GamePlayerManager attacker)
    //{
    //    Debug.Log("自分にダメージ" + strength);

    //    int damage = 0;

    //    switch (strength)
    //    {
    //        case 0:
    //            damage = 100;
    //            break;
    //        case 1:
    //            damage = 50;
    //            break;
    //        case 2:
    //            damage = 0;
    //            break;
    //    }

    //    attacker.hp -= damage;
    //}





}
