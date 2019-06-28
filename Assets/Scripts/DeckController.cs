using System.Linq;
using UnityEngine;

public class DeckController : MonoBehaviour {

    public Deck deck { get; set; }
    public DeckOwner currentOwner;
    public DeckOwner previousOwner;
    public Player player; 

    private void Awake()
    {
        deck = new Deck(false);
        player = GetComponent<Player>();
    }

    public Deck GetDeck()
    {
        return deck;
    }
 
    public void Shuffle()
    {
        System.Random rand = new System.Random();
        deck = new Deck(deck.GetCards().OrderBy(x => rand.Next()).ToDictionary(item => item.Key, item => item.Value));
    }
    
    public Card GetCard(IDType id)
    {
        Card card = deck.GetCard(id);
        return card;
    }

    public Card GetTopCard()
    {
        if(transform.childCount == 0)
        {
            Debug.Log("No avaliable children");
            return null;
        }
        return transform.GetChild(transform.childCount - 1).GetComponent<Card>();
    }


    public void AddCard(IDType cardKey, Card card)
    {
        deck.AddCard(cardKey, card);
    }

    public void AddCard(Card card)
    {
        deck.AddCard(card.Identifier, card);
    }

    public void AddCard(Card card, DeckOwner previous)
    {
        previousOwner = previous;
        deck.AddCard(card.Identifier, card);
    }

    public void RemoveCard(IDType id)
    {
        if(deck.GetCards().ContainsKey(id))
        {
            deck.GetCards().Remove(id);
        }
        else
        {
            Debug.Log("Wrong removing ID: " + id);
        }
    }

    public void OnCardClick(IDType cardID)
    { 
        GameController.Instance.OnCardClick(cardID, this, currentOwner, GetID());
    }

    private int GetID()
    {
        int errorID = -1;
        switch (currentOwner)
        {
            case DeckOwner.Player:
                if(player == null)
                {
                    Debug.Log("Player == null when owner == DeckOwner.Player");
                    return errorID;
                }
                return player.playerID;
            case DeckOwner.MainDeck:
                return 1000;
            case DeckOwner.WorkingDeck:
                return 1001;
            default:
                return errorID;
        }
    }

    public bool IsEmpty()
    {
        return deck.GetCards().Count == 0;
    }

    public int CardsCount
    {
        get { return deck.GetCards().Count; }
    }
}
