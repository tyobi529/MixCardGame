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
    int cardCost = 1;

    [SerializeField] UIManager uiManager;




    Transform handTransform;


    Transform[] mixFieldTransform = new Transform[3];


    public bool isMyTurn;





    //時間管理
    int timeCount;

    int playerID;




    SpecialController specialController;



    GameObject attackButtons;


    [SerializeField] int maxHand;
    int handCount;


    int[] playerHp = new int[2];

    int defaultMaxHand;


    int cardIndex = 0;

    int damageCal;

    CardController[] selectCardController = new CardController[2] { null, null };


    CardController[] mixCardController = new CardController[3] { null, null, null };

    //合成に必要なコスト
    [SerializeField] int mixCost;
    //貯まる最大コスト
    [SerializeField] int maxCost;


    int turnCount = 0;


    public int additionalTurn = 0;


    //0：効果なし
    //1：色効果
    //2：種類効果
    int effectCount = 0;


    int drawNum = 4;

    int strength = 0;


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
        //IDを１つ減らして扱う
        playerID--;
        

        defaultMaxHand = maxHand;


        handCount = 0;

        for (int i = 0; i < 2; i++)
        {
            playerHp[i] = playerManager[i].hp;
        }
    }

    void Start()
    {


        if (playerID == 1)
        {
            photonView.RPC(nameof(StartGame), RpcTarget.AllViaServer);



        }


    }



    [PunRPC]
    void StartGame()
    {



        //プレイヤー１が先行
        if (playerID == 0)
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

        drawNum = 0;


        uiManager.ShowStatus();


        uiManager.decideButtonObj.SetActive(false);

        //if (isMyTurn)
        //{
        //    if (playerManager[0].cost < mixCost)
        //    {
        //        playerManager[0].cost++;
        //    }
        //}
        //else
        //{
        //    if (playerManager[1].cost < mixCost)
        //    {
        //        playerManager[1].cost++;
        //    }
        //}


        turnCount++;
        Debug.Log("ターン " + turnCount);
    }


    void GiveCardToHand()
    {
        //int drawNum = maxHand - handCount;
        //handCount = maxHand;

        for (int i = 0; i < drawNum; i++)
        {
            int cardID = deck[cardIndex];
            //int cal = UnityEngine.Random.Range(20, 60);
            int specialID = -1;
            switch (effectCount)
            {
                case 0:
                    specialID = UnityEngine.Random.Range(0, 3);
                    break;
                case 1:
                    specialID = 6;
                    break;
                case 2:
                    specialID = UnityEngine.Random.Range(3, 6);
                    break;
                case 3:
                    specialID = 6;
                    break;
            }
            effectCount++;
            if (effectCount > 3)
            {
                effectCount = 0;
            }
            int cost = cardCost;

            cardCost++;
            if (cardCost > 3)
            {
                cardCost = 1;
            }

            CardController cardController = CreateCard(cardID, specialID, cost, handTransform);
            cardController.view.SetCard(cardController.model);

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

        //LineUpCard(handTransform);


    }


    CardController CreateCard(int cardID, int specialID, int cost, Transform position)
    {

        GameObject card = Instantiate(cardPrefab, position, false);


        card.GetComponent<CardController>().Init(KIND.INGREDIENT, cardID, specialID, cost);

        return card.GetComponent<CardController>();

    }

    CardController CreateEatCard(KIND kind, int cardID, int specialID, int cost)
    {
        GameObject card = Instantiate(dishCardPrefab, mixFieldTransform[2], false);

        card.GetComponent<CardController>().EatInit(kind, cardID, specialID, cost);

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
            int[] cal = new int[2] { -1, -1 };

            bool[] rare = new bool[2] { false, false };

            for (int i = 0; i < 2; i++)
            {
                if (selectCardController[i] != null)
                {
                    cardID[i] = selectCardController[i].model.cardID;
                    cost[i] = selectCardController[i].model.cost;
                    specialID[i] = selectCardController[i].model.specialID;
                    cal[i] = selectCardController[i].model.cal;

                    rare[i] = selectCardController[i].model.rare;
                    
                }

            }


            photonView.RPC(nameof(GenerateFieldCard), RpcTarget.AllViaServer, cardID, specialID, cal, cost, rare);

            ////使ったカードの削除
            for (int i = 0; i < 2; i++)
            {
                if (selectCardController[i] != null)
                {
                    Destroy(selectCardController[i].gameObject);
                    handCount--;
                }
                
            }

            


        }
        else
        {
            photonView.RPC(nameof(ChangeTurn), RpcTarget.AllViaServer);
            //ChangeTurn();
            //DamageCalculation();

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
                selectCardController[1].view.SelectView(false);
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
        int[] cal = new int[2] { -1, -1 };

        bool[] rare = new bool[2] { false, false };

        for (int i = 0; i < 2; i++)
        {
            if (selectCardController[i] != null)
            {
                cardID[i] = selectCardController[i].model.cardID;
                cost[i] = selectCardController[i].model.cost;
                specialID[i] = selectCardController[i].model.specialID;
                cal[i] = selectCardController[i].model.cal;

                rare[i] = selectCardController[i].model.rare;
            }

        }

        GenerateFieldCard(cardID, specialID, cal, cost, rare);
    }



    //受ける側で計算
    void DamageCalculation()
    {

        CardController attackCardController = mixFieldTransform[2].GetChild(0).GetComponent<CardController>();


        damageCal = attackCardController.model.cal;

        
        if (playerManager[1].paralysisCount > 0)
        {
            int a = UnityEngine.Random.Range(0, 100);

            if (a < 30)
            {
                Debug.Log("麻痺で動けない");
                damageCal = 0;
            }
        }

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


        //CardController attackCardController = mixCardController[2];


        if (attacker.nextAttack > 0)
        {
            damageCal *= (attacker.nextAttack + 1);
            attacker.nextAttack = 0;
        }

        defender.hp -= damageCal;


        

        if (mixCardController[2].model.kind == KIND.INGREDIENT)
        {
            //specialController.IngredientEffect(mixCardController[2].model.specialID, isMyTurn);
            attacker.cost += mixCardController[2].model.cost;
            if (attacker.cost > maxCost)
            {
                attacker.cost = maxCost;
            }

            //１枚使ったら１枚引く
            if (isMyTurn)
            {
                drawNum = 1;

            }
        }
        else
        {
            //2枚使ったら2枚引く
            if (isMyTurn)
            {
                drawNum = 2;
            }

            attacker.cost -= mixCost;
            //int strength = 0;

            //for (int i = 0; i < 2; i++ )
            //{
            //    if (attacker.dish[i] == DISH.NONE)
            //    {
            //        break;
            //    }
            //    else
            //    {
            //        for (int j = 0; j < 3; j++)
            //        {
            //            if (attacker.dish[i] == mixCardController[2].model.dish[j]) 
            //            {
            //                strength++;
            //                break;
            //            }
            //        }

            //    }
            //}

            //Debug.Log(strength);

            //for (int i = 0; i < 2; i++)
            //{
            //    attacker.dish[i] = DISH.NONE;
            //}

            //テスト用
            //specialController.DishEffect(19, 2, isMyTurn, damageCal);


            attacker.isMixed = true;
            //specialController.DishEffect(23, strength, isMyTurn, damageCal);

            specialController.DishEffect(mixCardController[2].model.specialID, strength, isMyTurn, damageCal);



        }




        //ChangeTurn();

        if (isMyTurn)
        {
            uiManager.decideButtonObj.SetActive(false);
        }
        else
        {
            uiManager.decideButtonObj.SetActive(true);
        }
        
    }


    //次のターンへ移る
    [PunRPC]
    public void ChangeTurn()
    {

        

        CleanField();



        CheckPoison();
        CheckDark();
        CheckParalysis();
        CheckHealth();


        //CheckHealth();
        //PoisonDamage();

        //uiManager.ShowHealth(player.isHealth, enemy.isHealth);
        //uiManager.ShowPoison(player.isPoison, player.poisonCount, enemy.isPoison, enemy.poisonCount);
        //uiManager.ShowDark(player.isDark, enemy.isDark);
        //uiManager.ShowAttackUp(player.attackUp, enemy.attackUp);
        //uiManager.ShowHitUp(player.hitUp, enemy.hitUp);

        //カードドロー
        //int drawNum = maxHand - handTransform.childCount;
        //Debug.Log("draw" + drawNum);
        //GiveCardToHand(drawNum);
        GiveCardToHand();


        drawNum = 0;

        if (additionalTurn > 0)
        {
            additionalTurn--;
        }
        else
        {
            isMyTurn = !isMyTurn;
        }


        //ターン開始時コスト貯まる
        //if (isMyTurn)
        //{
        //    if (playerManager[0].cost < maxCost)
        //    {
        //        playerManager[0].cost++;
        //    }

        //    playerManager[0].isMixed = false;

        //}
        //else
        //{
        //    if (playerManager[1].cost < maxCost)
        //    {
        //        playerManager[1].cost++;
        //    }

        //    playerManager[1].isMixed = false;
        //}


        for (int i = 0; i < 2; i++)
        {
            if (playerManager[i].cost < 0)
            {
                playerManager[i].cost = 0;
            }
            else if (playerManager[i].cost > maxCost)
            {
                playerManager[i].cost = maxCost;
            }
        }

        uiManager.ShowHP();
        uiManager.ShowStatus();





        //uiManager.ShowHP();
        uiManager.decideButtonObj.SetActive(false);


        uiManager.attackFields.SetActive(isMyTurn);

        for (int i = 0; i < 2; i++)
        {
            int damage = playerHp[i] - playerManager[i].hp;
            Debug.Log("プレイヤー" + i + "のダメージ：　" + damage);
            playerHp[i] = playerManager[i].hp;
        }

        Debug.Log("*************************************************************");

        turnCount++;
        Debug.Log("ターン " + turnCount);
    }


    [PunRPC]
    public void GenerateFieldCard(int[] cardID, int[] specialID, int[] cal, int[] cost, bool[] rare)
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
                
                mixCardController[i] = CreateCard(cardID[i], specialID[i], cost[i], mixFieldTransform[i]);
                mixCardController[i].model.cal = cal[i];
                mixCardController[i].view.SetCard(mixCardController[i].model);
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
            if (mixCardController[0].model.dish[0] == mixCardController[1].model.dish[0])
            {
                //合成不可
                uiManager.decideButtonObj.SetActive(false);
                return;
            }
            else
            {
                
                //合成
                int mixCardID = SpecialMix(mixCardController[0], mixCardController[1]);

                mixCardController[2] = CreateEatCard(KIND.DISH, mixCardID, mixCardID, 0);

                //素材の栄養素を代入
                mixCardController[2].model.dish[1] = mixCardController[0].model.dish[0];
                mixCardController[2].model.dish[2] = mixCardController[1].model.dish[0];

                mixCardController[2].view.SetEatCard(mixCardController[2].model);

                //for (int i = 0; i < 3; i++)
                //{
                //    Debug.Log(mixCardController[2].model.dish[i]);
                //}
                


                //コストがあり、暗闇でない時
                if (attacker.cost >= mixCost && attacker.darkCount == 0)
                {
                    uiManager.decideButtonObj.SetActive(true);
                }

            }
        }
        else
        {
            uiManager.decideButtonObj.SetActive(true);
            mixCardController[2] = CreateEatCard(KIND.INGREDIENT, cardID[0], specialID[0], cost[0]);

            mixCardController[2].model.cal = mixCardController[0].model.cal;
            mixCardController[2].model.cost = mixCardController[0].model.cost;
            mixCardController[2].view.SetEatCard(mixCardController[2].model);


            //テスト用
            //mixCardController[2].model.kind = KIND.DISH;
            //mixCardController[2].model.specialID = 0;
        }


        if (!isMyTurn)
        {
            DamageCalculation();
        }

        strength = 0;
        for (int i = 0; i < 2; i++)
        {
            if (rare[i])
            {
                strength++;
            }
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
        if (isMyTurn)
        {
            if (playerManager[0].poisonCount > 0)
            {
                playerManager[0].poisonCount--;
                playerManager[0].hp -= 50;
                Debug.Log("毒で自分" + 50 + "ダメージ");
            }
        }
        //相手
        else
        {
            if (playerManager[1].poisonCount > 0)
            {
                playerManager[1].poisonCount--;
                playerManager[1].hp -= 50;
                Debug.Log("毒で相手" + 50 + "ダメージ");
            }
        }
    }

    void CheckDark()
    {
        //自分
        if (isMyTurn)
        {
            if (playerManager[0].darkCount > 0)
            {
                playerManager[0].darkCount--;
            }
        }
        //相手
        else
        {
            if (playerManager[1].darkCount > 0)
            {
                playerManager[1].darkCount--;
            }
        }
    }

    void CheckParalysis()
    {
        //自分
        if (isMyTurn)
        {
            if (playerManager[0].paralysisCount > 0)
            {
                playerManager[0].paralysisCount--;
            }
        }
        //相手
        else
        {
            if (playerManager[1].paralysisCount > 0)
            {
                playerManager[1].paralysisCount--;
            }
        }
    }

    void CheckHealth()
    {
        //自分
        if (isMyTurn)
        {
            if (playerManager[0].healthCount > 0)
            {
                playerManager[0].healthCount--;
            }
        }
        //相手
        else
        {
            if (playerManager[1].healthCount > 0)
            {
                playerManager[1].healthCount--;
            }
        }
    }


    //カードを番号順に並べる
    void LineUpCard(Transform field)
    {
        List<GameObject> objList = new List<GameObject>();

        // 子階層のGameObject取得
        var childCount = field.childCount;
        for (int i = 0; i < childCount; i++)
        {
            objList.Add(field.GetChild(i).gameObject);
        }

        //objList.Sort((obj1, obj2) => int.Compare(obj1.model.cardID, obj2.model.cardID));
        objList.Sort((a, b) => a.GetComponent<CardController>().model.cardID - b.GetComponent<CardController>().model.cardID);

        foreach (var obj in objList)
        {
            //obj.SetSiblingIndex(childCount - 1);
            obj.transform.SetSiblingIndex(childCount - 1);

        }


    }


    public void ExchangeHandCard(int num)
    {
        int handCount = handTransform.childCount;

        Debug.Log("手札" + handCount);


        for (int i = 0; i < num; i++)
        {
            Destroy(handTransform.GetChild(i).gameObject);
            drawNum++;
            Debug.Log("ドロー" + drawNum);
            handCount--;
            if (handCount == 0)
            {
                break;
            }
        }

        //for (int i = 0; i < num; i++)
        //{
        //    Debug.Log(i);
        //    //if (handTransform.childCount == 0)
        //    //{
        //    //    break;
        //    //}
        //    int index = UnityEngine.Random.Range(0, handCount);
        //    Debug.Log("index" + index);
        //    Destroy(handTransform.GetChild(index).gameObject);
        //    drawNum++;
        //    handCount--;
        //    if (handCount == 0)
        //    {
        //        break;
        //    }
        //}

        //List<int> cardIndex = new List<int>();


        //for (int i = 0; i < handCount; i++)
        //{
        //    cardIndex.Add(i);
        //}

        //for (int i = 0; i < num; i++)
        //{
        //    int index = UnityEngine.Random.Range(0, cardIndex.Count);
        //    Destroy(handTransform.GetChild(cardIndex[index]).gameObject);
        //    drawNum++;
        //    cardIndex.Remove(index);
        //    handCount--;
        //    if (handCount == 0)
        //    {
        //        break;
        //    }
        //}

    }

    //ID=5
    public int IngredientCal()
    {
        int additionalCal = 0;
        for (int i = 0; i < 2; i++)
        {
            if (mixCardController[i] != null)
            {
                additionalCal += mixCardController[i].model.cal;
            }
        }
        return additionalCal;
    }

    //ID=12
    public void HealHp(int heal)
    {
        photonView.RPC(nameof(HealHp_RPC), RpcTarget.AllViaServer, heal);
    }
    [PunRPC]
    public void HealHp_RPC(int heal)
    {
        if (isMyTurn)
        {
            playerManager[0].hp += heal;
            Debug.Log("自分" + heal + "回復");
        }
        else
        {
            playerManager[1].hp += heal;
            Debug.Log("相手" + heal + "回復");
        }
    }


    //ID=22
    public void AdditionalDamage(GamePlayerManager damagePlayer, int damage)
    {
        int damagePlayerID = -1;

        //自分にダメージ
        if (playerManager[0] == damagePlayer)
        {
            damagePlayerID = this.playerID;
        }
        //相手にダメージ
        else
        {
            if (playerID == 0)
            {
                damagePlayerID = 1;
            }
            else
            {
                damagePlayerID = 0;
            }
        }

        photonView.RPC(nameof(AdditionalDamage_RPC), RpcTarget.AllViaServer, damagePlayerID, damage);

    }

    [PunRPC]
    public void AdditionalDamage_RPC(int damagePlayerID, int damage)
    {
        if (playerID == damagePlayerID)
        {
            playerManager[0].hp -= damage;
            Debug.Log("自分に追加ダメージ" + damage);
        }
        else
        {
            playerManager[1].hp -= damage;
            Debug.Log("相手に追加ダメージ" + damage);
        }      
    }



    //ID=22
    //public void ExchangeHp()
    //{
    //    photonView.RPC(nameof(ExchangeHp_RPC), RpcTarget.AllViaServer);
    //}

    //[PunRPC]
    //public void ExchangeHp_RPC()
    //{
    //    int tmp = playerManager[0].hp;
    //    playerManager[0].hp = playerManager[1].hp;
    //    playerManager[1].hp = tmp;
    //}


    //ID=23
    public void RandomEffect(int specialID, int strength, int damageCal)
    {
        photonView.RPC(nameof(RandomEffect_RPC), RpcTarget.AllViaServer, specialID, strength, damageCal);
    }

    [PunRPC]
    public void RandomEffect_RPC(int specialID, int strength, int damageCal)
    {
        specialController.DishEffect(specialID, strength, isMyTurn, damageCal);
    }


    //相手にレア素材を渡す
    //渡す側から
    public void StealRareIngredient(int num)
    {
        handCount = maxHand;
        int[] cardID = new int[num];
        for (int i = 0; i < num; i++)
        {
            cardID[i] = -1;
        }

        int index = 0;

        for (int i = 0; i < handCount; i++)
        {
            CardController cardController = handTransform.GetChild(i).GetComponent<CardController>();
            if (cardController.model.rare)
            {
                cardID[index] = cardController.model.cardID;
                Destroy(cardController.gameObject);
                drawNum++;
                index++;
                if (index > num)
                {
                    break;
                }
            }
        }

        photonView.RPC(nameof(StealRareIngredient_RPC), RpcTarget.Others, cardID);
    }

    //受け取る側だけで呼ぶ
    [PunRPC]
    public void StealRareIngredient_RPC(int[] cardID)
    {
        //Debug.Log(cardID[0]);
        //Debug.Log(cardID[1]);
        for (int i = 0; i < cardID.Length; i++)
        {
            if (cardID[i] == -1)
            {
                return;
            }

            if (drawNum > 0)
            {
                CardController cardController = CreateCard(cardID[i], 0, 3, handTransform);
                cardController.view.SetCard(cardController.model);
                drawNum--;
            }

        }
    }
}
