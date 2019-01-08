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
        /*if(card == null)
            Debug.Log("NUll ID: " + id.ID);*/
        return card;
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

	
	// Update is called once per frame
	void Update () {
		
	}
}
