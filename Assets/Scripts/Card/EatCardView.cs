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

    


    public void SetEatCard(CardModel cardModel)
    {
        nameText.text = cardModel.name;
        calText.text = cardModel.cal + "Kcal";

        //if (cardModel.kind == KIND.RED)
        //{
        //    costBGImage.color = new Color(248f / 255f, 132f / 255f, 132f / 255f, 1f);
        //}
        //else if (cardModel.kind == KIND.YELLOW)
        //{
        //    costBGImage.color = new Color(238f / 255f, 248f / 255f, 132f / 255f, 1f);
        //}
        //else if (cardModel.kind == KIND.GREEN)
        //{
        //    costBGImage.color = new Color(132f / 255f, 248f / 255f, 141f / 255f, 1f);
        //}


        //料理
        if (cardModel.kind == KIND.DISH) 
        {
            effectText.text = DecideDishEffectText(cardModel.cardID);
        }
        else
        {
            effectText.text = cardModel.cost + "コスト";
        }


        iconImage.sprite = cardModel.icon;



    }

    string DecideDishEffectText(int specialID)
    {
        string effectText = null;

        switch (specialID)
        {
            case 0:
                effectText = "ランダム";
                break;
            case 1:
                effectText = "ときどきカロリーが１になる";
                break;
            case 2:
                effectText = "どちらかにダメージ";
                break;
            case 3:
                effectText = "食材のコスト＋";
                break;
            case 4:
                effectText = "食材のカロリー＋";
                break;
            case 5:
                effectText = "食材をレアにする";
                break;
            case 6:
                effectText = "相手のコストー";
                break;
            case 7:
                effectText = "自分のコスト＋";
                break;
            case 8:
                effectText = "次料理カロリー＋";
                break;
            case 9:
                effectText = "敵食材のコストー";
                break;
            case 10:
                effectText = "敵食材のCalー";
                break;
            case 11:
                effectText = "レア食材を奪う";
                break;
            case 12:
                effectText = "HP差特攻";
                break;
            case 13:
                effectText = "異常特攻";
                break;
            case 14:
                effectText = "コスト差特攻";
                break;
            case 15:
                effectText = "麻痺";
                break;
            case 16:
                effectText = "暗闇";
                break;
            case 17:
                effectText = "毒";
                break;
            case 18:
                effectText = "異常回復";
                break;
            case 19:
                effectText = "状態回復";
                break;
            case 20:
                effectText = "ダメ回復";
                break;
            case 21:
                effectText = "ダメージ軽減";
                break;
            case 22:
                effectText = "行動回数＋";
                break;
            case 23:
                effectText = "２回目以降カロリー＋";
                break;
            case 24:
                effectText = "経過ターン分追加ダメ";
                break;
            case 25:
                effectText = "状態異常分追加ダメ";
                break;
            case 26:
                effectText = "相手のコスト分追加ダメ";
                break;
            default:
                effectText = "エラー";
                break;


        }

        return effectText;
    }

}
