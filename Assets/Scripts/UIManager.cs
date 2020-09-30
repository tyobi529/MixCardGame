using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject resultPanel;
    [SerializeField] Text resultText;


    [SerializeField] Text playerHeroHpText;
    [SerializeField] Text enemyHeroHpText;

    [SerializeField] Text playerManaCostText;
    [SerializeField] Text enemyManaCostText;

    [SerializeField] Text timeCountText;


    [SerializeField] GameObject turnPanel;
    [SerializeField] Text phaseText;
    [SerializeField] Text turnText;


    //[SerializeField] public GameObject buttonsObj;


    [SerializeField] public GameObject decideButtonObj;
    //[SerializeField] GameObject cancelButtonObj;

    [SerializeField] public Image playerFieldImage;
    [SerializeField] public Image enemyFieldImage;


    [SerializeField] public Image basicFieldImage;
    [SerializeField] public Image additionalFieldImage;
    [SerializeField] public Image resultFieldImage;


    [SerializeField] public GameObject attackFields;
    [SerializeField] public GameObject defenceFields;



    [SerializeField] public GameObject lackCostText;




    public void HideResultPanel()
    {
        resultPanel.SetActive(false);

    }

    public void ShowMixCost(int playerMixCost, int enemyMixCost)
    {
        playerManaCostText.text = playerMixCost.ToString();
        enemyManaCostText.text = enemyMixCost.ToString();
    }

    public void UpdateTime(int timeCount)
    {
        timeCountText.text = timeCount.ToString();
    }

    public void ShowHeroHP(int playerHeroHp, int enemyHeroHp)
    {
        playerHeroHpText.text = playerHeroHp.ToString();
        enemyHeroHpText.text = enemyHeroHp.ToString();
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


    public void ShowTurn(int attackID)
    {
        if (GameManager.instance.playerID == attackID)
            phaseText.text = "攻撃ターン";
        else
            phaseText.text = "防御ターン";

        if (GameManager.instance.isMyTurn)
        {
            turnPanel.GetComponent<Image>().color = Color.yellow;
            turnText.text = "あなたの番です";

        }
        else
        {
            turnPanel.GetComponent<Image>().color = Color.white;
            turnText.text = "相手の番です";
        }
            

    }






    //ターンエンドボタン
    public void OnDecideButton()
    {
        //if (GameManager.instance.playerID == 1)
        //{
        //    playerFieldImage.color = new Color(1, 1, 1, 100f / 255f);
        //}
        //else
        //{
        //    enemyFieldImage.color = new Color(1, 1, 1, 100f / 255f);
        //}

        //basicFieldImage.enabled = false;
        //additionalFieldImage.enabled = false;
        //resultFieldImage.enabled = false;

        attackFields.SetActive(false);

        playerFieldImage.enabled = true;
        enemyFieldImage.enabled = true;




        ////全てのボタンを消す
        //buttonsObj.SetActive(false);

        //GameManager.instance.CheckField();
        GameManager.instance.OnDecideButton();

        decideButtonObj.SetActive(false);
    }


    //キャンセルボタン
    public void OnCancelButton()
    {
        if (GameManager.instance.playerID == 1)
        {
            playerFieldImage.color = new Color(1, 1, 1, 100f / 255f);
            playerFieldImage.enabled = false;

        }
        else
        {
            enemyFieldImage.color = new Color(1, 1, 1, 100f / 255f);
            enemyFieldImage.enabled = false;

        }

        ShowButtonObj(true);

    }





    void ReturnFieldCard()
    {
        CardController[] fieldCardList;

        if (GameManager.instance.playerID == 1)
        {
            fieldCardList = GameManager.instance.playerFieldTransform.GetComponentsInChildren<CardController>();
        }
        else
        {
            fieldCardList = GameManager.instance.enemyFieldTransform.GetComponentsInChildren<CardController>();
        }

        //フィールドのカードを手札に戻す
        //foreach (CardController card in fieldCardList)
        //{
        //    card.MoveToHand();
        //}

        //カード並び替え
        GameManager.instance.LineUpCard(GameManager.instance.playerFieldTransform);
        GameManager.instance.LineUpCard(GameManager.instance.enemyFieldTransform);
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






}
