﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] Text nameText;
    //[SerializeField] Text hpText;
    [SerializeField] Text calText;
    [SerializeField] Text atText;
    [SerializeField] Text deText;
    //[SerializeField] Text deText;
    [SerializeField] Image iconImage;
    [SerializeField] GameObject selectablePanel;
    //[SerializeField] GameObject shieldPanel;
    [SerializeField] GameObject maskPanel;


    [SerializeField] GameObject attackPanel;
    [SerializeField] GameObject defencePanel;
    [SerializeField] GameObject spellPanel;


    [SerializeField] GameObject mixPanel;
    [SerializeField] GameObject specialMixPanel;




    public void SetCard(CardModel cardModel)
    {

        //合成カード
        if (cardModel.isSpecialMix)
        {
            Debug.Log("aa");
            nameText.text = cardModel.name;
            specialMixPanel.SetActive(true);

        }
        else if (cardModel.isMix)
        {

            nameText.text = cardModel.name + "+";
            mixPanel.SetActive(true);
        }
        else
        {
            nameText.text = cardModel.name;

        }

        //hpText.text = cardModel.hp.ToString();

        if (cardModel.kind == 0)
        {
            calText.text = cardModel.cal + "Cal";
            atText.text = cardModel.at.ToString();
            deText.text = cardModel.at.ToString();

            attackPanel.SetActive(true);
            defencePanel.SetActive(false);
            spellPanel.SetActive(false);
        }
        else if (cardModel.kind == 1)
        {
            calText.text = cardModel.cal + "Cal";
            //calText.color = Color.blue;

            attackPanel.SetActive(false);
            defencePanel.SetActive(true);
            spellPanel.SetActive(false);

        }
        else if (cardModel.kind == 2)
        {
            calText.text = cardModel.cal + "Cal";

            attackPanel.SetActive(false);
            defencePanel.SetActive(false);
            spellPanel.SetActive(true);
        }
        //deText.text = cardModel.de.ToString();
        iconImage.sprite = cardModel.icon;



        //相手のカードを裏返す
        //if (cardModel.playerID != GameManager.instance.playerID)
        //{
        //    maskPanel.SetActive(true);
        //}


    }


    public void Show()
    {
        maskPanel.SetActive(false);

    }

    public void Refresh(CardModel cardModel)
    {
        if (cardModel.kind == 0)
        {
            calText.text = cardModel.cal + "Cal";
            atText.text = cardModel.at.ToString();
            deText.text = cardModel.at.ToString();
        }
        else if (cardModel.kind == 1)
        {
            calText.text = cardModel.cal + "Cal";
        }
        //hpText.text = cardModel.hp.ToString();
        //atText.text = cardModel.at.ToString();
        //deText.text = cardModel.de.ToString();

        //合成カード
        if (cardModel.isSpecialMix)
        {
            nameText.text = cardModel.name;

        }
        else if (cardModel.isMix)
        {
            nameText.text = cardModel.name + "+";
            mixPanel.SetActive(true);
        }
        else
        {
            nameText.text = cardModel.name;

        }


    }

    public void SetActiveSelectablePanel(bool flag)
    {
        selectablePanel.SetActive(flag);
    }
}
