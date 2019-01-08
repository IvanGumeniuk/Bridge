using System.Collections.Generic;
using UnityEngine;
public class Card : MonoBehaviour
{
    [SerializeField] private IDType idType;
    public IDType Identifier
    {
        get { return idType; }
        set { idType = new IDType(value.ID); }
    }


    [SerializeField] private Suit _suit;
    public Suit cardSuit
    {
        get { return _suit; }
        set { _suit = value; }
    }

    [SerializeField] private Name _name;
    public Name cardName
    {
        get { return _name; }
        set { _name = value; }
    }

    public Points cardPoints { get; set; }

    [SerializeField]
    private List<Preference> cardPreferences = new List<Preference>();

    public Preference GetPreference(int index = 0)
    {
        Preference res = Preference.None;
        if(cardPreferences[index] != Preference.None)
            res = cardPreferences[index];
        return res;
    }

    public List<Preference> GetPreferences()
    {
        return cardPreferences;
    }

    public void AddReference(Preference preference)
    {
        cardPreferences.Add(preference);
    }
    public void AddReferenceRange(List<Preference> preferences)
    {
        cardPreferences.AddRange(preferences);
    }

    public Card(Suit cardSuit, Name cardName)
    {
        this.cardSuit = cardSuit;
        this.cardName = cardName;
    }
    
    public override string ToString()
    {
        string stringPref = string.Empty;
        foreach(Preference pref in cardPreferences)
            stringPref += pref.ToString() + " ";
        return string.Format("{0} {1} {2}", cardName.ToString(), cardSuit.ToString(), stringPref);
    }
}

