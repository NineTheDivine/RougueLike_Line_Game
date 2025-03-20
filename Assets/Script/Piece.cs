using UnityEngine;
using UnityEngine.Assertions;

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
        Revert();
    }

    public void Revert()
    {
        if (this.piece_name == null)
            return;
        this.spin_index = 0;
        this.piece_pos = Vector2Int.zero;
        this.piece_state = false;
        Assert.IsTrue(Global.Piece_Data.ContainsKey(this.piece_name));
        this.mino_list = Global.Piece_Data[this.piece_name];
        Assert.IsFalse(block_count == 0, "There is no mino in Piece");
        Assert.IsTrue(block_count == mino_list.Length, "Invalid Mino List Length");
        for (int i = 0; i < this.block_count; i++)
        {
            this.mino_list[i].pos = Global.Spin_Data[this.piece_name][this.spin_index, i];
        }
    }

    public void Copy(Piece p)
    {
        this.spin_index = p.spin_index;
        this.piece_pos = p.piece_pos;
        this.piece_state = p.piece_state;
        this.block_count = p.block_count;
        this.piece_name = p.piece_name;
        Assert.IsTrue(Global.Piece_Data.ContainsKey(this.piece_name));
        this.mino_list = new Mino[this.block_count];
        for (int i = 0; i < this.block_count; i++)
        {
            Mino temp = new Mino(p.mino_list[i].m_type, p.mino_list[i].pos, p.mino_list[i].p_type);
            this.mino_list[i] = temp;
        }
        this.piece_type = p.piece_type;
        this.spin_index = p.spin_index;
    }
    public void Set_Piece_By_Name(string piece_name)
    {
        this.spin_index = 0;
        this.piece_state = false;
        this.piece_pos = Vector2Int.zero;
        this.piece_name = piece_name;
        Assert.IsTrue(Global.Piece_Data.ContainsKey(this.piece_name));
        this.block_count = Global.Piece_Data[this.piece_name].Length;
        this.mino_list = Global.Piece_Data[this.piece_name];

        Assert.IsFalse(this.block_count == 0, "There is no mino in Piece");
        Assert.IsTrue(this.block_count == this.mino_list.Length, "Invalid Mino List Length");
        for (int i = 0; i < this.block_count; i++)
        {
            this.mino_list[i].pos = Global.Spin_Data[this.piece_name][this.spin_index, i];
        }
    }
}
