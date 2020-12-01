using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour
{
    [SerializeField] public GameObject messagePanel;
    //[SerializeField] Text messageText;
    [SerializeField] TextMeshProUGUI messageText;


    //食べたテキスト
    public void EatMessage(CardModel cardModel, bool isMyTurn)
    {
        //messagePanel.SetActive(true);

        //string name;
        //switch (cardID)
        //{
        //    default:
        //        name = "牛丼";
        //        break;

        //}

        if (isMyTurn)
        {
            messageText.text = cardModel.name + "\nをたべた！";
        }
        else
        {
            messageText.text = "あいては" + cardModel.name + "\nをたべた！";
        }

        //messageText.text = "あいては" + name + "をたべた！";
    }

    //攻撃のテキスト
    public void AttackText(bool isMyTurn)
    {
        if (isMyTurn)
        {
            messageText.text = "あなたのこうげき！";
        }
        else
        {
            messageText.text = "あいてのこうげき！";
        }
    }

    public void ParalysisText()
    {
        messageText.text = "麻痺で動けなかった";
    }

    //効果のテキスト
    public void EffectMessage(string message)
    {
        //string message;

        //switch (cardID)
        //{
        //    default:
        //        message = "自分のコストが１増えた！";
        //        break;
        //}

        messageText.text = message;
    }
}
