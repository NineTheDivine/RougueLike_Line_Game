using System.Collections.Generic;
using UnityEngine;

public static class Global
{
    public static float scale_background = 3.5f;
    public static int grid_x = 10;
    public static int grid_y = 20;

    public static void Add_Grid_X(int dx)
    {
        int temp = grid_x + dx;
        if (temp >= 2 && temp <= 16)
            grid_x = temp;
    }

    public static void Add_Grid_Y(int dy)
    {
        int temp = grid_y + dy;
        if (temp >= 4 && temp <= 30)
            grid_y = temp;
    }

    public enum Mino_Type
    {
        Normal,
        Bomb,
        Ghost,
    }
    public enum Piece_Type
    {
        I,
        O,
        L,
        J,
        Z,
        S,
        T,
        None,
    }

    public static readonly Dictionary<Global.Piece_Type, Color> Piece_Color_Dict = new Dictionary<Global.Piece_Type, Color>()
    {
        {Global.Piece_Type.I, Color.cyan },
        {Global.Piece_Type.O, Color.yellow },
        {Global.Piece_Type.L, new Color(1.0f, 0.7f, 0.2f)},
        {Global.Piece_Type.J, Color.blue },
        {Global.Piece_Type.Z, Color.red },
        {Global.Piece_Type.S, Color.green },
        {Global.Piece_Type.T, new Color(0.5f, 0.1f, 0.6f) },
        {Global.Piece_Type.None, new Color(0.7f, 0.7f, 0.7f) },
    };
}
