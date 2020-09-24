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
    public void OnClickTurnEndButton()
    {
        GameManager.instance.OnClickTurnEndButton();
    }
}
