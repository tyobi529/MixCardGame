using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject resultPanel;
    [SerializeField] Text resultText;


    [SerializeField] Text[] hpText = new Text[2];

    [SerializeField] EnergyBar[] hpBar = new EnergyBar[2];


    [SerializeField] EnergyBar[] costBar = new EnergyBar[2];

      

    [SerializeField] ImageNumber[] costText = new ImageNumber[2];
    


    [SerializeField] Text[] poisonText = new Text[2];
    [SerializeField] Text[] darkText = new Text[2];
    [SerializeField] Text[] paralysisText = new Text[2];

    


    [SerializeField] public GameObject attackFields;


    [SerializeField] public GameObject lackCostText;

    [SerializeField] GamePlayerManager[] player = new GamePlayerManager[2];

    [SerializeField] GameObject countNumber;
    [SerializeField] Sprite[] numberSprite = new Sprite[6];

    [SerializeField] public GameObject cookingObject;


    [SerializeField] ImageNumber[] damageText = new ImageNumber[2];
    [SerializeField] ImageNumber[] healText = new ImageNumber[2];


    int[] currentCost = new int[2] { 0, 0 };


    //シングルトン化（どこからでもアクセスできるようにする）
    public static UIManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }


        cookingObject.SetActive(true);
    }


    private void Start()
    {
        //テスト
        //int cost = 3;
        //costBar[0].valueCurrent = cost;

        //StartCoroutine(ShowCost(6, false));

    }

    public IEnumerator ShowCost(bool isMyTurn)
    {

        int id = -1;

        if (isMyTurn)
        {
            id = 0;
        }
        else
        {
            id = 1;
        }

    
        int cost = player[id].cost;

        Debug.Log("今" + cost);

        if (currentCost[id] < cost)
        {
            for (int i = 0; i < cost - currentCost[id]; i++)
            {
                costBar[id].valueCurrent = currentCost[id] + i + 1;
                yield return new WaitForSeconds(0.3f);

                Debug.Log("コスト" + (currentCost[id] + i + 1));

                costText[id].Setno(currentCost[id] + i + 1);

            }
        }
        else if (currentCost[id] > cost)
        {
            for (int i = 0; i < currentCost[id] - cost; i++)
            {
                costBar[id].valueCurrent = currentCost[id] - i - 1;
                yield return new WaitForSeconds(0.3f);

                Debug.Log("コスト" + (currentCost[id] - i - 1));

                costText[id].Setno(currentCost[id] - i - 1);

            }
        }

        currentCost[id] = cost;

    }


    public void HideResultPanel()
    {
        resultPanel.SetActive(false);

    }


    public void UpdateTime(int timeCount)
    {
        if (timeCount <= 5)
        {
            countNumber.SetActive(true);
            countNumber.GetComponent<Image>().sprite = numberSprite[timeCount];
        }
        else
        {
            countNumber.SetActive(false);
        }
    }

    public void ShowHP()
    {
        for (int i = 0; i < 2; i++)
        {
            hpBar[i].valueCurrent = player[i].hp;
            hpText[i].text = player[i].hp + " / " + 1500;
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


    public void ShowDamageText(int damageCal, bool isMyTurn)
    {        
        if (isMyTurn)
        {
            damageText[1].Setno(damageCal);
            damageText[1].gameObject.SetActive(true);
        }
        else
        {
            damageText[0].Setno(damageCal);
            damageText[0].gameObject.SetActive(true);
        }

        ShowHP();
    }

    public void ShowHealText(int healCal, bool isMyTurn)
    {
        if (isMyTurn)
        {
            healText[0].Setno(healCal);
            healText[0].gameObject.SetActive(true);
        }
        else
        {
            healText[1].Setno(healCal);
            healText[1].gameObject.SetActive(true);
        }

        ShowHP();
    }


    //void BlinkingMatchingText()
    //{

    //}

    



}
