using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] Text nameText;
    //[SerializeField] Text hpText;
    [SerializeField] Text atText;
    [SerializeField] Text deText;
    [SerializeField] Image iconImage;
    [SerializeField] GameObject selectablePanel;
    //[SerializeField] GameObject shieldPanel;
    [SerializeField] GameObject maskPanel;


    public void SetCard(CardModel cardModel)
    {
        nameText.text = cardModel.name;
        //hpText.text = cardModel.hp.ToString();
        atText.text = cardModel.at.ToString();
        deText.text = cardModel.de.ToString();
        iconImage.sprite = cardModel.icon;


        if (cardModel.playerID != GameManager.instance.playerID)
        {
            maskPanel.SetActive(true);
        }


        //if (cardModel.kind == KIND.SPELL)
        //{
        //}
        //if (cardModel.ability == ABILITY.SHIELD)
        //{
        //    shieldPanel.SetActive(true);
        //}
        //else
        //{
        //    shieldPanel.SetActive(false);

            //}

            //if (cardModel.spell != SPELL.NONE)
            //{
            //    hpText.gameObject.SetActive(false);
            //}
    }

    public void Show()
    {
        maskPanel.SetActive(false);

    }

    public void Refresh(CardModel cardModel)
    {
        //hpText.text = cardModel.hp.ToString();
        atText.text = cardModel.at.ToString();
        deText.text = cardModel.de.ToString();

    }

    public void SetActiveSelectablePanel(bool flag)
    {
        selectablePanel.SetActive(flag);
    }
}
