using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    //[SerializeField] Text nameText;
    //[SerializeField] Text hpText;
    [SerializeField] Text effectText;


    //料理
    [SerializeField] Text nameText;
    [SerializeField] Text calText;
    [SerializeField] Text hitText;


    [SerializeField] Image nutrientColor;

    [SerializeField] public Image iconImage;


    [SerializeField] GameObject selectObject;



    public void SetCard(CardModel cardModel)
    {

        //テスト用
        //effectText.text = cardModel.cardID.ToString();
        effectText.text = DecideEffectText(cardModel.specialID);

        if (cardModel.dish[0] == DISH.RED)
        {
            nutrientColor.color = Color.red;
        }
        else if (cardModel.dish[0] == DISH.YELLOW)
        {
            nutrientColor.color = Color.yellow;

        }
        else if (cardModel.dish[0] == DISH.GREEN)
        {
            nutrientColor.color = Color.green;

        }

        iconImage.sprite = cardModel.icon;



    }


    public void SetEatCard(CardModel cardModel)
    {
        calText.text = cardModel.cal + "Kcal";

        if (cardModel.kind == KIND.INGREDIENT)
        {
            nameText.text = cardModel.name;
            effectText.text = DecideEffectText(cardModel.specialID);


            iconImage.sprite = cardModel.icon;


        }

        else
        {

            nameText.text = cardModel.name;
            calText.text = cardModel.cal + "Kcal";

            iconImage.sprite = cardModel.icon;


            effectText.text = DecideDishEffectText(cardModel.specialID);

        }



    }

    public void SelectView(bool isSelected)
    {
        selectObject.SetActive(isSelected);
    }


    string DecideEffectText(int specialID)
    {
        string effectText = null;

        switch (specialID)
        {
            case 0:
                effectText = "赤料理＋";
                break;
            case 1:
                effectText = "黄料理＋";
                break;
            case 2:
                effectText = "緑料理＋";
                break;
            case 3:
                effectText = "和食＋";
                break;
            case 4:
                effectText = "洋食＋";
                break;
            case 5:
                effectText = "中華＋";
                break;
            default:
                effectText = "";
                break;


        }

        return effectText;
    }

    string DecideDishEffectText(int specialID)
    {
        string effectText = null;

        switch (specialID)
        {
            case 0:
                effectText = "コスト下げ";
                break;
            case 1:
                effectText = "素材効果付与";
                break;
            case 2:
                effectText = "カロリー＋";
                break;
            case 3:
                effectText = "相手コストー";
                break;
            case 4:
                effectText = "バフ消し";
                break;
            case 5:
                effectText = "コスト＋";
                break;
            case 6:
                effectText = "相手のコスト分";
                break;
            case 7:
                effectText = "状態異常分";
                break;
            case 8:
                effectText = "素材のCal分";
                break;
            case 9:
                effectText = "手札捨て";
                break;
            case 10:
                effectText = "複数回行動";
                break;
            case 11:
                effectText = "未実装";
                break;
            case 12:
                effectText = "合成特攻";
                break;
            case 13:
                effectText = "HP差特攻";
                break;
            case 14:
                effectText = "異常特攻";
                break;
            case 15:
                effectText = "麻痺";
                break;
            case 16:
                effectText = "毒";
                break;
            case 17:
                effectText = "暗闇";
                break;
            case 18:
                effectText = "ダメ回復";
                break;
            case 19:
                effectText = "状態回復";
                break;
            case 20:
                effectText = "HP回復";
                break;
            case 21:
                effectText = "行動＋";
                break;
            case 22:
                effectText = "次強化";
                break;
            case 23:
                effectText = "使う度";
                break;
            case 24:
                effectText = "複数攻撃";
                break;
            case 25:
                effectText = "自か相ダメージ";
                break;
            case 26:
                effectText = "ランダム";
                break;
            default:
                effectText = "";
                break;


        }

        return effectText;
    }


    public void Refresh(CardModel cardModel)
    {
        effectText.text = DecideEffectText(cardModel.specialID);

        //CardModel cardModel = transform.GetComponent<CardModel>();
        //calText.text = model.cal + "Kcal";

        //effectText.text = cardModel.cal + "Cal";
        //hitText.text = cardModel.hit + "%";

        //if (cardModel.kind == KIND.INGREDIENT)
        //{
        //    effectText.text = DecideEffectText(cardModel.specialID);
        //    nameText.text = cardModel.name;
        //    calText.text = cardModel.cal + "Kcal";

        //    iconImage.sprite = cardModel.icon;
        //}




    }

}
