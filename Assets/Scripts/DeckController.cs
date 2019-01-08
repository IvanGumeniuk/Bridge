using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckController : MonoBehaviour {

    public Deck deck { get; set; }

    private void Awake()
    {
        deck = new Deck(false);
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
        deck.AddCard(new IDType(deck.GetCards().Count - 1), card);
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

    public void OnCardClick(IDType id)
    { 
        GameController.Instance.OnCardClick(id);
    }

}
