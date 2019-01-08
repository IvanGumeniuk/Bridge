using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController> {
    
    public DeckController leftDeck;
    public DeckController rightDeck;

    public Deck mainDeck { get; private set; }


    private void Awake()
    {
        mainDeck = new Deck(true);
    }

    // Use this for initialization
    void Start () {
        leftDeck.deck = mainDeck;
    }

    public void Generate()
    {
        UIController.Instance.InitializeCardsUI(leftDeck, mainDeck);
    }

    public void Shuffle(DeckController deckController)
    {
        deckController.Shuffle();
        UIController.Instance.InitializeCardsUI(deckController, deckController.GetDeck());
    }


    public void OnCardClick(IDType id)
    {
        UIController.Instance.AddCard(rightDeck, leftDeck.GetCard(id));
        rightDeck.AddCard(leftDeck.GetCard(id));
        leftDeck.RemoveCard(id);
        UIController.Instance.RemoveCard(leftDeck, id);

        Debug.Log("Count main: " + mainDeck.GetCards().Count);
        Debug.Log("Count left: " + leftDeck.GetDeck().GetCards().Count);
        Debug.Log("Count right: " + rightDeck.GetDeck().GetCards().Count);
    }

}
