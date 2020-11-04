using System.Collections;
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



    [SerializeField] Text playerPoisonText;
    [SerializeField] Text enemyPoisonText;
    [SerializeField] Text playerDarkText;
    [SerializeField] Text enemyDarkText;

    [SerializeField] Text[] nutrientBonusText = new Text[2];
    [SerializeField] Text[] dishBonusText = new Text[2];






    [SerializeField] Text timeCountText;




    [SerializeField] public GameObject decideButtonObj;

    [SerializeField] public GameObject ChangeButtonObj;


    [SerializeField] public Image basicFieldImage;
    [SerializeField] public Image additionalFieldImage;
    [SerializeField] public Image resultFieldImage;


    [SerializeField] public GameObject attackFields;
    [SerializeField] public GameObject defenceFields;



    [SerializeField] public GameObject lackCostText;

    [SerializeField] GamePlayerManager[] player = new GamePlayerManager[2];


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



    public void ShowStatus()
    {

        for (int i = 0; i < 2; i++)
        {
            playerCostText[i].text = "コスト" + player[i].cost;

            if (player[i].nutrient == NUTRIENT.NONE)
            {
                nutrientBonusText[i].text = "";
            }
            else
            {
                nutrientBonusText[i].text = player[i].nutrient.ToString();
            }


            if (player[i].dish == DISH.NONE)
            {
                dishBonusText[i].text = "";
            }
            else
            {
                dishBonusText[i].text = player[i].dish.ToString();
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




}
