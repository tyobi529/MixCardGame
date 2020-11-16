using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EatCardView : MonoBehaviour
{

    [SerializeField] Text effectText;


    //料理
    [SerializeField] Text nameText;
    [SerializeField] Text calText;


    [SerializeField] Image nutrientColor;


    [SerializeField] public Image iconImage;



    public void SetEatCard(EatCardModel eatCardModel)
    {

        calText.text = eatCardModel.cal + "Kcal";

        if (eatCardModel.kind == KIND.RED)
        {
            nutrientColor.color = Color.red;
        }
        else if (eatCardModel.kind == KIND.YELLOW)
        {
            nutrientColor.color = Color.yellow;

        }
        else if (eatCardModel.kind == KIND.GREEN)
        {
            nutrientColor.color = Color.green;
        }

        //料理
        if (eatCardModel.kind == KIND.DISH) 
        {
            //nutrientColor.color = Color.blue;
            effectText.text = DecideDishEffectText(eatCardModel.cardID);
        }
        else
        {
            effectText.text = eatCardModel.cost + "コスト";
        }


        iconImage.sprite = eatCardModel.icon;



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

}
