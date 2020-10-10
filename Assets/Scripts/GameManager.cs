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

    //List<int> deck = new List<int>();
    int[] deck = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };


    //[SerializeField] AI enemyAI;
    [SerializeField] UIManager uiManager;


    [SerializeField]
    //public Transform playerHandTransform, playerFieldTransform, enemyHandTransform, enemyFieldTransform;
    public Transform playerHandTransform;

    //[SerializeField] CardController cardPrefab;


    //Transform basicFieldTransform, additionalFieldTransform;
    Transform field1Transform, field2Transform, resultFieldTransform;


    GameObject reverseObject;


    //public int attackID;
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


    int cardIndex = 0;

    public int damageCal;

    //bool isConfirm1 = false;
    //bool isConfirm2 = false;



    public CardController[] selectCard = new CardController[2] { null, null };

    //シングルトン化（どこからでもアクセスできるようにする）
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        cardPrefab = (GameObject)Resources.Load("Card");

        player = GameObject.Find("Player").GetComponent<GamePlayerManager>();
        enemy = GameObject.Find("Enemy").GetComponent<GamePlayerManager>();

        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        playerHandTransform = GameObject.Find("PlayerHand").transform;
        //playerFieldTransform = GameObject.Find("PlayerField").transform;
        //enemyHandTransform = GameObject.Find("EnemyHand").transform;
        //enemyFieldTransform = GameObject.Find("EnemyField").transform;

        field1Transform = GameObject.Find("Field1").transform;
        field2Transform = GameObject.Find("Field2").transform;
        resultFieldTransform = GameObject.Find("ResultField").transform;

        reverseObject = GameObject.Find("ReverseObject");

        frequencyController = GameObject.Find("FrequencyController").GetComponent<FrequencyController>();

        specialController = GameObject.Find("SpecialController").GetComponent<SpecialController>();

        attackButtons = GameObject.Find("AttackButtons");

        //battleObjects = GameObject.Find("BattleObjects");

        mixController = GameObject.Find("MixController").GetComponent<MixController>();

        playerID = PhotonNetwork.LocalPlayer.ActorNumber;

        defaultMaxHand = maxHand;

        //if (playerID == 2)
        //{
        //    //reverseObject.transform.localScale = new Vector3(-1, -1, 1);
        //    Vector3 tmp = playerFieldTransform.position;
        //    playerFieldTransform.position = enemyFieldTransform.position;
        //    enemyFieldTransform.transform.position = tmp;

        //    tmp = playerHandTransform.position;
        //    playerHandTransform.position = enemyHandTransform.position;
        //    enemyHandTransform.transform.position = tmp;

        //    tmp = player.transform.position;
        //    player.transform.position = enemy.transform.position;
        //    enemy.transform.position = tmp;

        //}


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
            //GameObject.Find("EnemyHand").GetComponent<DropPlace>().enabled = false;
            //GameObject.Find("EnemyField").GetComponent<DropPlace>().enabled = false;
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
            //GameObject.Find("PlayerHand").GetComponent<DropPlace>().enabled = false;
            //GameObject.Find("PlayerField").GetComponent<DropPlace>().enabled = false;
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

        ShuffleCard();
        SettingInitHand();

        //SetCardToHand();




        //カード並び替え
        LineUpCard();
        //LineUpCard(enemyHandTransform);

        //TurnCalc();

        //StartTurn();
        uiManager.ShowHealth(player.ishealth, enemy.ishealth);
        uiManager.ShowPoison(player.isPoison, player.poisonCount, enemy.isPoison, enemy.poisonCount);
        uiManager.ShowDark(player.isDark, enemy.isDark);
        uiManager.ShowAttackUp(player.attackUp, enemy.attackUp);
        uiManager.ShowHitUp(player.hitUp, enemy.hitUp);

        uiManager.decideButtonObj.SetActive(false);


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

    //public void Restart()
    //{
    //    //handとFieldのカードを削除
    //    foreach (Transform card in playerHandTransform)
    //    {
    //        Destroy(card.gameObject);
    //    }
    //    foreach (Transform card in playerFieldTransform)
    //    {
    //        Destroy(card.gameObject);
    //    }
    //    foreach (Transform card in enemyHandTransform)
    //    {
    //        Destroy(card.gameObject);
    //    }
    //    foreach (Transform card in enemyFieldTransform)
    //    {
    //        Destroy(card.gameObject);
    //    }

    //    //デッキ生成
    //    //player.deck = new List<int>();
    //    //for (int i = 0; i < 20; i++)
    //    //{
    //    //    int cardID = UnityEngine.Random.Range(1, 7);
    //    //    player.deck.Add(cardID);
    //    //}
    //    //enemy.deck = new List<int>();
    //    //for (int i = 0; i < 20; i++)
    //    //{
    //    //    int cardID = UnityEngine.Random.Range(1, 7);
    //    //    enemy.deck.Add(cardID);
    //    //}

    //    StartGame();
    //}

    public void SettingInitHand()
    {
        for (int i = 0; i < maxHand; i++)
        {

            //if (playerID == 1)
            //    GiveCardToHand(player.deck_cardKind, player.deck_cardID, playerHandTransform);
            //else
            //    GiveCardToHand(enemy.deck_cardKind, enemy.deck_cardID, enemyHandTransform);

            //GiveCardToHand(playerID);

            //if (playerID == 1)
            //{
            //    CreateCard(KIND.INGREDIENT, playerHandTransform);
            //}
            //else
            //{
            //    CreateCard(KIND.INGREDIENT, enemyHandTransform);
            //}

            GiveCardToHand();
            


        }


    }

    //public void GiveCardToHand(List<int> deck_cardKind ,List<int> deck_cardID, Transform hand)
    public void GiveCardToHand()
    {

        int cardID = deck[cardIndex];
        CreateCard(KIND.INGREDIENT, cardID, playerHandTransform);
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

    //void CreateCard(int kind, int cardID, Transform hand)
    void CreateCard(KIND kind, int cardID, Transform target)
    {
        //GameObject Card = PhotonNetwork.Instantiate("Card", new Vector3(0, 0, 0), Quaternion.identity);
        GameObject card = Instantiate(cardPrefab, target, false);


        card.GetComponent<CardController>().Init(kind, cardID);
        //Card.GetComponent<CardController>().Init(cardID, playerID);



    }


    public void OnDecideButton()
    {


        if (isMyTurn)
        {
            uiManager.decideButtonObj.SetActive(false);

            int[] cardID = new int[2] { -1, -1 };

            for (int i = 0; i < 2; i++)
            {
                if (selectCard[i] != null)
                {
                    cardID[i] = selectCard[i].model.cardID;
                    Destroy(selectCard[i].gameObject);
                }
            }

            


            photonView.RPC(nameof(Attack_RPC), RpcTarget.AllViaServer, cardID[0], cardID[1]);

        }
        else
        {
            photonView.RPC(nameof(ChangeTurn), RpcTarget.AllViaServer);

        }

    }

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
    void Attack_RPC(int cardID_1, int cardID_2)
    {
        StartCoroutine(Attack(cardID_1, cardID_2));
    }

    IEnumerator Attack(int cardID_1, int cardID_2)
    {
        uiManager.attackFields.SetActive(true);

        CleanField();

        CreateCard(KIND.INGREDIENT, cardID_1, field1Transform);
        yield return new WaitForSeconds(0.7f);


        if (cardID_2 >= 0)
        {
            CreateCard(KIND.INGREDIENT, cardID_2, field2Transform);
            yield return new WaitForSeconds(0.7f);

            //合成
            CardController card1 = field1Transform.GetChild(0).GetComponent<CardController>();
            CardController card2 = field2Transform.GetChild(0).GetComponent<CardController>();
            int mixCardID = SpecialMix(card1, card2);
            CreateCard(KIND.DISH, mixCardID, resultFieldTransform);

        }
        else
        {
            CreateCard(KIND.INGREDIENT, cardID_1, resultFieldTransform);
            yield return new WaitForSeconds(0.7f);

        }

        BuffEffect();



        CardController attackCard = resultFieldTransform.GetChild(0).GetComponent<CardController>();


        //回避計算
        //0~99
        if (isMyTurn)
        {
            int a = UnityEngine.Random.Range(0, 100);
            if (a >= attackCard.model.hit + player.hitUp)
            {
                //外れる
                //Debug.Log("外れた");
                //damageCal = 0;
                photonView.RPC(nameof(DamageCal), RpcTarget.AllViaServer, false);
            }
            else
            {
                photonView.RPC(nameof(DamageCal), RpcTarget.AllViaServer, true);

            }

        }




        //yield return new WaitForSeconds(1.0f);


        //フィールド掃除
        //CleanField();
        //ChangeTurn();
    }

    [PunRPC]
    void DamageCal(bool isHit)
    {
        //ダメージ計算
        CardController attackCard = resultFieldTransform.GetChild(0).GetComponent<CardController>();
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

        if (isHit)
        {
            //damageCal = attackCard.model.cal + attacker.attackUp * 20;
            damageCal = attackCard.model.cal;
        }
        else
        {
            Debug.Log("外れた");
            damageCal = 0;
        }

        specialController.SpecialEffect(attackCard.model.special, isMyTurn, damageCal);



        defender.hp -= damageCal;
        Debug.Log(damageCal + "kcalのダメージ");

        attacker.red += attackCard.model.red;
        attacker.yellow += attackCard.model.yellow;
        attacker.green += attackCard.model.green;

        //フィールド掃除
        //CleanField();
        //StartCoroutine(CleanField());
        //ChangeTurn();

        if (!isMyTurn)
        {
            uiManager.decideButtonObj.SetActive(true);
        }
    }


    //次のターンへ移る
    [PunRPC]
    public void ChangeTurn()
    {
        CleanField();


        CheckHealth();
        PoisonDamage();

        uiManager.ShowHealth(player.ishealth, enemy.ishealth);
        uiManager.ShowPoison(player.isPoison, player.poisonCount, enemy.isPoison, enemy.poisonCount);
        uiManager.ShowDark(player.isDark, enemy.isDark);
        uiManager.ShowAttackUp(player.attackUp, enemy.attackUp);
        uiManager.ShowHitUp(player.hitUp, enemy.hitUp);

        if (isMyTurn)
        {


            int drawNum = maxHand - playerHandTransform.childCount;


            for (int i = 0; i < drawNum; i++)
            {

                GiveCardToHand();

            }
            LineUpCard();

        }


        isMyTurn = !isMyTurn;
        uiManager.ShowHP();
        uiManager.ShowNutrients();
        uiManager.decideButtonObj.SetActive(false);


        uiManager.attackFields.SetActive(isMyTurn);
        

        
    }


    //[PunRPC]
    public void GenerateFieldCard()
    {
        //Debug.Log("aa");
        CleanField();

        if (selectCard[0] == null)
        {
            uiManager.decideButtonObj.SetActive(false);
            return;
        }

        if (selectCard[0] != null)
        {
            CreateCard(KIND.INGREDIENT, selectCard[0].model.cardID, field1Transform);
            //yield return new WaitForSeconds(0.7f);
        }

        if (selectCard[1] != null)
        {
            CreateCard(KIND.INGREDIENT, selectCard[1].model.cardID, field2Transform);
            //yield return new WaitForSeconds(0.7f);

        }


        if (selectCard[1] != null)
        {
            //if (selectCard[0].model.red > 0 && selectCard[1].model.red > 0)
            //{
            //    uiManager.decideButtonObj.SetActive(false);
            //    return;
            //}
            //if (selectCard[0].model.yellow > 0 && selectCard[1].model.yellow > 0)
            //{
            //    uiManager.decideButtonObj.SetActive(false);
            //    return;
            //}
            //if (selectCard[0].model.green > 0 && selectCard[1].model.green > 0)
            //{
            //    uiManager.decideButtonObj.SetActive(false);
            //    return;
            //}

            if (selectCard[0].model.nutrient == selectCard[1].model.nutrient)
            {
                uiManager.decideButtonObj.SetActive(false);
                return;
            }


            //合成
            int mixCardID = SpecialMix(selectCard[0], selectCard[1]);
            CreateCard(KIND.DISH, mixCardID, resultFieldTransform);
            BuffEffect();

            if (player.ishealth)
            {
                uiManager.decideButtonObj.SetActive(true);
            }
            else
            {
                uiManager.decideButtonObj.SetActive(false);
            }
            //yield return new WaitForSeconds(1.0f);

        }
        else if (selectCard[0] != null)
        {
            //単体
            CreateCard(KIND.INGREDIENT, selectCard[0].model.cardID, resultFieldTransform);
            uiManager.decideButtonObj.SetActive(true);
            //yield return new WaitForSeconds(1.0f);
        }




    }


    void BuffEffect()
    {
        //バフ効果を反映

        if (isMyTurn)
        {
            //Debug.Log("aaa");

            if (player.attackUp != 0 || player.hitUp != 0)
            {
                //Debug.Log("bbb");

                CardController attackCard = resultFieldTransform.GetChild(0).GetComponent<CardController>();
                attackCard.model.cal += player.attackUp * 20;
                attackCard.model.hit += player.hitUp * 20;
                attackCard.view.Refresh(attackCard.model, player.attackUp, player.hitUp);
            }
        }
        else
        {
            if (enemy.attackUp != 0 || enemy.hitUp != 0)
            {
                CardController attackCard = resultFieldTransform.GetChild(0).GetComponent<CardController>();
                attackCard.model.cal += enemy.attackUp * 20;
                attackCard.model.hit += enemy.hitUp * 20;
                attackCard.view.Refresh(attackCard.model, enemy.attackUp, enemy.hitUp);
            }
        }

    }

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

        if (field1Transform.childCount != 0)
        {
            Destroy(field1Transform.GetChild(0).gameObject);
        }
        if (field2Transform.childCount != 0)
        {
            Destroy(field2Transform.GetChild(0).gameObject);
        }
        if (resultFieldTransform.childCount != 0)
        {
            Destroy(resultFieldTransform.GetChild(0).gameObject);
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


    //カードを番号順に並べる
    public void LineUpCard()
    {

        List<GameObject> objList = new List<GameObject>();

        // 子階層のGameObject取得
        var childCount = playerHandTransform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            objList.Add(playerHandTransform.GetChild(i).gameObject);
        }

        //objList.Sort((obj1, obj2) => int.Compare(obj1.model.cardID, obj2.model.cardID));
        objList.Sort((a, b) => a.GetComponent<CardController>().model.cardID - b.GetComponent<CardController>().model.cardID);

        foreach (var obj in objList)
        {
            //obj.SetSiblingIndex(childCount - 1);
            obj.transform.SetSiblingIndex(childCount - 1);

        }


    }

    


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

        for (int i = deck.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            var tmp = deck[i];
            deck[i] = deck[j];
            deck[j] = tmp;
        }

    }


    void CheckHealth()
    {
        //健康
        if (Mathf.Abs(player.red - player.yellow) >= 3 ||
            Mathf.Abs(player.yellow - player.green) >= 3 ||
            Mathf.Abs(player.green - player.red) >= 3)
        {
            player.ishealth = false;
        }
        else
        {
            player.ishealth = true;
        }


        //健康
        if (Mathf.Abs(enemy.red - enemy.yellow) >= 3 ||
            Mathf.Abs(enemy.yellow - enemy.green) >= 3 ||
            Mathf.Abs(enemy.green - enemy.red) >= 3)
        {
            enemy.ishealth = false;
        }
        else
        {
            enemy.ishealth = true;
        }



    }

    void PoisonDamage()
    {
        //自分
        if (player.isPoison)
        {
            player.poisonCount++;
            int poisonDamage = player.poisonCount * 10;
            player.hp -= poisonDamage;
            Debug.Log("毒で" + poisonDamage + "ダメージ");
        }
        else
        {
            player.poisonCount = 0;
        }

        //相手
        if (enemy.isPoison)
        {
            enemy.poisonCount++;
            int poisonDamage = enemy.poisonCount * 10;
            enemy.hp -= poisonDamage;
            Debug.Log("毒で" + poisonDamage + "ダメージ");
        }
        else
        {
            enemy.poisonCount = 0;
        }
    }
}
