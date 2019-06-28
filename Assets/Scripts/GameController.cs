using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public DeckController mainDeckController;
    public DeckController workingDeckController;

    public List<Player> players = new List<Player>();

    private int _index;

    public int CurrentTurn
    {
        get { return _index; }
        set
        {
            if (value > players.Count - 1)
            {
                _index = 0;
            }
            else if(value < 0)
            {
                _index = players.Count - 1;
            }
            else
            {
                _index = value;
            }
        }
    }

    private int canTakeCardCount = 1;

    
    [SerializeField]
    private CardSuit trumpSuit;

    private void Awake()
    {
        mainDeckController.deck = new Deck(true);
    }

    // Use this for initialization
    void Start()
    {
        Generate();
        Shuffle(mainDeckController);

        StartCoroutine(Deal());
    }


    public void Generate()
    {
        UIController.Instance.InitializeCardsUI(mainDeckController, mainDeckController.deck);
    }

    public void Shuffle(DeckController deckController)
    {
        deckController.Shuffle();
        UIController.Instance.InitializeCardsUI(deckController, deckController.GetDeck());
    }

    // Карта, на якій я натиснув
    public void OnCardClick(IDType cardID, DeckController deckController, DeckOwner owner, int playerID)
    {
        Card interacted = deckController.GetCard(cardID);
        Debug.LogFormat("{0} {1} {2}", interacted.ToString(), owner, playerID);

        HandleMove(cardID, deckController, owner, playerID);
    }

    private void HandleMove(IDType cardID, DeckController deckController, DeckOwner owner, int playerID)
    {
        // Колоди, на які я натиснув 
        switch (owner)
        {
            // Мої карти
            case DeckOwner.Player:
                {

                    break;
                }
            // Банк
            case DeckOwner.MainDeck:
                {
                    if (canTakeCardCount != 0)
                    {
                        MoveCard(mainDeckController, players[CurrentTurn].cards, cardID);
                        canTakeCardCount--;
                    }

                    if (canTakeCardCount == 0)
                    {
                        UIController.Instance.ChangeTurnButtonEnable = true;
                    }
                    break;
                }
            // Гральна колода
            case DeckOwner.WorkingDeck:
                break;
            default:
                break;
        }
    }

    //Коли хтось завершив хід, треба проконтролювати карту, з якою він це зробив
    private void HandleEntireMoveCard()
    {
        Card top = workingDeckController.GetTopCard();
    }

    public void NextPlayer()
    {
        UIController.Instance.ChangeTurnButtonEnable = false;
        canTakeCardCount = 1;
        CurrentTurn++;
        HandleEntireMoveCard();
    }

    public void SetSuit(CardSuit suit)
    {
        trumpSuit = suit;
        ChangePlayerTurn();
    }

    private IEnumerator Deal()
    {
        int totalCount = (5 * players.Count) - 1;

        //time for initializing UI
        yield return new WaitForSeconds(1f);

        for(int cardCount=0; cardCount< totalCount; cardCount++)
        {   //replace with all players iteration 

            MoveCard(mainDeckController, players[CurrentTurn++].cards, mainDeckController.GetTopCard().Identifier);

            yield return new WaitForSeconds(0.1f);
        }

        MoveCard(mainDeckController, workingDeckController, mainDeckController.GetTopCard().Identifier);
    }

    public void ChangePlayerTurn()
    {
        players[CurrentTurn].IsMoving = false;
        CurrentTurn = CurrentTurn++;
        players[CurrentTurn].IsMoving = true;
        
        UIController.Instance.ChangeTurnButtonEnable = false;
    }
        
    private void ShowSuits()
    {
        UIController.Instance.ChangeEnableStateTrumpWindow(true);
        UIController.Instance.ChangeEnableStateMiniTrumpWindow(true);
    }

    private void MoveCard(DeckController fromDeck, DeckController toDeck, IDType id)
    {
        Card toMoveCard = fromDeck.GetCard(id);

        toDeck.AddCard(toMoveCard, fromDeck.currentOwner);
        fromDeck.RemoveCard(id);

        UIController.Instance.AddCard(toDeck, toMoveCard);
        UIController.Instance.RemoveCard(fromDeck, id);
    }
    

    private bool IsSuitEqual(Card firstCard, Card secondCard)
    {
        return firstCard.Suit == secondCard.Suit;
    }

    private bool IsNameEqual(Card firstCard, Card secondCard)
    {
        return firstCard.Name == secondCard.Name;
    }
    
}
