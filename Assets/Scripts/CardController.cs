using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CardController : MonoBehaviourPunCallbacks
{
    public CardView view; //見かけに関することを操作（view）
    public CardModel model; //データ（model）関することを操作（view）
    public CardMovement movement; //移動（movement）関することを操作（view）


    private void Awake()
    {
        view = GetComponent<CardView>();
        movement = GetComponent<CardMovement>();


    }

    [PunRPC]
    public void Init_RPC(int cardID, int ID)
    {
        model = new CardModel(cardID, ID);
        view.SetCard(model);

        //手札に移動する
        if (model.playerID == 1)
        {
            this.gameObject.transform.SetParent(GameManager.instance.playerHandTransform);

        }
        else
        {
            this.gameObject.transform.SetParent(GameManager.instance.enemyHandTransform);
        }
    }

    public void Init(int cardID, int ID)
    {
        photonView.RPC(nameof(Init_RPC), RpcTarget.All, cardID, ID);
    }

    [PunRPC]
    void MoveToField_RPC()
    {

        if (model.playerID == 1)
            this.gameObject.transform.SetParent(GameManager.instance.playerFieldTransform);
        else
            this.gameObject.transform.SetParent(GameManager.instance.enemyFieldTransform);

    }

    public void MoveToField()
    {
        photonView.RPC(nameof(MoveToField_RPC), RpcTarget.Others);
    }


    public void Attack(CardController enemyCard)
    {
        //model.Attack(enemyCard);
        SetCanAttack(false);
    }

    public void SetCanAttack(bool canAttack)
    {
        //model.canAttack = canAttack;
        view.SetActiveSelectablePanel(canAttack);

    }


    public void OnField(bool isPlayer)
    {
        //GameManager.instance.ReduceManaCost(model.cost, isPlayer);
        model.isFieldCard = true;
        //if (model.ability == ABILITY.INIT_ATTACKABLE)
        //{
        //    SetCanAttack(true);
        //}
    }


    public void Destroy()
    {
        PhotonNetwork.Destroy(this.gameObject);

    }

    [PunRPC]
    void Show_RPC()
    {
        view.Show();
    }


    public void Show()
    {
        photonView.RPC(nameof(Show_RPC), RpcTarget.All);

    }



    public void Spell(int ID, int spellNum)
    {
        switch (spellNum)
        {
            case 1:
                photonView.RPC(nameof(Heal), RpcTarget.All, ID);
                break;
            case 2:
                photonView.RPC(nameof(PlusMixCost), RpcTarget.All, ID);
                break;
            default:
                break;
        }
    }

    [PunRPC]
    void Heal(int ID)
    {
        if (ID == 1)
            GameManager.instance.player.heroHp += 5;
        else
            GameManager.instance.enemy.heroHp += 5;

        Debug.Log("HPを５回復");
    }


    [PunRPC]
    void PlusMixCost(int ID)
    {
        if (ID == 1)
            GameManager.instance.player.mixCost += 2;
        else
            GameManager.instance.enemy.mixCost += 2;

        Debug.Log("合成コスト+2");
    }

    //public void CheckAlive()
    //{
    //    if (model.isAlive)
    //    {
    //        view.Refresh(model);
    //    }
    //    else
    //    {
    //        Destroy(this.gameObject);
    //    }
    //}


    //public void UseSpellTo(CardController target)
    //{
    //    switch (model.spell)
    //    {
    //        case SPELL.DAMAGE_ENEMY_CARD:
    //            //特定の敵を攻撃する
    //            Attack(target);
    //            target.CheckAlive();
    //            break;
    //        case SPELL.DAMAGE_ENEMY_CARDS:
    //            break;
    //        case SPELL.DAMAGE_ENEMY_HERO:
    //            break;
    //        case SPELL.HEAL_FRIEND_CARD:
    //            break;
    //        case SPELL.HEAL_FRIEND_CARDS:
    //            break;
    //        case SPELL.HEAL_FRIEND_HERO:
    //            break;
    //        case SPELL.NONE:
    //            return;
    //    }

    //    Destroy(this.gameObject);
    //}

}
