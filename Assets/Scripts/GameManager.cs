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


    public bool isChangeCard = false;


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


        attackButtons = GameObject.Find("AttackButtons");

        //battleObjects = GameObject.Find("BattleObjects");

        mixController = GameObject.Find("MixController").GetComponent<MixController>();

        playerID = PhotonNetwork.LocalPlayer.ActorNumber;


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
            for (int i = 0; i < 20; i++)
            {
                //int num = UnityEngine.Random.Range(0, frequencyController.sum);

                (int kind, int cardID) = frequencyController.DecideCard();

                //player.deck.Add(frequencyController.DecideCard(num));
                player.deck_cardKind.Add(kind);
                player.deck_cardID.Add(cardID);
            }

            //相手のDropPlaceを消す
            GameObject.Find("EnemyHand").GetComponent<DropPlace>().enabled = false;
            GameObject.Find("EnemyField").GetComponent<DropPlace>().enabled = false;
        }
        else
        {
            //enemy.deck = new List<int>();
            for (int i = 0; i < 20; i++)
            {
                //int num = UnityEngine.Random.Range(0, frequencyController.sum);

                //(int kind, int cardID) = frequencyController.DecideCard(num);
                //enemy.deck[i].Add(kind);
                //enemy.deck[i].Add(cardID);

                //enemy.deck.Add(frequencyController.DecideCard(num));

                (int kind, int cardID) = frequencyController.DecideCard();

                //player.deck.Add(frequencyController.DecideCard(num));
                enemy.deck_cardKind.Add(kind);
                enemy.deck_cardID.Add(cardID);

            }


            //相手のDropPlaceを消す
            GameObject.Find("PlayerHand").GetComponent<DropPlace>().enabled = false;
            GameObject.Find("PlayerField").GetComponent<DropPlace>().enabled = false;
        }



    }

    void Start()
    {


        if (playerID == 2)
        {
            photonView.RPC(nameof(StartGame), RpcTarget.All);



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
        uiManager.ShowHeroCal(player.cal, enemy.cal);


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

    void SettingInitHand()
    {
        //カードをそれぞれに5枚配る
        for (int i = 0; i < 6; i++)
        {

            if (playerID == 1)
                GiveCardToHand(player.deck_cardKind, player.deck_cardID, playerHandTransform);
            else
                GiveCardToHand(enemy.deck_cardKind, enemy.deck_cardID, enemyHandTransform);



        }


    }

    void GiveCardToHand(List<int> deck_cardKind ,List<int> deck_cardID, Transform hand)
    {
        if (deck_cardKind.Count == 0)
        {
            return;
        }
        int kind = deck_cardKind[0];
        int cardID = deck_cardID[0];

        deck_cardKind.RemoveAt(0);
        deck_cardID.RemoveAt(0);

        CreateCard(kind, cardID, hand);


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
        //CardController[] fieldCardList;

        //if (playerID == 1)
        //    fieldCardList = playerFieldTransform.GetComponentsInChildren<CardController>();
        //else
        //    fieldCardList = enemyFieldTransform.GetComponentsInChildren<CardController>();

        //カードを移動を共有して表にする
        //foreach (CardController card in fieldCardList)
        //{
        //    card.MoveToField();
        //    card.Show();
        //}



        if (playerID == attackID)
        {
            uiManager.changeCardButtonObj.SetActive(false);


            if (isChangeCard)
            {
                Transform hand;
                if (playerID == 1)
                    hand = playerHandTransform;
                else
                    hand = enemyHandTransform;

                uiManager.OnChangeCardButton();

                //カード交換
                int disKind = basicFieldTransform.GetChild(0).GetComponent<CardController>().model.kind;
                (int kind, int cardID) = frequencyController.ChangeCard(disKind);
                CreateCard(kind, cardID, hand);
                Destroy(basicFieldTransform.GetChild(0).gameObject);

                photonView.RPC(nameof(ChangeTurn), RpcTarget.All);
                return;
            }


            CheckField();

        }
        else
        {
            //CardController[] fieldCardList;

            //相手側に防御側のカードの作成
            //if (playerID == 1)
            //{
            //    fieldCardList = playerFieldTransform.GetComponentsInChildren<CardController>();

            //}
            //else
            //{
            //    fieldCardList = enemyFieldTransform.GetComponentsInChildren<CardController>();

            //}



            //foreach (CardController card in fieldCardList)
            //{
            //    photonView.RPC(nameof(CreateDefenceCard), RpcTarget.Others, card.model.kind, card.model.cardID, playerID);

            //}

            CheckField();

            photonView.RPC(nameof(Battle), RpcTarget.All);
            //photonView.RPC(nameof(ChangeTurn), RpcTarget.All);
        }

       


  


    }





    //攻撃を実行する
    [PunRPC]
    void Battle()
    {
        uiManager.mixFields.SetActive(false);

        Debug.Log("バトル");

        //CardController[] playerFieldCardList = playerFieldTransform.GetComponentsInChildren<CardController>();
        //CardController[] enemyFieldCardList = enemyFieldTransform.GetComponentsInChildren<CardController>();

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


        int damage = attackCardList[0].model.at;

        Debug.Log(damage);

        //摂取カロリー
        int attackCal = attackCardList[0].model.cal;
        int defenceCal = 0;



        foreach (CardController card in defenceCardList)
        {
            defenceCal += card.model.cal;
            damage -= card.model.de;

            Debug.Log(card.model.de);
        }


        if (damage < 0)
            damage = 0;

        if (attackID == 1)
            enemy.heroHp -= damage;
        else
            player.heroHp -= damage;

        Debug.Log("プレイヤー" + attackID + "の攻撃");
        Debug.Log(damage + "ダメージ");


        //カロリー
        if (attackID == 1)
        {
            ChangeHeroCal(1, attackCal);
            ChangeHeroCal(2, defenceCal);
        }
        else
        {
            ChangeHeroCal(2, attackCal);
            ChangeHeroCal(1, defenceCal);
        }


        //一旦ここにかく
        ChangeTurn();


    }


    IEnumerator CleanField()
    {
        yield return new WaitForSeconds(2);


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


        //合成フィールド掃除
        mixController.CleanField();

        //一旦ここにかく
        StartAttack();
    }

   
       

    //攻撃側の開始
    void StartAttack()
    {
        //Debug.Log("攻撃開始");



        CardController[] handCardList;

        if (playerID == 1)
            handCardList = playerHandTransform.GetComponentsInChildren<CardController>();
        else
            handCardList = enemyHandTransform.GetComponentsInChildren<CardController>();

        if (playerID == attackID)
        {
            isMyTurn = true;



            foreach (CardController card in handCardList)
            {
                //if (card.model.kind == 0 || card.model.kind == 2)
                //{
                //    card.movement.isDraggable = true;
                //}
                //else
                //{
                //    card.movement.isDraggable = false;
                //}
                card.movement.isDraggable = true;


            }

            uiManager.mixFields.SetActive(true);
            uiManager.defenceFields.SetActive(false);

            //uiManager.buttonsObj.SetActive(true);
            uiManager.ShowButtonObj(true);

            uiManager.changeCardButtonObj.SetActive(true);


        }
        else
        {
            isMyTurn = false;

            foreach (CardController card in handCardList)
            {

                card.movement.isDraggable = false;
            
            }

            //uiManager.buttonsObj.SetActive(false);

            isAttackButton = false;
            isMixButton = false;
            isThrowButton = false;


            uiManager.mixFields.SetActive(false);
            uiManager.defenceFields.SetActive(false);

            uiManager.changeCardButtonObj.SetActive(false);

        }

        //決定ボタン消す
        uiManager.decideButtonObj.SetActive(false);
        uiManager.ShowTurn(attackID);


    }

    //防御側の開始
    [PunRPC]
    public void StartDefence()
    {
        //フィールドのImageを消す
        //uiManager.ShowFieldImage(true);

        //battleObjects.SetActive(true);

        CardController[] handCardList;

        if (playerID == 1)
            handCardList = playerHandTransform.GetComponentsInChildren<CardController>();
        else
            handCardList = enemyHandTransform.GetComponentsInChildren<CardController>();

        if (playerID != attackID)
        {
            isMyTurn = true;



            //自分のフィールドだけ表示
            ////if (playerID == 1)
            //    uiManager.playerFieldImage.enabled = true;
            ////else
            //    uiManager.enemyFieldImage.enabled = true;



            foreach (CardController card in handCardList)
            {
                if (card.model.kind == 0)
                {
                    card.movement.isDraggable = true;
                }
                else
                {
                    card.movement.isDraggable = false;

                }

            }

            //決定ボタン表示
            uiManager.decideButtonObj.SetActive(true);

            uiManager.mixFields.SetActive(true);
            uiManager.defenceFields.SetActive(true);

            
        }
        else
        {
            isMyTurn = false;

            foreach (CardController card in handCardList)
            {

                card.movement.isDraggable = false;

            }

            uiManager.mixFields.SetActive(false);
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

    //防御側の開始
    //[PunRPC]
    //void StartDefence()
    //{
    //    isMyTurn = !isMyTurn;

    //    uiManager.ShowTurn(attackID);

    //}

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
        photonView.RPC(nameof(ChangeTurn), RpcTarget.All);

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
        //CleanField();

        //手札補充
        if (playerID == 1)
        {
            while (playerHandTransform.childCount < 6)
            {
                GiveCardToHand(player.deck_cardKind, player.deck_cardID, playerHandTransform);
            }
        }
        else
        {
            while (enemyHandTransform.childCount < 6)
            {
                GiveCardToHand(enemy.deck_cardKind, enemy.deck_cardID, enemyHandTransform);
            }
        }


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

        uiManager.ShowHeroCal(player.cal, enemy.cal);

      

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
        //フィールドのカードを確認
        if (basicFieldTransform.childCount != 0)
        {
            if (additionalFieldTransform.childCount == 0)
            {
                //合成なし
                CardController basicCardController = basicFieldTransform.GetChild(0).GetComponent<CardController>();

                MixController.instance.CleanField();

                photonView.RPC(nameof(SingleCard), RpcTarget.All, basicCardController.model.kind, basicCardController.model.cardID, playerID);


            }
            else
            {
                //合成
                //int basicCardKind = basicFieldTransform.GetChild(0).GetComponent<CardController>().model.cardKind;
                CardController basicCardController = basicFieldTransform.GetChild(0).GetComponent<CardController>();
                //int additionalCardKind = additionalFieldTransform.GetChild(0).GetComponent<CardController>().model.cardKind;
                //int additionalCardID = additionalFieldTransform.GetChild(0).GetComponent<CardController>().model.cardID;
                CardController additionalCardController = additionalFieldTransform.GetChild(0).GetComponent<CardController>();


                MixController.instance.CleanField();

                photonView.RPC(nameof(MixCard), RpcTarget.All, basicCardController.model.kind, basicCardController.model.cardID, additionalCardController.model.kind, additionalCardController.model.cardID, playerID);

            }
        }

        uiManager.ShowHeroHP(player.heroHp, enemy.heroHp);

        uiManager.ShowHeroCal(player.cal, enemy.cal);



        //一旦ここにかく
        //if (playerID != attackID)
        //{
        //    photonView.RPC(nameof(Battle), RpcTarget.All);

        //}


    }


    [PunRPC]
    void SingleCard(int kind, int cardID, int ID)
    {
        uiManager.mixFields.SetActive(false);
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
        else if (cardController.model.kind == 1)
        {
            Excercise(ID, cardController);
            ChangeTurn();
        }
        else if (cardController.model.kind == 2)
        {
            //スペルの場合は防御なしで次のターン
            cardController.Spell(ID, cardController.model.spellNum);
            ChangeTurn();
        }
    }

    [PunRPC]
    void MixCard(int kind_1, int cardID_1, int kind_2, int cardID_2, int ID)
    {
        uiManager.mixFields.SetActive(false);
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


    void ChangeHeroCal(int ID, int cal)
    {
        if (ID == 1)
            player.cal += cal;
        else
            enemy.cal += cal;
    }


    void Excercise(int ID, CardController card)
    {
        if (ID == 1)
        {
            player.cal -= card.model.cal;
            if (player.cal < 0)
                player.cal = 0;
           
        }
        else
        {
            enemy.cal -= card.model.cal;

            if (enemy.cal < 0)
                enemy.cal = 0;
        }
            
    }

}
