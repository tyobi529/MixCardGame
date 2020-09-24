//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;

////攻撃される側
//public class SpellDropManager : MonoBehaviour, IDropHandler
//{
//    public void OnDrop(PointerEventData eventData)
//    {
//        //attackerカードを選択
//        CardController spellCard = eventData.pointerDrag.GetComponent<CardController>();
//        //defenderカードを選択
//        CardController target = GetComponent<CardController>(); //nullの可能性あり

//        if (spellCard == null)
//        {
//            return;
//        }
//        spellCard.UseSpellTo(target);



//    }
//}
