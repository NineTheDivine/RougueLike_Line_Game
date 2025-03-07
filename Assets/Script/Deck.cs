using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Deck : MonoBehaviour
{
    [SerializeField]
    private List<Piece> Deck_Data;
    [SerializeField]
    public List<Piece> Current_Deck;
    public Piece Pop_Piece()
    {
        Piece t = null;
        if (Current_Deck.Any())
        {
            t = Current_Deck[0];
            Current_Deck.RemoveRange(0, 1);
        }

        if (!Current_Deck.Any())
        {
            Current_Deck = new List<Piece>(Deck_Data);
            //Shuffle the Deck
        }
        Assert.IsNotNull(t);
        return t;
    }
    public void Awake()
    {
        Current_Deck = new List<Piece>(Deck_Data);
    }
}