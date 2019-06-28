using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController> {
    
    [SerializeField] private PrefabContainer prefabContainer;
    public GameObject TrumpSuit;
    public Image trumpSuitMini;
    [SerializeField] private GameObject ChangePlayerMovingButton;

    public bool ChangeTurnButtonEnable
    {
        get { return ChangePlayerMovingButton.activeSelf; }
        set { ChangePlayerMovingButton.SetActive(value); }
    }

    public bool MiniTrumpSuitActive
    {
        get
        {
            return trumpSuitMini.enabled;
        }
    }

    private void Start()
    {
        ChangeEnableStateTrumpWindow(false);
        ChangeEnableStateMiniTrumpWindow(false);
    }

    public void ChangeEnableStateTrumpWindow(bool enableState)
    {
        TrumpSuit.SetActive(enableState);
    }

    public void ChangeEnableStateMiniTrumpWindow(bool enableState)
    {
        trumpSuitMini.enabled = enableState;
    }

    public void OnTrumpSuitChanged(int suitIndex)
    {
        GameController.Instance.SetSuit((CardSuit)suitIndex);
        trumpSuitMini.sprite = Resources.Load<Sprite>("Suits/" + (CardSuit)suitIndex);

        ChangeEnableStateTrumpWindow(false);
    }

    public void InitializeCardsUI(DeckController controller, Deck deck)
    {
        RemoveCards(controller);
        foreach(var k in deck.GetCards().Values)
        {
            AddCard(controller, k);
        }
    }

    public void RemoveCards(DeckController controller)
    {
        foreach(Transform child in controller.gameObject.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void RemoveCard(DeckController controller, IDType id)
    {
        foreach(Transform child in controller.gameObject.transform)
            if(child.GetComponent<Card>().Identifier.ID == id.ID)
            {
                Destroy(child.gameObject);
                break;
            }
    }

    public void AddCard(DeckController deckController, Card card)
    {
        GameObject cardObject = Instantiate(prefabContainer.CardPrefab, transform.position, Quaternion.identity, deckController.gameObject.transform);

        cardObject.name = card.Name + " " + card.Suit.ToString();
        Card cardComponent = cardObject.GetComponent<Card>();
        cardComponent.Name = card.Name;
        cardComponent.Suit = card.Suit;
        cardComponent.Points = card.Points;
        cardComponent.Identifier = card.Identifier;
        cardObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Cards/" + card.Name + card.Suit.ToString());
        cardObject.GetComponent<CardIDObserver>().InitializeDeckController(deckController);
    }

    public void OnNextButton()
    {
        GameController.Instance.NextPlayer();
    }
}
