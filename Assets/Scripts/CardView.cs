using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] Text nameText;
    //[SerializeField] Text hpText;
    [SerializeField] Text effectText;
    //[SerializeField] Text deText;
    [SerializeField] Image iconImage;
    //[SerializeField] GameObject selectablePanel;
    //[SerializeField] GameObject maskPanel;


    //[SerializeField] GameObject RedPanel;
    //[SerializeField] GameObject YellowPanel;
    //[SerializeField] GameObject GreenPanel;



    [SerializeField] GameObject attackPanel;
    [SerializeField] GameObject nutrientsPanel;
    [SerializeField] GameObject defencePanel;
    [SerializeField] GameObject mixPanel;

    //[SerializeField] GameObject spellPanel;


    //[SerializeField] GameObject mixPanel;
    //[SerializeField] GameObject specialMixPanel;

    //[SerializeField] Image panelImage;





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

        if (cardModel.kind == 0)
        {
            effectText.text = cardModel.cal + "Cal";
            attackPanel.SetActive(true);
            nutrientsPanel.SetActive(true);

            if (cardModel.red != 0)
            {
                nutrientsPanel.GetComponent<Image>().color = Color.red;
            }
            else if (cardModel.yellow != 0)
            {
                nutrientsPanel.GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                nutrientsPanel.GetComponent<Image>().color = Color.green;
            }

            //attackPanel.SetActive(true);
            //if (cardModel)
            //RedPanel.GetComponent<Image>

            //defencePanel.SetActive(false);
            //spellPanel.SetActive(false);

            //defencePanel.GetComponent<Image>().color = new Color(248f/255f, 109f/255f, 78f/255f, 1f);
            //defencePanel.GetComponent<Image>().color = Color.h;

        }
        else if (cardModel.kind == 1)
        {
            effectText.text = cardModel.cal + "Cal";
            //effectText.color = Color.blue;

            defencePanel.SetActive(true);
            //panelImage.color = new Color(78f / 255f, 192f / 255f, 248f / 255f, 1f);

            //attackPanel.SetActive(false);
            //defencePanel.SetActive(true);
            //spellPanel.SetActive(false);

        }
        //合成
        //else if (cardModel.kind == 2)
        //{
        //    effectText.text = cardModel.cal + "Cal";
        //    //effectText.color = Color.blue;


        //    //panelImage.color = new Color(248f / 255f, 246f / 255f, 126f / 255f, 1f);
        //    //attackPanel.SetActive(false);
        //    //defencePanel.SetActive(false);
        //    //spellPanel.SetActive(true);
        //}

        if (cardModel.isMix)
        {
            mixPanel.SetActive(true);

        }
        //deText.text = cardModel.de.ToString();
        iconImage.sprite = cardModel.icon;



        //相手のカードを裏返す
        //if (cardModel.playerID != GameManager.instance.playerID)
        //{
        //    maskPanel.SetActive(true);
        //}


    }


    //public void Show()
    //{
    //    maskPanel.SetActive(false);

    //}

    public void Refresh(CardModel cardModel)
    {
        if (cardModel.kind == 0)
        {
            effectText.text = cardModel.cal + "Cal";
        }
        else if (cardModel.kind == 1)
        {
            effectText.text = cardModel.cal + "Cal";
        }
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
