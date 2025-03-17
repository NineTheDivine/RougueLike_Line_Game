using NUnit.Framework;
using System.Linq;
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
    [SerializeField]
    public int spin_index;

    private void Awake()
    {
        Reset();
    }

    public void Reset()
    {
        this.spin_index = 0;
        this.piece_pos = Vector2Int.zero;
        this.piece_state = false;
        Assert.True(Global.Piece_Data.ContainsKey(this.piece_name));
        this.mino_list = Global.Piece_Data[this.piece_name];
        Assert.False(block_count == 0, "There is no mino in Piece");
        Assert.True(block_count == mino_list.Length, "Invalid Mino List Length");
        for (int i = 0; i < this.block_count; i++)
        {
            this.mino_list[i].pos = Global.Spin_Data[this.piece_name][this.spin_index, i];
        }
    }

    public void Copy(Piece p)
    {
        this.spin_index = p.spin_index;
        this.piece_pos = p.piece_pos;
        this.piece_state=p.piece_state;
        this.block_count = p.block_count;
        this.piece_name = p.piece_name;
        Assert.True(Global.Piece_Data.ContainsKey(this.piece_name));
        this.mino_list = new Mino[this.block_count];
        for (int i = 0; i < this.block_count; i++)
        {
            Mino temp = new Mino(p.mino_list[i].m_type, p.mino_list[i].pos, p.mino_list[i].p_type);
            this.mino_list[i] = temp;
        }
        this.piece_type = p.piece_type;
        this.spin_index=p.spin_index;
    }
}
