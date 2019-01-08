using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardIDObserver : MonoBehaviour {

    private DeckController deckController;
    private IDType cardID;

	void Start () {
        cardID = GetComponent<Card>().Identifier;
	}

    public void InitializeDeckController(DeckController controller)
    {
        deckController = controller;
    }
	
    public void OnCardClick()
    {
        if(cardID == null)
        {
            Debug.Log("NULYARA");
            return;
        }

        deckController.OnCardClick(cardID);
    }
}
