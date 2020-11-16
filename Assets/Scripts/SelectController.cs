using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectController : MonoBehaviour
{
    public CardController[] selectCardController = new CardController[2];
    //public EatCardController eatCardController;

    [SerializeField] CardGenerator cardGenerator;

    [SerializeField] Transform[] mixFieldTransform = new Transform[3];

    [SerializeField] GamePlayerManager player;

    public bool canSelect = true;

    //合成予測を出す
    public void SelectCard(CardController cardController, bool isSelected)
    {
        if (!canSelect)
        {
            return;
        }

        //選択した時
        if (isSelected)
        {
            //何も選択されていない
            if (selectCardController[0] == null)
            {
                selectCardController[0] = cardController;
            }
            //２つ目に選択
            else if (selectCardController[1] == null)
            {
                selectCardController[1] = cardController;

            }
            //２つ目と入れ替え
            else
            {
                selectCardController[1].model.isSelected = false;
                selectCardController[1].view.SelectView(false);
                selectCardController[1] = cardController;
            }

        }
        //キャンセルした時
        else
        {
            //１枚目に選択してた時
            if (selectCardController[0] == cardController)
            {
                //２枚目に選択してたカードなし
                if (selectCardController[1] == null)
                {
                    selectCardController[0] = null;
                }
                //２枚目に選択したカードを１枚目にする
                else
                {
                    selectCardController[0] = selectCardController[1];
                    selectCardController[1] = null;
                }
            }
            //２枚目に選択してた時
            else
            {
                selectCardController[1] = null;
            }

        }

        //生成
        //int[] cardID = new int[2] { -1, -1 };
        //int[] cost = new int[2] { -1, -1 };
        //int[] cal = new int[2] { -1, -1 };

        //bool[] isRare = new bool[2] { false, false };

        //for (int i = 0; i < 2; i++)
        //{
        //    if (selectCardController[i] != null)
        //    {
        //        cardID[i] = selectCardController[i].model.cardID;
        //        cost[i] = selectCardController[i].model.cost;
        //        cal[i] = selectCardController[i].model.cal;

        //        isRare[i] = selectCardController[i].model.isRare;
        //    }

        //}

        GenerateFieldCard();
    }

    //public void GenerateFieldCard(int[] cardID, int[] cal, int[] cost, bool[] isRare)
    public void GenerateFieldCard()
    {


        CleanField();

        CardController[] mixCardController = new CardController[2] { null, null };

        EatCardController eatCardController;


        for (int i = 0; i < 2; i++)
        {
            mixCardController[i] = null;
        }

        for (int i = 0; i < 2; i++)
        {
            if (selectCardController[i] != null)
            {                
                mixCardController[i] = cardGenerator.CreateCard(selectCardController[i].model.cardID, selectCardController[i].model.cost, selectCardController[i].model.isRare, mixFieldTransform[i]);
                mixCardController[i].model.cal = selectCardController[i].model.cal;
                mixCardController[i].view.SetCard(mixCardController[i].model);
            }
        }




        //if (cardID[0] == -1)
        //{
        //    uiManager.decideButtonObj.SetActive(false);

        //    return;
        //}


        //合成チェック
        if (mixCardController[1] != null)
        {
            if (mixCardController[0].model.kind == mixCardController[1].model.kind)
            {
                //合成不可
                //uiManager.decideButtonObj.SetActive(false);
                return;
            }
            else
            {

                //合成
                int mixCardID = cardGenerator.SpecialMix(mixCardController[0], mixCardController[1]);

                eatCardController = cardGenerator.CreateEatCard(KIND.DISH, mixCardID, mixCardID, false, mixFieldTransform[2]);
                eatCardController.view.SetEatCard(eatCardController.model);

                //コストがあり、暗闇でない時
                //if (player.cost >= mixCost && attacker.darkCount == 0)
                //{
                //    uiManager.decideButtonObj.SetActive(true);
                //}

            }
        }
        else if (mixCardController[0] != null)
        {
            //uiManager.decideButtonObj.SetActive(true);
            eatCardController = cardGenerator.CreateEatCard(mixCardController[0].model.kind, mixCardController[0].model.cardID, mixCardController[0].model.cost, mixCardController[0].model.isRare, mixFieldTransform[2]);

            eatCardController.model.cal = mixCardController[0].model.cal;
            eatCardController.model.cost = mixCardController[0].model.cost;
            eatCardController.view.SetEatCard(eatCardController.model);

            //mixCardController[2].model.cal = mixCardController[0].model.cal;
            //mixCardController[2].model.cost = mixCardController[0].model.cost;
            //mixCardController[2].view.SetEatCard(mixCardController[2].model);


            //テスト用
            //mixCardController[2].model.kind = KIND.DISH;
            //mixCardController[2].model.specialID = 0;
        }
        else
        {
            return;
        }


        //if (!isMyTurn)
        //{
        //    DamageCalculation();
        //}

        //strength = 0;
        //for (int i = 0; i < 2; i++)
        //{
        //    if (isRare[i])
        //    {
        //        strength++;
        //    }
        //}

    }


    public void CleanField()
    {


        foreach (Transform field in mixFieldTransform)
        {
            if (field.childCount != 0)
            {
                Destroy(field.GetChild(0).gameObject);
            }
        }



    }

}
