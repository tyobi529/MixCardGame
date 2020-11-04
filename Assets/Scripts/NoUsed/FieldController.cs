//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class FieldController : MonoBehaviour
//{
//    [SerializeField] int position;
//    bool isSelected = false;

//    public int cost = 0;
//    public int deadLine = 5;

//    public void SelectCard()
//    {
//        if (transform.childCount == 0)
//        {
//            return;
//        }

//        isSelected = !isSelected;

//        if (isSelected)
//        {
//            this.transform.GetComponent<Image>().color = Color.yellow;
//        }
//        else
//        {
//            this.transform.GetComponent<Image>().color = Color.white;
//        }

//        GameManager.instance.SelectCard(isSelected, position);


//    }

//    public void CancelCard()
//    {
//        isSelected = false;
//        this.transform.GetComponent<Image>().color = Color.white;

//    }


//}
