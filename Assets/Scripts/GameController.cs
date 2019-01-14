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
        second.IsMoving = true;

        Generate();
        Shuffle(mainDeckController);

        StartCoroutine(Deal());
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
        //перевірка чи зараз хід гравця, який настиснув на карту
        Player current = cardOwner.transform.GetComponent<Player>();
        if(current != null)
        {
            if(!current.IsMoving)
                return;
        }

        // визначення цільової колоди, до якої має переміститись карта
        DeckController targetDeck;
        DetermineTargetDeck(cardOwner, out targetDeck);

        //
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

    public void GetTopCard()
    {
        Debug.Log(workingDeckController.GetTopCard().ToString());
    }

    public void SetSuit(Suit suit)
    {
        trumpSuit = suit;
        EndMove();
    }

    private IEnumerator Deal()
    {
        int totalCount = 9; //2 * players count - 1

        //time for initializing UI
        yield return new WaitForSeconds(1f);

        for(int cardCount=0; cardCount< totalCount; cardCount++)
        {   //replace with all players iteration 
            DeckController player = cardCount % 2 == 0 ? secondPlayerDeckController : firstPlayerDeckController;

            MoveCard(mainDeckController, player, mainDeckController.GetTopCard().Identifier);

            yield return new WaitForSeconds(0.2f);
        }

        MoveCard(mainDeckController, workingDeckController, mainDeckController.GetTopCard().Identifier);
    }

    private void HandlingPreference(Preference preference)
    {
        switch(preference)
        {
            case Preference.None:
                EndMove();
                break;
            case Preference.Cover:
                break;
            case Preference.TakeTwo:
                break;
            case Preference.TakeOne:
                break;
            case Preference.SkipMove:
                break;
            case Preference.CoverAnyCard:
                break;
            case Preference.SetMainSuit:
                break;
            case Preference.OnAnyCard:
                break;
            case Preference.MultiplyScore:
                break;
            default:
                break;
        }
    }

    private bool NeedChangeTrumpSuit(Card card, DeckController targetDeck)
    {
        return card.HasPreference(Preference.SetMainSuit) && targetDeck.owner == DeckOwner.Working;
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
                if(first.IsMoving)
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
