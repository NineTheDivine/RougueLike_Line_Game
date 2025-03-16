using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Deck : MonoBehaviour
{
    public int Reload_Count;
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

        if (Reload_Count > 0 && !Current_Deck.Any())
        {
            Shuffle();
        }
        return t;
    }
    public void Awake()
    {
        this.Reload_Count = Global.base_reload_count;
        if (this.Current_Deck.Count == 0)
            Shuffle();
    }
    public void Shuffle()
    {
        this.Reload_Count--;
        List<Piece> temp_deck = new List<Piece>(Deck_Data);
        for (int i = Random.Range(0, temp_deck.Count()); temp_deck.Count() != 0; i = Random.Range(0, temp_deck.Count()))
        {
            this.Current_Deck.Add(temp_deck[i]);
            temp_deck.RemoveRange(i, 1);
        }
    }
}