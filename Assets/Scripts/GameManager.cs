using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;


public class GameManager : MonoBehaviourPunCallbacks
{
    //CardController cardPrefab;
    GameObject cardPrefab;
    GameObject dishCardPrefab;


    GamePlayerManager[] playerManager = new GamePlayerManager[2];

   
    int[] deck = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
    int cardCost = 0;

    [SerializeField] UIManager uiManager;




    Transform handTransform;


    Transform[] mixFieldTransform = new Transform[3];


    bool isMyTurn;





    //時間管理
    int timeCount;

    int playerID;




    SpecialController specialController;



    GameObject attackButtons;

    MixController mixController;







    [SerializeField] int maxHand;
    int defaultMaxHand;


    int cardIndex = 0;

    int damageCal;

    CardController[] selectCardController = new CardController[2] { null, null };


    CardController[] mixCardController = new CardController[3] { null, null, null };


    //賞味期限
    [SerializeField] int deadLine;
    //合成に必要なコスト
    [SerializeField] int mixCost;


    //シングルトン化（どこからでもアクセスできるようにする）
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        cardPrefab = (GameObject)Resources.Load("Card");
        dishCardPrefab = (GameObject)Resources.Load("DishCard");

        playerManager[0] = GameObject.Find("Player").GetComponent<GamePlayerManager>();
        playerManager[1] = GameObject.Find("Enemy").GetComponent<GamePlayerManager>();

        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();



        GameObject[] fields = new GameObject[2];

        fields[0] = GameObject.Find("NextFields");
        fields[1] = GameObject.Find("CurrentFields");

        handTransform = GameObject.Find("Hand").transform;


        for (int i = 0; i < 3; i++)
        {
            mixFieldTransform[i] = GameObject.Find("MixField_" + i.ToString()).transform;
        }



        specialController = GameObject.Find("SpecialController").GetComponent<SpecialController>();

        attackButtons = GameObject.Find("AttackButtons");

        playerID = PhotonNetwork.LocalPlayer.ActorNumber;

        defaultMaxHand = maxHand;



    }

    void Start()
    {


        if (playerID == 2)
        {
            photonView.RPC(nameof(StartGame), RpcTarget.AllViaServer);



        }


    }



    [PunRPC]
    void StartGame()
    {



        //プレイヤー１が先行
        if (playerID == 1)
        {
            isMyTurn = true;
        }
        else
        {
            isMyTurn = false;
        }

        uiManager.attackFields.SetActive(isMyTurn);



        uiManager.ShowHP();


        ShuffleCard();
        GiveCardToHand();


        uiManager.ShowStatus();

        //uiManager.ShowHealth(player.isHealth, enemy.isHealth);
        //uiManager.ShowPoison(player.isPoison, player.poisonCount, enemy.isPoison, enemy.poisonCount);
        //uiManager.ShowDark(player.isDark, enemy.isDark);
        //uiManager.ShowAttackUp(player.attackUp, enemy.attackUp);
        //uiManager.ShowHitUp(player.hitUp, enemy.hitUp);

        uiManager.decideButtonObj.SetActive(false);


    }


    void GiveCardToHand()
    {
        int drawNum = maxHand - handTransform.childCount;

        for (int i = 0; i < drawNum; i++)
        {
            int cardID = deck[cardIndex];
            //int cal = UnityEngine.Random.Range(20, 60);
            int specialID = UnityEngine.Random.Range(0, 6);
            int cost = cardCost;

            cardCost++;
            if (cardCost > 3)
            {
                cardCost = 0;
            }

            CreateCard(cardID, cost, specialID, handTransform);
           

            if (cardIndex == 8)
            {
                ShuffleCard();
                cardIndex = 0;
            }
            else
            {
                cardIndex++;
            }
        }



    }


    CardController CreateCard(int cardID, int cost, int specialID, Transform position)
    {

        GameObject card = Instantiate(cardPrefab, position, false);


        card.GetComponent<CardController>().Init(KIND.INGREDIENT, cardID, cost, specialID);

        return card.GetComponent<CardController>();

    }

    CardController CreateMixCard(KIND kind, int cardID, int cost, int specialID)
    {
        GameObject card = Instantiate(dishCardPrefab, mixFieldTransform[2], false);

        card.GetComponent<CardController>().MixInit(kind, cardID, cost, specialID);

        return card.GetComponent<CardController>();
    }




    public void OnDecideButton()
    {


        if (isMyTurn)
        {


            //生成
            int[] cardID = new int[2] { -1, -1 };
            int[] cost = new int[2] { -1, -1 };
            int[] specialID = new int[2] { -1, -1 };
            

            for (int i = 0; i < 2; i++)
            {
                if (selectCardController[i] != null)
                {
                    cardID[i] = selectCardController[i].model.cardID;
                    cost[i] = selectCardController[i].model.cost;
                    specialID[i] = selectCardController[i].model.specialID;
                    
                }

            }


            photonView.RPC(nameof(GenerateFieldCard), RpcTarget.AllViaServer, cardID, cost, specialID);

            ////使ったカードの削除
            for (int i = 0; i < 2; i++)
            {
                if (selectCardController[i] != null)
                {
                    Destroy(selectCardController[i].gameObject);
                }
                
            }

            


        }
        else
        {

            DamageCalculation();

        }


    }

    //合成予測を出す
    public void SelectCard(CardController cardController, bool isSelected)
    {
        //選択した時
        if (isSelected)
        {
            //何も選択されていない
            if (selectCardController[0] == null)
            {
                selectCardController[0] = cardController;
            }
            //２つ目に選択
            else if (selectCardController[1] == null)
            {
                selectCardController[1] = cardController;

            }
            //２つ目と入れ替え
            else
            {
                selectCardController[1].model.isSelected = false;
                selectCardController[1] = cardController;
            }

        }
        //キャンセルした時
        else
        {
            //１枚目に選択してた時
            if (selectCardController[0] == cardController)
            {
                //２枚目に選択してたカードなし
                if (selectCardController[1] == null)
                {
                    selectCardController[0] = null;
                }
                //２枚目に選択したカードを１枚目にする
                else
                {
                    selectCardController[0] = selectCardController[1];
                    selectCardController[1] = null;
                }
            }
            //２枚目に選択してた時
            else
            {
                selectCardController[1] = null;
            }

        }

        //生成
        int[] cardID = new int[2] { -1, -1 };
        int[] specialID = new int[2] { -1, -1 };
        int[] cost = new int[2] { -1, -1 };

        for (int i = 0; i < 2; i++)
        {
            if (selectCardController[i] != null)
            {
                cardID[i] = selectCardController[i].model.cardID;
                cost[i] = selectCardController[i].model.cost;
                specialID[i] = selectCardController[i].model.specialID;
            }

        }

        GenerateFieldCard(cardID, cost, specialID);
    }



    //受ける側で計算
    void DamageCalculation()
    {

        CardController attackCardController = mixFieldTransform[2].GetChild(0).GetComponent<CardController>();



        int a = UnityEngine.Random.Range(0, 100);

        damageCal = attackCardController.model.cal;


        photonView.RPC(nameof(Battle), RpcTarget.AllViaServer, damageCal);




    }

    [PunRPC]
    void Battle(int damageCal)
    {
        GamePlayerManager attacker;
        GamePlayerManager defender;

        if (isMyTurn)
        {
            attacker = playerManager[0];
            defender = playerManager[1];
        }
        else
        {
            attacker = playerManager[1];
            defender = playerManager[0];
        }


        CardController attackCardController = mixFieldTransform[2].GetChild(0).GetComponent<CardController>();



        defender.hp -= damageCal;


        

        if (attackCardController.model.kind == KIND.INGREDIENT)
        {
            //attacker.cost += currentFieldTransform[selectCardPosition[0]].GetComponent<FieldController>().cost;
            specialController.IngredientEffect(attackCardController.model.specialID, isMyTurn);
        }
        else
        {
            attacker.cost -= mixCost;
            int strength = 0;
            if (attacker.nutrient == mixCardController[2].model.nutrient[0] || attacker.nutrient == mixCardController[2].model.nutrient[1])
            {
                strength++;
            }
            if (attacker.dish == mixCardController[2].model.dish)
            {
                strength++;
            }
                specialController.DishEffect(attackCardController.model.specialID, strength, isMyTurn);
        }


        uiManager.ShowHP();

        ChangeTurn();
    }


    //次のターンへ移る
    [PunRPC]
    public void ChangeTurn()
    {
        CleanField();



        CheckPoison();
        CheckDark();


        //CheckHealth();
        //PoisonDamage();

        //uiManager.ShowHealth(player.isHealth, enemy.isHealth);
        //uiManager.ShowPoison(player.isPoison, player.poisonCount, enemy.isPoison, enemy.poisonCount);
        //uiManager.ShowDark(player.isDark, enemy.isDark);
        //uiManager.ShowAttackUp(player.attackUp, enemy.attackUp);
        //uiManager.ShowHitUp(player.hitUp, enemy.hitUp);





        isMyTurn = !isMyTurn;




        uiManager.ShowStatus();

        GiveCardToHand();


        //uiManager.ShowHP();
        uiManager.decideButtonObj.SetActive(false);


        uiManager.attackFields.SetActive(isMyTurn);
        

        
    }


    [PunRPC]
    public void GenerateFieldCard(int[] cardID, int[] cost, int[] specialID)
    {
        uiManager.decideButtonObj.SetActive(false);

        GamePlayerManager attacker;

        if (isMyTurn)
        {
            attacker = playerManager[0];
        }
        else
        {
            attacker = playerManager[1];
        }

        uiManager.attackFields.SetActive(true);


        CleanField();


        for (int i = 0; i < 3; i++)
        {
            mixCardController[i] = null;
        }

        for (int i = 0; i < 2; i++)
        {
            if (cardID[i] != -1)
            {
                
                mixCardController[i] = CreateCard(cardID[i], cost[i], specialID[i], mixFieldTransform[i]);

            }
        }

     


        if (cardID[0] == -1)
        {
            uiManager.decideButtonObj.SetActive(false);

            return;
        }


        //合成チェック
        if (cardID[1] != -1)
        {
            if (mixCardController[0].model.nutrient == mixCardController[1].model.nutrient)
            {
                //合成不可
                uiManager.decideButtonObj.SetActive(false);
                return;
            }
            else
            {
                
                //合成
                int mixCardID = SpecialMix(mixCardController[0], mixCardController[1]);

                mixCardController[2] = CreateMixCard(KIND.DISH, mixCardID, 0, mixCardID);


                if (attacker.cost >= mixCost)
                {
                    uiManager.decideButtonObj.SetActive(true);
                }

            }
        }
        else
        {
            uiManager.decideButtonObj.SetActive(true);
            mixCardController[2] = CreateMixCard(KIND.INGREDIENT, cardID[0], cost[0], specialID[0]);


        }



    }


   

    int SpecialMix(CardController card_0, CardController card_1)
    {
        int specialMixID = -1;

        for (int i = 0; i < card_0.model.partnerID.Length; i++)
        {
            if (card_0.model.partnerID[i] == card_1.model.cardID)
            {
                specialMixID = card_0.model.specialMixID[i];
                break;

            }
        }

        return specialMixID;
    }





    void CleanField()
    {


        foreach (Transform field in mixFieldTransform)
        {
            if (field.childCount != 0)
            {
                Destroy(field.GetChild(0).gameObject);
            }
        }



    }






    IEnumerator CountDown()
    {
        timeCount = 20;
        uiManager.UpdateTime(timeCount);

        while (timeCount > 0)
        {
            yield return new WaitForSeconds(1);
            timeCount--;
            uiManager.UpdateTime(timeCount);
        }

        //ChangeTurn();
        photonView.RPC(nameof(ChangeTurn), RpcTarget.AllViaServer);

    }








    void ShowResultPanel(int heroHp)
    {
        StopAllCoroutines();
        uiManager.ShowResultPanel(heroHp);


    }


    


    [PunRPC]
    void MixCard(int kind_1, int cardID_1, int kind_2, int cardID_2, int ID)
    {
        uiManager.attackFields.SetActive(false);
        uiManager.defenceFields.SetActive(true);
        StartCoroutine(mixController.MixCard(kind_1, cardID_1, kind_2, cardID_2, ID));
    }


   
    void ShuffleCard()
    {
        for (int i = deck.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            var tmp = deck[i];
            deck[i] = deck[j];
            deck[j] = tmp;
        }


    }







    void CheckPoison()
    {
        //自分
        //if (isMyTurn)
        //{
        //    if (player.poisonCount > 0)
        //    {
        //        player.poisonCount--;
        //    }

        //    if (player.poisonCount > 0)
        //    {
        //        int poisonDamage = player.poisonCount * 5;
        //        player.hp -= poisonDamage;
        //        Debug.Log("毒で" + poisonDamage + "ダメージ");
        //    }

        //}
        ////相手
        //else
        //{
        //    if (enemy.poisonCount > 0)
        //    {
        //        enemy.poisonCount--;
        //    }

        //    if (enemy.poisonCount > 0)
        //    {
        //        int poisonDamage = enemy.poisonCount * 5;
        //        enemy.hp -= poisonDamage;
        //        Debug.Log("毒で" + poisonDamage + "ダメージ");
        //    }
        //}




    }

    void CheckDark()
    {
        //自分
        //if (isMyTurn)
        //{
        //    if (player.darkCount > 0)
        //    {
        //        player.darkCount--;
        //    }

        //    if (player.darkCount > 0)
        //    {
        //        Debug.Log("暗闇");
        //    }

        //}
        ////相手
        //else
        //{
        //    if (enemy.darkCount > 0)
        //    {
        //        enemy.darkCount--;
        //    }

        //    if (enemy.darkCount > 0)
        //    {
        //        Debug.Log("暗闇");
        //    }
        //}


    }
}
