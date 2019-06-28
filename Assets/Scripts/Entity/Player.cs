using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    public int playerID;
    public int age;
    public bool IsMoving = false;
    public bool isDealer = false;
    public string Name;
    public int Score;
    public DeckController cards;

    private void Awake()
    {
        cards = GetComponent<DeckController>();
    }
}

