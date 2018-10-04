using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardGenerator : MonoBehaviour {
    public static CardGenerator instance;

    public HandDeck firstPlayerHandDeck;
    public HandDeck secondPlayerHandDeck;

    public bool areCardsGenerated = false;

    private void Start()
    {
        instance = GetComponent<CardGenerator>();
    }

    private void Update()
    {
        //Check if dealer is ready
        if (Dealer.instance.isDealingDone && !areCardsGenerated)
        {
            GenerateCards(firstPlayerHandDeck, secondPlayerHandDeck);
        }
    }

    public static void GenerateCard(HandDeck handDeck, Vector2 position)
    {
        var dealerDeck = Dealer.instance.dealerDeck;

        //Check if dealer has any cards in his deck
        if (!dealerDeck.Any())
        {
            Debug.Log("No more cards left!");
            return;
        }
        
        //Generate random index and card
        int randomIndex = Random.Range(0, dealerDeck.Count);

        var cardToGenerate = dealerDeck[randomIndex];

        if(dealerDeck.Count != 1)
        {
            if (cardToGenerate == Dealer.instance.trumpCard)
            {
                randomIndex = Random.Range(0, dealerDeck.Count);

                cardToGenerate = dealerDeck[randomIndex];
            }
            //Add generated card to the player hand deck
            handDeck.cards.Add(cardToGenerate);

            //Remove the card from dealer's deck
            dealerDeck.Remove(cardToGenerate);

            //Instantiate generated card
            Instantiate(cardToGenerate, position, Quaternion.identity);
        }
        else
        {
            //Add generated card to the player hand deck
            handDeck.cards.Add(cardToGenerate);

            //Remove the card from dealer's deck
            dealerDeck.Remove(cardToGenerate);

            //Instantiate generated card
            Instantiate(cardToGenerate, position, Quaternion.identity);
        }
    }


    private void GenerateCards(HandDeck firstPlayerHandDeck, HandDeck secondPlayerHandDeck)
    {
        //Instantiate player cards
        for (int i = 0; i < firstPlayerHandDeck.cards.Count; i++)
        {
            Instantiate(firstPlayerHandDeck.cards[i], new Vector2(firstPlayerHandDeck.gameObject.transform.
                position.x + i, firstPlayerHandDeck.gameObject.transform.
                position.y), Quaternion.identity);
        }
        //Instantiate computer cards
        for (int i = 0; i < secondPlayerHandDeck.cards.Count; i++)
        {
            Instantiate(secondPlayerHandDeck.cards[i], new Vector2(secondPlayerHandDeck.gameObject.transform.
                position.x + i, secondPlayerHandDeck.gameObject.transform.
                position.y), Quaternion.identity);
            secondPlayerHandDeck.cards[i].transform.position = new Vector2(secondPlayerHandDeck.gameObject.transform.
                position.x + i, secondPlayerHandDeck.gameObject.transform.
                position.y);
        }
        areCardsGenerated = true;
    }
}
