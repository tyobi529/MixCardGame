﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject resultPanel;
    [SerializeField] Text resultText;


    [SerializeField] Text[] playerHpText = new Text[2];

    [SerializeField] Slider[] playerHpBar = new Slider[2];

    [SerializeField] Text[] playerCostText;

    [SerializeField] Text playerHeroRedText;
    [SerializeField] Text playerHeroYellowText;
    [SerializeField] Text playerHeroGreenText;

    [SerializeField] Text enemyHeroRedText;
    [SerializeField] Text enemyHeroYellowText;
    [SerializeField] Text enemyHeroGreenText;

    [SerializeField] Text playerPoisonText;
    [SerializeField] Text enemyPoisonText;
    [SerializeField] Text playerDarkText;
    [SerializeField] Text enemyDarkText;

    [SerializeField] Text[] dishBonusText = new Text[2];
    [SerializeField] Text[] redBonusText = new Text[2];
    [SerializeField] Text[] yellowBonusText = new Text[2];
    [SerializeField] Text[] greenBonusText = new Text[2];


    [SerializeField] Text playerDefenceUpText;
    [SerializeField] Text enemyDefenceUpText;


    [SerializeField] Text playerHitUpText;
    [SerializeField] Text enemyHitUpText;

    [SerializeField] Text playerHealthText;
    [SerializeField] Text enemyHealthText;




    [SerializeField] Text timeCountText;




    [SerializeField] public GameObject decideButtonObj;

    [SerializeField] public GameObject ChangeButtonObj;


    [SerializeField] public Image playerFieldImage;
    [SerializeField] public Image enemyFieldImage;


    [SerializeField] public Image basicFieldImage;
    [SerializeField] public Image additionalFieldImage;
    [SerializeField] public Image resultFieldImage;


    [SerializeField] public GameObject attackFields;
    [SerializeField] public GameObject defenceFields;



    [SerializeField] public GameObject lackCostText;

    [SerializeField] GamePlayerManager[] player = new GamePlayerManager[2];

    [SerializeField] TriangleDrawer playerTriangle;
    [SerializeField] TriangleDrawer enemyTriangle;

    [SerializeField] FieldController[] currentFieldController = new FieldController[4];
    [SerializeField] Text[] deadLineText = new Text[4];

    //シングルトン化（どこからでもアクセスできるようにする）
    public static UIManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void HideResultPanel()
    {
        resultPanel.SetActive(false);

    }

    //public void ShowMixCost(int playerMixCost, int enemyMixCost)
    //{
    //    playerManaCostText.text = playerMixCost.ToString();
    //    enemyManaCostText.text = enemyMixCost.ToString();
    //}

    public void UpdateTime(int timeCount)
    {
        timeCountText.text = timeCount.ToString();
    }

    public void ShowHP()
    {
        for (int i = 0; i < 2; i++)
        {
            playerHpBar[i].value = player[i].hp;
            playerHpText[i].text = player[i].hp.ToString();
        }

    }

    public void ShowDeadLine()
    {
        for (int i = 0; i < 4; i++)
        {
            deadLineText[i].text = currentFieldController[i].deadLine + "ターン";
        }
    }

    public void ShowNutrients()
    {

    }

    public void ShowResultPanel(int heroHp)
    {
        resultPanel.SetActive(true);
        if (heroHp <= 0)
        {
            resultText.text = "LOSE";
        }
        else
        {
            resultText.text = "WIN";
        }
    }




    public void ShowTriangle()
    {
        playerTriangle.SetVerticesDirty();
        enemyTriangle.SetVerticesDirty();

    }








    //ターンエンドボタン
    public void OnDecideButton()
    {
        GameManager.instance.OnDecideButton();

    }



    //true:攻撃、合成ボタン出す
    //false:決定、キャンセルボタン出す
    public void ShowButtonObj(bool canView)
    {


        decideButtonObj.SetActive(!canView);
        //cancelButtonObj.SetActive(!canView);

    }

    //falseにするとフィールドのimageを消す
    public void ShowFieldImage(bool canView)
    {

        playerFieldImage.enabled = canView;
        enemyFieldImage.enabled = canView;


    }




    public void ShowHealth(bool playerHealth, bool enemyHealth)
    {
        if (playerHealth)
        {
            playerHealthText.text = "健康";

        }
        else
        {
            playerHealthText.text = "不調";

        }

        if (enemyHealth)
        {
            enemyHealthText.text = "健康";

        }
        else
        {
            enemyHealthText.text = "不調";

        }

       
    }


    public void ShowStatus()
    {

        for (int i = 0; i < 2; i++)
        {
            playerCostText[i].text = "コスト" + player[i].cost;

            if (player[i].dishBonus)
            {
                dishBonusText[i].text = "全＋";
            }
            else
            {
                dishBonusText[i].text = "";
            }

            if (player[i].redBonus)
            {
                redBonusText[i].text = "赤＋";
            }
            else
            {
                redBonusText[i].text = "";
            }

            if (player[i].yellowBonus)
            {
                yellowBonusText[i].text = "黄＋";
            }
            else
            {
                yellowBonusText[i].text = "";
            }

            if (player[i].greenBonus)
            {
                greenBonusText[i].text = "緑＋";
            }
            else
            {
                greenBonusText[i].text = "";
            }

        }

        //playerPoisonText.text = "毒" + player.poisonCount;
        //enemyPoisonText.text = "毒" + enemy.poisonCount;

        //playerDarkText.text = "闇" + player.darkCount;
        //enemyDarkText.text = "闇" + enemy.darkCount;

        //playerAttackUpText.text = "攻" + player.attackUpCount;
        //enemyAttackUpText.text = "攻" + enemy.attackUpCount;

        //playerDefenceUpText.text = "守" + player.defenceUpCount;
        //enemyDefenceUpText.text = "守" + enemy.defenceUpCount;

    }

    public void ShowPoison()
    {
        //playerPoisonText.text = "毒" + player.poisonCount;
        //enemyPoisonText.text = "毒" + enemy.poisonCount;



    }

    public void ShowDark()
    {


    }

    public void ShowAttackUp(int playerAttackUp, int enemyAttackUp)
    {
        //if (playerAttackUp == 0)
        //{
        //    playerAttackUpText.text = "";
        //}
        //else
        //{
        //    playerAttackUpText.text = "攻" + playerAttackUp;
        //}
        

        //if (enemyAttackUp == 0)
        //{
        //    enemyAttackUpText.text = "";
        //}
        //else
        //{
        //    enemyAttackUpText.text = "攻" + enemyAttackUp;
        //}
        

    }


    public void ShowHitUp(int playerHitUp, int enemyHitUp)
    {
        if (playerHitUp == 0)
        {
            playerHitUpText.text = "";
        }
        else
        {
            playerHitUpText.text = "命" + playerHitUp;
        }

        if (enemyHitUp == 0)
        {
            enemyHitUpText.text = "";
        }
        else
        {
            enemyHitUpText.text = "命" + enemyHitUp;
        }
        
        

    }






}
