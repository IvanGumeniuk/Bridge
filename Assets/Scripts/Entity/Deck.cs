using System;
using System.Collections.Generic;
using UnityEngine;
public class Deck
{
    private Dictionary<IDType, Card> deck;

    public Deck(bool generateCards)
    {
        deck = new Dictionary<IDType, Card>();
        if (generateCards)
            GenerateDeck();
    }

    public Deck(Dictionary<IDType, Card> cards)
    {
        deck = cards;
    }

    public void AddCard(IDType cardIndex, Card card)
    {
        if (deck == null)
            deck = new Dictionary<IDType, Card>();
        deck.Add(cardIndex, card);
    }

    public Card GetCard(IDType index)
    {
        return deck[index];
    }

    public Dictionary<IDType, Card> GetCards()
    {
        return deck;
    }

    public System.Text.StringBuilder DeckToString()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        foreach (var card in deck)
            builder.AppendLine(card.ToString());
        return builder;
    }

    private void GenerateDeck()
    {
        for (int suit = 0, cardID = 0; suit < Enum.GetValues(typeof(CardSuit)).Length; suit++)
        {
            for (int name = 0; name < Enum.GetValues(typeof(CardName)).Length; name++)
            {
                CardSuit cardSuit = (CardSuit)Enum.GetValues(typeof(CardSuit)).GetValue(suit);
                CardName cardName = (CardName)Enum.GetValues(typeof(CardName)).GetValue(name);
                CardPoints cardPoints = GetPoints(cardName);

                Card card = new Card(cardSuit, cardName, cardPoints);
                
                card.Identifier = new IDType(cardID);
                deck.Add(new IDType(cardID), card);

                cardID++;
            }
        }
    }

    private CardPoints GetPoints(CardName name)
    {
        switch (name)
        {
            case CardName.Six:
            case CardName.Seven:
            case CardName.Eigth:
            case CardName.Nine:
                return CardPoints.Zero;
            case CardName.Ten:
            case CardName.Queen:
            case CardName.King:
                return CardPoints.Ten;
            case CardName.Ace:
                return CardPoints.Fifteen;
            case CardName.Jack:
                return CardPoints.TwentyFive;
            default:
                return CardPoints.Zero;
        }
    }


}




