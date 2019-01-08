using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController> {

    public GameObject cardPrefab;

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
        GameObject cardObject = Instantiate(cardPrefab, transform.position, Quaternion.identity, deckController.gameObject.transform);

        cardObject.name = card.cardName + " " + card.cardSuit.ToString();
        Card cardComponent = cardObject.GetComponent<Card>();
        cardComponent.cardName = card.cardName;
        cardComponent.cardSuit = card.cardSuit;
        cardComponent.Identifier = card.Identifier;
        cardComponent.AddReferenceRange(card.GetPreferences());
        cardObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Cards/" + card.cardName + card.cardSuit.ToString());
        cardObject.GetComponent<CardIDObserver>().InitializeDeckController(deckController);
    }
}
