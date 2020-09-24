using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GamePlayerManager player;
    public GamePlayerManager enemy;

    //[SerializeField] AI enemyAI;
    [SerializeField] UIManager uiManager;


    [SerializeField]
    public Transform playerHandTransform, playerFieldTransform, enemyHandTransform, enemyFieldTransform;

    [SerializeField] CardController cardPrefab;

    GameObject reverseObject;


    public int attackID;
    public bool isMyTurn;



    public Transform playerHero;


    //時間管理
    int timeCount;

    public int playerID;

    FrequencyController frequencyController;

    //シングルトン化（どこからでもアクセスできるようにする）
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        player = GameObject.Find("PlayerHero").GetComponent<GamePlayerManager>();
        enemy = GameObject.Find("EnemyHero").GetComponent<GamePlayerManager>();

        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        playerHandTransform = GameObject.Find("PlayerHand").transform;
        playerFieldTransform = GameObject.Find("PlayerField").transform;
        enemyHandTransform = GameObject.Find("EnemyHand").transform;
        enemyFieldTransform = GameObject.Find("EnemyField").transform;

        reverseObject = GameObject.Find("ReverseObject");

        frequencyController = GameObject.Find("FrequencyController").GetComponent<FrequencyController>();


        playerID = PhotonNetwork.LocalPlayer.ActorNumber;

        if (playerID == 2)
            reverseObject.transform.localScale = new Vector3(1, -1, 1);

        
        //デッキ生成
        if (playerID == 1)
        {
            player.deck = new List<int>();
            for (int i = 0; i < 20; i++)
            {
                int num = UnityEngine.Random.Range(0, frequencyController.sum);
                
                player.deck.Add(frequencyController.DecideCard(num));
            }
        }
        else
        {
            enemy.deck = new List<int>();
            for (int i = 0; i < 20; i++)
            {
                int num = UnityEngine.Random.Range(0, frequencyController.sum);

                enemy.deck.Add(frequencyController.DecideCard(num));

            }
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
        player.Init(player.deck);
        enemy.Init(enemy.deck);


        //プレイヤー１が先行
        attackID = 1;
        if (playerID == 1)
        {
            isMyTurn = true;
        }
        else
        {
            isMyTurn = false;
        }


        uiManager.ShowTurn(attackID);

        uiManager.ShowHeroHP(player.heroHp, enemy.heroHp);
        uiManager.ShowMixCost(player.mixCost, enemy.mixCost);

        SettingInitHand();

        //SetCardToHand();


        
        //TurnCalc();
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
        player.deck = new List<int>();
        for (int i = 0; i < 20; i++)
        {
            int cardID = UnityEngine.Random.Range(1, 7);
            player.deck.Add(cardID);
        }
        enemy.deck = new List<int>();
        for (int i = 0; i < 20; i++)
        {
            int cardID = UnityEngine.Random.Range(1, 7);
            enemy.deck.Add(cardID);
        }

        StartGame();
    }

    void SettingInitHand()
    {
        //カードをそれぞれに5枚配る
        for (int i = 0; i < 5; i++)
        {

            if (playerID == 1)
                GiveCardToHand(player.deck, playerHandTransform);
            else
                GiveCardToHand(enemy.deck, enemyHandTransform);



        }


    }

    void GiveCardToHand(List<int> deck, Transform hand)
    {
        if (deck.Count == 0)
        {
            return;
        }
        int cardID = deck[0];

        deck.RemoveAt(0);
        CreateCard(cardID, hand);


    }

    void CreateCard(int cardID, Transform hand)
    {
        GameObject Card = PhotonNetwork.Instantiate("Card", new Vector3(0, 0, 0), Quaternion.identity);

        Card.GetComponent<CardController>().Init(cardID, playerID);



    }


    public void OnClickTurnEndButton()
    {
        if (!isMyTurn)
            return;


        CardController[] fieldCardList;

        if (playerID == 1)
            fieldCardList = playerFieldTransform.GetComponentsInChildren<CardController>();
        else
            fieldCardList = enemyFieldTransform.GetComponentsInChildren<CardController>();

        //カードを表にする
        foreach (CardController card in fieldCardList)
        {
            card.Show();
        }

        if (fieldCardList.Length == 2)
        {
            //合成
            //通常合成
            photonView.RPC(nameof(Mix), RpcTarget.All, playerID);

        }
        else if (fieldCardList.Length == 1)
        {
            //攻撃カードなら

            //特殊カードなら
            if (fieldCardList[0].model.kind == KIND.SPELL)
            {
                fieldCardList[0].Spell(playerID, fieldCardList[0].model.spellNum);
                photonView.RPC(nameof(ChangeTurn), RpcTarget.All);
                return;

            }


        }
        //カードを出さなかった時
        else
        {
            if (attackID == playerID)
            {
                photonView.RPC(nameof(ChangeTurn), RpcTarget.All);
                return;
            }

        }


        //攻撃側がターンエンドを押した時
        if (attackID == playerID)
        {
            photonView.RPC(nameof(StartDefence), RpcTarget.All);

        }

        else
        {
            photonView.RPC(nameof(Battle), RpcTarget.All);

            photonView.RPC(nameof(ChangeTurn), RpcTarget.All);

        }
    }




    [PunRPC]
    void Mix(int ID)
    {
        Debug.Log("合成");

        CardController[] fieldCardList;

        if (ID == 1)
        {
            fieldCardList = playerFieldTransform.GetComponentsInChildren<CardController>();
            player.mixCost -= 2;
        }
        else
        {
            fieldCardList = enemyFieldTransform.GetComponentsInChildren<CardController>();
            enemy.mixCost -= 2;
        }

        //合成
        //通常合成
        fieldCardList[0].model.at += fieldCardList[1].model.at;
        fieldCardList[0].model.de += fieldCardList[1].model.de;

        Destroy(fieldCardList[1].gameObject);
        fieldCardList[0].view.Refresh(fieldCardList[0].model);

    }


    //攻撃を実行する
    [PunRPC]
    void Battle()
    {
        Debug.Log("バトル");

        CardController[] playerFieldCardList = playerFieldTransform.GetComponentsInChildren<CardController>();
        CardController[] enemyFieldCardList = enemyFieldTransform.GetComponentsInChildren<CardController>();

        int damage = 0;

        //Debug.Log(enemyFieldCardList);

        if (attackID == 1)
        {
            if (enemyFieldCardList.Length != 0)
                damage = playerFieldCardList[0].model.at - enemyFieldCardList[0].model.de;
            else
                damage = playerFieldCardList[0].model.at;

            if (damage < 0)
                damage = 0;

            enemy.heroHp -= damage;
        }
        else
        {
            if (playerFieldCardList.Length != 0)
                damage = enemyFieldCardList[0].model.at - playerFieldCardList[0].model.de;
            else
                damage = enemyFieldCardList[0].model.at;

            if (damage < 0)
                damage = 0;


            player.heroHp -= damage;

        }


        Debug.Log("プレイヤー" + attackID + "の攻撃");
        Debug.Log(damage + "ダメージ");





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
    }


    //void TurnCalc()
    //{
    //    StopAllCoroutines();
    //    StartCoroutine(CountDown());

    //    if (isMyTurn)
    //    {
    //        PlayerTurn();
    //    }
    //    else
    //    {
    //        //StartCoroutine(enemyAI.EnemyTurn());
    //    }
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






    //攻撃側の開始
    //[PunRPC]
    //void StartAttack()
    //{
    //    isMyTurn = !isMyTurn;

    //    if (isMyTurn)
    //    {
    //        if (playerID == 1)
    //            GiveCardToHand(player.deck, playerHandTransform);
    //        else
    //            GiveCardToHand(enemy.deck, enemyHandTransform);
    //    }
    //}

    //防御側の開始
    [PunRPC]
    void StartDefence()
    {
        isMyTurn = !isMyTurn;

        uiManager.ShowTurn(attackID);


        //if (isMyTurn)
        //{
        //    if (playerID == 1)
        //        GiveCardToHand(player.deck, playerHandTransform);
        //    else
        //        GiveCardToHand(enemy.deck, enemyHandTransform);
        //}
    }

    //次のターンへ移る
    [PunRPC]
    public void ChangeTurn()
    {

        //フィールドのカード消去
        StartCoroutine(CleanField());


        //手札補充
        if (playerID == 1)
        {
            while (playerHandTransform.childCount < 5)
            {
                GiveCardToHand(player.deck, playerHandTransform);
            }
        }
        else
        {
            while (enemyHandTransform.childCount < 5)
            {
                GiveCardToHand(enemy.deck, enemyHandTransform);
            }
        }



        


        if (attackID == 1)
            attackID = 2;
        else if (attackID == 2)
            attackID = 1;

        if (attackID == playerID)
            isMyTurn = true;
        else
            isMyTurn = false;

        //Debug.Log("プレイヤー" + attackID + "の攻撃");

        uiManager.ShowTurn(attackID);

        //isMyTurn = !isMyTurn;

        CardController[] playerFieldCardList = playerFieldTransform.GetComponentsInChildren<CardController>();
        //SettingCanAttackView(playerFieldCardList, false);
        CardController[] enemyFieldCardList = enemyFieldTransform.GetComponentsInChildren<CardController>();
        //SettingCanAttackView(enemyFieldCardList, false);


        //if (isMyTurn)
        //{

        //}

        player.mixCost++;
        enemy.mixCost++;
        uiManager.ShowHeroHP(player.heroHp, enemy.heroHp);
        uiManager.ShowMixCost(player.mixCost, enemy.mixCost);
        //TurnCalc();
    }

    //public void SettingCanAttackView(CardController[] fieldCardList, bool canAttack)
    //{
    //    foreach (CardController card in fieldCardList)
    //    {
    //        //カードを攻撃可能にする
    //        card.SetCanAttack(canAttack);
    //    }
    //}

    //void PlayerTurn()
    //{
    //    Debug.Log("プレイヤーのターン");
    //    //フィールドのカードを攻撃可能にする
    //    CardController[] playerFieldCardList = playerFieldTransform.GetComponentsInChildren<CardController>();
    //    //SettingCanAttackView(playerFieldCardList, true);
    //}

    

    //public void CardsBattle(CardController attacker, CardController defender)
    //{
    //    Debug.Log("CardsBattle");
    //    Debug.Log("attacker HP" + attacker.model.hp);
    //    Debug.Log("defender HP" + defender.model.hp);

    //    attacker.Attack(defender);
    //    defender.Attack(attacker);

    //    Debug.Log("attacker HP" + attacker.model.hp);
    //    Debug.Log("defender HP" + defender.model.hp);

    //    //attacker.CheckAlive();
    //    //defender.CheckAlive();
    //}




    public void AttackToHero(CardController attacker, bool isPlayerCard)
    {
        if (isPlayerCard)
        {
            enemy.heroHp -= attacker.model.at;
        }
        else
        {
            player.heroHp -= attacker.model.at;
        }
        attacker.SetCanAttack(false);
        uiManager.ShowHeroHP(player.heroHp, enemy.heroHp);
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

    //[PunRPC]
    //public void CardMoveShare_RPC(GameObject CardPrefab, Transform field)
    //{
    //    //場に出すカードを選択
    //    CardController card = CardPrefab.GetComponent<CardController>();
    //    //カードを移動
    //    StartCoroutine(card.movement.MoveToField(field));

    //}

    //public void CardMoveShare(GameObject CardPrefab , GameObject Field)
    //{
    //    photonView.RPC(nameof(ChangeTurn), RpcTarget.Others, CardPrefab, Field.transform);

    //}


}
