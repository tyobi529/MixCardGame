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

    [SerializeField] Text nutrientsText;

    [SerializeField] public GameObject deadLineObject;
    [SerializeField] Text deadLineText;






    public void SetCard(CardModel cardModel)
    {

        //テスト用
        effectText.text = cardModel.cardID.ToString();
        //effectText.text = DecideEffectText(cardModel.specialID);

        if (cardModel.nutrient[0] == NUTRIENT.RED)
        {
            //ingredientPanel.GetComponent<Image>().color = Color.red;
            nutrientColor.color = Color.red;


        }
        else if (cardModel.nutrient[0] == NUTRIENT.YELLOW)
        {
            //ingredientPanel.GetComponent<Image>().color = Color.yellow;
            nutrientColor.color = Color.yellow;

        }
        else if (cardModel.nutrient[0] == NUTRIENT.GREEN)
        {
            //ingredientPanel.GetComponent<Image>().color = Color.green;
            nutrientColor.color = Color.green;

        }

        iconImage.sprite = cardModel.icon;



    }


    public void SetMixCard(CardModel cardModel)
    {
        calText.text = cardModel.cal + "Kcal";

        if (cardModel.kind == KIND.INGREDIENT)
        {

            effectText.text = DecideEffectText(cardModel.specialID);

            if (cardModel.nutrient[0] == NUTRIENT.RED)
            {
                nutrientColor.color = Color.red;


            }
            else if (cardModel.nutrient[0] == NUTRIENT.YELLOW)
            {
                nutrientColor.color = Color.yellow;

            }
            else if (cardModel.nutrient[0] == NUTRIENT.GREEN)
            {
                nutrientColor.color = Color.green;

            }

            iconImage.sprite = cardModel.icon;


        }

        else
        {

 
            nameText.text = cardModel.name;
            calText.text = cardModel.cal + "Kcal";

            iconImage.sprite = cardModel.icon;




        }



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
                Debug.Log("範囲外");
                break;


        }

        return effectText;
    }


    public void Refresh(CardModel cardModel)
    {
        effectText.text = cardModel.cal.ToString();

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
