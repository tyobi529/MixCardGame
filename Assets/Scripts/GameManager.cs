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

   
    int[] deck_0 = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
    int[] deck_1 = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };


    //[SerializeField] AI enemyAI;
    [SerializeField] UIManager uiManager;


    //public Transform playerHandTransform, playerFieldTransform, enemyHandTransform, enemyFieldTransform;
    //public Transform[,] conveyorTransform = new Transform[2, 4];
    Transform[] nextFieldTransform = new Transform[4];
    Transform[] currentFieldTransform = new Transform[4];


    Transform[] mixFieldTransform = new Transform[3];


    //public int attackID;
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

   

    int[] selectCardPosition = new int[2] { -1, -1 };


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

        for (int i = 0; i < maxHand; i++)
        {
            nextFieldTransform[i] = fields[0].transform.GetChild(i).transform;
        }

        for (int i = 0; i < maxHand; i++)
        {
            currentFieldTransform[i] = fields[1].transform.GetChild(i).transform;
        }




        for (int i = 0; i < 3; i++)
        {
            mixFieldTransform[i] = GameObject.Find("MixField_" + i.ToString()).transform;
        }



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
        uiManager.ShowNutrients();



        
        if (playerID == 1)
        {

            GiveCardToHand();

            photonView.RPC(nameof(MoveConveyor), RpcTarget.AllViaServer);

            GiveCardToHand();



        }

        uiManager.ShowStatus();

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
                currentFieldTransform[selectCardPosition[i]].GetComponent<FieldController>().CancelCard();
                Destroy(currentFieldTransform[selectCardPosition[i]].GetChild(0).gameObject);
            }
        }

    }




    public void GiveCardToHand()
    {


        for (int i = 0; i < maxHand; i++)
        {
            if (nextFieldTransform[i].childCount == 0)
            {
                int cardID = UnityEngine.Random.Range(0, 9);
                int cal = UnityEngine.Random.Range(20, 60);
                int specialID = UnityEngine.Random.Range(0, 5);
                int position = i;
                photonView.RPC(nameof(CreateHandCard), RpcTarget.AllViaServer, cardID, cal, specialID, position);

            }
        }

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


    [PunRPC]
    void CreateHandCard(int cardID, int cal, int specialID, int position)
    {
        //GameObject Card = PhotonNetwork.Instantiate("Card", new Vector3(0, 0, 0), Quaternion.identity);
        Transform field = nextFieldTransform[position];

        GameObject card = Instantiate(cardPrefab, field, false);


        card.GetComponent<CardController>().Init(KIND.INGREDIENT, cardID, cal, specialID);
        //Card.GetComponent<CardController>().Init(cardID, playerID);

        //return card.GetComponent<CardController>();

    }

    void CreateMixCard(KIND kind, int cardID, int cal, int specialID)
    {
        GameObject card = Instantiate(dishCardPrefab, mixFieldTransform[2], false);

        card.GetComponent<CardController>().MixInit(kind, cardID, cal, specialID);

    }




    public void OnDecideButton()
    {


        if (isMyTurn)
        {
            uiManager.decideButtonObj.SetActive(false);



            photonView.RPC(nameof(GenerateFieldCard), RpcTarget.AllViaServer, selectCardPosition);

            ////使ったカードの削除
            photonView.RPC(nameof(DestroyIngredient), RpcTarget.AllViaServer, selectCardPosition);



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
                currentFieldTransform[selectCardPosition[1]].GetComponent<FieldController>().CancelCard();
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



    [PunRPC]
    void Attack_RPC(int[] cardID)
    {
        StartCoroutine(Attack(cardID));
    }

    IEnumerator Attack(int[] cardID)
    {

        yield return new WaitForSeconds(0.7f);


    }

    //[PunRPC]
    //受ける側で計算
    void DamageCalculation()
    {

        CardController attackCardController = mixFieldTransform[2].GetChild(0).GetComponent<CardController>();



        //bool isHit = true;

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
        //isHit = true;


        photonView.RPC(nameof(Battle), RpcTarget.AllViaServer, damageCal);

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
    void Battle(int damageCal)
    {
        //CardController attackCard = mixFieldTransform[2].GetChild(0).GetComponent<CardController>();
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

        //バフ効果
        if (attacker.attackUpCount > 0)
        {
            damageCal *= attacker.attackUpCount;
        }

        if (defender.defenceUpCount > 0)
        {
            damageCal /= defender.defenceUpCount;
        }

  

        defender.hp -= damageCal;


        

        if (attackCardController.model.kind == KIND.INGREDIENT)
        {
            attacker.cost += currentFieldTransform[selectCardPosition[0]].GetComponent<FieldController>().cost;
            specialController.IngredientEffect(attackCardController.model.specialID, isMyTurn, damageCal);
        }
        else
        {
            attacker.cost -= mixCost;
            specialController.DishEffect(attackCardController.model.specialID, isMyTurn, damageCal);
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

        uiManager.ShowTriangle();




        isMyTurn = !isMyTurn;



        MoveConveyor();

        uiManager.ShowStatus();

        selectCardPosition[0] = -1;
        selectCardPosition[1] = -1;

        if (isMyTurn)
        {

            GiveCardToHand();


        }
        else
        {

        }

        //uiManager.ShowHP();
        uiManager.ShowNutrients();
        uiManager.decideButtonObj.SetActive(false);


        uiManager.attackFields.SetActive(isMyTurn);
        

        
    }


    [PunRPC]
    public void GenerateFieldCard(int[] selectCardPosition)
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

        this.selectCardPosition = selectCardPosition;

        uiManager.attackFields.SetActive(true);


        CleanField();

        int[] cardID = new int[2] { -1, -1 };
        int[] cal = new int[2] { -1, -1 };
        int[] specialID = new int[2] { -1, -1 };
        CardController[] cardController = new CardController[2] { null, null };


        for (int i = 0; i < 2; i++)
        {
            if (selectCardPosition[i] != -1)
            {
                GameObject selectCard = currentFieldTransform[selectCardPosition[i]].GetChild(0).gameObject;
                cardID[i] = selectCard.GetComponent<CardController>().model.cardID;
                cal[i] = selectCard.GetComponent<CardController>().model.cal;
                specialID[i] = selectCard.GetComponent<CardController>().model.specialID;
                

                GameObject card = GameObject.Instantiate(selectCard, mixFieldTransform[i], false);
                cardController[i] = card.GetComponent<CardController>();
                cardController[i].Init(KIND.INGREDIENT, cardID[i], cal[i], specialID[i]);
                //cardController[i].model.cost = selectCard.GetComponent<CardController>().model.cost;

                //Debug.Log("カード" + i + cardController[i].model.cost);
            }
        }

     


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

                CreateMixCard(KIND.DISH, mixCardID, 0, mixCardID);

                if (attacker.cost >= mixCost)
                {
                    uiManager.decideButtonObj.SetActive(true);
                }
                
            }
        }
        else
        {
            uiManager.decideButtonObj.SetActive(true);
            CreateMixCard(KIND.INGREDIENT, cardID[0], cal[0], specialID[0]);


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

    }



    [PunRPC]
    void MoveConveyor()
    {


        for (int i = 0; i < maxHand; i++)
        {
            FieldController fieldController = currentFieldTransform[i].GetComponent<FieldController>();

            //コスト減少
            if (currentFieldTransform[i].childCount != 0)
            {
                fieldController.deadLine--;

                if (fieldController.deadLine == 0)
                {
                    Destroy(fieldController.gameObject.transform.GetChild(0).gameObject);


                    CardController cardController = nextFieldTransform[i].GetChild(0).GetComponent<CardController>();
                    cardController.transform.SetParent(currentFieldTransform[i], false);

                    fieldController.deadLine = deadLine;
                    fieldController.cost = 0;
                 
                }
                else
                {

                    fieldController.cost = (deadLine - currentFieldTransform[i].GetComponent<FieldController>().deadLine) / 2;

                }
            }



            //新しい食材
            else if (currentFieldTransform[i].childCount == 0)
            {
                CardController cardController = nextFieldTransform[i].GetChild(0).GetComponent<CardController>();
                cardController.transform.SetParent(currentFieldTransform[i], false);


                fieldController.deadLine = deadLine;
                fieldController.cost = 0;

            }


        }

        uiManager.ShowDeadLine();



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
