using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixController : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;

    [SerializeField] Transform playerFieldTransform;
    [SerializeField] Transform enemyFieldTransform;

    [SerializeField] Transform basicFieldTransform;
    [SerializeField] Transform additionalFieldTransform;
    [SerializeField] Transform resultFieldTransform;

    [SerializeField] UIManager uiManager;


    [SerializeField] GamePlayerManager player;
    [SerializeField] GamePlayerManager enemy;


    //シングルトン
    public static MixController instance;


    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }



    //合成結果の予測
    public void ExpectMix()
    {
        //if (GameManager.instance.attackID != GameManager)
        if (resultFieldTransform.childCount != 0)
        {
            foreach (Transform card in resultFieldTransform)
            {
                Destroy(card.gameObject);
            }
            //Destroy(resultFieldTransform.GetChild(0).gameObject);

        }

        //警告を消す
        uiManager.lackCostText.SetActive(false);

        if (basicFieldTransform.childCount == 0)
        {
            if (GameManager.instance.attackID == GameManager.instance.playerID)
            {
                //決定ボタンを消す
                uiManager.decideButtonObj.SetActive(false);
            }
            else
            {
                //決定ボタンを出す
                uiManager.decideButtonObj.SetActive(true);
            }
            return;

        }

        //決定ボタンを出す
        uiManager.decideButtonObj.SetActive(true);


        GameObject basicCard = basicFieldTransform.GetChild(0).gameObject;
        CardController basicCardController = basicCard.GetComponent<CardController>();

        GameObject additionalCard;
        CardController additionalCardController;


        //合成なし
        if (additionalFieldTransform.childCount == 0)
        {
            GameObject cloneCard = Instantiate(basicCard, resultFieldTransform, false);
            cloneCard.GetComponent<CardController>().movement.isDraggable = false;
            //cloneCard.GetComponent<CardController>().Init(basicCard.GetComponent<CardModel>().cardID, basicCard.GetComponent<CardModel>().playerID, true);
            //return;

            //カロリー確認
            if (CheckCal())
            {

            }
            else
            {
                uiManager.decideButtonObj.SetActive(false);

                uiManager.lackCostText.SetActive(true);

                return;
            }
        }

        //合成
        else
        {

            //Debug.Log("通常合成");
            additionalCard = additionalFieldTransform.GetChild(0).gameObject;
            additionalCardController = additionalCard.GetComponent<CardController>();


            //合成
            int specialMixID = SpecialMix(basicCardController, additionalCardController);
            if (specialMixID >= 0)
            {
                GameObject specialCard = Instantiate(cardPrefab, resultFieldTransform, false);
                CardController specialCardController = specialCard.GetComponent<CardController>();
                specialCardController.Init(3, specialMixID, true);


                //カロリー０、攻撃防御は和にする
                specialCardController.model.at = basicCardController.model.at + additionalCardController.model.at;
                specialCardController.model.cal = 0;
                specialCardController.view.Refresh(specialCardController.model);
                return;
            }
            else
            {
                GameObject cloneCard = Instantiate(basicCard, resultFieldTransform, false);
                CardController cloneCardController = cloneCard.GetComponent<CardController>();

                cloneCardController.Init(basicCardController.model.kind, basicCardController.model.cardID, true);

                //Debug.Log(cloneCard.GetComponent<CardController>().model);
                cloneCardController.model.cal += additionalCardController.model.cal;
                cloneCardController.model.at += additionalCardController.model.at;
                cloneCardController.model.de += additionalCardController.model.de;
                cloneCardController.movement.isDraggable = false;
                cloneCardController.view.Refresh(cloneCardController.model);
            }


            //カロリー確認
            if (CheckCal())
            {

            }
            else
            {
                uiManager.decideButtonObj.SetActive(false);

                uiManager.lackCostText.SetActive(true);

                //return;
            }

            //コスト確認
            //if (GameManager.instance.playerID == 1 && GameManager.instance.player.mixCost < player.mixCost ||
            //    GameManager.instance.playerID == 2 && GameManager.instance.enemy.mixCost < enemy.mixCost)
            //{
            //    uiManager.decideButtonObj.SetActive(false);

            //    uiManager.lackCostText.SetActive(true);
            //}

        }







    }


    bool CheckCal()
    {
        int cal = 0;
        GameObject basicCard = basicFieldTransform.GetChild(0).gameObject;
        CardController basicCardController = basicCard.GetComponent<CardController>();

        cal += basicCardController.model.cal;



        if (additionalFieldTransform.childCount != 0)
        {           
            GameObject additionalCard = additionalFieldTransform.GetChild(0).gameObject;
            CardController additionalCardController = additionalCard.GetComponent<CardController>();
            cal += additionalCardController.model.cal;
        }

        int currentCal = 0;

        if (GameManager.instance.playerID == 1)
        {
            currentCal = player.cal;
        }
        else
        {
            currentCal = enemy.cal;
        }

        if (currentCal + cal <= 1000)
        {
            return true;
        }
        else
        {
            return false;
        }


    }


    public void CleanField()
    {
        CardController[] basicFieldCardList = basicFieldTransform.GetComponentsInChildren<CardController>();
        CardController[] additionalFieldCardList = additionalFieldTransform.GetComponentsInChildren<CardController>();
        CardController[] resultFieldCardList = resultFieldTransform.GetComponentsInChildren<CardController>();

        foreach (Transform card in basicFieldTransform)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in additionalFieldTransform)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in resultFieldTransform)
        {
            Destroy(card.gameObject);
        }
    }



    public IEnumerator MixCard(int kind_1, int cardID_1, int kind_2, int cardID_2, int ID)
    {
        GameObject basicCard;
        GameObject additionalCard;

        Transform field;

        if (ID == 1)
        {
            field = playerFieldTransform;

            //basicCard = Instantiate(cardPrefab, playerFieldTransform, false);

            //yield return new WaitForSeconds(0.7f);

            //additionalCard = Instantiate(cardPrefab, playerFieldTransform, false);
        }
        else
        {
            field = enemyFieldTransform;
            //basicCard = Instantiate(cardPrefab, enemyFieldTransform, false);

            //yield return new WaitForSeconds(0.7f);

            //additionalCard = Instantiate(cardPrefab, enemyFieldTransform, false);
        }

        basicCard = Instantiate(cardPrefab, field, false);
        CardController basicCardController = basicCard.GetComponent<CardController>();
        basicCardController.Init(kind_1, cardID_1, false);


        yield return new WaitForSeconds(0.7f);

        additionalCard = Instantiate(cardPrefab, field, false);
        CardController additionalCardController = additionalCard.GetComponent<CardController>();
        additionalCardController.Init(kind_2, cardID_2, false);


        yield return new WaitForSeconds(0.7f);

        basicCard.GetComponent<CardController>().Init(kind_1, cardID_1, false);
        additionalCard.GetComponent<CardController>().Init(kind_2, cardID_2, false);





        //合成
        int specialMixID = SpecialMix(basicCardController, additionalCardController);
        if (specialMixID >= 0)
        {
            GameObject specialCard = Instantiate(cardPrefab, field, false);
            CardController specialCardController = specialCard.GetComponent<CardController>();

            specialCardController.Init(3, specialMixID, true);


            //カロリー０、攻撃防御は和にする
            specialCardController.model.at = basicCardController.model.at + additionalCardController.model.at;
            specialCardController.model.cal = 0;
            specialCardController.view.Refresh(specialCardController.model);
            //return;
        }
        else
        {
            //basicCardController.model.at += additionalCardController.model.at;
            //basicCardController.model.isMix = true;
            //basicCardController.view.Refresh(basicCardController.model);

            GameObject cloneCard = Instantiate(basicCard, field, false);
            CardController cloneCardController = cloneCard.GetComponent<CardController>();

            cloneCardController.Init(basicCardController.model.kind, basicCardController.model.cardID, true);

            //Debug.Log(cloneCard.GetComponent<CardController>().model);
            cloneCardController.model.cal += additionalCardController.model.cal;
            cloneCardController.model.at += additionalCardController.model.at;
            cloneCardController.model.de += additionalCardController.model.de;
            cloneCardController.movement.isDraggable = false;
            cloneCardController.view.Refresh(cloneCardController.model);
        }





        //1秒後
        Destroy(basicCard);
        Destroy(additionalCard);


        //コスト減らす
        if (ID == 1)
        {
            GameManager.instance.player.mixCost -= 3;
        }
        else
        {
            GameManager.instance.enemy.mixCost -= 3;

        }

        GameManager.instance.StartDefence();
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


}
