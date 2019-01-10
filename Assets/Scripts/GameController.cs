using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController> {
    
    public DeckController mainDeckController;
    public DeckController workingDeckController;
    public DeckController playerDeckController;

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


    public void OnCardClick(IDType id, DeckController cardOwner)
    {
        DeckController targetDeck;
        DetermineTargetDeck(cardOwner, out targetDeck);
        
        if(targetDeck.IsEmpty() || targetDeck == playerDeckController)
         {
             MoveCard(cardOwner, targetDeck, id);
             return;
         }

         if(CanCoverCard(cardOwner.GetCard(id), targetDeck.GetTopCard()))
             MoveCard(cardOwner, targetDeck, id);
         else
             Debug.Log("Card can`t be covered");
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

    private bool CanCoverCard(Card coverer, Card willCovered)
    {
        return coverer.cardSuit == willCovered.cardSuit;

       /* if(willCovered.GetPreferences().Contains(Preference.None))
            return IsSuitEqual(coverer, willCovered);

        if(willCovered.GetPreferences().Contains(Preference.)*/
    }

    private bool IsSuitEqual(Card firstCard, Card secondCard)
    {
        return firstCard.cardSuit == secondCard.cardSuit;
    }

    private void DetermineTargetDeck(DeckController controller, out DeckController targetController)
    {
        switch(controller.owner)
        {
            case DeckOwner.Main:
                targetController = playerDeckController;
                break;
            case DeckOwner.Player:
                targetController = workingDeckController;
                break;
            case DeckOwner.Working:
                targetController = mainDeckController;
                break;
            default:
                Debug.Log("Deck controller is not setted");
                targetController = null;
                break;
        }
    }
    
}
