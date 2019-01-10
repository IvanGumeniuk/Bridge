using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public DeckController mainDeckController;
    public DeckController workingDeckController;
    public DeckController firstPlayerDeckController;
    public DeckController secondPlayerDeckController;

    private Player first;
    private Player second;

    public Deck mainDeck { get; private set; }

    [SerializeField]
    private Suit trumpSuit;

    private void Awake()
    {
        mainDeck = new Deck(true);
    }

    // Use this for initialization
    void Start()
    {
        mainDeckController.deck = mainDeck;

        first = firstPlayerDeckController.transform.GetComponent<Player>();
        second = secondPlayerDeckController.transform.GetComponent<Player>();
        first.IsMoving = true;

        Generate();
        Shuffle(mainDeckController);
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
        Player current = cardOwner.transform.GetComponent<Player>();
        if(current != null)
        {
            if(!current.IsMoving)
                return;
        }

        DeckController targetDeck;
        DetermineTargetDeck(cardOwner, out targetDeck);

        if(targetDeck.IsEmpty() || cardOwner.owner == DeckOwner.Main)
        {
            MoveCard(cardOwner, targetDeck, id);
            if(NeedChangeTrumpSuit(targetDeck.GetTopCard(), targetDeck))
            {
                ShowSuits();
            }
            return;
        }

        if(CanCoverCard(cardOwner.GetCard(id), targetDeck.GetTopCard()))
        {
            MoveCard(cardOwner, targetDeck, id);

            //changing suit
            if(NeedChangeTrumpSuit(targetDeck.GetTopCard(), targetDeck))
            {
                ShowSuits();
                return;
            }
            else
            {
                if(UIController.Instance.MiniTrumpSuitActive)
                    UIController.Instance.ChangeEnableStateMiniTrumpWindow(false);
            }


            if(targetDeck.GetTopCard().HasPreference(Preference.None))
                EndMove();
        }
        else
            Debug.Log("Card can`t be covered");
    }

    private bool NeedChangeTrumpSuit(Card card, DeckController targetDeck)
    {
        return card.HasPreference(Preference.SetMainSuit) && targetDeck.owner == DeckOwner.Working;
    }

    public void SetSuit(Suit suit)
    {
        trumpSuit = suit;
        EndMove();
    }

    private void ShowSuits()
    {
        UIController.Instance.ChangeEnableStateTrumpWindow(true);
        UIController.Instance.ChangeEnableStateMiniTrumpWindow(true);
    }

    private void EndMove()
    {
        first.IsMoving = !first.IsMoving;
        second.IsMoving = !second.IsMoving;
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
        if(coverer.HasPreference(Preference.OnAnyCard))
            return true;

        if(willCovered.HasPreference(Preference.SetMainSuit))
            return coverer.cardSuit == trumpSuit;

        if(willCovered.HasPreference(Preference.CoverAnyCard))
            return true;

        if(IsSuitEqual(coverer, willCovered))
            return true;

        if(IsNameEqual(coverer, willCovered))
            return true;

        return false;
    }

    private bool IsSuitEqual(Card firstCard, Card secondCard)
    {
        return firstCard.cardSuit == secondCard.cardSuit;
    }

    private bool IsNameEqual(Card firstCard, Card secondCard)
    {
        return firstCard.cardName == secondCard.cardName;
    }

    private void DetermineTargetDeck(DeckController controller, out DeckController targetController)
    {
        switch(controller.owner)
        {
            case DeckOwner.Main:
                if(UIController.Instance.ToFirst())
                    targetController = firstPlayerDeckController;
                else
                    targetController = secondPlayerDeckController;
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
