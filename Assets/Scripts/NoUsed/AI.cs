//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;

//public class AI : MonoBehaviour
//{
//    GameManager gameManager;

//    private void Start()
//    {
//        gameManager = GameManager.instance;    
//    }
//    public IEnumerator EnemyTurn()
//    {
//        Debug.Log("Enemyのターン");
//        CardController[] enemyFieldCardList = gameManager.enemyFieldTransform.GetComponentsInChildren<CardController>();

//        gameManager.SettingCanAttackView(enemyFieldCardList, true);


//        yield return new WaitForSeconds(1);

//        //手札のカードリストを取得
//        CardController[] handCardList = gameManager.enemyHandTransform.GetComponentsInChildren<CardController>();

//        //コスト以下のカードがあれば、カードをフィールドに出し続ける
//        while (Array.Exists(handCardList, card => card.model.cost <= gameManager.enemy.manaCost))
//        {
//            //コスト以下のカードリストを取得
//            CardController[] selectableHandCardList = Array.FindAll(handCardList, card => card.model.cost <= gameManager.enemy.manaCost);
//            //場に出すカードを選択
//            CardController enemyCard = selectableHandCardList[0];
//            //カードを移動
//            StartCoroutine(enemyCard.movement.MoveToField(gameManager.enemyFieldTransform));

//            enemyCard.OnField(false);
//            handCardList = gameManager.enemyHandTransform.GetComponentsInChildren<CardController>();
//            yield return new WaitForSeconds(1);


//        }

//        yield return new WaitForSeconds(1);






//        //攻撃
//        //フィールドのカードリストを取得
//        CardController[] fieldCardList = gameManager.enemyFieldTransform.GetComponentsInChildren<CardController>();

//        //攻撃可能カードがあれば攻撃を繰り返す
//        while (Array.Exists(fieldCardList, card => card.model.canAttack))
//        {
//            //攻撃可能カードを取得
//            CardController[] enemyCanAttackCardList = Array.FindAll(fieldCardList, card => card.model.canAttack);
//            CardController[] playerFieldCardList = gameManager.playerFieldTransform.GetComponentsInChildren<CardController>();


//            CardController attacker = enemyCanAttackCardList[0];
//            //defenderカードを選択
//            if (playerFieldCardList.Length > 0)
//            {
//                //シールドカードのみ攻撃対象にする
//                if (Array.Exists(playerFieldCardList, card => card.model.ability == ABILITY.SHIELD))
//                {
//                    playerFieldCardList = Array.FindAll(playerFieldCardList, card => card.model.ability == ABILITY.SHIELD);
//                }
//                CardController defender = playerFieldCardList[0];
//                //attackerとdefenderを戦わせる
//                StartCoroutine(attacker.movement.MoveToTarget(defender.transform));
//                yield return new WaitForSeconds(0.51f);
//                gameManager.CardsBattle(attacker, defender);
//            }
//            else
//            {
//                StartCoroutine(attacker.movement.MoveToTarget(gameManager.playerHero));
//                yield return new WaitForSeconds(0.25f);
//                gameManager.AttackToHero(attacker, false);
//                yield return new WaitForSeconds(0.25f);
//                gameManager.CheckHeroHP();

//            }
//            fieldCardList = gameManager.enemyFieldTransform.GetComponentsInChildren<CardController>();
//            yield return new WaitForSeconds(1);


//        }



//        yield return new WaitForSeconds(1);


//        gameManager.ChangeTurn();

//    }
//}
