using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject resultPanel;
    [SerializeField] Text resultText;


    [SerializeField] Text[] playerHpText = new Text[2];

    [SerializeField] EnergyBar[] playerHpBar = new EnergyBar[2];

    [SerializeField] EnergyBar[] playerCostBar1 = new EnergyBar[2];
    [SerializeField] EnergyBar[] playerCostBar2 = new EnergyBar[2];
    [SerializeField] Text[] playerCostText1 = new Text[2];
    [SerializeField] Text[] playerCostText2 = new Text[2];



    [SerializeField] Text[] poisonText = new Text[2];
    [SerializeField] Text[] darkText = new Text[2];
    [SerializeField] Text[] paralysisText = new Text[2];



    [SerializeField] Text timeCountText;




    //[SerializeField] public GameObject decideButtonObj;



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


    public void UpdateTime(int timeCount)
    {
        timeCountText.text = timeCount.ToString();
    }

    public void ShowHP()
    {
        for (int i = 0; i < 2; i++)
        {
            playerHpBar[i].valueCurrent = player[i].hp;
            playerHpText[i].text = player[i].hp + " / " + 1500;
        }

    }

    public void ShowCost()
    {
        for (int i = 0; i < 2; i++)
        {
            if (player[i].cost > 3)
            {
                playerCostBar1[i].valueCurrent = 3;
                playerCostText1[i].text = 3.ToString();
                playerCostBar2[i].valueCurrent = player[i].cost - 3;
                playerCostText2[i].text = (player[i].cost - 3).ToString();
            }
            else
            {
                playerCostBar1[i].valueCurrent = player[i].cost;
                playerCostText1[i].text = player[i].cost.ToString();
                playerCostBar2[i].valueCurrent = 0;
                playerCostText2[i].text = 0.ToString();
            }
 
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







    public void ShowStatus()
    {

        for (int i = 0; i < 2; i++)
        { 

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
                darkText[i].text = "闇" + player[i].darkCount;
            }
            else
            {
                darkText[i].text = "";
            }
            if (player[i].paralysisCount > 0)
            {
                paralysisText[i].text = "麻" + player[i].paralysisCount;
            }
            else
            {
                paralysisText[i].text = "";
            }


        }


    }





}
