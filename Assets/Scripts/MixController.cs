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


    //Inspectorに複数データを表示するためのクラス
    //[System.SerializableAttribute]
    //public class ValueList
    //{
    //    //public List<int> List = new List<int>();
    //    public string[] list = new string[2];

    //    public ValueList(string[] list)
    //    {
    //        this.list = list;
    //    }
    //}

    //Inspectorに表示される
    //[SerializeField]
    //private string[] foodName = new string<ValueList>();

    //private List<ValueList> _valueListList = new List<ValueList>();


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

        if (basicFieldTransform.childCount == 0)
        {
            //決定ボタンを消す
            uiManager.decideButtonObj.SetActive(false);
            return;

        }

        //決定ボタンを出す
        uiManager.decideButtonObj.SetActive(true);
        //警告を消す
        uiManager.lackCostText.SetActive(false);

        GameObject basicCard = basicFieldTransform.GetChild(0).gameObject;
        CardController basicCardController = basicCard.GetComponent<CardController>();

        GameObject additionalCard;
        CardController additionalCardController;


        //合成なし
        if (additionalFieldTransform.childCount == 0)
        {
            GameObject cloneCard = Instantiate(basicCard, resultFieldTransform, false);
            //cloneCard.GetComponent<CardController>().Init(basicCard.GetComponent<CardModel>().cardID, basicCard.GetComponent<CardModel>().playerID, true);
            //return;
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
                specialCard.GetComponent<CardController>().Init(3, specialMixID, true);

            }
            else
            {
                GameObject cloneCard = Instantiate(basicCard, resultFieldTransform, false);
                CardController cloneCardController = cloneCard.GetComponent<CardController>();

                cloneCardController.Init(basicCardController.model.kind, basicCardController.model.cardID, true);

                //Debug.Log(cloneCard.GetComponent<CardController>().model);
                cloneCardController.model.cal += additionalCardController.model.cal;
                cloneCardController.view.Refresh(cloneCardController.model);
            }




            //コスト確認
            if (GameManager.instance.playerID == 1 && GameManager.instance.player.mixCost < 3 ||
                GameManager.instance.playerID == 2 && GameManager.instance.enemy.mixCost < 3)
            {
                uiManager.decideButtonObj.SetActive(false);

                uiManager.lackCostText.SetActive(true);
            }

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
            specialCard.GetComponent<CardController>().Init(3, specialMixID, true);

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

        //foreach (int partnerID in card_1.model.partnerID)
        //{
        //    if (partnerID == card_2.model.cardID)
        //    {
        //        //特殊合成
        //    }
        //}
        //switch (cardName_1)
        //{
        //    case "たまご":
        //        if (cardName_2 == "ごはん")
        //        {
        //            card = Instantiate(cardPrefab, field, false);
        //            card.GetComponent<CardController>().Init(0, true, true);
        //        }
        //        break;
        //    default:
        //        break;
        //}

        //if ((cardName_1 == "たまご" && cardName_2 == "とりにく") || (cardName_1 == "とりにく" && cardName_2 == "たまご"))
        //{
        //    card = Instantiate(cardPrefab, field, false);
        //    card.GetComponent<CardController>().Init(0, true, true);
        //}
        //else if ((cardName_1 == "ぎゅうにく" && cardName_2 == "じゃがいも") || (cardName_1 == "じゃがいも" && cardName_2 == "ぎゅうにく"))
        //{
        //    card = Instantiate(cardPrefab, field, false);
        //    card.GetComponent<CardController>().Init(1, true, true);
        //}
        //else if ((cardName_1 == "ぶたにく" && cardName_2 == "ピーマン") || (cardName_1 == "ピーマン" && cardName_2 == "ぶたにく"))
        //{
        //    card = Instantiate(cardPrefab, field, false);
        //    card.GetComponent<CardController>().Init(2, true, true);
        //}


        //if (card != null)
        //{

        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
    }



    void Combine(string name1, string name2)
    {

    }
}
