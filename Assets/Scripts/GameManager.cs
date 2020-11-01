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


    public GamePlayerManager player;
    public GamePlayerManager enemy;

    //List<int> deck = new List<int>();
    //int[,] deck = new int[2, 9] { { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, { 0, 1, 2, 3, 4, 5, 6, 7, 8 } };
    int[] deck_0 = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
    int[] deck_1 = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };


    //[SerializeField] AI enemyAI;
    [SerializeField] UIManager uiManager;


    //public Transform playerHandTransform, playerFieldTransform, enemyHandTransform, enemyFieldTransform;
    //public Transform[,] conveyorTransform = new Transform[2, 4];
    Transform[] NextFieldTransform = new Transform[4];
    Transform[] CurrentFieldTransform = new Transform[4];

    //public object[] Conveyor = new object[2];

    //public Transform playerHandTransform;

    //[SerializeField] CardController cardPrefab;


    //Transform basicFieldTransform, additionalFieldTransform;
    //Transform mixFieldTransform[0], field2Transform, resultFieldTransform;
    Transform[] mixFieldTransform = new Transform[3];


    GameObject reverseObject;


    //public int attackID;
    public bool isMyTurn;



    public Transform playerHero;


    //時間管理
    int timeCount;

    public int playerID;

    


    SpecialController specialController;

    //List<int> moveCardIndex;


    //GameObject attackButton;
    //GameObject mixButton;
    //GameObject throwButton;

    public bool isAttackButton = true;
    public bool isMixButton = false;
    public bool isThrowButton = false;

    //public bool isDefence = false;

    GameObject attackButtons;

    MixController mixController;

    //bool canDefence = true;

    public bool isChange = false;



    //bool isPlayerUnhealth = false;
    //bool isEnemyUnHealth = false;

    public int maxHand;
    int defaultMaxHand;


    int cardIndex = 0;

    public int damageCal;

    //bool isConfirm1 = false;
    //bool isConfirm2 = false;


    //CardController card1 = null;
    //CardController card2 = null;
    //CardController resultCard = null;

    //CardController[] cardController = new CardController[3] {null, null, null};



    //public CardController[] selectCard = new CardController[2] { null, null };
    //public int[,] selectCardPosition = new int[2, 2] {{-1, -1}, {-1, -1}};
    //int[] selectCardPosition_0 = new int[2] { -1, -1 };
    //int[] selectCardPosition_1 = new int[2] { -1, -1 };

    int[] selectCardPosition = new int[2] { -1, -1 };


    [SerializeField] int deadLine;


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

        player = GameObject.Find("Player").GetComponent<GamePlayerManager>();
        enemy = GameObject.Find("Enemy").GetComponent<GamePlayerManager>();

        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        //playerHandTransform = GameObject.Find("PlayerHand").transform;
        //conveyorTransform[0] = GameObject.Find("Conveyor_1").transform;
        //conveyorTransform[1] = GameObject.Find("Conveyor_2").transform;

        //Conveyor[0] = GameObject.Find("Conveyor_1");
        //Conveyor[1] = GameObject.Find("Conveyor_2");

        GameObject[] fields = new GameObject[2];

        fields[0] = GameObject.Find("NextFields");
        fields[1] = GameObject.Find("CurrentFields");

        for (int i = 0; i < maxHand; i++)
        {
            NextFieldTransform[i] = fields[0].transform.GetChild(i).transform;
        }

        for (int i = 0; i < maxHand; i++)
        {
            CurrentFieldTransform[i] = fields[1].transform.GetChild(i).transform;
        }


        //playerFieldTransform = GameObject.Find("PlayerField").transform;
        //enemyHandTransform = GameObject.Find("EnemyHand").transform;
        //enemyFieldTransform = GameObject.Find("EnemyField").transform;

        //mixFieldTransform[0] = GameObject.Find("Field1").transform;
        //field2Transform = GameObject.Find("Field2").transform;
        //resultFieldTransform = GameObject.Find("ResultField").transform;

        for (int i = 0; i < 3; i++)
        {
            mixFieldTransform[i] = GameObject.Find("MixField_" + i.ToString()).transform;
        }

        reverseObject = GameObject.Find("ReverseObject");



        specialController = GameObject.Find("SpecialController").GetComponent<SpecialController>();

        attackButtons = GameObject.Find("AttackButtons");

        //battleObjects = GameObject.Find("BattleObjects");

        //mixController = GameObject.Find("MixController").GetComponent<MixController>();

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
        //uiManager.HideResultPanel();
        //player.Init(player.deck);
        //enemy.Init(enemy.deck);


        //プレイヤー１が先行
        //attackID = 1;
        if (playerID == 1)
        {
            isMyTurn = true;
        }
        else
        {
            isMyTurn = false;
        }

        uiManager.attackFields.SetActive(isMyTurn);


        //uiManager.ShowTurn(attackID);

        uiManager.ShowHP();
        uiManager.ShowNutrients();

        //uiManager.ShowMixCost(player.mixCost, enemy.mixCost);

        //ShuffleCard();

        
        if (playerID == 1)
        {

            GiveCardToHand();

            photonView.RPC(nameof(MoveConveyor), RpcTarget.AllViaServer);

            GiveCardToHand();

            //for (int i = 0; i < maxHand + 1; i++)
            //{

            //    int cardID_0 = UnityEngine.Random.Range(0, 9);
            //    int cardID_1 = UnityEngine.Random.Range(0, 9);
            //    int specialID_0 = UnityEngine.Random.Range(0, 5);
            //    int specialID_1 = UnityEngine.Random.Range(0, 5);
            //    photonView.RPC(nameof(GiveCardToHand), RpcTarget.AllViaServer, cardID_0, cardID_1, specialID_0, specialID_1);

            //    if (i == maxHand)
            //    {
            //        break;
            //    }

            //    photonView.RPC(nameof(MoveConveyor), RpcTarget.AllViaServer);

            //}


        }





        //カード並び替え
        //LineUpCard();


        //uiManager.ShowHealth(player.isHealth, enemy.isHealth);
        //uiManager.ShowPoison(player.isPoison, player.poisonCount, enemy.isPoison, enemy.poisonCount);
        //uiManager.ShowDark(player.isDark, enemy.isDark);
        //uiManager.ShowAttackUp(player.attackUp, enemy.attackUp);
        //uiManager.ShowHitUp(player.hitUp, enemy.hitUp);

        uiManager.decideButtonObj.SetActive(false);


    }


    [PunRPC]
    void DestroyIngredient(int[] selectCardPosition)
    {
        for (int i = 0; i < 2; i++)
        {
            if (selectCardPosition[i] != -1)
            {
                CurrentFieldTransform[selectCardPosition[i]].GetComponent<FieldController>().CancelCard();
                Destroy(CurrentFieldTransform[selectCardPosition[i]].GetChild(0).gameObject);
            }
        }

    }




    public void GiveCardToHand()
    {

        //int cardID = deck[cardIndex];

        for (int i = 0; i < maxHand; i++)
        {
            if (NextFieldTransform[i].childCount == 0)
            {
                int cardID = UnityEngine.Random.Range(0, 9);
                int specialID = UnityEngine.Random.Range(0, 5);
                int position = i;
                photonView.RPC(nameof(CreateHandCard), RpcTarget.AllViaServer, cardID, specialID, position);

            }
        }


        //CreateHandCard(KIND.INGREDIENT, cardID, specialID, position);


        //CreateCard(KIND.INGREDIENT, cardID_0, specialID_0, conveyorTransform[0, 0]);
        //CreateCard(KIND.INGREDIENT, cardID_1, specialID_1, conveyorTransform[1, 0]);


        //if (cardIndex == 8)
        //{
        //    ShuffleCard();
        //    cardIndex = 0;
        //}
        //else
        //{
        //    cardIndex++;
        //}

        //Debug.Log(cardIndex);


        //for (int i = 0; i < 2; i++)
        //{
        //    CreateCard(KIND.INGREDIENT, deck[i, cardIndex], conveyorTransform[i]);


        //}




    }

    //void CreateCard(int kind, int cardID, Transform hand)

    [PunRPC]
    void CreateHandCard(int cardID, int specialID, int position)
    {
        //GameObject Card = PhotonNetwork.Instantiate("Card", new Vector3(0, 0, 0), Quaternion.identity);
        Transform field = NextFieldTransform[position];

        GameObject card = Instantiate(cardPrefab, field, false);


        card.GetComponent<CardController>().Init(KIND.INGREDIENT, cardID, specialID);
        //Card.GetComponent<CardController>().Init(cardID, playerID);

        //return card.GetComponent<CardController>();

    }

    void CreateMixCard(KIND kind, int cardID, int specialID)
    {
        GameObject card = Instantiate(dishCardPrefab, mixFieldTransform[2], false);

        card.GetComponent<CardController>().Init(kind, cardID, specialID);

    }


    //CardController CreateCard(KIND kind, int cardID, int specialID, Transform target)
    //{
    //    //GameObject Card = PhotonNetwork.Instantiate("Card", new Vector3(0, 0, 0), Quaternion.identity);
    //    GameObject card = Instantiate(cardPrefab, target, false);


    //    card.GetComponent<CardController>().Init(kind, cardID, specialID);
    //    //Card.GetComponent<CardController>().Init(cardID, playerID);

    //    return card.GetComponent<CardController>();

    //}


    //CardController CreateDishCard(KIND kind, int cardID, int specialID, Transform target)
    //{
    //    GameObject card = Instantiate(dishCardPrefab, target, false);


    //    card.GetComponent<CardController>().Init(kind, cardID, specialID);

    //    return card.GetComponent<CardController>();

    //}


    public void OnDecideButton()
    {


        if (isMyTurn)
        {
            uiManager.decideButtonObj.SetActive(false);



            photonView.RPC(nameof(GenerateFieldCard), RpcTarget.AllViaServer, selectCardPosition);

            ////使ったカードの削除
            photonView.RPC(nameof(DestroyIngredient), RpcTarget.AllViaServer, selectCardPosition);



            selectCardPosition[0] = -1;
            selectCardPosition[1] = -1;




        }
        else
        {

            DamageCalculation();

        }


    }

    //合成予測を出す
    public void SelectCard(bool isSelected, int position)
    {
        //選択した時
        if (isSelected)
        {
            //何も選択されていない
            if (selectCardPosition[0] == -1)
            {
                selectCardPosition[0] = position;
            }
            //２つ目に選択
            else if (selectCardPosition[1] == -1)
            {
                selectCardPosition[1] = position;

            }
            //２つ目と入れ替え
            else
            {
                CurrentFieldTransform[selectCardPosition[1]].GetComponent<FieldController>().CancelCard();
                selectCardPosition[1] = position;           
            }

        }
        //キャンセルした時
        else
        {
            //１枚目に選択してた時
            if (selectCardPosition[0] == position)
            {
                //２枚目に選択してたカードなし
                if (selectCardPosition[1] == -1)
                {
                    selectCardPosition[0] = -1;                    
                }
                //２枚目に選択したカードを１枚目にする
                else
                {
                    selectCardPosition[0] = selectCardPosition[1];                    
                    selectCardPosition[1] = -1;
                }
            }
            //２枚目に選択してた時
            else
            {
                selectCardPosition[1] = -1;                
            }

        }

        //生成
        GenerateFieldCard(selectCardPosition);
    }


    //public void OnSwapButton()
    //{
    //    var card = selectCard[0];
    //    selectCard[0] = selectCard[1];
    //    selectCard[1] = card;
    //    GenerateFieldCard();
    //}

    //[PunRPC]
    //void ChangeConfirm(bool isConfirm)
    //{
    //    if (playerID == 1)
    //    {
    //        isConfirm1 = true;
    //    }
    //    else
    //    {
    //        isConfirm2 = true;
    //    }
    //}


    [PunRPC]
    void Attack_RPC(int[] cardID)
    {
        StartCoroutine(Attack(cardID));
    }

    IEnumerator Attack(int[] cardID)
    {
        //uiManager.attackFields.SetActive(true);

        //CleanField();

        //for (int i = 0; i < 3; i++)
        //{
        //    cardController[i] = null;
        //}

        //for (int i = 0; i < 2; i++)
        //{
        //    if (cardID[i] == -1)
        //    {
        //        continue;
        //    }
        //    cardController[i] = CreateCard(KIND.INGREDIENT, cardID[i], mixFieldTransform[i]);
        //}


        ////合成
        //if (cardID[1] != -1)
        //{
        //    int mixCardID = SpecialMix(cardController[0], cardController[1]);
        //    cardController[2] = CreateDishCard(KIND.DISH, mixCardID, mixFieldTransform[2]);
        //}
        //else
        //{
        //    cardController[2] = CreateCard(KIND.INGREDIENT, cardID[0], mixFieldTransform[2]);
        //}





        //card1 = CreateCard(KIND.INGREDIENT, cardID_1, mixFieldTransform[0]);
        yield return new WaitForSeconds(0.7f);



        //if (cardID_2 >= 0)
        //{
        //    card2 = CreateCard(KIND.INGREDIENT, cardID_2, field2Transform);
        //    //yield return new WaitForSeconds(0.7f);

        //    //合成
        //    int mixCardID = SpecialMix(card1, card2);
        //    resultCard = CreateDishCard(KIND.DISH, mixCardID, resultFieldTransform);
        //    resultCard.model.special = card1.model.cardID;
        //    resultCard.gameObject.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);

        //}
        //else
        //{
        //    resultCard = CreateCard(KIND.INGREDIENT, cardID_1, resultFieldTransform);
        //    resultCard.gameObject.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        //    //yield return new WaitForSeconds(0.7f);

        //}

        //BuffEffect();

        //if (isMyTurn)
        //{
        //    resultCard.view.Refresh(resultCard.model, player.attackUp, player.hitUp);
        //}
        //else
        //{
        //    resultCard.view.Refresh(resultCard.model, enemy.attackUp, enemy.hitUp);
        //}


        //if (!isMyTurn)
        //{
        //    uiManager.decideButtonObj.SetActive(true);
        //}


        //yield return new WaitForSeconds(1.0f);


    }

    //[PunRPC]
    //受ける側で計算
    void DamageCalculation()
    {
        //ダメージ計算
        //CardController attackCard = mixFieldTransform[2].GetChild(0).GetComponent<CardController>();
        //GamePlayerManager attacker;
        //GamePlayerManager defender;

        //if (isMyTurn)
        //{
        //    attacker = player;
        //    defender = enemy;
        //}
        //else
        //{
        //    attacker = enemy;
        //    defender = player;
        //}

        CardController attackCardController = mixFieldTransform[2].GetChild(0).GetComponent<CardController>();



        bool isHit = true;

        int a = UnityEngine.Random.Range(0, 100);
        //if (a >= cardController[2].model.hit + player.hitUp)
        //{
        //    //外れる
        //    //Debug.Log("外れた");
        //    damageCal = 0;
        //    isHit = false;
        //}
        //else
        //{
        //    //photonView.RPC(nameof(DamageCal), RpcTarget.AllViaServer, true);
        //    damageCal = cardController[2].model.cal;
        //    isHit = true;
        //}

        //photonView.RPC(nameof(DamageCal), RpcTarget.AllViaServer, true);
        damageCal = attackCardController.model.cal;
        isHit = true;


        //特殊効果
        

        photonView.RPC(nameof(Battle), RpcTarget.AllViaServer, isHit, damageCal);

        //回避計算
        //0~99
        //if (isMyTurn)
        //{
        //int a = UnityEngine.Random.Range(0, 100);
        //if (a >= attackCard.model.hit + player.hitUp)
        //{
        //    //外れる
        //    //Debug.Log("外れた");
        //    damageCal = 0;
        //    isHit = false;
        //}
        //else
        //{
        //    //photonView.RPC(nameof(DamageCal), RpcTarget.AllViaServer, true);
        //    damageCal = attackCard.model.cal;
        //    isHit = true;
        //}




    }

    [PunRPC]
    void Battle(bool isHit, int damageCal)
    {
        //CardController attackCard = mixFieldTransform[2].GetChild(0).GetComponent<CardController>();
        GamePlayerManager attacker;
        GamePlayerManager defender;

        if (isMyTurn)
        {
            attacker = player;
            defender = enemy;
        }
        else
        {
            attacker = enemy;
            defender = player;
        }

        //テスト用
        //if (attacker.isHealth)
        //{
        //    specialController.SpecialEffect(attackCard.model.special, isMyTurn, damageCal);
        //}

        CardController attackCardController = mixFieldTransform[2].GetChild(0).GetComponent<CardController>();

        //バフ効果
        if (attacker.attackUpCount > 0)
        {
            damageCal *= attacker.attackUpCount;
        }

        if (defender.defenceUpCount > 0)
        {
            damageCal /= defender.defenceUpCount;
        }

        if (isHit)
        {
            defender.hp -= damageCal;
            Debug.Log(damageCal + "kcalのダメージ");
        }
        else
        {
            Debug.Log("外れた");
        }

        //栄養素
        //attacker.red += attackCard.model.red;
        //attacker.yellow += attackCard.model.yellow;


        //attacker.green += attackCard.model.green;

        Debug.Log("獲得" + attackCardController.model.cost);
        attacker.cost += attackCardController.model.cost;

        if (attackCardController.model.kind == KIND.INGREDIENT)
        {
            specialController.SpecialEffect(attackCardController.model.specialID, isMyTurn, damageCal);
        }
        else
        {
            specialController.DishSpecialEffect(attackCardController.model.specialID, isMyTurn, damageCal);
        }

        

        uiManager.ShowHP();

        ChangeTurn();
    }


    //次のターンへ移る
    [PunRPC]
    public void ChangeTurn()
    {
        CleanField();

        //ProgressTurnCount();


        CheckPoison();
        CheckDark();


        //CheckHealth();
        //PoisonDamage();

        //uiManager.ShowHealth(player.isHealth, enemy.isHealth);
        //uiManager.ShowPoison(player.isPoison, player.poisonCount, enemy.isPoison, enemy.poisonCount);
        //uiManager.ShowDark(player.isDark, enemy.isDark);
        //uiManager.ShowAttackUp(player.attackUp, enemy.attackUp);
        //uiManager.ShowHitUp(player.hitUp, enemy.hitUp);

        uiManager.ShowTriangle();




        isMyTurn = !isMyTurn;



        MoveConveyor();

        uiManager.ShowStatus();


        if (isMyTurn)
        {


            //uiManager.attackFields.SetActive(true);

            GiveCardToHand();

            //int cardID_0 = UnityEngine.Random.Range(0, 9);
            //int cardID_1 = UnityEngine.Random.Range(0, 9);
            //int specialID_0 = UnityEngine.Random.Range(0, 5);
            //int specialID_1 = UnityEngine.Random.Range(0, 5);

            //photonView.RPC(nameof(GiveCardToHand), RpcTarget.AllViaServer, cardID_0, cardID_1, specialID_0, specialID_1);


            //player.red--;
            //player.yellow--;
            //player.green--;
        }
        else
        {
            //uiManager.attackFields.SetActive(false);

            //enemy.red--;
            //enemy.yellow--;
            //enemy.green--;
        }

        //uiManager.ShowHP();
        uiManager.ShowNutrients();
        uiManager.decideButtonObj.SetActive(false);


        uiManager.attackFields.SetActive(isMyTurn);
        

        
    }


    [PunRPC]
    public void GenerateFieldCard(int[] selectCardPosition)
    {
        //Debug.Log(selectCardPosition[0]);
        //Debug.Log(selectCardPosition[1]);

        uiManager.attackFields.SetActive(true);


        CleanField();

        int[] cardID = new int[2] { -1, -1 };
        int[] specialID = new int[2] { -1, -1 };
        CardController[] cardController = new CardController[3] { null, null, null };

        //for (int i = 0; i < 3; i++)
        //{
        //    cardController[i] = null;
        //}

        for (int i = 0; i < 2; i++)
        {
            if (selectCardPosition[i] != -1)
            {
                GameObject selectCard = CurrentFieldTransform[selectCardPosition[i]].GetChild(0).gameObject;
                cardID[i] = selectCard.GetComponent<CardController>().model.cardID;
                specialID[i] = selectCard.GetComponent<CardController>().model.specialID;

                GameObject card = GameObject.Instantiate(selectCard, mixFieldTransform[i], false);
                cardController[i] = card.GetComponent<CardController>();
                cardController[i].Init(KIND.INGREDIENT, cardID[i], specialID[i]);
                cardController[i].model.cost = selectCard.GetComponent<CardController>().model.cost;

                //Debug.Log("カード" + i + cardController[i].model.cost);
            }
        }

        

        //if (selectCardPosition[0] == -1)
        //{
        //    return;
        //}
        //else
        //{
        //    cardID[0] = CurrentFieldTransform[selectCardPosition[0]].GetChild(0).gameObject.GetComponent<CardController>().model.cardID;
        //    specialID[0] = cardID[0] = CurrentFieldTransform[selectCardPosition[0]].GetChild(0).gameObject.GetComponent<CardController>().model.specialID;
        //}

        //if (selectCardPosition[1] == -1)
        //{
        //    //２枚目はなし
        //}
        //else
        //{
        //    cardID[1] = conveyorTransform[selectCardPosition_1[0], selectCardPosition_1[1]].GetChild(0).gameObject.GetComponent<CardController>().model.cardID;
        //    specialID[1] = conveyorTransform[selectCardPosition_1[0], selectCardPosition_1[1]].GetChild(0).gameObject.GetComponent<CardController>().model.specialID;
        //}


        //if (cardID[0] != -1)
        //{
        //    GameObject card_0 = GameObject.Instantiate(conveyorTransform[selectCardPosition_0[0], selectCardPosition_0[1]].GetChild(0).gameObject, mixFieldTransform[0], false);
        //    cardController[0] = card_0.GetComponent<CardController>();
        //    cardController[0].Init(KIND.INGREDIENT, cardID[0], specialID[0]);
        //}

        //if (cardID[1] != -1)
        //{
        //    GameObject card_1 = GameObject.Instantiate(conveyorTransform[selectCardPosition_1[0], selectCardPosition_1[1]].GetChild(0).gameObject, mixFieldTransform[1], false);
        //    cardController[1] = card_1.GetComponent<CardController>();
        //    cardController[1].Init(KIND.INGREDIENT, cardID[1], specialID[1]);
        //}








        if (cardController[0] == null)
        {
            uiManager.decideButtonObj.SetActive(false);

            return;
        }


        //合成チェック
        if (cardController[1] != null)
        {
            if (cardController[0].model.nutrient == cardController[1].model.nutrient)
            {
                //合成不可
                uiManager.decideButtonObj.SetActive(false);
                return;
            }
            else
            {
                //合成
                int mixCardID = SpecialMix(cardController[0], cardController[1]);

                CreateMixCard(KIND.DISH, mixCardID, mixCardID);

                //cardController[2] = CreateDishCard(KIND.DISH, mixCardID, mixCardID, mixFieldTransform[2]);
                uiManager.decideButtonObj.SetActive(true);
            }
        }
        else
        {
            uiManager.decideButtonObj.SetActive(true);
            CreateMixCard(KIND.INGREDIENT, cardID[0], specialID[0]);

            //食材のコストを代入
            cardController[2] = mixFieldTransform[2].GetChild(0).GetComponent<CardController>();
            //Debug.Log("コスト" + cardController[0].model.cost);
            cardController[2].model.cost = cardController[0].model.cost;
            Debug.Log("コスト" + cardController[2].model.cost);
            //cardController[2] = CreateDishCard(KIND.INGREDIENT, cardID[0], specialID[0], mixFieldTransform[2]);

        }



    }


    //void BuffEffect()
    //{
    //    //バフ効果を反映

    //    if (isMyTurn)
    //    {

    //        if (player.attackUp != 0 || player.hitUp != 0)
    //        {
    //            //CardController attackCard = resultFieldTransform.GetChild(0).GetComponent<CardController>();
    //            //Debug.Log(attackCard.model.cal);
    //            resultCard.model.cal += player.attackUp * 20;
    //            resultCard.model.hit += player.hitUp * 20;
    //            resultCard.view.Refresh(resultCard.model, player.attackUp, player.hitUp);
    //            //Debug.Log(attackCard.model.cal);
    //        }
    //    }
    //    else
    //    {
    //        if (enemy.attackUp != 0 || enemy.hitUp != 0)
    //        {
    //            //CardController attackCard = resultFieldTransform.GetChild(0).GetComponent<CardController>();
    //            resultCard.model.cal += enemy.attackUp * 20;
    //            resultCard.model.hit += enemy.hitUp * 20;
    //            resultCard.view.Refresh(resultCard.model, enemy.attackUp, enemy.hitUp);
    //        }
    //    }

    //}

    int SpecialMix(CardController card_1, CardController card_2)
    {
        //GameObject card = null;
        int specialMixID = -1;

        for (int i = 0; i < card_1.model.partnerID.Length; i++)
        {
            if (card_1.model.partnerID[i] == card_2.model.cardID)
            {
                specialMixID = card_1.model.specialMixID[i];
                break;

            }
        }

        return specialMixID;
    }





    void CleanField()
    {
        //yield return new WaitForSeconds(2);

        //if (mixFieldTransform[0].childCount != 0)
        //{
        //    Destroy(mixFieldTransform[0].GetChild(0).gameObject);
        //}
        //if (field2Transform.childCount != 0)
        //{
        //    Destroy(field2Transform.GetChild(0).gameObject);
        //}
        //if (resultFieldTransform.childCount != 0)
        //{
        //    Destroy(resultFieldTransform.GetChild(0).gameObject);
        //}

        foreach (Transform field in mixFieldTransform)
        {
            if (field.childCount != 0)
            {
                Destroy(field.GetChild(0).gameObject);
            }
        }


        //for (int i = 0; i < 3; i++)
        //{
        //    if (cardController[i] != null)
        //    {
        //        Destroy(cardController[i].gameObject);

        //    }
        //}

        //if (card1 != null)
        //{
        //    Destroy(card1.gameObject);
        //}
        //if (card2 != null)
        //{
        //    Destroy(card2.gameObject);
        //}
        //if (resultCard != null)
        //{
        //    Destroy(resultCard.gameObject);
        //}

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


    //カードを番号順に並べる
    //public void LineUpCard()
    //{

    //    List<GameObject> objList = new List<GameObject>();

    //    // 子階層のGameObject取得
    //    var childCount = playerHandTransform.childCount;
    //    for (int i = 0; i < childCount; i++)
    //    {
    //        objList.Add(playerHandTransform.GetChild(i).gameObject);
    //    }

    //    //objList.Sort((obj1, obj2) => int.Compare(obj1.model.cardID, obj2.model.cardID));
    //    objList.Sort((a, b) => a.GetComponent<CardController>().model.cardID - b.GetComponent<CardController>().model.cardID);

    //    foreach (var obj in objList)
    //    {
    //        //obj.SetSiblingIndex(childCount - 1);
    //        obj.transform.SetSiblingIndex(childCount - 1);

    //    }


    //}

    


    [PunRPC]
    void MixCard(int kind_1, int cardID_1, int kind_2, int cardID_2, int ID)
    {
        uiManager.attackFields.SetActive(false);
        uiManager.defenceFields.SetActive(true);
        //MixController.instance.MixCard(cardID_1, cardID_2, ID);
        StartCoroutine(mixController.MixCard(kind_1, cardID_1, kind_2, cardID_2, ID));
    }


   
    void ShuffleCard()
    {
        for (int i = deck_0.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            var tmp = deck_0[i];
            deck_0[i] = deck_0[j];
            deck_0[j] = tmp;
        }

        for (int i = deck_1.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            var tmp = deck_1[i];
            deck_1[i] = deck_1[j];
            deck_1[j] = tmp;
        }




        //for (int i = 0; i < 2; i++)
        //{
        //    for (int j = deck.Length - 1; j < 0; j--)
        //    {
        //        int k = UnityEngine.Random.Range(0, i + 1);
        //        var tmp = deck[i, j];
        //        deck[i, j] = deck[i, j];
        //        deck[i, j] = tmp;
        //    }
        //}


    }


    //[PunRPC]
    //void SyncDeck(int[,] deck)
    //{
    //    //for (int i = 0; i < Deck_1)
    //    //Deck_1[0]
    //    this.deck = deck;
    //}

    void ProgressTurnCount()
    {
        for (int i = 0; i < maxHand; i++)
        {
            if (CurrentFieldTransform[i].childCount != 0)
            {
                CardController cardController = CurrentFieldTransform[i].GetChild(0).GetComponent<CardController>();
                cardController.model.deadLine--;                

                if (cardController.model.deadLine == 0)
                {
                    Destroy(cardController.gameObject);
                }
                else
                {
                    //Debug.Log("aa");
                    cardController.view.ShowDeadLine(cardController.model.deadLine);
                    //cardController.model.cost = (deadLine - cardController.model.turnCount + 1) / 2;
                    //cardController.view.ChangeCost(cardController.model);
                }
            }

        }


    }

    [PunRPC]
    void MoveConveyor()
    {


        for (int i = 0; i < maxHand; i++)
        {
            //コスト減少
            if (CurrentFieldTransform[i].childCount != 0)
            {
                CardController cardController = CurrentFieldTransform[i].GetChild(0).GetComponent<CardController>();
                cardController.model.deadLine--;

                if (cardController.model.deadLine == 0)
                {
                    Destroy(cardController.gameObject);


                    cardController = NextFieldTransform[i].GetChild(0).GetComponent<CardController>();
                    cardController.transform.SetParent(CurrentFieldTransform[i], false);
                    cardController.view.deadLineObject.SetActive(true);
                    cardController.view.ShowDeadLine(cardController.model.deadLine);
                }
                else
                {
                    //Debug.Log("aa");

                    cardController.view.ShowDeadLine(cardController.model.deadLine);
                    cardController.model.cost = (deadLine - cardController.model.deadLine) / 2;


                    Debug.Log(i + "コスト" + cardController.model.cost);
                    //cardController.view.ChangeCost(cardController.model);
                }
            }



            //新しい食材
            else if (CurrentFieldTransform[i].childCount == 0)
            {
                CardController cardController = NextFieldTransform[i].GetChild(0).GetComponent<CardController>();
                cardController.transform.SetParent(CurrentFieldTransform[i], false);
                cardController.view.deadLineObject.SetActive(true);
                cardController.view.ShowDeadLine(cardController.model.deadLine);

                //cardController.view.costObject.SetActive(true);
                //cardController.view.ChangeCost(cardController.model);

                //NextFieldTransform[i].GetChild(0).transform.SetParent(CurrentFieldTransform[i], false);
                //CurrentFieldTransform[i].GetChild(0).GetComponent<CardController>().view.ChangeCost(cardController.model);

            }


        }
        
    }



    void CheckHealth()
    {
        if (player.red > 0 || player.yellow > 0 || player.green > 0)
        {
            player.isHealth = true;
        }
        else
        {
            player.isHealth = false;
        }

        if (enemy.red > 0 || enemy.yellow > 0 || enemy.green > 0)
        {
            enemy.isHealth = true;
        }
        else
        {
            enemy.isHealth = false;
        }

        ////健康
        //if (Mathf.Abs(player.red - player.yellow) >= 3 ||
        //    Mathf.Abs(player.yellow - player.green) >= 3 ||
        //    Mathf.Abs(player.green - player.red) >= 3)
        //{
        //    player.ishealth = false;
        //}
        //else
        //{
        //    player.ishealth = true;
        //}


        ////健康
        //if (Mathf.Abs(enemy.red - enemy.yellow) >= 3 ||
        //    Mathf.Abs(enemy.yellow - enemy.green) >= 3 ||
        //    Mathf.Abs(enemy.green - enemy.red) >= 3)
        //{
        //    enemy.ishealth = false;
        //}
        //else
        //{
        //    enemy.ishealth = true;
        //}



    }

    void CheckPoison()
    {
        //自分
        if (isMyTurn)
        {
            if (player.poisonCount > 0)
            {
                player.poisonCount--;
            }

            if (player.poisonCount > 0)
            {
                int poisonDamage = player.poisonCount * 5;
                player.hp -= poisonDamage;
                Debug.Log("毒で" + poisonDamage + "ダメージ");
            }

        }
        //相手
        else
        {
            if (enemy.poisonCount > 0)
            {
                enemy.poisonCount--;
            }

            if (enemy.poisonCount > 0)
            {
                int poisonDamage = enemy.poisonCount * 5;
                enemy.hp -= poisonDamage;
                Debug.Log("毒で" + poisonDamage + "ダメージ");
            }
        }




    }

    void CheckDark()
    {
        //自分
        if (isMyTurn)
        {
            if (player.darkCount > 0)
            {
                player.darkCount--;
            }

            if (player.darkCount > 0)
            {
                Debug.Log("暗闇");
            }

        }
        //相手
        else
        {
            if (enemy.darkCount > 0)
            {
                enemy.darkCount--;
            }

            if (enemy.darkCount > 0)
            {
                Debug.Log("暗闇");
            }
        }


    }
}
