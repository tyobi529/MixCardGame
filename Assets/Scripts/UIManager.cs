using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject resultPanel;
    [SerializeField] Text resultText;


    [SerializeField] Text playerHpText;
    [SerializeField] Text enemyHpText;

    [SerializeField] Slider playerHpBar;
    [SerializeField] Slider enemyHpBar;

    [SerializeField] Text playerCostText;
    [SerializeField] Text enemyCostText;

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

    [SerializeField] Text playerAttackUpText;
    [SerializeField] Text enemyAttackUpText;

    [SerializeField] Text playerDefenceUpText;
    [SerializeField] Text enemyDefenceUpText;


    [SerializeField] Text playerHitUpText;
    [SerializeField] Text enemyHitUpText;

    [SerializeField] Text playerHealthText;
    [SerializeField] Text enemyHealthText;


    //[SerializeField] Text expectDamageText;



    //[SerializeField] Text playerManaCostText;
    //[SerializeField] Text enemyManaCostText;

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

    [SerializeField] GamePlayerManager player;
    [SerializeField] GamePlayerManager enemy;

    [SerializeField] TriangleDrawer playerTriangle;
    [SerializeField] TriangleDrawer enemyTriangle;

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
        //Debug.Log("aa");
        playerHpBar.value = player.hp;
        enemyHpBar.value = enemy.hp;
        playerHpText.text = player.hp.ToString();
        enemyHpText.text = enemy.hp.ToString();
    }

    public void ShowNutrients()
    {
        //if (player.isDark)
        //{
        //    playerHeroRedText.text = "?";
        //    playerHeroYellowText.text = "?";
        //    playerHeroGreenText.text = "?";

        //    enemyHeroRedText.text = "?";
        //    enemyHeroYellowText.text = "?";
        //    enemyHeroGreenText.text = "?";
        //}
        //else
        //{
        //    playerHeroRedText.text = player.red.ToString();
        //    playerHeroYellowText.text = player.yellow.ToString();
        //    playerHeroGreenText.text = player.green.ToString();

        //    enemyHeroRedText.text = enemy.red.ToString();
        //    enemyHeroYellowText.text = enemy.yellow.ToString();
        //    enemyHeroGreenText.text = enemy.green.ToString();
        //}

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

    //交換ボタン
    //public void OnSwapButton()
    //{
    //    GameManager.instance.OnSwapButton();

    //}



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
        playerCostText.text = "コスト" + player.cost;
        enemyCostText.text = "コスト" + enemy.cost;

        playerPoisonText.text = "毒" + player.poisonCount;
        enemyPoisonText.text = "毒" + enemy.poisonCount;

        playerDarkText.text = "闇" + player.darkCount;
        enemyDarkText.text = "闇" + enemy.darkCount;

        playerAttackUpText.text = "攻" + player.attackUpCount;
        enemyAttackUpText.text = "攻" + enemy.attackUpCount;

        playerDefenceUpText.text = "守" + player.defenceUpCount;
        enemyDefenceUpText.text = "守" + enemy.defenceUpCount;

    }

    public void ShowPoison()
    {
        playerPoisonText.text = "毒" + player.poisonCount;
        enemyPoisonText.text = "毒" + enemy.poisonCount;




        //if (playerPoison)
        //{
        //    playerPoisonText.text = "毒" + playerPoisonCount;
        //}
        //else
        //{
        //    playerPoisonText.text = "";
        //}


        //if (enemyPoison)
        //{
        //    enemyPoisonText.text = "毒" + enemyPoisonCount;
        //}
        //else
        //{
        //    enemyPoisonText.text = "";

        //}

    }

    public void ShowDark()
    {
        //if (playerDark)
        //{
        //    playerDarkText.text = "闇";
        //}
        //else
        //{
        //    playerDarkText.text = "";
        //}


        //if (enemyDark)
        //{
        //    enemyDarkText.text = "闇";
        //}
        //else
        //{
        //    enemyDarkText.text = "";

        //}

    }

    public void ShowAttackUp(int playerAttackUp, int enemyAttackUp)
    {
        if (playerAttackUp == 0)
        {
            playerAttackUpText.text = "";
        }
        else
        {
            playerAttackUpText.text = "攻" + playerAttackUp;
        }
        

        if (enemyAttackUp == 0)
        {
            enemyAttackUpText.text = "";
        }
        else
        {
            enemyAttackUpText.text = "攻" + enemyAttackUp;
        }
        

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
