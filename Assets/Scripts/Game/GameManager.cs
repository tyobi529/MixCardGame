using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;


public class GameManager : MonoBehaviourPunCallbacks
{



    GamePlayerManager[] player = new GamePlayerManager[2];

   
    int[] deck = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
    int cardCost = 1;

    [SerializeField] UIManager uiManager;




    Transform handTransform;


    Transform[] mixFieldTransform = new Transform[3];


    
    Transform cookFieldTransform;

    Transform eatFieldTransform;

    public bool isMyTurn;





    //時間管理
    [SerializeField] int timeLimit;

    int playerID;




    SpecialController specialController;

    
    MessageController messageController;

    SelectController selectController;

    CardGenerator cardGenerator;

    GameObject attackButtons;


    [SerializeField] int maxHand;
    int handCount;


    int[] playerHp = new int[2];

    int defaultMaxHand;


    int cardIndex = 0;

    int damageCal;

    //CardController[] selectCardController = new CardController[2] { null, null };


    CardController[] mixCardController = new CardController[2] { null, null };
    CardController eatCardController = null;

    //EatCardController eatCardController = null;


    //EatCardController[] eatCardController = new CardController[3] { null, null, null };

    //合成に必要なコスト
    [SerializeField] int mixCost;
    //貯まる最大コスト
    [SerializeField] int maxCost;


    public int turnCount = 0;


    public int additionalTurn = 0;


    //0：効果なし
    //1：色効果
    //2：種類効果
    int effectCount = 0;


    int drawNum = 4;

    int strength = 0;

    public bool rareDraw;


    //シングルトン化（どこからでもアクセスできるようにする）
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }


        player[0] = GameObject.Find("Player").GetComponent<GamePlayerManager>();
        player[1] = GameObject.Find("Enemy").GetComponent<GamePlayerManager>();

        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();


        handTransform = GameObject.Find("Hand").transform;


        for (int i = 0; i < 3; i++)
        {
            mixFieldTransform[i] = GameObject.Find("MixField_" + i.ToString()).transform;
        }


        //cookingObject = GameObject.Find("Cooking");
        cookFieldTransform = GameObject.Find("CookField").transform;
        uiManager.cookingObject.SetActive(false);


        eatFieldTransform = GameObject.Find("EatField").transform;


        //messagePanel = GameObject.Find("MessagePanel");
        //uiManager.messagePanel.SetActive(false);

        specialController = GameObject.Find("SpecialController").GetComponent<SpecialController>();

        selectController = GameObject.Find("SelectController").GetComponent<SelectController>();

        messageController = GameObject.Find("MessageController").GetComponent<MessageController>();

        cardGenerator = GameObject.Find("CardGenerator").GetComponent<CardGenerator>();

        attackButtons = GameObject.Find("AttackButtons");

        playerID = PhotonNetwork.LocalPlayer.ActorNumber;
        //IDを１つ減らして扱う
        playerID--;
        

        defaultMaxHand = maxHand;


        handCount = 0;

        for (int i = 0; i < 2; i++)
        {
            playerHp[i] = player[i].hp;
        }

        rareDraw = false;
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

        //uiManager.attackFields.SetActive(isMyTurn);
        uiManager.attackFields.SetActive(true);



        uiManager.ShowHP();
        //uiManager.ShowCost(isMyTurn);
        //uiManager.ShowCost(!isMyTurn);
        StartCoroutine(uiManager.ShowCost(isMyTurn));
        StartCoroutine(uiManager.ShowCost(!isMyTurn));



        ShuffleCard();

        GiveCardToHand();

        drawNum = 0;


        uiManager.ShowStatus();



        turnCount++;
        Debug.Log("ターン " + turnCount);

        uiManager.UpdateTime(timeLimit);

        if (isMyTurn)
        {
            //StartCoroutine(CountDown());
        }
    }


    void GiveCardToHand()
    {
        //int drawNum = maxHand - handCount;
        //handCount = maxHand;

        for (int i = 0; i < drawNum; i++)
        {
            int cardID = deck[cardIndex];
            int cost = cardCost;

            cardCost++;
            if (cardCost > 3)
            {
                cardCost = 1;
            }
            bool isRare;
            if (cost == 3 || rareDraw)
            {
                isRare = true;
                rareDraw = false;
            }
            else
            {
                isRare = false;
            }

            CardController cardController = cardGenerator.CreateCard(false, cardID, cost, isRare, handTransform);
            cardController.GetComponent<CardView>().SetCard(cardController.model);
            //cardController.view.SetCard(cardController.model);

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




    public void OnDecideButton()
    {


        if (isMyTurn)
        {
            StopAllCoroutines();

            uiManager.UpdateTime(timeLimit);


            selectController.CleanField();

            //生成
            int[] cardID = new int[2] { -1, -1 };
            int[] cost = new int[2] { -1, -1 };
            int[] cal = new int[2] { -1, -1 };

            bool[] isRare = new bool[2] { false, false };

            for (int i = 0; i < 2; i++)
            {
                if (selectController.selectCardController[i] != null)
                {
                    cardID[i] = selectController.selectCardController[i].model.cardID;
                    cost[i] = selectController.selectCardController[i].model.cost;
                    cal[i] = selectController.selectCardController[i].model.cal;

                    isRare[i] = selectController.selectCardController[i].model.isRare;
                    
                }

            }


            photonView.RPC(nameof(GenerateMixCard_RPC), RpcTarget.AllViaServer, cardID, cal, cost, isRare);




            ////使ったカードの削除
            for (int i = 0; i < 2; i++)
            {
                if (selectController.selectCardController[i] != null)
                {
                    Destroy(selectController.selectCardController[i].gameObject);
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

    //[PunRPC]
    //void Single_RPC(int cardID, int cost, bool isRare)
    //{
    //    eatCardController = cardGenerator.CreateEatCard(false, cardID, cost, isRare, eatFieldTransform);
    //    //eatCardController.eatView.SetEatCard(eatCardController.model);
    //    eatCardController.GetComponent<EatCardView>().SetEatCard(eatCardController.model);


    //    if (isMyTurn)
    //    {
    //        DamageCalculation();
    //    }
    //}



    //[PunRPC]
    //void Cooking_RPC(int[] cardID, int[] cal, int[] cost, bool[] isRare)
    //{
    //    selectController.canSelect = false;

    //    //使うカードを生成する。
    //    //GenerateMixCard(cardID, cal, cost, isRare);


    //    StartCoroutine(Cooking(cardID, cal, cost, isRare));
    //}

    //生成のみ
    [PunRPC]
    void GenerateMixCard_RPC(int[] cardID, int[] cal, int[] cost, bool[] isRare)
    {
        for (int i = 0; i < 2; i++)
        {
            mixCardController[i] = null;
        }

        int strength = 0;

        for (int i = 0; i < 2; i++)
        {
            if (cardID[i] != -1)
            {
                mixCardController[i] = cardGenerator.CreateCard(false, cardID[i], cost[i], isRare[i], cookFieldTransform);
                mixCardController[i].model.cal = cal[i];
                //mixCardController[i].view.SetCard(mixCardController[i].model);
                mixCardController[i].GetComponent<CardView>().SetCookingCard(mixCardController[i].model);

                if (mixCardController[i].model.isRare)
                {
                    strength++;
                }

                mixCardController[i].gameObject.SetActive(false);

            }
        }

        if (mixCardController[1] != null)
        {
            //合成
            int mixCardID = cardGenerator.SpecialMix(mixCardController[0], mixCardController[1]);


            eatCardController = cardGenerator.CreateCard(true, mixCardID, 0, false, cookFieldTransform);
            //eatCardController.view.SetEatCard(eatCardController.model);
            eatCardController.model.strength = strength;
            eatCardController.GetComponent<CardView>().SetCookingCard(eatCardController.model);
        }
        else
        {
            eatCardController = cardGenerator.CreateCard(false, cardID[0], cost[0], isRare[0], cookFieldTransform);
            eatCardController.model.cal = cal[0];
            //mixCardController[i].view.SetCard(mixCardController[i].model);
            eatCardController.GetComponent<CardView>().SetCookingCard(eatCardController.model);
        }



        //for (int i = 0; i < 2; i++)
        //{
        //}

        eatCardController.gameObject.SetActive(false);


        //食事場移動
        //eatCardController.transform.SetParent(eatFieldTransform);

        //messageController.EatMessage(eatCardController.model.cardID);

        //攻撃側でダメージ計算しておく
        //if (isMyTurn)
        //{
        //    DamageCalculation();
        //}

        if (eatCardController.model.kind == KIND.DISH)
        {
            StartCoroutine(Cooking());
        }
        else
        {
            if (isMyTurn)
            {
                //photonView.RPC(nameof(Battle_RPC), RpcTarget.AllViaServer, damageCal);
                DamageCalculation();
            }

        }


    }

    IEnumerator Cooking()
    {
        uiManager.cookingObject.SetActive(true);


        for (int i = 0; i < 2; i++)
        {
            mixCardController[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.7f);

        }


        for (int i = 0; i < 2; i++)
        {
            if (mixCardController[i] != null)
            {
                Destroy(mixCardController[i].gameObject);
            }
        }

        eatCardController.gameObject.SetActive(true);

        yield return new WaitForSeconds(1);


        uiManager.cookingObject.SetActive(false);
        //uiManager.messagePanel.SetActive(true);


        //食事場移動
        eatCardController.transform.SetParent(eatFieldTransform);


        //if (isMyTurn)
        //{
        //    DamageCalculation();
        //}

        if (isMyTurn)
        {
            //Debug.Log("ダメージ" + damageCal);
            //photonView.RPC(nameof(Battle_RPC), RpcTarget.AllViaServer, damageCal);
            DamageCalculation();
        }


    }


    //基礎カロリー、麻痺、ギャンブル効果の計算
    void DamageCalculation()
    {


        if (player[0].paralysisCount > 0)
        {
            int a = UnityEngine.Random.Range(0, 100);

            if (a < 30)
            {
                Debug.Log("麻痺で動けない");
                damageCal = 0;
            }
        }
        else
        {
            damageCal = eatCardController.model.cal;

            if (player[0].nextAttack > 0)
            {
                damageCal /= 2;
                damageCal *= player[0].nextAttack;
                player[0].nextAttack = 0;
            }

            if (player[1].defenceDish > 0)
            {
                damageCal /= (player[1].defenceDish + 1);
                player[1].defenceDish = 0;
            }
        }

        ////テスト
        //eatCardController.model.cardID = 1;
        //eatCardController.model.strength = 0;

        //ラーメン
        if (eatCardController.model.kind == KIND.DISH && eatCardController.model.cardID == 1)
        {
            Debug.Log("ラーメン");

            int a = UnityEngine.Random.Range(0, 100);

            switch (eatCardController.model.strength)
            {
                case 0:
                    if (a < 40)
                    {
                        Debug.Log("0 ダメージが１になる");
                        damageCal = 1;
                    }
                    break;
                case 1:
                    if (a < 30)
                    {
                        Debug.Log("1 ダメージが１になる");
                        damageCal = 1;
                    }
                    break;
                case 2:
                    if (a < 10)
                    {
                        Debug.Log("2 ダメージが１になる");
                        damageCal = 1;
                    }
                    break;
            }

        }

        photonView.RPC(nameof(Battle_RPC), RpcTarget.AllViaServer, damageCal);


        //if (eatCardController.model.kind == KIND.DISH)
        //{
        //    switch (eatCardController.model.cardID)
        //    {
        //        case 9:
        //            break;
        //    }
        //}
        //else
        //{
        //    damageCal = cardModel.cal;
        //}

        //messageController.EatMessage(eatCardController.model.cardID);


        //damageCal = eatCardController.model.cal;




        //photonView.RPC(nameof(Battle_RPC), RpcTarget.AllViaServer, damageCal);




    }



    [PunRPC]
    void Battle_RPC(int damageCal)
    {
        StartCoroutine(Battle(damageCal));
    }


    IEnumerator Battle(int damageCal)
    {


        GamePlayerManager attacker;
        GamePlayerManager defender;

        if (isMyTurn)
        {
            attacker = player[0];
            defender = player[1];
        }
        else
        {
            attacker = player[1];
            defender = player[0];
        }



        //食べたメッセージ
        messageController.messagePanel.SetActive(true);
        messageController.EatMessage(eatCardController.model, isMyTurn);
        


        //料理
        if (eatCardController.model.kind == KIND.DISH)
        {
            //2枚使ったら2枚引く
            if (isMyTurn)
            {
                drawNum = 2;
            }

            attacker.cost -= mixCost;

            StartCoroutine(uiManager.ShowCost(isMyTurn));

            yield return new WaitForSeconds(2);

            int cardID = eatCardController.model.cardID;
            int strength = eatCardController.model.strength;

            //テスト
            //cardID = 1;
            //strength = 0;

            //ランダム効果
            if (cardID == 0)
            {
                messageController.EffectMessage("ランダムで効果が発生する！");
                yield return new WaitForSeconds(2);

                if (isMyTurn)
                {
                    cardID = UnityEngine.Random.Range(1, 27);
                    RandomEffect(cardID, strength, damageCal);
                }
            }
            else
            {
                //yield return new WaitForSeconds(2);
                StartCoroutine(specialController.DishEffect(cardID, strength, damageCal, isMyTurn));
                //StartCoroutine(specialController.DishEffect(6, strength, damageCal, isMyTurn));

            }




        }
        //素材
        else
        {
            yield return new WaitForSeconds(2);

            //「あなたの攻撃」テキスト
            messageController.AttackText(isMyTurn);
            yield return new WaitForSeconds(1);


            defender.hp -= eatCardController.model.cal;
            uiManager.ShowDamageText(damageCal, isMyTurn);
            yield return new WaitForSeconds(2);



            attacker.cost += eatCardController.model.cost;
            if (attacker.cost > maxCost)
            {
                attacker.cost = maxCost;
            }

            //１枚使ったら１枚引く
            if (isMyTurn)
            {
                drawNum = 1;

            }

            messageController.messagePanel.SetActive(false);

            ChangeTurn();

        }




        //if (isMyTurn)
        //{
        //    uiManager.decideButtonObj.SetActive(false);
        //}
        //else
        //{
        //    uiManager.decideButtonObj.SetActive(true);
        //}
    }


   

   //次のターンへ移る
   [PunRPC]
    public void ChangeTurn()
    {
        selectController.canSelect = true;

        Destroy(eatCardController.gameObject);

        uiManager.cookingObject.SetActive(false);
        //uiManager.messagePanel.SetActive(false);

        //CleanField();



        CheckPoison();
        CheckDark();
        CheckParalysis();


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

        uiManager.ShowHP();
        StartCoroutine(uiManager.ShowCost(isMyTurn));
        uiManager.ShowStatus();

        if (additionalTurn > 0)
        {
            additionalTurn--;
        }
        else
        {
            isMyTurn = !isMyTurn;
        }




        for (int i = 0; i < 2; i++)
        {
            if (player[i].cost < 0)
            {
                player[i].cost = 0;
            }
            else if (player[i].cost > maxCost)
            {
                player[i].cost = maxCost;
            }
        }


        //Debug.Log(player[1].hp);

 






        for (int i = 0; i < 2; i++)
        {
            int damage = playerHp[i] - player[i].hp;
            Debug.Log("プレイヤー" + i + "のダメージ：　" + damage);
            playerHp[i] = player[i].hp;
        }

        Debug.Log("*************************************************************");

        turnCount++;
        Debug.Log("ターン " + turnCount);

        if (isMyTurn)
        {
            //StartCoroutine(CountDown());
        }
    }


    //public void DamageAction(int damageCal)
    //{
    //    if (isMyTurn)
    //    {
    //        Debug.Log("hp" + player[1]);
    //        player[1].hp -= damageCal;
    //        Debug.Log("hp" + player[1]);
    //        uiManager.ShowDamageText(damageCal, isMyTurn);
    //    }
    //    else
    //    {
    //        player[0].hp -= damageCal;
    //        uiManager.ShowDamageText(damageCal, isMyTurn);
    //    }
    //}


    IEnumerator CountDown()
    {
        //timeLimit = 10;
        //uiManager.UpdateTime(timeLimit);

        int count = timeLimit;

        while (count > 0)
        {
            yield return new WaitForSeconds(1);
            count--;
            uiManager.UpdateTime(count);
        }

        yield return new WaitForSeconds(1);

        //uiManager.UpdateTime(timeLimit);

        handTransform.GetChild(0).gameObject.GetComponent<CardController>().OnCardObject();
        OnDecideButton();

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
            if (player[0].poisonCount > 0)
            {
                player[0].poisonCount--;
                player[0].hp -= 50;
                Debug.Log("毒で自分" + 50 + "ダメージ");
            }
        }
        //相手
        else
        {
            if (player[1].poisonCount > 0)
            {
                player[1].poisonCount--;
                player[1].hp -= 50;
                Debug.Log("毒で相手" + 50 + "ダメージ");
            }
        }
    }

    void CheckDark()
    {
        //自分
        if (isMyTurn)
        {
            if (player[0].darkCount > 0)
            {
                player[0].darkCount--;
            }
        }
        //相手
        else
        {
            if (player[1].darkCount > 0)
            {
                player[1].darkCount--;
            }
        }
    }

    void CheckParalysis()
    {
        //自分
        if (isMyTurn)
        {
            if (player[0].paralysisCount > 0)
            {
                player[0].paralysisCount--;
            }
        }
        //相手
        else
        {
            if (player[1].paralysisCount > 0)
            {
                player[1].paralysisCount--;
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
            player[0].hp += heal;
            Debug.Log("自分" + heal + "回復");
        }
        else
        {
            player[1].hp += heal;
            Debug.Log("相手" + heal + "回復");
        }
    }


    //ID=22
    public void AdditionalDamage(GamePlayerManager damagePlayer, int damage)
    {
        int damagePlayerID = -1;

        //自分にダメージ
        if (player[0] == damagePlayer)
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
            player[0].hp -= damage;
            Debug.Log("自分に追加ダメージ" + damage);

            uiManager.ShowDamageText(damage, false);
            //messageController.EffectMessage("大ダメージを受けてしまった！");
        }
        else
        {
            player[1].hp -= damage;
            Debug.Log("相手に追加ダメージ" + damage);

            uiManager.ShowDamageText(damage, true);
            //messageController.EffectMessage("相手に大ダメージを与えた！");
        }      
    }



    //ランダム
    public void RandomEffect(int cardID, int strength, int damageCal)
    {
        photonView.RPC(nameof(RandomEffect_RPC), RpcTarget.AllViaServer, cardID, strength, damageCal);
    }

    [PunRPC]
    public void RandomEffect_RPC(int cardID, int strength, int damageCal)
    {
        StartCoroutine(specialController.DishEffect(cardID, strength, damageCal, isMyTurn));
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
            if (cardController.model.isRare)
            {
                cardID[index] = cardController.model.cardID;
                Destroy(cardController.gameObject);
                drawNum++;
                index++;
                if (index >= num)
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
                CardController cardController = cardGenerator.CreateCard(false, cardID[i], 3, true, handTransform);
                //cardController.view.SetCard(cardController.model);
                cardController.GetComponent<CardView>().SetCard(cardController.model);
                drawNum--;
            }

        }
    }
}
