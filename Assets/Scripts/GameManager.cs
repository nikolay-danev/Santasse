using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public HandDeck playerHandDeck;
    public HandDeck computerHandDeck;

    public Text playerPoints;
    public int playerPointsSum;
    public Text computerPoints;
    public int computerPointsSum;

    public Text playerScore;
    public int playerScoreSum;
    public Text computerScore;
    public int computerScoreSum;

    public GameObject playerCard = null;
    public GameObject computerCard = null;

    public GameObject trumpCard;

    public bool isPlayerTurn = true;
    public bool areScoresEnough = false;
    public bool isWinner = false;

    public Text winText;
    public Button replay;

    private void Start()
    {
        winText.enabled = false;
        replay.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerTurn)
        {
            PickAndSetCard(playerHandDeck);
        }
        else
        {
            ComputerPickAndSetCard(computerHandDeck);
        }
        if (playerCard != null && computerCard != null)
        {
            GetScore(playerScore, computerScore);
            playerCard = null;
            computerCard = null;
        }
        if (!isWinner)
        {
            CheckIfThereIsAWinner(playerScoreSum, computerScoreSum);
        }
        if(!areScoresEnough && playerHandDeck.cards.Count == 0 && computerHandDeck.cards.Count == 0)
        {
            if(playerScoreSum > computerScoreSum)
            {
                Debug.Log("Player wins!");
            }
            else if(computerScoreSum > playerScoreSum)
            {
                Debug.Log("Computer wins!");
            }
        }
    }

    private void CheckIfThereIsAWinner(int playerScoreSum, int computerScoreSum)
    {
        if(playerScoreSum >= 66)
        {
            Debug.Log("Player wins!");
            if(computerScoreSum >= 33)
            {
                playerPointsSum += 1;
            }
            else
            {
                playerPointsSum += 2;
            }
            areScoresEnough = true;
        }
        else if (computerScoreSum >= 66)
        {
            Debug.Log("Computer wins!");
            if (playerPointsSum >= 33)
            {
                computerPointsSum += 1;
            }
            else
            {
                computerPointsSum += 2;
            }
            areScoresEnough = true;
        }
        if (areScoresEnough)
        {

            playerPoints.text = playerPointsSum.ToString();
            computerPoints.text = computerPointsSum.ToString();

            playerScoreSum = 0;
            computerScoreSum = 0;
            computerScore.text = computerScoreSum.ToString();
            playerScore.text = playerScoreSum.ToString();
            isWinner = true;
            Reload();
        }
        if(playerPointsSum >= 11)
        {
            winText.enabled = true;
            winText.text = "Congratulations, you won! Wanna play again?";
            replay.gameObject.SetActive(true);
        }
        else if(computerPointsSum >= 11)
        {
            winText.enabled = true;
            winText.text = "Game over! Wanna play again?";
            replay.gameObject.SetActive(true);
        }
    }

    private void Reload()
    {
        var remainingCards = GameObject.FindGameObjectsWithTag("Card");
        foreach (var card in remainingCards)
        {
            Destroy(card);
        }
        playerHandDeck.cards.Clear();
        computerHandDeck.cards.Clear();
        playerScoreSum = 0;
        computerScoreSum = 0;
        areScoresEnough = false;
        isWinner = false;
        Dealer.instance.dealerDeck.Clear();
        Dealer.instance.isDealingDone = false;
        CardGenerator.instance.areCardsGenerated = false;
    }

    private void GetScore(Text playerScore, Text computerScore)
    {
        var playerCardPower = GetCardPower(playerCard);
        var computerCardPower = GetCardPower(computerCard);
        var playerCardName = playerCard.name.Split(' ')[0];
        var computerCardName = computerCard.name.Split(' ')[0];
        var trumpCardName = Dealer.instance.trumpCard.name.Split(' ')[0];

        if (playerCardName == computerCardName)
        {
            if (playerCardPower > computerCardPower)
            {
                AddPlayerScore(playerScore, playerCardPower, computerCardPower);
            }
            else
            {
                AddComputerScore(computerScore, playerCardPower, computerCardPower);
            }
        }
        else if(playerCardName != computerCardName && (playerCardName != trumpCardName && computerCardName != trumpCardName))
        {
            if (isPlayerTurn)
            {
                AddPlayerScore(playerScore, playerCardPower, computerCardPower);
            }
            else
            {
                AddComputerScore(computerScore, playerCardPower, computerCardPower);
            }
        }
        else if(playerCardName != computerCardName && (playerCardName == trumpCardName || computerCardName == trumpCardName))
        {
            if(playerCardName == trumpCardName)
            {
                AddPlayerScore(playerScore, playerCardPower, computerCardPower);
            }
            else
            {
                AddComputerScore(computerScore, playerCardPower, computerCardPower);
            }
        }
        Destroy(playerCard);
        Destroy(computerCard);
    }

    private void AddComputerScore(Text computerScore, int playerCardPower, int computerCardPower)
    {
        computerScoreSum += playerCardPower + computerCardPower;
        computerScore.text = computerScoreSum.ToString();
        isPlayerTurn = false;
    }

    private void AddPlayerScore(Text playerScore, int playerCardPower, int computerCardPower)
    {
        playerScoreSum += playerCardPower + computerCardPower;
        playerScore.text = playerScoreSum.ToString();
        isPlayerTurn = true;
    }

    public static int GetCardPower(GameObject playerCard)
    {
        var splitName = playerCard.name.Split(' ');
        var index = splitName[1].IndexOf('(');
        var power = splitName[1].Substring(0, index);
        return int.Parse(power);
    }

    private void ComputerPickAndSetCard(HandDeck computerHandDeck)
    {
        if (playerCard != null && computerHandDeck.ContainsCard(playerCard.name.Split(' ')[0]))
        {
            GameObject card = computerHandDeck.cards.Where(c => c.gameObject.name.Split(' ')[0] == playerCard.gameObject.name.Split(' ')[0]).FirstOrDefault();

            var oldPos = card.transform.position;
            computerCard = GameObject.Find(card.gameObject.name + "(Clone)");

            computerCard.transform.position = new Vector2(2.5f, 1);

            computerHandDeck.cards.Remove(card);
            var cardName = computerCard.name.Split(' ')[0];
            var cardPower = GetCardPower(computerCard);
            if (CheckIfHaveTwenty(playerHandDeck, cardName, cardPower))
            {
                computerScoreSum += 20;
            }
            CardGenerator.GenerateCard(computerHandDeck, oldPos);
        }
        else
        {
            if (computerHandDeck.ContainsCard(Dealer.instance.trumpCard.name.Split(' ')[0]))
            {
                GameObject card = computerHandDeck.cards.Where(c => c.gameObject.name.Split(' ')[0] == Dealer.instance.trumpCard.name.Split(' ')[0]).FirstOrDefault();

                var oldPos = card.transform.position;
                computerCard = GameObject.Find(card.gameObject.name + "(Clone)");

                computerCard.transform.position = new Vector2(2.5f, 1);

                computerHandDeck.cards.Remove(card);
                var cardName = computerCard.name.Split(' ')[0];
                var cardPower = GetCardPower(computerCard);
                if (CheckIfHaveTwenty(playerHandDeck, cardName, cardPower))
                {
                    computerScoreSum += 20;
                }
                CardGenerator.GenerateCard(computerHandDeck, oldPos);
                isPlayerTurn = true;
            }
            else
            {
                int random = Random.Range(0, computerHandDeck.cards.Count);
                GameObject card = computerHandDeck.cards[random];

                var oldPos = card.transform.position;
                computerCard = GameObject.Find(card.gameObject.name + "(Clone)");

                computerCard.transform.position = new Vector2(2.5f, 1);

                computerHandDeck.cards.Remove(card);

                var cardName = computerCard.name.Split(' ')[0];
                var cardPower = GetCardPower(computerCard);
                if (CheckIfHaveTwenty(playerHandDeck, cardName, cardPower))
                {
                    computerScoreSum += 20;
                }
                CardGenerator.GenerateCard(computerHandDeck, oldPos);
                isPlayerTurn = true;
            }
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }
    private void PickAndSetCard(HandDeck playerHandDeck)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                var card = hit.collider.gameObject;
                var cardName = card.name.Split(' ')[0];
                var cardPower = GetCardPower(card);

                var oldPos = card.transform.position;
                playerCard = card;
                card.transform.position = new Vector2(2.5f, -1);

                playerHandDeck.cards.Remove(card);

                if (CheckIfHaveTwenty(playerHandDeck, cardName, cardPower))
                {
                    playerScoreSum += 20;
                }

                CardGenerator.GenerateCard(playerHandDeck, oldPos);
            }
            isPlayerTurn = false;
        }
    }

    private static bool CheckIfHaveTwenty(HandDeck playerHandDeck, string cardName, int cardPower)
    {
        var pairCard = playerHandDeck.cards.Where(pc => pc.name.Split(' ')[0] == cardName).ToArray();
        foreach (var pc in pairCard)
        {
            var pairPower = int.Parse(pc.name.Split(' ')[1]);
            if (pairPower == 3 || pairPower == 4) return true;
        }
        return false;
    }
}
