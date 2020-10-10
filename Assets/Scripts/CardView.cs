using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] Text nameText;
    //[SerializeField] Text hpText;
    [SerializeField] Text effectText;

    [SerializeField] Text hitText;

    //[SerializeField] Text deText;
    [SerializeField] Image iconImage;
    //[SerializeField] GameObject selectablePanel;
    //[SerializeField] GameObject maskPanel;


    //[SerializeField] GameObject RedPanel;
    //[SerializeField] GameObject YellowPanel;
    //[SerializeField] GameObject GreenPanel;



    [SerializeField] GameObject ingredientPanel;
    //[SerializeField] GameObject nutrientsPanel;
    //[SerializeField] GameObject defencePanel;
    [SerializeField] GameObject mixPanel;

    //[SerializeField] GameObject spellPanel;


    //[SerializeField] GameObject mixPanel;
    //[SerializeField] GameObject specialMixPanel;

    //[SerializeField] Image panelImage;

    [SerializeField] public GameObject selectPanel;





    public void SetCard(CardModel cardModel)
    {

        //合成カード
        //if (cardModel.isSpecialMix)
        //{
        //    Debug.Log("aa");
        //    nameText.text = cardModel.name;
        //    specialMixPanel.SetActive(true);

        //}
        //else if (cardModel.isMix)
        //{

        //    nameText.text = cardModel.name + "+";
        //    mixPanel.SetActive(true);
        //}
        //else
        //{
        nameText.text = cardModel.name;

        //}

        //hpText.text = cardModel.hp.ToString();

        if (cardModel.kind == KIND.INGREDIENT)
        {

            effectText.text = DecideEffectText(cardModel.cardID);
            ingredientPanel.SetActive(true);

            //if (cardModel.red != 0)
            //{
            //    //nutrientsPanel.GetComponent<Image>().color = Color.red;
            //    ingredientPanel.GetComponent<Image>().color = Color.red;
            //}
            //else if (cardModel.yellow != 0)
            //{
            //    //nutrientsPanel.GetComponent<Image>().color = Color.yellow;
            //    ingredientPanel.GetComponent<Image>().color = Color.yellow;
            //}
            //else
            //{
            //    //nutrientsPanel.GetComponent<Image>().color = Color.green;
            //    ingredientPanel.GetComponent<Image>().color = Color.green;
            //}


        }
        else if (cardModel.kind == KIND.DISH)
        {
            effectText.text = cardModel.cal + "Cal";
            hitText.text = cardModel.hit + "%";

            //mixPanel.SetActive(true);
            ingredientPanel.SetActive(true);

        }

        if (cardModel.nutrient == NUTRIENT.RED)
        {
            ingredientPanel.GetComponent<Image>().color = Color.red;

        }
        else if (cardModel.nutrient == NUTRIENT.YELLOW)
        {
            ingredientPanel.GetComponent<Image>().color = Color.yellow;

        }
        else if (cardModel.nutrient == NUTRIENT.GREEN)
        {
            ingredientPanel.GetComponent<Image>().color = Color.green;

        }

        iconImage.sprite = cardModel.icon;


    }


    string DecideEffectText(int cardID)
    {
        string effectText = null;

        switch (cardID)
        {
            case 0:
                effectText = "攻撃UP";
                break;
            case 1:
                effectText = "命中UP";
                break;
            case 2:
                effectText = "バフ解除";
                break;
            case 3:
                effectText = "毒特攻";
                break;
            case 4:
                effectText = "暗闇特攻";
                break;
            case 5:
                effectText = "HP差特攻";
                break;
            case 6:
                effectText = "毒";
                break;
            case 7:
                effectText = "暗闇";
                break;
            case 8:
                effectText = "異常解除";
                break;
            default:
                Debug.Log("範囲外");
                break;
        }

        return effectText;
    }

    public void Refresh(CardModel cardModel, int attackUp, int hitUp)
    {
        effectText.text = cardModel.cal + "Cal";
        hitText.text = cardModel.hit + "%";


        if (attackUp != 0)
        {
            effectText.color = Color.red;
        }
        if (hitUp != 0)
        {
            hitText.color = Color.red;
        }

        //if (cardModel.kind == KIND.INGREDIENT)
        //{
        //    effectText.text = cardModel.cal + "Cal";
        //}
        //else if (cardModel.kind == KIND.DISH)
        //{
        //    effectText.text = cardModel.cal + "Cal";
        //}
        //hpText.text = cardModel.hp.ToString();
        //atText.text = cardModel.at.ToString();
        //deText.text = cardModel.de.ToString();

        //合成カード
        //if (cardModel.isMix)
        //{
        //    nameText.text = cardModel.name + "+";
        //    mixPanel.SetActive(true);
        //}
        //else
        //{
        //    nameText.text = cardModel.name;

        //}


    }

    //public void SetActiveSelectablePanel(bool flag)
    //{
    //    selectablePanel.SetActive(flag);
    //}
}
