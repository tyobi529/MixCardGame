using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour
{
    public CardView view; //見かけに関することを操作（view）
    public CardModel model; //データ（model）関することを操作
                            //public CardMovement movement; //移動（movement）関することを操作

    //Transform field1Transform, field2Transform, resultFieldTransform;

    private void Awake()
    {
        view = GetComponent<CardView>();
        //movement = GetComponent<CardMovement>();

    }

    //public void Init(int cardID, int ID, bool isMix)
    public void Init(KIND kind, int cardID)
    {
        model = new CardModel(kind, cardID);
        view.SetCard(model);

    }

    public void SelectCard()
    {

        if (GameManager.instance.isMyTurn)
        {

            model.isSelect = !model.isSelect;

            if (model.isSelect)
            {
                view.selectPanel.SetActive(true);

                if (GameManager.instance.selectCard[0] == null)
                {
                    GameManager.instance.selectCard[0] = this;
                }
                else if (GameManager.instance.selectCard[1] == null)
                {
                    GameManager.instance.selectCard[1] = this;
                }
                else
                {
                    GameManager.instance.selectCard[1].model.isSelect = false;
                    GameManager.instance.selectCard[1].view.selectPanel.SetActive(false);
                    GameManager.instance.selectCard[1] = this;
                }
            }
            else
            {
                view.selectPanel.SetActive(false);

                if (GameManager.instance.selectCard[0] == this)
                {

                    if (GameManager.instance.selectCard[1] == null)
                    {
                        GameManager.instance.selectCard[0] = null;

                    }
                    else
                    {
                        GameManager.instance.selectCard[0] = GameManager.instance.selectCard[1];
                        GameManager.instance.selectCard[1] = null;
                    }
                   
                }
                else if (GameManager.instance.selectCard[1] == this)
                {
                    GameManager.instance.selectCard[1] = null;
                }
            }

            GameManager.instance.GenerateFieldCard();
        }
    }

    

    //クリックしたときの処理
    void OnMouseDown()
    {
        Debug.Log("セレクト");

        if (GameManager.instance.isMyTurn)
        {

            model.isSelect = !model.isSelect;

            if (model.isSelect)
            {
                view.selectPanel.SetActive(true);

                if (GameManager.instance.selectCard[0] == null)
                {
                    GameManager.instance.selectCard[0] = this;
                }
                else if (GameManager.instance.selectCard[1] == null)
                {
                    GameManager.instance.selectCard[1] = this;
                }
                else
                {
                    GameManager.instance.selectCard[1].model.isSelect = false;
                    GameManager.instance.selectCard[1] = this;
                }
            }
            else
            {
                view.selectPanel.SetActive(false);

                if (GameManager.instance.selectCard[0] == this)
                {
                    GameManager.instance.selectCard[0] = null;

                    GameManager.instance.selectCard[1].model.isSelect = false;
                    GameManager.instance.selectCard[1].view.selectPanel.SetActive(false);
                    GameManager.instance.selectCard[1] = null;
                }
                else if (GameManager.instance.selectCard[1] == this)
                {
                    GameManager.instance.selectCard[1] = null;
                }
            }

        }
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


   




}
