using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialController : MonoBehaviour
{
    public GamePlayerManager player;
    public GamePlayerManager enemy;

    public void CheckSpecial(int special, int damage)
    {
        int attackID = GameManager.instance.attackID;

        switch (special)
        {
            case 1:
                Heal1(attackID);
                break;
            case 2:
                Heal2(attackID);
                break;
            case 3:
                Poison(attackID, damage);
                break;
            case 4:
                DeadlyPoison(attackID, damage);
                break;
            case 5:
                ChangeHandCard(attackID);
                break;
            case 6:
                DecreaseRed(attackID, damage);
                break;
            case 7:
                DecreaseYellow(attackID, damage);
                break;
            case 8:
                DecreaseGreen(attackID, damage);
                break;
            case 9:
                IncreaseRed(attackID);
                break;
            case 10:
                IncreaseYellow(attackID);
                break;
            case 11:
                IncreaseGreen(attackID);
                break;
            case 12:
                IncreaseRed(attackID);
                IncreaseYellow(attackID);
                IncreaseGreen(attackID);
                break;
            case 13:
                DecreaseHandCard(attackID, damage);
                break;
            case 14:
                AbsorbHp(attackID, damage);
                break;
            case 15:
                DecreaseMyHp1(attackID);
                break;
            case 16:
                DecreaseMyHp2(attackID);
                break;
            default:
                break;
        }
    }


    void Poison(int attackID, int damage)
    {
        if (damage == 0)
        {
            return;
        }

        if (attackID == 1)
        {
            if (enemy.isPoison)
            {
                enemy.isDeadlyPoison = true;
            }
            else
            {
                enemy.isPoison = true;

            }
        }
        else
        {
            if (player.isPoison)
            {
                player.isDeadlyPoison = true;
            }
            else
            {
                player.isPoison = true;

            }
        }

        Debug.Log("毒付与");
    }

    void DeadlyPoison(int attackID, int damage)
    {
        if (damage == 0)
        {
            return;
        }

        if (attackID == 1)
        {

            enemy.isDeadlyPoison = true;

        }
        else
        {

            player.isDeadlyPoison = true;


        }

        Debug.Log("猛毒付与");
    }

    void Heal1(int attackID)
    {

        if (attackID == 1)
        {
            player.heroHp += 500;
        }
        else
        {
            enemy.heroHp += 500;

        }

        Debug.Log("500回復");
    }

    void Heal2(int attackID)
    {

        if (attackID == 1)
        {
            player.heroHp += 1000;
        }
        else
        {
            enemy.heroHp += 1000;

        }

        Debug.Log("1000回復");
    }

    void ChangeHandCard(int attackID)
    {
        int playerID = GameManager.instance.playerID;




        Transform hand;
        GamePlayerManager hero;

        if (attackID == 1)
        {
            hand = GameManager.instance.playerHandTransform;
            hero = player;
        }
        else
        {
            hand = GameManager.instance.enemyHandTransform;
            hero = enemy;
        }

        foreach (Transform card in hand)
        {
            Destroy(card.gameObject);

        }

        Debug.Log("手札" + GameManager.instance.maxHand);
        for (int i = 0; i < GameManager.instance.maxHand; i++)
        {
            GameManager.instance.GiveCardToHand(attackID);

        }



        //Debug.Log("手札" + hand.childCount);

        //GameManager.instance.SettingInitHand();

        Debug.Log("手札入れ替え");
    }


    void DecreaseRed(int attackID, int damage)
    {
        if (damage == 0)
        {
            return;
        }

        if (attackID == 1)
        {
            enemy.heroRed--;

        }
        else
        {
            player.heroRed--;

        }

        Debug.Log("赤が１減った");
    }

    void DecreaseYellow(int attackID, int damage)
    {
        if (damage == 0)
        {
            return;
        }

        if (attackID == 1)
        {
            enemy.heroYellow--;

        }
        else
        {
            player.heroYellow--;

        }

        Debug.Log("黄が１減った");
    }

    void DecreaseGreen(int attackID, int damage)
    {
        if (damage == 0)
        {
            return;
        }

        if (attackID == 1)
        {
            enemy.heroGreen--;

        }
        else
        {
            player.heroGreen--;

        }

        Debug.Log("緑が１減った");
    }

    void IncreaseRed(int attackID)
    {
        if (attackID == 1)
        {
            player.heroRed++;

        }
        else
        {
            enemy.heroRed++;

        }

        Debug.Log("赤が１増えた");
    }

    void IncreaseYellow(int attackID)
    {
        if (attackID == 1)
        {
            player.heroYellow++;

        }
        else
        {
            enemy.heroYellow++;

        }

        Debug.Log("黄が１増えた");
    }

    void IncreaseGreen(int attackID)
    {
        if (attackID == 1)
        {
            player.heroGreen++;

        }
        else
        {
            enemy.heroGreen++;

        }

        Debug.Log("緑が１増えた");
    }



    void DecreaseHandCard(int attackID, int damage)
    {
        if (damage == 0)
        {
            return;
        }

        if (attackID != GameManager.instance.playerID)
        {
            GameManager.instance.maxHand--;

        }

        Debug.Log("最大手札が１減った");
    }

    void AbsorbHp(int attackID, int damage)
    {
        if (damage == 0)
        {
            return;
        }

        if (attackID == 1)
        {
            player.heroHp += damage;
        }
        else
        {
            enemy.heroHp += damage;
        }

        Debug.Log(damage + "カロリー吸収した");
    }

    void DecreaseMyHp1(int attackID)
    {

        if (attackID == 1)
        {
            player.heroHp -= 100;
        }
        else
        {
            enemy.heroHp -= 100;
        }

        Debug.Log("100Calの反動");
    }

    void DecreaseMyHp2(int attackID)
    {

        if (attackID == 1)
        {
            player.heroHp -= 200;
        }
        else
        {
            enemy.heroHp -= 200;
        }

        Debug.Log("200Calの反動");
    }


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
