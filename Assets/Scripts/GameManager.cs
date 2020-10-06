using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;


public class GameManager : MonoBehaviourPunCallbacks
{
    //CardController cardPrefab;
    GameObject cardPrefab;


    public GamePlayerManager player;
    public GamePlayerManager enemy;

    //[SerializeField] AI enemyAI;
    [SerializeField] UIManager uiManager;


    [SerializeField]
    public Transform playerHandTransform, playerFieldTransform, enemyHandTransform, enemyFieldTransform;

    //[SerializeField] CardController cardPrefab;


    Transform basicFieldTransform, additionalFieldTransform;


    GameObject reverseObject;


    public int attackID;
    public bool isMyTurn;



    public Transform playerHero;


    //時間管理
    int timeCount;

    public int playerID;

    FrequencyController frequencyController;


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

    //シングルトン化（どこからでもアクセスできるようにする）
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        cardPrefab = (GameObject)Resources.Load("Card");

        player = GameObject.Find("PlayerHero").GetComponent<GamePlayerManager>();
        enemy = GameObject.Find("EnemyHero").GetComponent<GamePlayerManager>();

        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        playerHandTransform = GameObject.Find("PlayerHand").transform;
        playerFieldTransform = GameObject.Find("PlayerField").transform;
        enemyHandTransform = GameObject.Find("EnemyHand").transform;
        enemyFieldTransform = GameObject.Find("EnemyField").transform;

        basicFieldTransform = GameObject.Find("BasicField").transform;
        additionalFieldTransform = GameObject.Find("AdditionalField").transform;

        reverseObject = GameObject.Find("ReverseObject");

        frequencyController = GameObject.Find("FrequencyController").GetComponent<FrequencyController>();

        specialController = GameObject.Find("SpecialController").GetComponent<SpecialController>();

        attackButtons = GameObject.Find("AttackButtons");

        //battleObjects = GameObject.Find("BattleObjects");

        mixController = GameObject.Find("MixController").GetComponent<MixController>();

        playerID = PhotonNetwork.LocalPlayer.ActorNumber;

        defaultMaxHand = maxHand;

        if (playerID == 2)
        {
            //reverseObject.transform.localScale = new Vector3(-1, -1, 1);
            Vector3 tmp = playerFieldTransform.position;
            playerFieldTransform.position = enemyFieldTransform.position;
            enemyFieldTransform.transform.position = tmp;

            tmp = playerHandTransform.position;
            playerHandTransform.position = enemyHandTransform.position;
            enemyHandTransform.transform.position = tmp;

            tmp = player.transform.position;
            player.transform.position = enemy.transform.position;
            enemy.transform.position = tmp;

        }


        //デッキ生成
        if (playerID == 1)
        {
            //player.deck = new List<int>();
            //for (int i = 0; i < 20; i++)
            //{
            //    //int num = UnityEngine.Random.Range(0, frequencyController.sum);

            //    (int kind, int cardID) = frequencyController.DecideCard();

            //    //player.deck.Add(frequencyController.DecideCard(num));
            //    player.deck_cardKind.Add(kind);
            //    player.deck_cardID.Add(cardID);
            //}

            //相手のDropPlaceを消す
            GameObject.Find("EnemyHand").GetComponent<DropPlace>().enabled = false;
            GameObject.Find("EnemyField").GetComponent<DropPlace>().enabled = false;
        }
        else
        {
            //enemy.deck = new List<int>();
            //for (int i = 0; i < 20; i++)
            //{
            //    //int num = UnityEngine.Random.Range(0, frequencyController.sum);

            //    //(int kind, int cardID) = frequencyController.DecideCard(num);
            //    //enemy.deck[i].Add(kind);
            //    //enemy.deck[i].Add(cardID);

            //    //enemy.deck.Add(frequencyController.DecideCard(num));

            //    (int kind, int cardID) = frequencyController.DecideCard();

            //    //player.deck.Add(frequencyController.DecideCard(num));
            //    enemy.deck_cardKind.Add(kind);
            //    enemy.deck_cardID.Add(cardID);

            //}


            //相手のDropPlaceを消す
            GameObject.Find("PlayerHand").GetComponent<DropPlace>().enabled = false;
            GameObject.Find("PlayerField").GetComponent<DropPlace>().enabled = false;
        }



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
        uiManager.HideResultPanel();
        //player.Init(player.deck);
        //enemy.Init(enemy.deck);


        //プレイヤー１が先行
        attackID = 1;
        //if (playerID == 1)
        //{
        //    isMyTurn = true;
        //}
        //else
        //{
        //    isMyTurn = false;
        //}


        uiManager.ShowTurn(attackID);

        uiManager.ShowHeroHP(player.heroHp, enemy.heroHp);
        uiManager.ShowMixCost(player.mixCost, enemy.mixCost);

        SettingInitHand();

        //SetCardToHand();




        //カード並び替え
        LineUpCard(playerHandTransform);
        LineUpCard(enemyHandTransform);

        //TurnCalc();

        StartAttack();


    }



    public void ReducemixCost(int cost, bool isPlayerCard)
    {
        if (isPlayerCard)
        {
            player.mixCost -= cost;
        }
        else
        {
            enemy.mixCost -= cost;
        }
        uiManager.ShowMixCost(player.mixCost, enemy.mixCost);
    }

    public void Restart()
    {
        //handとFieldのカードを削除
        foreach (Transform card in playerHandTransform)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in playerFieldTransform)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in enemyHandTransform)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in enemyFieldTransform)
        {
            Destroy(card.gameObject);
        }

        //デッキ生成
        //player.deck = new List<int>();
        //for (int i = 0; i < 20; i++)
        //{
        //    int cardID = UnityEngine.Random.Range(1, 7);
        //    player.deck.Add(cardID);
        //}
        //enemy.deck = new List<int>();
        //for (int i = 0; i < 20; i++)
        //{
        //    int cardID = UnityEngine.Random.Range(1, 7);
        //    enemy.deck.Add(cardID);
        //}

        StartGame();
    }

    public void SettingInitHand()
    {
        for (int i = 0; i < maxHand; i++)
        {

            //if (playerID == 1)
            //    GiveCardToHand(player.deck_cardKind, player.deck_cardID, playerHandTransform);
            //else
            //    GiveCardToHand(enemy.deck_cardKind, enemy.deck_cardID, enemyHandTransform);

            GiveCardToHand(playerID);


        }


    }

    //public void GiveCardToHand(List<int> deck_cardKind ,List<int> deck_cardID, Transform hand)
    public void GiveCardToHand(int ID)
    {
        //if (deck_cardKind.Count == 0)
        //{
        //    return;
        //}
        //int kind = deck_cardKind[0];
        //int cardID = deck_cardID[0];

        //deck_cardKind.RemoveAt(0);
        //deck_cardID.RemoveAt(0);


        int kind = frequencyController.DecideCardKind();
        int cardID = frequencyController.DecideCardID(kind);

        if (ID == 1)
        {
            CreateCard(kind, cardID, playerHandTransform);
        }
        else
        {
            CreateCard(kind, cardID, enemyHandTransform);
        }



    }

    void CreateCard(int kind, int cardID, Transform hand)
    {
        //GameObject Card = PhotonNetwork.Instantiate("Card", new Vector3(0, 0, 0), Quaternion.identity);
        GameObject card = Instantiate(cardPrefab, hand, false);
        card.GetComponent<CardController>().Init(kind, cardID, false);
        //Card.GetComponent<CardController>().Init(cardID, playerID);



    }


    public void OnDecideButton()
    {


        if (playerID == attackID)
        {

            CheckField();

        }
        else
        {
            CardController[] fieldCardList;

            //相手側に防御側のカードの作成
            if (playerID == 1)
            {
                fieldCardList = playerFieldTransform.GetComponentsInChildren<CardController>();


            }
            else
            {
                fieldCardList = enemyFieldTransform.GetComponentsInChildren<CardController>();


            }



            foreach (CardController card in fieldCardList)
            {
                photonView.RPC(nameof(CreateDefenceCard), RpcTarget.Others, card.model.kind, card.model.cardID, playerID);

            }

            photonView.RPC(nameof(Battle), RpcTarget.AllViaServer);
            photonView.RPC(nameof(ChangeTurn), RpcTarget.AllViaServer);
        }

       


  


    }





    //攻撃を実行する
    [PunRPC]
    void Battle()
    {
        Debug.Log("バトル");


        CardController[] attackCardList;

        if (attackID == 1)
        {
            attackCardList = playerFieldTransform.GetComponentsInChildren<CardController>();
        }
        else
        {
            attackCardList = enemyFieldTransform.GetComponentsInChildren<CardController>();
        }

        ////Debug.Log(defenceCardList[0].model.cal);


        //int damage = attackCardList[0].model.cal;


        //foreach (CardController card in defenceCardList)
        //{
        //    Debug.Log(card.model.cal);

        //    damage -= card.model.cal;


        //}


        //if (damage < 0)
        //    damage = 0;

        int damage = CalculateDamage();

        if (attackID == 1)
        {
            enemy.heroHp -= damage;

            player.heroRed += attackCardList[0].model.red;
            player.heroYellow += attackCardList[0].model.yellow;
            player.heroGreen += attackCardList[0].model.green;
        }
        else
        {
            player.heroHp -= damage;

            enemy.heroRed += attackCardList[0].model.red;
            enemy.heroYellow += attackCardList[0].model.yellow;
            enemy.heroGreen += attackCardList[0].model.green;
        }

        Debug.Log("プレイヤー" + attackID + "の攻撃");
        Debug.Log(damage + "ダメージ");

        //bool isDamage = false;
        //if (damage > 0)
        //{
        //    isDamage = true;
        //}
        //特殊カードチェック
        specialController.CheckSpecial(attackCardList[0].model.special, damage);

        uiManager.ShowHeroNutrients(player.heroRed, player.heroYellow, player.heroGreen, enemy.heroRed, enemy.heroYellow, enemy.heroGreen);



    }


    IEnumerator CleanField()
    {
        yield return new WaitForSeconds(1);


        CardController[] playerFieldCardList = playerFieldTransform.GetComponentsInChildren<CardController>();
        CardController[] enemyFieldCardList = enemyFieldTransform.GetComponentsInChildren<CardController>();

        foreach (Transform card in playerFieldTransform)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in enemyFieldTransform)
        {
            Destroy(card.gameObject);
        }


        //一旦ここにかく
        StartAttack();
    }

   
       

    //攻撃側の開始
    void StartAttack()
    {
        Debug.Log("攻撃開始");


        //CardController[] handCardList;

        //if (playerID == 1)
        //    handCardList = playerHandTransform.GetComponentsInChildren<CardController>();
        //else
        //    handCardList = enemyHandTransform.GetComponentsInChildren<CardController>();

        Transform hand;

        if (playerID == 1)
            hand = playerHandTransform;
        else
            hand = enemyHandTransform;

        if (playerID == attackID)
        {
            isMyTurn = true;

            ChangeDraggable(hand, 0, true);
            ChangeDraggable(hand, 1, false);

            //foreach (CardController card in handCardList)
            //{
            //    if (card.model.kind == 0 || card.model.kind == 2)
            //    {
            //        card.movement.isDraggable = true;
            //    }
            //    else
            //    {
            //        card.movement.isDraggable = false;
            //    }

            //}

            uiManager.attackFields.SetActive(true);
            uiManager.defenceFields.SetActive(false);

            //uiManager.buttonsObj.SetActive(true);
            uiManager.ShowButtonObj(true);

            

        }
        else
        {
            isMyTurn = false;

            ChangeDraggable(hand, 0, false);
            ChangeDraggable(hand, 1, false);

            //foreach (CardController card in handCardList)
            //{

            //    card.movement.isDraggable = false;

            //}

            //uiManager.buttonsObj.SetActive(false);

            isAttackButton = false;
            isMixButton = false;
            isThrowButton = false;


            uiManager.attackFields.SetActive(false);
            uiManager.defenceFields.SetActive(false);

        }

        //決定ボタン消す
        uiManager.decideButtonObj.SetActive(false);
        uiManager.ShowTurn(attackID);


    }

    //防御側の開始
    [PunRPC]
    public void StartDefence()
    {

        uiManager.ShowExpectDamage();

        //CardController[] handCardList;

        //if (playerID == 1)
        //    handCardList = playerHandTransform.GetComponentsInChildren<CardController>();
        //else
        //    handCardList = enemyHandTransform.GetComponentsInChildren<CardController>();

        Transform hand;

        if (playerID == 1)
            hand = playerHandTransform;
        else
            hand = enemyHandTransform;


        if (playerID != attackID)
        {
            isMyTurn = true;


            ChangeDraggable(hand, 0, false);

            if ((playerID == 1 && player.health == 0) || (playerID == 2 && enemy.health == 0))
            {
                ChangeDraggable(hand, 1, false);

            }
            else
            {
                ChangeDraggable(hand, 1, true);

            }


            //foreach (CardController card in handCardList)
            //{
            //    if (card.model.kind == 1 && canDefence)
            //    {
            //        card.movement.isDraggable = true;
            //    }
            //    else
            //    {
            //        card.movement.isDraggable = false;

            //    }

            //}

            //決定ボタン表示
            uiManager.decideButtonObj.SetActive(true);

            uiManager.attackFields.SetActive(false);
            uiManager.defenceFields.SetActive(true);
        }
        else
        {
            isMyTurn = false;

            //foreach (CardController card in handCardList)
            //{

            //    card.movement.isDraggable = false;

            //}

            ChangeDraggable(hand, 0, false);
            ChangeDraggable(hand, 1, false);

            uiManager.attackFields.SetActive(false);
            uiManager.defenceFields.SetActive(true);
        }


        //attackButtons.SetActive(false);


        //自分のフィールドだけ表示
        //if (playerID == 1)
        uiManager.playerFieldImage.enabled = true;
        //else
        uiManager.enemyFieldImage.enabled = true;

        uiManager.ShowTurn(attackID);

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

    public CardController[] GetEnemyFieldCards()
    {
        return enemyFieldTransform.GetComponentsInChildren<CardController>();

    }









    //次のターンへ移る
    [PunRPC]
    public void ChangeTurn()
    {

        //フィールドのカード消去
        StartCoroutine(CleanField());
        MixController.instance.CleanField();

        //CleanField();

        //手札補充
        if (playerID == 1)
        {
            int drawNum = maxHand - playerHandTransform.childCount;

            //Debug.Log("draw" + drawNum);

            for (int i = 0; i < drawNum; i++)
            {
                //GiveCardToHand(player.deck_cardKind, player.deck_cardID, playerHandTransform);
                GiveCardToHand(playerID);
            }






        }
        else
        {
            int drawNum = maxHand - enemyHandTransform.childCount;

            for (int i = 0; i < drawNum; i++)
            {
                //GiveCardToHand(enemy.deck_cardKind, enemy.deck_cardID, enemyHandTransform);
                GiveCardToHand(playerID);
            }



        }

        CheckHealth();
        CheckPoison();


        if (attackID == 1)
            attackID = 2;
        else if (attackID == 2)
            attackID = 1;

        //if (attackID == playerID)
        //    isMyTurn = true;
        //else
        //    isMyTurn = false;


        //uiManager.ShowTurn(attackID);

        //isMyTurn = !isMyTurn;

        CardController[] playerFieldCardList = playerFieldTransform.GetComponentsInChildren<CardController>();
        CardController[] enemyFieldCardList = enemyFieldTransform.GetComponentsInChildren<CardController>();



        isAttackButton = false;
        isMixButton = false;
        isThrowButton = false;

        player.mixCost++;
        enemy.mixCost++;
        uiManager.ShowHeroHP(player.heroHp, enemy.heroHp);
        uiManager.ShowMixCost(player.mixCost, enemy.mixCost);


        LineUpCard(playerHandTransform);
        LineUpCard(enemyHandTransform);

        //CleanFieldに記載
        //StartAttack();
    }







    public void CheckHeroHP()
    {
        if (player.heroHp <= 0 || enemy.heroHp <= 0)
        {
            ShowResultPanel(player.heroHp);

        }



        
    }

    void ShowResultPanel(int heroHp)
    {
        StopAllCoroutines();
        uiManager.ShowResultPanel(heroHp);


    }


    //カードを番号順に並べる
    public void LineUpCard(Transform field)
    {
        List<GameObject> objList = new List<GameObject>();

        // 子階層のGameObject取得
        var childCount = field.childCount;
        for (int i = 0; i < childCount; i++)
        {
            objList.Add(field.GetChild(i).gameObject);
        }

        //objList.Sort((obj1, obj2) => int.Compare(obj1.model.cardID, obj2.model.cardID));
        objList.Sort((a, b) => a.GetComponent<CardController>().model.kind - b.GetComponent<CardController>().model.kind);

        foreach (var obj in objList)
        {
            //obj.SetSiblingIndex(childCount - 1);
            obj.transform.SetSiblingIndex(childCount - 1);

        }


    }

    [PunRPC]
    void MoveCard(int ID, int Index)
    {
        if (ID == 1)
        {
            CardController card = playerFieldTransform.GetChild(Index).GetComponent<CardController>();

            card.movement.MoveToField(playerFieldTransform);

            //card.Show();
        }
        else
        {
            CardController card = enemyFieldTransform.GetChild(Index).GetComponent<CardController>();

            card.movement.MoveToField(enemyFieldTransform);

            //card.Show();
        }

    }


    public void CheckField()
    {
        //カード交換
        if (isChange)
        {
            uiManager.OnChangeButton();
            //Destroy(basicFieldTransform.GetChild(0).gameObject);
            ChangeCard(basicFieldTransform.GetChild(0).GetComponent<CardController>());
            photonView.RPC(nameof(ChangeTurn), RpcTarget.AllViaServer);

        }
        else
        {
            //フィールドのカードを確認
            if (additionalFieldTransform.childCount == 0)
            {
                //合成なし
                CardController basicCardController = basicFieldTransform.GetChild(0).GetComponent<CardController>();

                //MixController.instance.CleanField();

                photonView.RPC(nameof(SingleCard), RpcTarget.AllViaServer, basicCardController.model.kind, basicCardController.model.cardID, playerID);


            }
            else
            {
                //合成
                //int basicCardKind = basicFieldTransform.GetChild(0).GetComponent<CardController>().model.cardKind;
                CardController basicCardController = basicFieldTransform.GetChild(0).GetComponent<CardController>();
                //int additionalCardKind = additionalFieldTransform.GetChild(0).GetComponent<CardController>().model.cardKind;
                //int additionalCardID = additionalFieldTransform.GetChild(0).GetComponent<CardController>().model.cardID;
                CardController additionalCardController = additionalFieldTransform.GetChild(0).GetComponent<CardController>();


                //MixController.instance.CleanField();

                photonView.RPC(nameof(MixCard), RpcTarget.AllViaServer, basicCardController.model.kind, basicCardController.model.cardID, additionalCardController.model.kind, additionalCardController.model.cardID, playerID);

            }

            uiManager.ShowHeroHP(player.heroHp, enemy.heroHp);


        }




    }


    [PunRPC]
    void SingleCard(int kind, int cardID, int ID)
    {
        uiManager.attackFields.SetActive(false);
        uiManager.defenceFields.SetActive(true);

        //MixController.instance.MixCard(cardID_1, cardID_2, ID);

        GameObject card;
        //CardController
        if (ID == 1)
        {
            //CreateCard(cardID, playerFieldTransform);
            card = Instantiate(cardPrefab, playerFieldTransform, false);
            card.GetComponent<CardController>().Init(kind, cardID, false);
        }
        else
        {
            //CreateCard(cardID, enemyFieldTransform);
            card = Instantiate(cardPrefab, enemyFieldTransform, false);
            card.GetComponent<CardController>().Init(kind, cardID, false);
        }


        CardController cardController = card.GetComponent<CardController>();

        if (cardController.model.kind == 0)
        {
            StartDefence();
        }
        //else if (cardController.model.kind == 2)
        //{
        //    //スペルの場合は防御なしで次のターン
        //    cardController.Spell(ID, cardController.model.spellNum);
        //    ChangeTurn();
        //}
    }

    [PunRPC]
    void MixCard(int kind_1, int cardID_1, int kind_2, int cardID_2, int ID)
    {
        uiManager.attackFields.SetActive(false);
        uiManager.defenceFields.SetActive(true);
        //MixController.instance.MixCard(cardID_1, cardID_2, ID);
        StartCoroutine(mixController.MixCard(kind_1, cardID_1, kind_2, cardID_2, ID));
    }


    [PunRPC]
    void CreateDefenceCard(int kind, int cardID, int ID)
    {
        GameObject card;

        if (ID == 1)
        {
            //CreateCard(cardID, playerFieldTransform);
            card = Instantiate(cardPrefab, playerFieldTransform, false);
            card.GetComponent<CardController>().Init(kind, cardID, false);
        }
        else
        {
            //CreateCard(cardID, enemyFieldTransform);
            card = Instantiate(cardPrefab, enemyFieldTransform, false);
            card.GetComponent<CardController>().Init(kind, cardID, false);
        }
    }


    void CheckHealth()
    {

        //栄養素確認
        if (Mathf.Abs(player.heroRed - player.heroYellow) >= 3 || Mathf.Abs(player.heroRed - player.heroGreen) >= 3
            || Mathf.Abs(player.heroYellow - player.heroGreen) >= 3)
        {
            player.health = 0;

            Debug.Log("栄養不足");

        }
        else if (player.heroRed == player.heroYellow && player.heroYellow == player.heroGreen)
        {

            if (playerID == 1)
            {
                maxHand = defaultMaxHand;
            }
            Debug.Log("状態回復");

            player.health = 2;
        }
        else
        {
            player.health = 1;
        }


        //栄養素確認
        if (Mathf.Abs(enemy.heroRed - enemy.heroYellow) >= 3 || Mathf.Abs(enemy.heroRed - enemy.heroGreen) >= 3
        || Mathf.Abs(enemy.heroYellow - enemy.heroGreen) >= 3)
        {
            enemy.health = 0;

            Debug.Log("栄養不足");
        }
        else if (enemy.heroRed == enemy.heroYellow && enemy.heroYellow == enemy.heroGreen)
        {
            enemy.health = 2;
        }
        else
        {
            enemy.health = 1;
        }

        uiManager.ShowHealth(player.health, enemy.health);

    }

    void CheckPoison()
    {
       
        if (player.health == 2)
        {
            player.isDeadlyPoison = false;
            player.isPoison = false;
        }
        //毒
        else if (player.isDeadlyPoison)
        {
            player.heroHp -= 100;
            Debug.Log("毒ダメージ" + 100);

        }
        else if (player.isPoison)
        {
            player.heroHp -= 50;

            Debug.Log("毒ダメージ" + 50);

        }


        if (enemy.health == 2)
        {
            enemy.isDeadlyPoison = false;
            enemy.isPoison = false;
        }
        //毒
        else if (enemy.isDeadlyPoison)
        {
            enemy.heroHp -= 100;

            Debug.Log("毒ダメージ" + 100);
        }
        else if (enemy.isPoison)
        {
            enemy.heroHp -= 50;

            Debug.Log("毒ダメージ" + 50);

        }


        uiManager.ShowPoison(player.isPoison, player.isDeadlyPoison, enemy.isPoison, enemy.isDeadlyPoison);



    }

    //カードの移動可否切り替え
    public void ChangeDraggable(Transform hand, int kind, bool isDraggable)
    {
        CardController[] handCardList = hand.GetComponentsInChildren<CardController>(); ;


        foreach (CardController card in handCardList)
        {
            if (card.model.kind == kind)
            {
                card.movement.isDraggable = isDraggable;
            }

        }
    }


    //ダメージ予測
    public int CalculateDamage()
    {
        CardController[] attackCardList;
        CardController[] defenceCardList;

        if (attackID == 1)
        {
            attackCardList = playerFieldTransform.GetComponentsInChildren<CardController>();
            defenceCardList = enemyFieldTransform.GetComponentsInChildren<CardController>();
        }
        else
        {
            attackCardList = enemyFieldTransform.GetComponentsInChildren<CardController>();
            defenceCardList = playerFieldTransform.GetComponentsInChildren<CardController>();
        }

        //Debug.Log(defenceCardList[0].model.cal);


        int damage = attackCardList[0].model.cal;


        foreach (CardController card in defenceCardList)
        {
            Debug.Log(damage);
            //Debug.Log(card.model.cal);

            damage -= card.model.cal;

            
        }

        if (damage < 0)
            damage = 0;

        return damage;
    }

    void ChangeCard(CardController card)
    {
        int kind = -1;
        if (card.model.kind == 0)
        {
            kind = 1;
        }
        else
        {
            kind = 0;
        }

        Destroy(card.gameObject);


        int cardID = frequencyController.DecideCardID(kind);

        if (playerID == 1)
        {
            CreateCard(kind, cardID, playerHandTransform);
        }
        else
        {
            CreateCard(kind, cardID, enemyHandTransform);
        }

    }
}
