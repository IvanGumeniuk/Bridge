using System.Collections.Generic;
public class Player
{
    public string Name { get; set; }
    public int Score { get; set; }

    public Dictionary<Name, Card> cards = new Dictionary<Name, Card>();
}

