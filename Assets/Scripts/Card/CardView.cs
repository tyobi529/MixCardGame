using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] GameObject statusObject;

    //料理
    //[SerializeField] Text nameText;
    [SerializeField] Text calText;


    [SerializeField] Image nutrientColor;


    //レア素材
    [SerializeField] Image costBGImage;
    [SerializeField] Text costText;
    [SerializeField] GameObject rareBG;

    [SerializeField] public Image iconImage;


    [SerializeField] GameObject selectObject;

    [SerializeField] Image costBG;



    public void SetCard(CardModel cardModel)
    {


        //effectText.text = DecideEffectText(cardModel.specialID);
        costText.text = cardModel.cost.ToString();

        calText.text = cardModel.cal + "Kcal";


        if (cardModel.kind == KIND.RED)
        {
            costBG.color = new Color(248f / 255f, 132f / 255f, 132f / 255f, 1f);
        }
        else if (cardModel.kind == KIND.YELLOW)
        {
            costBG.color = new Color(238f / 255f, 248f / 255f, 132f / 255f, 1f);
        }
        else if (cardModel.kind == KIND.GREEN)
        {
            costBG.color = new Color(132f / 255f, 248f / 255f, 141f / 255f, 1f);
        }

        //if (cardModel.kind == KIND.RED)
        //{
        //    nutrientColor.color = Color.red;
        //}
        //else if (cardModel.kind == KIND.YELLOW)
        //{
        //    nutrientColor.color = Color.yellow;

        //}
        //else if (cardModel.kind == KIND.GREEN)
        //{
        //    nutrientColor.color = Color.green;

        //}

        //rareBG.SetActive(cardModel.isRare);

        iconImage.sprite = cardModel.icon;



    }

    public void SetCookingCard(CardModel cardModel)
    {
        iconImage.sprite = cardModel.icon;
        statusObject.SetActive(false);
    }


    //public void SetEatCard(CardModel cardModel)
    //{
    //    calText.text = cardModel.cal + "Kcal";

    //    if (cardModel.kind == KIND.INGREDIENT)
    //    {
    //        nameText.text = cardModel.name;
    //        effectText.text = cardModel.cost + "コスト";


    //        iconImage.sprite = cardModel.icon;


    //    }

    //    else
    //    {

    //        nameText.text = cardModel.name;
    //        calText.text = cardModel.cal + "Kcal";

    //        iconImage.sprite = cardModel.icon;


    //        effectText.text = DecideDishEffectText(cardModel.specialID);

    //    }



    //}

    public void SelectView(bool isSelected)
    {
        selectObject.SetActive(isSelected);
    }





    public void Refresh(CardModel cardModel)
    {
        //effectText.text = DecideEffectText(cardModel.specialID);

        //CardModel cardModel = transform.GetComponent<CardModel>();
        calText.text = cardModel.cal + "Kcal";
        costText.text = cardModel.cost.ToString();

        rareBG.SetActive(cardModel.isRare);


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
