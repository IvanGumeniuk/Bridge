using System;
using System.Collections.Generic;
using UnityEngine;
public class Deck 
{
    private Dictionary<int,Card> deck;

    public Deck(bool generateCards)
    {
        deck = new Dictionary<int, Card>();
        if(generateCards)
            GenerateDeck();
    }

    public Deck(Dictionary<int, Card> cards)
    {
        deck = cards;
    }

    public void AddCard(int cardIndex, Card card)
    {
        if(deck == null)
            deck = new Dictionary<int, Card>();
        deck.Add(cardIndex,card);
    }

    public Card GetCard(int index)
    {
        return deck[index];
    }

    public Card GetCard(IDType type)
    {
        Card card = null;
        foreach(var c in deck.Values)
        {
            if(c.Identifier.ID == type.ID)
            {
                card = c;
                break;
            }
        }
        return card;
    }

    public Dictionary<int, Card> GetCards()
    {
        return deck;
    }

    public System.Text.StringBuilder DeckToString()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        foreach(var card in deck)
            builder.AppendLine(card.ToString());
        return builder;
    }

    private void GenerateDeck()
    {
        for(int suitIndex = 0, cardID = 0; suitIndex < Enum.GetValues(typeof(Suit)).Length; suitIndex++)
        {
            for(int nameIndex = 0; nameIndex < Enum.GetValues(typeof(Name)).Length; nameIndex++)
            {
                Card card = new Card((Suit)Enum.GetValues(typeof(Suit)).GetValue(suitIndex), (Name)Enum.GetValues(typeof(Name)).GetValue(nameIndex));
                card.Identifier = new IDType(cardID);
                deck.Add(cardID++, card);
            }
        }
        GenerateCardPreferences();
    }

    private void GenerateCardPreferences()
    {
        for(int cardIndex=0; cardIndex < deck.Count; cardIndex++)
        {
            Card card = deck[cardIndex];
            switch(card.cardName)
            {
                case Name.Six:
                    card.AddReference(Preference.Cover);
                    card.cardPoints = Points.Zero;
                    break;
                case Name.Seven:
                    card.AddReference(Preference.TakeTwo);
                    card.cardPoints = Points.Zero;
                    break;
                case Name.Eigth:
                    card.AddReference(Preference.TakeOne);
                    card.AddReference(Preference.SkipMove);
                    card.cardPoints = Points.Zero;
                    break;
                case Name.Nine:
                    card.AddReference(Preference.CoverAnyCard);
                    card.cardPoints = Points.Zero;
                    break;
                case Name.Jack:
                    card.AddReference(Preference.OnAnyCard);
                    card.AddReference(Preference.SetMainSuit);
                    card.AddReference(Preference.MultiplyScore);
                    card.cardPoints = Points.TwentyFive;
                    break;
                case Name.Ace:
                    card.AddReference(Preference.SkipMove);
                    card.cardPoints = Points.Fifteen;
                    break;
                case Name.Ten:
                case Name.Queen:
                case Name.King:
                    card.AddReference(Preference.None);
                    card.cardPoints = Points.Ten;
                    break;
                default:
                    Debug.Log("Unknown card");
                    break;
            }
        }
    }


}

