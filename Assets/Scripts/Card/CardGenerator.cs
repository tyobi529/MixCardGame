using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGenerator : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject eatCardPrefab;

    public CardController CreateCard(bool isDish, int cardID, int cost, bool isRare, Transform position)
    {

        GameObject card = Instantiate(cardPrefab, position, false);


        card.GetComponent<CardController>().Init(isDish, cardID, cost, isRare);

        return card.GetComponent<CardController>();

    }

    public CardController CreateEatCard(bool isDish, int cardID, int cost, bool isRare, Transform position)
    {
        GameObject card = Instantiate(eatCardPrefab, position, false);

        card.GetComponent<CardController>().Init(isDish, cardID, cost, isRare);

        return card.GetComponent<CardController>();
    }


    //合成カードの番号
    public int SpecialMix(CardController card_0, CardController card_1)
    {
        int specialMixID = -1;

        for (int i = 0; i < card_0.model.partnerID.Length; i++)
        {
            if (card_0.model.partnerID[i] == card_1.model.cardID)
            {
                specialMixID = card_0.model.specialMixID[i];
                break;

            }
        }

        return specialMixID;
    }
}
