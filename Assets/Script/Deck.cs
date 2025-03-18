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
    [SerializeField]
    public List<Piece> Temp_Deck = new List<Piece> { };
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
        if(this.Reload_Count == 0)
            this.Reload_Count = GameManager.base_reload_count;
        if (this.Current_Deck.Count == 0)
            Shuffle();
    }
    public void Shuffle()
    {
        this.Reload_Count--;
        List<Piece> t_deck = new List<Piece>(Deck_Data);
        t_deck.AddRange(Temp_Deck);
        for (int i = Random.Range(0, t_deck.Count()); t_deck.Count() != 0; i = Random.Range(0, t_deck.Count()))
        {
            this.Current_Deck.Add(t_deck[i]);
            t_deck.RemoveRange(i, 1);
        }
    }

    public void From_Temp_To_Deck(bool[] is_include)
    {
        Assert.IsTrue(is_include.Length == this.Temp_Deck.Count());
        for (int i = 0; i < is_include.Length; i++)
        {
            if (is_include[i])
                this.Deck_Data.Add(Temp_Deck[i]);
        }
        Temp_Deck.Clear();
    }

    public void From_Piece_To_Temp(Piece p)
    {
        Assert.IsNotNull(p);
        Piece temp_p = gameObject.AddComponent<Piece>();
        temp_p.Copy(p);
        this.Temp_Deck.Add(temp_p);
    }
}