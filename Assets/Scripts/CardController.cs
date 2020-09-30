using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public CardView view; //見かけに関することを操作（view）
    public CardModel model; //データ（model）関することを操作
    public CardMovement movement; //移動（movement）関することを操作


    private void Awake()
    {
        view = GetComponent<CardView>();
        movement = GetComponent<CardMovement>();


    }

    //public void Init(int cardID, int ID, bool isMix)
    public void Init(int kind, int cardID, bool isMix)
    {
        model = new CardModel(kind, cardID, isMix);
        view.SetCard(model);

    }


    //public void Init(int cardID, int ID, bool isMix)
    //public void SpecialInit(int cardID, bool isMix)
    //{
    //    model = new CardModel(cardID, isMix);
    //    view.SetCard(model);

    //}


    //[PunRPC]
    //void MoveToField_RPC()
    //{

    //    if (model.playerID == 1)
    //        this.gameObject.transform.SetParent(GameManager.instance.playerFieldTransform);
    //    else
    //        this.gameObject.transform.SetParent(GameManager.instance.enemyFieldTransform);

    //}

    //public void MoveToField()
    //{
    //    photonView.RPC(nameof(MoveToField_RPC), RpcTarget.All);
    //}


    //[PunRPC]
    //void MoveToHand_RPC()
    //{

    //    if (model.playerID == 1)
    //        this.gameObject.transform.SetParent(GameManager.instance.playerHandTransform);
    //    else
    //        this.gameObject.transform.SetParent(GameManager.instance.enemyHandTransform);

    //}

    //public void MoveToHand()
    //{
    //    photonView.RPC(nameof(MoveToHand_RPC), RpcTarget.All);
    //}


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


    //public void Destroy()
    //{
    //    PhotonNetwork.Destroy(this.gameObject);

    //}

    //[PunRPC]
    //void Show_RPC()
    //{
    //    view.Show();
    //}


    //public void Show()
    //{
    //    photonView.RPC(nameof(Show_RPC), RpcTarget.All);

    //}



    public void Spell(int ID, int spellNum)
    {
        switch (spellNum)
        {
            case 0:
                Heal1(ID);
                break;
            case 1:
                Heal2(ID);
                break;
            case 2:
                PlusMixCost1(ID);
                break;
            case 3:
                PlusMixCost2(ID);
                break;
            default:
                break;
        }
    }




    //特殊カード

    void Heal1(int ID)
    {
        if (ID == 1)
            GameManager.instance.player.heroHp += 5;
        else
            GameManager.instance.enemy.heroHp += 5;

        Debug.Log("HPを５回復");

    }

    void Heal2(int ID)
    {
        if (ID == 1)
            GameManager.instance.player.heroHp += 10;
        else
            GameManager.instance.enemy.heroHp += 10;

        Debug.Log("HPを10回復");

    }


    void PlusMixCost1(int ID)
    {
        if (ID == 1)
            GameManager.instance.player.mixCost += 2;
        else
            GameManager.instance.enemy.mixCost += 2;

        Debug.Log("合成コスト+2");
    }


    void PlusMixCost2(int ID)
    {
        if (ID == 1)
            GameManager.instance.player.mixCost += 4;
        else
            GameManager.instance.enemy.mixCost += 4;

        Debug.Log("合成コスト+4");
    }






}
