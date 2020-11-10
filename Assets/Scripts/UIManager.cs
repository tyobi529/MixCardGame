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



    [SerializeField] Text[] poisonText = new Text[2];
    [SerializeField] Text[] darkText = new Text[2];
    [SerializeField] Text[] paralysisText = new Text[2];
    [SerializeField] Text[] healthText = new Text[2];




    [SerializeField] Text[] dishBonusText_0 = new Text[2];
    [SerializeField] Text[] dishBonusText_1 = new Text[2];




    [SerializeField] Text timeCountText;




    [SerializeField] public GameObject decideButtonObj;



    [SerializeField] public GameObject attackFields;


    [SerializeField] public GameObject lackCostText;

    [SerializeField] GamePlayerManager[] player = new GamePlayerManager[2];


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

            dishBonusText_0[i].text = player[i].dish[0].ToString();
            dishBonusText_1[i].text = player[i].dish[1].ToString();

            if (player[i].dish[0] == DISH.NONE)
            {
                dishBonusText_0[i].text = "";
            }

            else if (player[i].dish[0] == DISH.RED)
            {
                dishBonusText_0[i].text = "赤料理";
            }
            else if (player[i].dish[0] == DISH.YELLOW)
            {
                dishBonusText_0[i].text = "黄料理";
            }
            else if (player[i].dish[0] == DISH.GREEN)
            {
                dishBonusText_0[i].text = "緑料理";
            }

            else if (player[i].dish[0] == DISH.JAPANESE)
            {
                dishBonusText_0[i].text = "和食";
            }
            else if (player[i].dish[0] == DISH.WESTERN)
            {
                dishBonusText_0[i].text = "洋食";
            }
            else if (player[i].dish[0] == DISH.CHINESE)
            {
                dishBonusText_0[i].text = "中華";
            }

            if (player[i].dish[1] == DISH.NONE)
            {
                dishBonusText_1[i].text = "";
            }

            else if (player[i].dish[1] == DISH.RED)
            {
                dishBonusText_1[i].text = "赤料理";
            }
            else if (player[i].dish[1] == DISH.YELLOW)
            {
                dishBonusText_1[i].text = "黄料理";
            }
            else if (player[i].dish[1] == DISH.GREEN)
            {
                dishBonusText_1[i].text = "緑料理";
            }

            else if (player[i].dish[0] == DISH.JAPANESE)
            {
                dishBonusText_1[i].text = "和食";
            }
            else if (player[i].dish[0] == DISH.WESTERN)
            {
                dishBonusText_1[i].text = "洋食";
            }
            else if (player[i].dish[0] == DISH.CHINESE)
            {
                dishBonusText_1[i].text = "中華";
            }



            if (player[i].poisonCount > 0)
            {
                poisonText[i].text = "毒" + player[i].poisonCount;
            }
            else
            {
                poisonText[i].text = "";
            }
            if (player[i].darkCount > 0)
            {
                darkText[i].text = "暗闇" + player[i].darkCount;
            }
            else
            {
                darkText[i].text = "";
            }
            if (player[i].paralysisCount > 0)
            {
                paralysisText[i].text = "麻痺" + player[i].paralysisCount;
            }
            else
            {
                paralysisText[i].text = "";
            }

            if (player[i].healthCount > 0)
            {
                healthText[i].text = "健康" + player[i].healthCount;
            }
            else
            {
                healthText[i].text = "";
            }


        }


    }





}
