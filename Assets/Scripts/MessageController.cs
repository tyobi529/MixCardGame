using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour
{
    [SerializeField] Text messageText;


    public void EatMessage(int cardID)
    {
        string name;
        switch (cardID)
        {
            default:
                name = "牛丼";
                break;

        }

        messageText.text = "あいては" + name + "をたべた！";
    }

}
