using System.Collections.Generic;
using UnityEngine;
public class Card : MonoBehaviour
{
    [SerializeField] private IDType idType;
    public IDType Identifier
    {
        get { return idType; }
        set { idType = new IDType(value.ID); }
    }

    [SerializeField] private CardSuit _suit;
    public CardSuit Suit
    {
        get { return _suit; }
        set { _suit = value; }
    }

    [SerializeField] private CardName _name;
    public CardName Name
    {
        get { return _name; }
        set { _name = value; }
    }

    [SerializeField] private CardPoints _points;
    public CardPoints Points
    {
        get { return _points; }
        set { _points = value; }
    }

    public Card(CardSuit cardSuit, CardName cardName, CardPoints cardPoints)
    {
        Suit = cardSuit;
        Name = cardName;
        Points = cardPoints;
    }
    
    public override string ToString()
    {
        return string.Format("{0} {1} {2}", Name.ToString(), Suit.ToString(), Points.ToString());
    }
}

