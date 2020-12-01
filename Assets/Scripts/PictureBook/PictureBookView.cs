using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PictureBookView : MonoBehaviour
{
    //[SerializeField] string name;

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Image iconImage;


    public void SetView(int cardID)
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Dishes/Card" + cardID);

        nameText.text = cardEntity.name;
        iconImage.sprite = cardEntity.icon;

    }
}
