using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController> {
    
    public DeckController mainDeckController;
    public DeckController workingDeckController;

    public Deck mainDeck { get; private set; }


    private void Awake()
    {
        mainDeck = new Deck(true);
    }

    // Use this for initialization
    void Start () {
        mainDeckController.deck = mainDeck;
    }

    public void Generate()
    {
        UIController.Instance.InitializeCardsUI(mainDeckController, mainDeck);
    }

    public void Shuffle(DeckController deckController)
    {
        deckController.Shuffle();
        UIController.Instance.InitializeCardsUI(deckController, deckController.GetDeck());
    }


    public void OnCardClick(IDType id)
    {
        MoveCard(mainDeckController, workingDeckController, id);
    }

    public void GetTopCard()
    {
        Debug.Log(workingDeckController.GetTopCard().ToString());
    }

    private void MoveCard(DeckController fromDeck, DeckController toDeck, IDType id)
    {
        UIController.Instance.AddCard(toDeck, fromDeck.GetCard(id));
        toDeck.AddCard(fromDeck.GetCard(id));
        fromDeck.RemoveCard(id);
        UIController.Instance.RemoveCard(fromDeck, id);
    }

}
