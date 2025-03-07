using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public static class Global
{
    public static float scale_background = 4.0f;
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
}
