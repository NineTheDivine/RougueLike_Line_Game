using UnityEngine.Assertions;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Mino
{
    public Global.Mino_Type m_type { get; set; }
    public Vector2Int pos { get; set; }
    public Global.Piece_Type p_type { get; set; }

    public Tile t_type { get; set; }

    public Mino(Global.Mino_Type m_type, Vector2Int pos, Global.Piece_Type p_type)
    {
        this.m_type = m_type;
        this.pos = pos;
        this.p_type = p_type;
        this.t_type = Global.Tile_Data[(this.m_type, this.p_type)];
        Assert.IsTrue(this.t_type != null, "Tile data not found");
    }

    public void Set_Color(Global.Piece_Type p)
    {
        this.p_type = p_type;
        this.t_type = Global.Tile_Data[(this.m_type, this.p_type)];
    }
}
