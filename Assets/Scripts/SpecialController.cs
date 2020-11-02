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
                attacker.dishBonus = true;
                break;
            case 1:
                attacker.redBonus = true;
                break;
            case 2:
                attacker.yellowBonus = true;
                break;
            case 3:
                attacker.greenBonus = true;
                break;
            case 4:
                attacker.japaneseBonus = true;
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


    //攻撃アップ
    void AttackUp(bool isMyTurn)
    {
        Debug.Log("攻撃アップ");
        if (isMyTurn)
        {
            player.attackUpCount = 3;
        }
        else
        {
            enemy.attackUpCount = 3;
        }

        //Debug.Log("playerA" + player.attackUp);
        //Debug.Log("enemyA" + enemy.attackUp);
    }

    //防御アップ
    void DefenceUp(bool isMyTurn)
    {
        Debug.Log("防御アップ");
        if (isMyTurn)
        {
            player.defenceUpCount = 3;
        }
        else
        {
            enemy.defenceUpCount = 3;
        }

        //Debug.Log("playerA" + player.attackUp);
        //Debug.Log("enemyA" + enemy.attackUp);
    }

    /// <summary>
    /// 赤栄養素
    /// </summary>




    //命中アップ
    void HitRateUp(bool isMyTurn)
    {
        Debug.Log("命中アップ");

        if (isMyTurn)
        {
            player.hitUp++;
        }
        else
        {
            enemy.hitUp++;
        }

        Debug.Log("playerH" + player.hitUp);
        Debug.Log("enemyH" + enemy.hitUp);

    }





    //相手のバフ解除
    //void BuffRelease(bool isMyTurn, int damageCal)
    //{

    //    if (damageCal == 0)
    //    {
    //        return;
    //    }

    //    Debug.Log("バフ解除");

    //    if (isMyTurn)
    //    {
    //        enemy.attackUp = 0;
    //        enemy.hitUp = 0;
    //    }
    //    else
    //    {
    //        player.attackUp = 0;
    //        player.hitUp = 0;
    //    }
    //}

    /// <summary>
    /// 黄栄養素
    /// </summary>

    //毒特攻
    //void PoisonSpecialAttack(bool isMyTurn)
    //{
    //    if (isMyTurn)
    //    {
    //        if (enemy.isPoison)
    //        {
    //            Debug.Log("毒特攻");
    //            GameManager.instance.damageCal *= 2;
    //        }

    //    }
    //    else
    //    {
    //        if (player.isPoison)
    //        {
    //            Debug.Log("毒特攻");
    //            GameManager.instance.damageCal *= 2;
    //        }
    //    }
    //}


    //暗闇特攻
    //void DarkSpecialAttack(bool isMyTurn)
    //{
    //    if (isMyTurn)
    //    {
    //        if (enemy.isDark)
    //        {
    //            Debug.Log("暗闇特攻");
    //            GameManager.instance.damageCal *= 2;
    //        }

    //    }
    //    else
    //    {
    //        if (player.isDark)
    //        {
    //            Debug.Log("暗闇特攻");
    //            GameManager.instance.damageCal *= 2;
    //        }
    //    }
    //}


    //HP差特攻
    //void HpSpecialAttack(bool isMyTurn)
    //{
    //    if (isMyTurn)
    //    {
    //        if (player.hp + 300 < enemy.hp)
    //        {
    //            Debug.Log("HP特攻");
    //            GameManager.instance.damageCal *= 2;
    //        }

    //    }
    //    else
    //    {
    //        if (enemy.hp + 300 < player.hp)
    //        {
    //            Debug.Log("HP特攻");
    //            GameManager.instance.damageCal *= 2;
    //        }
    //    }
    //}



    /// <summary>
    /// 緑栄養素
    /// </summary>



    //毒解除



   

    //void Poison(int attackID, int damage)
    //{
    //    if (damage == 0)
    //    {
    //        return;
    //    }

    //    if (attackID == 1)
    //    {
    //        if (enemy.isPoison)
    //        {
    //            enemy.isDeadlyPoison = true;
    //        }
    //        else
    //        {
    //            enemy.isPoison = true;

    //        }
    //    }
    //    else
    //    {
    //        if (player.isPoison)
    //        {
    //            player.isDeadlyPoison = true;
    //        }
    //        else
    //        {
    //            player.isPoison = true;

    //        }
    //    }

    //    Debug.Log("毒付与");
    //}

    //void DeadlyPoison(int attackID, int damage)
    //{
    //    if (damage == 0)
    //    {
    //        return;
    //    }

    //    if (attackID == 1)
    //    {

    //        enemy.isDeadlyPoison = true;

    //    }
    //    else
    //    {

    //        player.isDeadlyPoison = true;


    //    }

    //    Debug.Log("猛毒付与");
    //}

    //void Heal1(int attackID)
    //{

    //    if (attackID == 1)
    //    {
    //        player.hp += 500;
    //    }
    //    else
    //    {
    //        enemy.hp += 500;

    //    }

    //    Debug.Log("500回復");
    //}

    //void Heal2(int attackID)
    //{

    //    if (attackID == 1)
    //    {
    //        player.hp += 1000;
    //    }
    //    else
    //    {
    //        enemy.hp += 1000;

    //    }

    //    Debug.Log("1000回復");
    //}

    //void ChangeHandCard(int attackID)
    //{
    //    int playerID = GameManager.instance.playerID;




    //    Transform hand;
    //    GamePlayerManager hero;

    //    if (attackID == 1)
    //    {
    //        hand = GameManager.instance.playerHandTransform;
    //        hero = player;
    //    }
    //    else
    //    {
    //        hand = GameManager.instance.enemyHandTransform;
    //        hero = enemy;
    //    }

    //    foreach (Transform card in hand)
    //    {
    //        Destroy(card.gameObject);

    //    }

    //    Debug.Log("手札" + GameManager.instance.maxHand);
    //    for (int i = 0; i < GameManager.instance.maxHand; i++)
    //    {
    //        //GameManager.instance.GiveCardToHand(attackID);

    //    }



    //    //Debug.Log("手札" + hand.childCount);

    //    //GameManager.instance.SettingInitHand();

    //    Debug.Log("手札入れ替え");
    //}


    //void DecreaseRed(int attackID, int damage)
    //{
    //    if (damage == 0)
    //    {
    //        return;
    //    }

    //    if (attackID == 1)
    //    {
    //        enemy.red--;

    //    }
    //    else
    //    {
    //        player.red--;

    //    }

    //    Debug.Log("赤が１減った");
    //}

    //void DecreaseYellow(int attackID, int damage)
    //{
    //    if (damage == 0)
    //    {
    //        return;
    //    }

    //    if (attackID == 1)
    //    {
    //        enemy.yellow--;

    //    }
    //    else
    //    {
    //        player.yellow--;

    //    }

    //    Debug.Log("黄が１減った");
    //}

    //void DecreaseGreen(int attackID, int damage)
    //{
    //    if (damage == 0)
    //    {
    //        return;
    //    }

    //    if (attackID == 1)
    //    {
    //        enemy.green--;

    //    }
    //    else
    //    {
    //        player.green--;

    //    }

    //    Debug.Log("緑が１減った");
    //}

    //void IncreaseRed(int attackID)
    //{
    //    if (attackID == 1)
    //    {
    //        player.red++;

    //    }
    //    else
    //    {
    //        enemy.red++;

    //    }

    //    Debug.Log("赤が１増えた");
    //}

    //void IncreaseYellow(int attackID)
    //{
    //    if (attackID == 1)
    //    {
    //        player.yellow++;

    //    }
    //    else
    //    {
    //        enemy.yellow++;

    //    }

    //    Debug.Log("黄が１増えた");
    //}

    //void IncreaseGreen(int attackID)
    //{
    //    if (attackID == 1)
    //    {
    //        player.green++;

    //    }
    //    else
    //    {
    //        enemy.green++;

    //    }

    //    Debug.Log("緑が１増えた");
    //}



    //void DecreaseHandCard(int attackID, int damage)
    //{
    //    if (damage == 0)
    //    {
    //        return;
    //    }

    //    if (attackID != GameManager.instance.playerID)
    //    {
    //        GameManager.instance.maxHand--;

    //    }

    //    Debug.Log("最大手札が１減った");
    //}

    //void AbsorbHp(int attackID, int damage)
    //{
    //    if (damage == 0)
    //    {
    //        return;
    //    }

    //    if (attackID == 1)
    //    {
    //        player.hp += damage;
    //    }
    //    else
    //    {
    //        enemy.hp += damage;
    //    }

    //    Debug.Log(damage + "カロリー吸収した");
    //}

    //void DecreaseMyHp1(int attackID)
    //{

    //    if (attackID == 1)
    //    {
    //        player.hp -= 100;
    //    }
    //    else
    //    {
    //        enemy.hp -= 100;
    //    }

    //    Debug.Log("100Calの反動");
    //}

    //void DecreaseMyHp2(int attackID)
    //{

    //    if (attackID == 1)
    //    {
    //        player.hp -= 200;
    //    }
    //    else
    //    {
    //        enemy.hp -= 200;
    //    }

    //    Debug.Log("200Calの反動");
    //}


    //相手の防御カード弱体
    //void WeakDefenceCard(int attackID, int damage)
    //{
    //    if (damage == 0)
    //    {
    //        return;
    //    }

    //    int playerID = GameManager.instance.playerID;




    //    Transform hand;

    //    if (attackID == 1)
    //    {
    //        hand = GameManager.instance.enemyHandTransform;
    //    }
    //    else
    //    {
    //        hand = GameManager.instance.playerHandTransform;
    //    }

    //    foreach (Transform card in hand)
    //    {
    //        CardController cardController = card.GetComponent<CardController>();

    //        if (cardController.model.kind == 1)
    //        {
    //            cardController.model.cal -= 200;
    //            if (cardController.model.cal < 0)
    //            {
    //                cardController.model.cal = 1;
    //            }
    //            cardController.view.Refresh(cardController.model);
    //        }

    //    }

    //    Debug.Log("防御カードのCalが200減った");

    //}


    //自分の防御カード強化
    //void StrengthenDefenceCard(int attackID)
    //{

    //    Transform hand;

    //    if (attackID == 1)
    //    {
    //        hand = GameManager.instance.playerHandTransform;
    //    }
    //    else
    //    {
    //        hand = GameManager.instance.enemyHandTransform;
    //    }

    //    foreach (Transform card in hand)
    //    {
    //        CardController cardController = card.GetComponent<CardController>();

    //        if (cardController.model.kind == 1)
    //        {
    //            cardController.model.cal += 200;
    //            cardController.view.Refresh(cardController.model);
    //        }

    //    }

    //    Debug.Log("防御カードのCalが200増えた");

    //}
}
