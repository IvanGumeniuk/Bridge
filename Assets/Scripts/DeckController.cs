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


    public void AddCard(int cardIndex, Card card)
    {
        deck.AddCard(cardIndex, card);
    }

    public void AddCard(Card card)
    {
        deck.AddCard(deck.GetCards().Count - 1, card);
    }

    public void RemoveCard(IDType id)
    {
        for(int i = 0; i< deck.GetCards().Count; i++)
        {
            if(deck.GetCards().ContainsKey(i))
            {
                if(deck.GetCards()[i].Identifier.ID == id.ID)
                {
                    deck.GetCards().Remove(i);
                    break;
                }
            }
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
