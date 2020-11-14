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


    [SerializeField] Image nutrientColor;


    //レア素材案
    [SerializeField] Image costBGImage;
    [SerializeField] Text costText;
    [SerializeField] GameObject rareBG;

    [SerializeField] public Image iconImage;


    [SerializeField] GameObject selectObject;



    public void SetCard(CardModel cardModel)
    {


        //effectText.text = DecideEffectText(cardModel.specialID);
        costText.text = cardModel.cost.ToString();

        calText.text = cardModel.cal + "Kcal";

        if (cardModel.dish[0] == DISH.RED)
        {
            //costBGImage.color = Color.red;
            nutrientColor.color = Color.red;
        }
        else if (cardModel.dish[0] == DISH.YELLOW)
        {
            //costBGImage.color = Color.yellow;
            nutrientColor.color = Color.yellow;

        }
        else if (cardModel.dish[0] == DISH.GREEN)
        {
            //costBGImage.color = Color.green;
            nutrientColor.color = Color.green;

        }

        rareBG.SetActive(cardModel.rare);

        iconImage.sprite = cardModel.icon;



    }


    public void SetEatCard(CardModel cardModel)
    {
        calText.text = cardModel.cal + "Kcal";

        if (cardModel.kind == KIND.INGREDIENT)
        {
            nameText.text = cardModel.name;
            //effectText.text = DecideEffectText(cardModel.specialID);
            effectText.text = cardModel.cost + "コスト";


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
                effectText = "使うたび強化";
                break;
            case 1:
                effectText = "自分-100kCal";
                break;
            case 2:
                effectText = "連続行動";
                break;
            case 3:
                effectText = "素材コスト＋";
                break;
            case 4:
                effectText = "素材をレアに";
                break;
            case 5:
                effectText = "素材のCal＋";
                break;
            case 6:
                effectText = "素材分Cal＋";
                break;
            case 7:
                effectText = "状態異常分＋";
                break;
            case 8:
                effectText = "敵コスト分＋";
                break;
            case 9:
                effectText = "次料理Cal+";
                break;
            case 10:
                effectText = "自コスト＋";
                break;
            case 11:
                effectText = "敵コスト-";
                break;
            case 12:
                effectText = "コスト差特攻";
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
                effectText = "敵手札-";
                break;
            case 22:
                effectText = "レア奪う";
                break;
            case 23:
                effectText = "自手札-";
                break;
            case 24:
                effectText = "複数回攻撃";
                break;
            case 25:
                effectText = "自か相ダメージ";
                break;
            case 26:
                effectText = "ランダム";
                break;
            default:
                effectText = "エラー";
                break;


        }

        return effectText;
    }


    public void Refresh(CardModel cardModel)
    {
        //effectText.text = DecideEffectText(cardModel.specialID);

        //CardModel cardModel = transform.GetComponent<CardModel>();
        calText.text = cardModel.cal + "Kcal";
        costText.text = cardModel.cost.ToString();

        rareBG.SetActive(cardModel.rare);


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
