using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;


public class GameManager : MonoBehaviourPunCallbacks
{



    GamePlayerManager[] playerManager = new GamePlayerManager[2];

   
    int[] deck = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
    int cardCost = 1;

    [SerializeField] UIManager uiManager;




    Transform handTransform;


    Transform[] mixFieldTransform = new Transform[3];


    GameObject cookingObject;
    Transform cookFieldTransform;

    Transform eatFieldTransform;

    public bool isMyTurn;





    //時間管理
    int timeCount;

    int playerID;




    SpecialController specialController;

    GameObject messagePanel;
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

    EatCardController eatCardController = null;

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


        playerManager[0] = GameObject.Find("Player").GetComponent<GamePlayerManager>();
        playerManager[1] = GameObject.Find("Enemy").GetComponent<GamePlayerManager>();

        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();


        handTransform = GameObject.Find("Hand").transform;


        for (int i = 0; i < 3; i++)
        {
            mixFieldTransform[i] = GameObject.Find("MixField_" + i.ToString()).transform;
        }


        cookingObject = GameObject.Find("Cooking");
        cookFieldTransform = GameObject.Find("CookField").transform;
        cookingObject.SetActive(false);


        eatFieldTransform = GameObject.Find("EatField").transform;


        messagePanel = GameObject.Find("MessagePanel");
        messagePanel.SetActive(false);

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

        //uiManager.attackFields.SetActive(isMyTurn);
        uiManager.attackFields.SetActive(true);



        uiManager.ShowHP();
        uiManager.ShowCost();


        ShuffleCard();

        GiveCardToHand();

        drawNum = 0;


        uiManager.ShowStatus();


        //uiManager.decideButtonObj.SetActive(false);

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
            int cost = cardCost;

            cardCost++;
            if (cardCost > 3)
            {
                cardCost = 1;
            }
            bool isRare;
            if (cost == 3)
            {
                isRare = true;
            }
            else
            {
                isRare = false;
            }

            CardController cardController = cardGenerator.CreateCard(cardID, cost, isRare, handTransform);
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




    public void OnDecideButton()
    {


        if (isMyTurn)
        {
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

            if (cardID[1] == -1)
            {
                //photonView.RPC(nameof(Battle_RPC), RpcTarget.AllViaServer, cal[0]);

                //Single(cardID[0], cost[0], isRare[0]);
                photonView.RPC(nameof(Single_RPC), RpcTarget.AllViaServer, cardID[0], cost[0], isRare[0]);
                //DamageCalculation();
            }
            else
            {
                photonView.RPC(nameof(Cooking_RPC), RpcTarget.AllViaServer, cardID, cal, cost, isRare);
            }
            

            

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

    [PunRPC]
    void Single_RPC(int cardID, int cost, bool isRare)
    {
        eatCardController = cardGenerator.CreateEatCard(KIND.RED, cardID, cost, isRare, eatFieldTransform);
        eatCardController.view.SetEatCard(eatCardController.model);


        if (isMyTurn)
        {
            DamageCalculation();
        }
    }



    [PunRPC]
    void Cooking_RPC(int[] cardID, int[] cal, int[] cost, bool[] isRare)
    {
        selectController.canSelect = false;

        StartCoroutine(Cooking(cardID, cal, cost, isRare));
    }

    IEnumerator Cooking(int[] cardID, int[] cal, int[] cost, bool[] isRare)
    {
        cookingObject.SetActive(true);


        for (int i = 0; i < 2; i++)
        {
            mixCardController[i] = null;
        }

        for (int i = 0; i < 2; i++)
        {
            if (cardID[i] != -1)
            {
                mixCardController[i] = cardGenerator.CreateCard(cardID[i], cost[i], isRare[i], cookFieldTransform);
                mixCardController[i].model.cal = cal[i];                
                mixCardController[i].view.SetCard(mixCardController[i].model);
                

                yield return new WaitForSeconds(0.7f);
            }
        }


        //合成
        int mixCardID = cardGenerator.SpecialMix(mixCardController[0], mixCardController[1]);

        for (int i = 0; i < 2; i++)
        {
            if (cardID[i] != -1)
            {
                Destroy(mixCardController[i].gameObject);
            }
        }

        eatCardController = cardGenerator.CreateEatCard(KIND.DISH, mixCardID, 0, false, cookFieldTransform);
        eatCardController.view.SetEatCard(eatCardController.model);

        yield return new WaitForSeconds(1);
        

        cookingObject.SetActive(false);
        messagePanel.SetActive(true);


        //食事場移動
        eatCardController.transform.SetParent(eatFieldTransform);

        messageController.EatMessage(eatCardController.model.cardID);

        if (isMyTurn)
        {
            DamageCalculation();
        }
    }


    //与える側で計算
    void DamageCalculation()
    {

        damageCal = eatCardController.model.cal;

        
        if (playerManager[0].paralysisCount > 0)
        {
            int a = UnityEngine.Random.Range(0, 100);

            if (a < 30)
            {
                Debug.Log("麻痺で動けない");
                damageCal = 0;
            }
        }

        photonView.RPC(nameof(Battle_RPC), RpcTarget.AllViaServer, damageCal);




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
            attacker = playerManager[0];
            defender = playerManager[1];
        }
        else
        {
            attacker = playerManager[1];
            defender = playerManager[0];
        }



        if (attacker.nextAttack > 0)
        {
            damageCal *= (attacker.nextAttack + 1);
            attacker.nextAttack = 0;
        }

        defender.hp -= damageCal;

        yield return new WaitForSeconds(1);



        //料理
        if (eatCardController.model.kind == KIND.DISH)
        {
            //2枚使ったら2枚引く
            if (isMyTurn)
            {
                drawNum = 2;
            }

            attacker.cost -= mixCost;


            //テスト用
            //specialController.DishEffect(19, 2, isMyTurn, damageCal);


            //specialController.DishEffect(23, strength, isMyTurn, damageCal);

            specialController.DishEffect(eatCardController.model.cardID, strength, isMyTurn, damageCal);



        }
        else
        {
            //specialController.IngredientEffect(mixCardController[2].model.specialID, isMyTurn);
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
        }



        ChangeTurn();

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

        cookingObject.SetActive(false);
        messagePanel.SetActive(false);

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
        uiManager.ShowCost();
        uiManager.ShowStatus();





        //uiManager.ShowHP();
        //uiManager.decideButtonObj.SetActive(false);


        //uiManager.attackFields.SetActive(isMyTurn);

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
            if (cardController.model.isRare)
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
                CardController cardController = cardGenerator.CreateCard(cardID[i], 3, true, handTransform);
                cardController.view.SetCard(cardController.model);
                drawNum--;
            }

        }
    }
}
