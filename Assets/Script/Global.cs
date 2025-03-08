using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public static class Global
{
    public static float scale_background = 4.5f;
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

    public static readonly Dictionary<(Mino_Type, Piece_Type), Tile> Tile_Data = new Dictionary<(Mino_Type, Piece_Type), Tile>()
    {
        //{(Mino_Type.Bomb, Piece_Type.None) , Resources.Load<Tile>("")},
        {(Mino_Type.Normal, Piece_Type.I), Resources.Load<Tile>("Prefab/Mino/Puzzle_Block_Cyan_0") },
        {(Mino_Type.Normal, Piece_Type.J), Resources.Load<Tile>("Prefab/Mino/Puzzle_Block_Blue_0") },
        {(Mino_Type.Normal, Piece_Type.L), Resources.Load<Tile>("Prefab/Mino/Puzzle_Block_Orange_0") },
        {(Mino_Type.Normal, Piece_Type.O), Resources.Load<Tile>("Prefab/Mino/Puzzle_Block_Yellow_0") },
        {(Mino_Type.Normal, Piece_Type.S), Resources.Load<Tile>("Prefab/Mino/Puzzle_Block_Green_0") },
        {(Mino_Type.Normal, Piece_Type.T), Resources.Load<Tile>("Prefab/Mino/Puzzle_Block_Purple_0") },
        {(Mino_Type.Normal, Piece_Type.Z), Resources.Load<Tile>("Prefab/Mino/Puzzle_Block_Red_0") },
        {(Mino_Type.Normal, Piece_Type.None), Resources.Load<Tile>("Prefab/Mino/Puzzle_Block_Gray_0") },
    };

    public static readonly Dictionary<string, Mino[]> Piece_Data = new Dictionary<string, Mino[]>()
    {
        { "Tetromino_I", 
            new Mino[] 
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,1), Piece_Type.I),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,1), Piece_Type.I),
                new Mino(Mino_Type.Normal, new Vector2Int(1,1), Piece_Type.I),
                new Mino(Mino_Type.Normal, new Vector2Int(2,1), Piece_Type.I),
            } 
        },
        { "Tetromino_J",
            new Mino[]
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,0), Piece_Type.J),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,1), Piece_Type.J),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,0), Piece_Type.J),
                new Mino(Mino_Type.Normal, new Vector2Int(1,0), Piece_Type.J),
            }
        },
        { "Tetromino_L",
            new Mino[]
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,0), Piece_Type.L),
                new Mino(Mino_Type.Normal, new Vector2Int(1,1), Piece_Type.L),
                new Mino(Mino_Type.Normal, new Vector2Int(1,0), Piece_Type.L),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,0), Piece_Type.L),
            }
        },
        { "Tetromino_O",
            new Mino[]
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,0), Piece_Type.O),
                new Mino(Mino_Type.Normal, new Vector2Int(1,0), Piece_Type.O),
                new Mino(Mino_Type.Normal, new Vector2Int(1,1), Piece_Type.O),
                new Mino(Mino_Type.Normal, new Vector2Int(0,1), Piece_Type.O),
            }
        },
        { "Tetromino_S",
            new Mino[]
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,0), Piece_Type.S),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,0), Piece_Type.S),
                new Mino(Mino_Type.Normal, new Vector2Int(0,1), Piece_Type.S),
                new Mino(Mino_Type.Normal, new Vector2Int(1,1), Piece_Type.S),
            }
        },
        { "Tetromino_T",
            new Mino[]
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,0), Piece_Type.T),
                new Mino(Mino_Type.Normal, new Vector2Int(0,1), Piece_Type.T),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,0), Piece_Type.T),
                new Mino(Mino_Type.Normal, new Vector2Int(1,0), Piece_Type.T),
            }
        },
        { "Tetromino_Z",
            new Mino[]
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,0), Piece_Type.Z),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,1), Piece_Type.Z),
                new Mino(Mino_Type.Normal, new Vector2Int(0,1), Piece_Type.Z),
                new Mino(Mino_Type.Normal, new Vector2Int(1,0), Piece_Type.Z),
            }
        },
    };




}
