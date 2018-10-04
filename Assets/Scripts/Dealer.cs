using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dealer : MonoBehaviour
{
    public static Dealer instance;

    public GameObject[] gameDeck;

    public IList<GameObject> dealerDeck;

    public GameObject trumpCard;

    public HandDeck firstPlayerHandDeck;
    public HandDeck secondPlayerHandDeck;

    public bool isDealingDone = false;

    private void Awake()
    {
        instance = this.GetComponent<Dealer>();
    }

    private IList<GameObject> InitializeDealerDeck(GameObject[] gameDeck)
    {
        var dealerDeck = new List<GameObject>();

        foreach (var deck in gameDeck)
        {
            foreach (var card in deck.GetComponentsInChildren<Transform>())
            {
                if (CheckCard(card))
                {
                    dealerDeck.Add(card.gameObject);
                }
            }
        }
        return dealerDeck;
    }

    private bool CheckCard(Transform card)
    {
        return card.gameObject.name.Split(' ').Length == 2;
    }

    private void Update()
    {
        if (!isDealingDone)
        {
            this.dealerDeck = InitializeDealerDeck(gameDeck);
            GiveFirstPlayerCards(dealerDeck, firstPlayerHandDeck);
            GiveSecondPlayerCards(dealerDeck, secondPlayerHandDeck);
            SetTrumpCard(dealerDeck,trumpCard);
            Debug.Log(firstPlayerHandDeck.cards.Count);
            Debug.Log(secondPlayerHandDeck.cards.Count);
        }
    }

    public void SetTrumpCard(IList<GameObject> dealerDeck, GameObject trumpCard)
    {
        int randomIndex = Random.Range(0, dealerDeck.Count);

        this.trumpCard = dealerDeck[randomIndex];

        Instantiate(this.trumpCard, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity);

        Debug.Log(this.trumpCard.name);
        isDealingDone = true;
    }

    private void GiveSecondPlayerCards(IList<GameObject> dealerDeck, HandDeck secondPlayerHandDeck)
    {
        for (int i = 0; i < 6; i++)
        {
            int randomIndex = Random.Range(0, dealerDeck.Count);

            var cardToAdd = dealerDeck[randomIndex];

            secondPlayerHandDeck.cards.Add(cardToAdd);

            dealerDeck.Remove(cardToAdd);
        }
    }

    private void GiveFirstPlayerCards(IList<GameObject> dealerDeck, HandDeck firstPlayerHandDeck)
    {
        for (int i = 0; i < 6; i++)
        {
            int randomIndex = Random.Range(0, dealerDeck.Count);

            var cardToAdd = dealerDeck[randomIndex];

            firstPlayerHandDeck.cards.Add(cardToAdd);

            dealerDeck.Remove(cardToAdd);
        }
    }
}
