using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Mino
{
    public Global.Mino_Type m_type { get; private set; }
    public Vector2Int pos { get; set; }
    public Global.Piece_Type p_type { get; private set; }

    public Tile t_type { get; private set; }

    public Mino(Global.Mino_Type m_type, Vector2Int pos, Global.Piece_Type p_type)
    {
        this.m_type = m_type;
        this.pos = pos;
        this.p_type = p_type;
        this.t_type = Global.Tile_Data[(this.m_type, this.p_type)];
        Assert.True(this.t_type != null, "Tile data not found");
    }
}
