using NUnit.Framework;
using UnityEngine;

public class Piece : MonoBehaviour
{
    //State of Piece whether it can move
    public bool piece_state;
    //Position of Piece of mino (0,0)
    public Vector2Int piece_pos;
    //Name of Piece
    public string piece_name;
    //number of minos in piece
    public int block_count;
    //list of Mino in piece
    [SerializeField]
    public Mino[] mino_list;
    //Type of Piece
    [SerializeField]
    public Global.Piece_Type piece_type;

    private void Awake()
    {
        this.piece_state = false;
        Assert.True(Global.Piece_Data.ContainsKey(this.piece_name));
        this.mino_list = Global.Piece_Data[this.piece_name];
        Assert.False(block_count == 0, "There is no mino in Piece");
        Assert.True(block_count == mino_list.Length, "Invalid Mino List Length");
    }
}
