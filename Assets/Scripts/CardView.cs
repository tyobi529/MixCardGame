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

    //[SerializeField] Text hitText;

    //[SerializeField] Text deText;
    [SerializeField] Image nutrientColor;

    [SerializeField] public Image iconImage;

    [SerializeField] Text nutrientsText;

    [SerializeField] public GameObject deadLineObject;
    [SerializeField] Text deadLineText;

    //[SerializeField] Image selectIconImage;

    //[SerializeField] GameObject selectablePanel;
    //[SerializeField] GameObject maskPanel;


    //[SerializeField] GameObject RedPanel;
    //[SerializeField] GameObject YellowPanel;
    //[SerializeField] GameObject GreenPanel;



    //[SerializeField] GameObject ingredientPanel;
    //[SerializeField] GameObject nutrientsPanel;
    //[SerializeField] GameObject defencePanel;
    //[SerializeField] GameObject mixPanel;

    //[SerializeField] GameObject spellPanel;


    //[SerializeField] GameObject mixPanel;
    //[SerializeField] GameObject specialMixPanel;

    //[SerializeField] Image panelImage;

    //[SerializeField] public GameObject selectPanel;





    public void SetCard(CardModel cardModel)
    {


        //nameText.text = cardModel.name;


        if (cardModel.kind == KIND.INGREDIENT)
        {

            effectText.text = DecideEffectText(cardModel.specialID);
            //ingredientPanel.SetActive(true);

            if (cardModel.nutrient == NUTRIENT.RED)
            {
                //ingredientPanel.GetComponent<Image>().color = Color.red;
                nutrientColor.color = Color.red;


            }
            else if (cardModel.nutrient == NUTRIENT.YELLOW)
            {
                //ingredientPanel.GetComponent<Image>().color = Color.yellow;
                nutrientColor.color = Color.yellow;

            }
            else if (cardModel.nutrient == NUTRIENT.GREEN)
            {
                //ingredientPanel.GetComponent<Image>().color = Color.green;
                nutrientColor.color = Color.green;

            }

            iconImage.sprite = cardModel.icon;


        }

        else
        {

            //effectText.text = DecideEffectText(cardModel.specialID);

            nameText.text = cardModel.name;
            calText.text = cardModel.cal + "Kcal";
            hitText.text = cardModel.hit + "%";

            iconImage.sprite = cardModel.icon;


            nutrientsText.text = "赤" + cardModel.red + "黄" + cardModel.yellow + "緑" + cardModel.green;


        }



        //else if (cardModel.kind == KIND.DISH)
        //{
        //    effectText.text = cardModel.cal + "Cal";
        //    hitText.text = cardModel.hit + "%";

        //    //mixPanel.SetActive(true);
        //    ingredientPanel.SetActive(true);

        //    mixPanel.SetActive(true);

        //}




    }


    public void ShowDeadLine(int deadLine)
    {
        deadLineText.text = "残 " + deadLine;
    }



    string DecideEffectText(int specialID)
    {
        string effectText = null;

        switch (specialID)
        {
            case 0:
                effectText = "毒";
                break;
            case 1:
                effectText = "暗闇";
                break;
            case 2:
                effectText = "異常回復";
                break;
            case 3:
                effectText = "攻撃UP";
                break;
            case 4:
                effectText = "防御UP";
                break;
            default:
                Debug.Log("範囲外");
                break;

                //case 0:
                //    effectText = "攻撃UP";
                //    break;
                //case 1:
                //    effectText = "命中UP";
                //    break;
                //case 2:
                //    effectText = "バフ解除";
                //    break;
                //case 3:
                //    effectText = "毒特攻";
                //    break;
                //case 4:
                //    effectText = "暗闇特攻";
                //    break;
                //case 5:
                //    effectText = "HP差特攻";
                //    break;
                //case 6:
                //    effectText = "毒";
                //    break;
                //case 7:
                //    effectText = "暗闇";
                //    break;
                //case 8:
                //    effectText = "異常解除";
                //    break;
                //default:
                //    Debug.Log("範囲外");
                //    break;
        }

        return effectText;
    }

    //public void ChangeDeadLine(CardModel cardModel)
    //{
    //    costText.text = cardModel.cost.ToString();
    //}

    public void Refresh(CardModel cardModel, int attackUp, int hitUp)
    {
        //effectText.text = cardModel.cal + "Cal";
        //hitText.text = cardModel.hit + "%";

        if (cardModel.kind == KIND.INGREDIENT)
        {
            effectText.text = DecideEffectText(cardModel.specialID);
            nameText.text = cardModel.name;
            calText.text = cardModel.cal + "Kcal";
            hitText.text = cardModel.hit + "%";

            iconImage.sprite = cardModel.icon;
        }

        //if (attackUp != 0)
        //{
        //    effectText.color = Color.red;
        //}
        //if (hitUp != 0)
        //{
        //    hitText.color = Color.red;
        //}
        //if (cardModel.kind == KIND.DISH)
        //{
        //    effectText.text = DecideEffectText(cardModel.specialID);
        //    nameText.text = cardModel.name;
        //    calText.text = cardModel.cal + "Kcal";
        //    hitText.text = cardModel.hit + "%";

        //    iconImage.sprite = cardModel.icon;
        //}



    }

    //public void SetActiveSelectablePanel(bool flag)
    //{
    //    selectablePanel.SetActive(flag);
    //}
}
