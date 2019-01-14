using UnityEngine;

public class PrefabContainer : MonoBehaviour {

    [SerializeField] private GameObject card;
    [SerializeField] private GameObject cardBackSide;

    public GameObject CardPrefab
    {
        get { return card; }
    }

    public GameObject CardBackSidePrefab
    {
        get { return cardBackSide; }
    }
}
