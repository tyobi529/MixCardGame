//using System.Collections;
//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;

////攻撃される側
//public class AttackedHero : MonoBehaviour, IDropHandler
//{
//    public void OnDrop(PointerEventData eventData)
//    {
//        //attackerカードを選択
//        CardController attacker = eventData.pointerDrag.GetComponent<CardController>();


//        if (attacker == null)
//        {
//            return;
//        }

//        //敵フィールドにシールドカードがあれば攻撃できない
//        CardController[] enemyFieldCards = GameManager.instance.GetEnemyFieldCards();
//        if (Array.Exists(enemyFieldCards, card => card.model.ability == ABILITY.SHIELD))
//        {
//            return;
//        }

//        if (attacker.model.canAttack)
//        {
//            //attackerがheroに攻撃する
//            GameManager.instance.AttackToHero(attacker, true);
//            GameManager.instance.CheckHeroHP();

//        }


//    }
//}
