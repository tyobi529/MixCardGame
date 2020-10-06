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

    [SerializeField] Text playerHeroRedText;
    [SerializeField] Text playerHeroYellowText;
    [SerializeField] Text playerHeroGreenText;

    [SerializeField] Text enemyHeroRedText;
    [SerializeField] Text enemyHeroYellowText;
    [SerializeField] Text enemyHeroGreenText;

    [SerializeField] Text playerStatusText;
    [SerializeField] Text enemyStatusText;

    [SerializeField] Text playerHealthText;
    [SerializeField] Text enemyHealthText;


    [SerializeField] Text expectDamageText;



    [SerializeField] Text playerManaCostText;
    [SerializeField] Text enemyManaCostText;

    [SerializeField] Text timeCountText;


    [SerializeField] GameObject turnPanel;
    [SerializeField] Text phaseText;
    [SerializeField] Text turnText;


    //[SerializeField] public GameObject buttonsObj;


    [SerializeField] public GameObject decideButtonObj;

    [SerializeField] public GameObject ChangeButtonObj;

    //[SerializeField] GameObject cancelButtonObj;

    [SerializeField] public Image playerFieldImage;
    [SerializeField] public Image enemyFieldImage;


    [SerializeField] public Image basicFieldImage;
    [SerializeField] public Image additionalFieldImage;
    [SerializeField] public Image resultFieldImage;


    [SerializeField] public GameObject attackFields;
    [SerializeField] public GameObject defenceFields;



    [SerializeField] public GameObject lackCostText;



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

    public void ShowHeroNutrients(int playerHeroRed, int playerHeroYellow, int playerHeroGreen, int enemyHeroRed, int enemyHeroYellow, int enemyHeroGreen)
    {
        playerHeroRedText.text = playerHeroRed.ToString();
        playerHeroYellowText.text = playerHeroYellow.ToString();
        playerHeroGreenText.text = playerHeroGreen.ToString();

        enemyHeroRedText.text = enemyHeroRed.ToString();
        enemyHeroYellowText.text = enemyHeroYellow.ToString();
        enemyHeroGreenText.text = enemyHeroGreen.ToString();
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


    public void ShowPoison(bool playerPoison, bool playerDeadlyPoison, bool enemyPoison, bool enemyDeadlyPoison)
    {
        if (playerDeadlyPoison)
        {
            playerStatusText.text = "猛毒";
        }
        else if (playerPoison)
        {
            playerStatusText.text = "毒";
        }
        else
        {
            playerStatusText.text = "";
        }

        if (enemyDeadlyPoison)
        {
            enemyStatusText.text = "猛毒";
        }
        else if (enemyPoison)
        {
            enemyStatusText.text = "毒";
        }
        else
        {
            enemyStatusText.text = "";

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

    public void OnChangeButton()
    {
        GameManager.instance.isChange = !GameManager.instance.isChange;

        Transform hand;

        if (GameManager.instance.playerID == 1)
        {
            hand = GameManager.instance.playerHandTransform;
        }
        else
        {
            hand = GameManager.instance.enemyHandTransform;
        }

        if (GameManager.instance.isChange)
        {
            ChangeButtonObj.GetComponent<Image>().color = Color.yellow;

            //防御カードも移動可能に
            GameManager.instance.ChangeDraggable(hand, 1, true);
        }
        else
        {
            ChangeButtonObj.GetComponent<Image>().color = Color.white;

            GameManager.instance.ChangeDraggable(hand, 1, false);
        }
    }


    //キャンセルボタン
    //public void OnCancelButton()
    //{
    //    if (GameManager.instance.playerID == 1)
    //    {
    //        playerFieldImage.color = new Color(1, 1, 1, 100f / 255f);
    //        playerFieldImage.enabled = false;

    //    }
    //    else
    //    {
    //        enemyFieldImage.color = new Color(1, 1, 1, 100f / 255f);
    //        enemyFieldImage.enabled = false;

    //    }

    //    ShowButtonObj(true);

    //}





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


    public void ShowExpectDamage()
    {
        int damage = GameManager.instance.CalculateDamage();
        expectDamageText.text = damage + "Cal";
    }


    public void ShowHealth(int playerHealth, int enemyHealth)
    {
        //if (playerHealth == 0)
        //{
        //    playerHealthText.text = "不調";
        //}
        //else if (playerHealth == 1)
        //{
        //    playerHealthText.text = "普通";
        //}
        //else if (playerHealth == 2)
        //{
        //    playerHealthText.text = "健康";
        //}

        switch (playerHealth)
        {
            case 0:
                playerHealthText.text = "不調";
                break;
            case 1:
                playerHealthText.text = "普通";
                break;
            case 2:
                playerHealthText.text = "健康";
                break;
            default:
                break;
        }

        switch (enemyHealth)
        {
            case 0:
                enemyHealthText.text = "不調";
                break;
            case 1:
                enemyHealthText.text = "普通";
                break;
            case 2:
                enemyHealthText.text = "健康";
                break;
            default:
                break;
        }
    }



}
