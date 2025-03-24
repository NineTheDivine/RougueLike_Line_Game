using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public static class Global
{
    public static float scale_background = 4.5f;
    public static int grid_x = 10;
    public static int grid_y = 20;

    public static int update_delay_x = 5;
    public static int update_delay_y = 40;
    public static int floor_delay = 40;


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
        /*
        //Duomino
        { "Duomino",
            new Mino[]
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,0), Piece_Type.I),
                new Mino(Mino_Type.Normal, new Vector2Int(0,1), Piece_Type.I),
            }
        },
        */
        /*
        //Trimino
        { "Trimino_I",
            new Mino[]
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,1), Piece_Type.I),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,1), Piece_Type.I),
                new Mino(Mino_Type.Normal, new Vector2Int(1,1), Piece_Type.I),
            }
        },
        { "Triomino_V",
            new Mino[]
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,0), Piece_Type.L),
                new Mino(Mino_Type.Normal, new Vector2Int(0,1), Piece_Type.L),
                new Mino(Mino_Type.Normal, new Vector2Int(1,0), Piece_Type.L),
            }
        },
        */
        //Tetromino
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
        /*
        //Pentomino
        { "Pentomino_I",
            new Mino[]
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,1), Piece_Type.I),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,1), Piece_Type.I),
                new Mino(Mino_Type.Normal, new Vector2Int(-2,1), Piece_Type.I),
                new Mino(Mino_Type.Normal, new Vector2Int(1,1), Piece_Type.I),
                new Mino(Mino_Type.Normal, new Vector2Int(2,1), Piece_Type.I),
            }
        },
        { "Pentomino_T",
            new Mino[]
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,0), Piece_Type.T),
                new Mino(Mino_Type.Normal, new Vector2Int(0,1), Piece_Type.T),
                new Mino(Mino_Type.Normal, new Vector2Int(0,2), Piece_Type.T),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,0), Piece_Type.T),
                new Mino(Mino_Type.Normal, new Vector2Int(1,0), Piece_Type.T),
            }
        },
        { "Pentomino_U",
            new Mino[]
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,0), Piece_Type.J),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,1), Piece_Type.J),
                new Mino(Mino_Type.Normal, new Vector2Int(1,1), Piece_Type.J),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,0), Piece_Type.J),
                new Mino(Mino_Type.Normal, new Vector2Int(1,0), Piece_Type.J),
            }
        },
        { "Pentomino_V",
            new Mino[]
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,0), Piece_Type.L),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,1), Piece_Type.L),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,2), Piece_Type.L),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,0), Piece_Type.L),
                new Mino(Mino_Type.Normal, new Vector2Int(1,0), Piece_Type.L),
            }
        },
        { "Pentomino_W",
            new Mino[]
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,0), Piece_Type.Z),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,0), Piece_Type.Z),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,1), Piece_Type.Z),
                new Mino(Mino_Type.Normal, new Vector2Int(0,-1), Piece_Type.Z),
                new Mino(Mino_Type.Normal, new Vector2Int(1,-1), Piece_Type.Z),
            }
        },
        { "Pentomino_X",
            new Mino[]
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,0), Piece_Type.T),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,0), Piece_Type.T),
                new Mino(Mino_Type.Normal, new Vector2Int(0,-1), Piece_Type.T),
                new Mino(Mino_Type.Normal, new Vector2Int(1,0), Piece_Type.T),
                new Mino(Mino_Type.Normal, new Vector2Int(0,1), Piece_Type.T),
            }
        },
        { "Pentomino_F1",
            new Mino[]
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,0), Piece_Type.J),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,1), Piece_Type.J),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,0), Piece_Type.J),
                new Mino(Mino_Type.Normal, new Vector2Int(1,0), Piece_Type.J),
                new Mino(Mino_Type.Normal, new Vector2Int(0,-1), Piece_Type.J),
            }
        },
        { "Pentomino_F2",
            new Mino[]
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,0), Piece_Type.L),
                new Mino(Mino_Type.Normal, new Vector2Int(1,1), Piece_Type.L),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,0), Piece_Type.L),
                new Mino(Mino_Type.Normal, new Vector2Int(1,0), Piece_Type.L),
                new Mino(Mino_Type.Normal, new Vector2Int(0,-1), Piece_Type.L),
            }
        },
        { "Pentomino_S",
            new Mino[]
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,0), Piece_Type.S),
                new Mino(Mino_Type.Normal, new Vector2Int(1,1), Piece_Type.S),
                new Mino(Mino_Type.Normal, new Vector2Int(1,0), Piece_Type.S),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,0), Piece_Type.S),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,-1), Piece_Type.S),
            }
        },
        { "Pentomino_Z",
            new Mino[]
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,0), Piece_Type.Z),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,1), Piece_Type.Z),
                new Mino(Mino_Type.Normal, new Vector2Int(0,1), Piece_Type.Z),
                new Mino(Mino_Type.Normal, new Vector2Int(0,-1), Piece_Type.Z),
                new Mino(Mino_Type.Normal, new Vector2Int(1,-1), Piece_Type.Z),
            }
        },
        { "Pentomino_J",
            new Mino[]
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,0), Piece_Type.J),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,1), Piece_Type.J),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,0), Piece_Type.J),
                new Mino(Mino_Type.Normal, new Vector2Int(0,1), Piece_Type.J),
                new Mino(Mino_Type.Normal, new Vector2Int(0,2), Piece_Type.J),
            }
        },
        { "Pentomino_L",
            new Mino[]
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,0), Piece_Type.L),
                new Mino(Mino_Type.Normal, new Vector2Int(2,1), Piece_Type.L),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,0), Piece_Type.L),
                new Mino(Mino_Type.Normal, new Vector2Int(1,0), Piece_Type.L),
                new Mino(Mino_Type.Normal, new Vector2Int(2,0), Piece_Type.L),
            }
        },
        { "Pentomino_Y1",
            new Mino[]
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,0), Piece_Type.T),
                new Mino(Mino_Type.Normal, new Vector2Int(0,1), Piece_Type.T),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,0), Piece_Type.T),
                new Mino(Mino_Type.Normal, new Vector2Int(1,0), Piece_Type.T),
                new Mino(Mino_Type.Normal, new Vector2Int(2,0), Piece_Type.T),
            }
        },
        { "Pentomino_Y2",
            new Mino[]
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,0), Piece_Type.T),
                new Mino(Mino_Type.Normal, new Vector2Int(1,1), Piece_Type.T),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,0), Piece_Type.T),
                new Mino(Mino_Type.Normal, new Vector2Int(1,0), Piece_Type.T),
                new Mino(Mino_Type.Normal, new Vector2Int(2,0), Piece_Type.T),
            }
        },
        { "Pentomino_N1",
            new Mino[]
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,0), Piece_Type.S),
                new Mino(Mino_Type.Normal, new Vector2Int(1,1), Piece_Type.S),
                new Mino(Mino_Type.Normal, new Vector2Int(1,0), Piece_Type.S),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,0), Piece_Type.S),
                new Mino(Mino_Type.Normal, new Vector2Int(-2,0), Piece_Type.S),
            }
        },
        { "Pentomino_N2",
            new Mino[]
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,0), Piece_Type.Z),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,1), Piece_Type.Z),
                new Mino(Mino_Type.Normal, new Vector2Int(0,1), Piece_Type.Z),
                new Mino(Mino_Type.Normal, new Vector2Int(1,0), Piece_Type.Z),
                new Mino(Mino_Type.Normal, new Vector2Int(2,0), Piece_Type.Z),
            }
        },
        { "Pentomino_P",
            new Mino[]
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,0), Piece_Type.J),
                new Mino(Mino_Type.Normal, new Vector2Int(0,1), Piece_Type.J),
                new Mino(Mino_Type.Normal, new Vector2Int(1,0), Piece_Type.J),
                new Mino(Mino_Type.Normal, new Vector2Int(1,1), Piece_Type.J),
                new Mino(Mino_Type.Normal, new Vector2Int(0,2), Piece_Type.J),
            }
        },
        { "Pentomino_Q",
            new Mino[]
            {
                new Mino(Mino_Type.Normal, new Vector2Int(0,0), Piece_Type.L),
                new Mino(Mino_Type.Normal, new Vector2Int(0,1), Piece_Type.L),
                new Mino(Mino_Type.Normal, new Vector2Int(1,0), Piece_Type.L),
                new Mino(Mino_Type.Normal, new Vector2Int(1,1), Piece_Type.L),
                new Mino(Mino_Type.Normal, new Vector2Int(-1,0), Piece_Type.L),
            }
        },
        */

    };

    public static readonly Dictionary<string, Vector2Int[,]> Spin_Data = new Dictionary<string, Vector2Int[,]>()
    {
        { "Tetromino_I",
            new Vector2Int[,]
            {
                { new Vector2Int(0,1), new Vector2Int(-1,1), new Vector2Int(1,1), new Vector2Int(2,1) },
                { new Vector2Int(1,1), new Vector2Int(1,2), new Vector2Int(1,0), new Vector2Int(1,-1) },
                { new Vector2Int(1,0), new Vector2Int(2,0), new Vector2Int(0,0), new Vector2Int(-1,0) },
                { new Vector2Int(0,0), new Vector2Int(0,-1), new Vector2Int(0,1), new Vector2Int(0,2) }
            }
        },
        { "Tetromino_J",
            new Vector2Int[,]
            {
                { new Vector2Int(0,0), new Vector2Int(-1,1), new Vector2Int(-1,0), new Vector2Int(1,0) },
                { new Vector2Int(0,0), new Vector2Int(1,1), new Vector2Int(0,1), new Vector2Int(0,-1) },
                { new Vector2Int(0,0), new Vector2Int(1,-1), new Vector2Int(1,0), new Vector2Int(-1,0) },
                { new Vector2Int(0,0), new Vector2Int(-1,-1), new Vector2Int(0,-1), new Vector2Int(0,1) }
            }
        },
        { "Tetromino_L",
            new Vector2Int[,]
            {
                { new Vector2Int(0,0), new Vector2Int(1,1), new Vector2Int(1,0), new Vector2Int(-1,0) },
                { new Vector2Int(0,0), new Vector2Int(1,-1), new Vector2Int(0,-1), new Vector2Int(0,1) },
                { new Vector2Int(0,0), new Vector2Int(-1,-1), new Vector2Int(-1,0), new Vector2Int(1,0) },
                { new Vector2Int(0,0), new Vector2Int(-1,1), new Vector2Int(0,1), new Vector2Int(0,-1) }
            }
        },
        { "Tetromino_O",
            new Vector2Int[,]
            {
                { new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(1,1), new Vector2Int(0,1) }
            }
        },
        { "Tetromino_S",
            new Vector2Int[,]
            {
                { new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(0,1), new Vector2Int(1,1) },
                { new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(1,0), new Vector2Int(1,-1) },
                { new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(0,-1), new Vector2Int(-1,-1) },
                { new Vector2Int(0,0), new Vector2Int(0,-1), new Vector2Int(-1,0), new Vector2Int(-1,1) }
            }
        },
        { "Tetromino_T",
            new Vector2Int[,]
            {
                { new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(-1,0), new Vector2Int(1,0) },
                { new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(0,1), new Vector2Int(0,-1) },
                { new Vector2Int(0,0), new Vector2Int(0,-1), new Vector2Int(1,0), new Vector2Int(-1,0) },
                { new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(0,-1), new Vector2Int(0,1) }
            }
        },
        { "Tetromino_Z",
            new Vector2Int[,]
            {
                { new Vector2Int(0,0), new Vector2Int(-1,1), new Vector2Int(0,1), new Vector2Int(1,0) },
                { new Vector2Int(0,0), new Vector2Int(1,1), new Vector2Int(1,0), new Vector2Int(0,-1) },
                { new Vector2Int(0,0), new Vector2Int(1,-1), new Vector2Int(0,-1), new Vector2Int(-1,0) },
                { new Vector2Int(0,0), new Vector2Int(-1,-1), new Vector2Int(-1,0), new Vector2Int(0,1) }
            }
        },
    };

    public static readonly Dictionary<Piece_Type, Vector2Int[,]> SRS_Data = new Dictionary<Piece_Type, Vector2Int[,]>()
    {
        { Piece_Type.I,
            new Vector2Int[,]
            {
                { new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(2,0), new Vector2Int(-1,0), new Vector2Int(2,0) },
                { new Vector2Int(-1,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(0,-2) },
                { new Vector2Int(-1,1), new Vector2Int(1,1), new Vector2Int(-2,1), new Vector2Int(1,0), new Vector2Int(-2,0) },
                { new Vector2Int(0,1), new Vector2Int(0,1), new Vector2Int(0,1), new Vector2Int(0,-1), new Vector2Int(0,2) },
            }
        },
        { Piece_Type.None,
            new Vector2Int[,]
            {
                { new Vector2Int(0,0)},
                { new Vector2Int(0,-1)},
                { new Vector2Int(-1,-1)},
                { new Vector2Int(-1,0)},
            }
        },

        { Piece_Type.O,
            new Vector2Int[,]
            {
                { new Vector2Int(0,0)},
                { new Vector2Int(0,-1)},
                { new Vector2Int(-1,-1)},
                { new Vector2Int(-1,0)},
            }
        },

        { Piece_Type.J,
            new Vector2Int[,]
            {
                { new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0) },
                { new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(1,-1), new Vector2Int(0,2), new Vector2Int(1,2) },
                { new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0) },
                { new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(-1,-1), new Vector2Int(0,2), new Vector2Int(-1,2) },
            }
        },

        { Piece_Type.L,
            new Vector2Int[,]
            {
                { new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0) },
                { new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(1,-1), new Vector2Int(0,2), new Vector2Int(1,2) },
                { new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0) },
                { new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(-1,-1), new Vector2Int(0,2), new Vector2Int(-1,2) },
            }
        },

        { Piece_Type.S,
            new Vector2Int[,]
            {
                { new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0) },
                { new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(1,-1), new Vector2Int(0,2), new Vector2Int(1,2) },
                { new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0) },
                { new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(-1,-1), new Vector2Int(0,2), new Vector2Int(-1,2) },
            }
        },

        { Piece_Type.T,
            new Vector2Int[,]
            {
                { new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0) },
                { new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(1,-1), new Vector2Int(0,2), new Vector2Int(1,2) },
                { new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0) },
                { new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(-1,-1), new Vector2Int(0,2), new Vector2Int(-1,2) },
            }
        },

        { Piece_Type.Z,
            new Vector2Int[,]
            {
                { new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0) },
                { new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(1,-1), new Vector2Int(0,2), new Vector2Int(1,2) },
                { new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0) },
                { new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(-1,-1), new Vector2Int(0,2), new Vector2Int(-1,2) },
            }
        },
    };


    public static readonly Dictionary<int, int> Score_Line_Data = new Dictionary<int, int>()
    {
        {0, 0},
        {1,  100},
        {2, 250},
        {3, 400},
        {4, 800},
        {5, 1200}
    };
    
    public static readonly Dictionary<int, int> Score_TSpin_Data = new Dictionary<int, int>()
    {
        {0, 0},
        {1,  150},
        {2, 400},
        {3, 800}
    };
    public static readonly Dictionary<int, int> Score_Mino_Data = new Dictionary<int, int>()
    {
        {0, 50},
        {10,  100},
        {20, 200},
        {30, 350}
    };
    public static readonly Dictionary<int, int> Score_B2B_Data = new Dictionary<int, int>()
    {
        {0, 0},
        {2,  50},
        {4, 100},
        {6, 150},
        {9, 250}
    };
    public static readonly Dictionary<int, float> Score_Combo_Data = new Dictionary<int, float>()
    {
        {0, 1.0f},
        {2,  1.1f},
        {4, 1.2f},
        {6, 1.4f},
        {8, 1.7f},
        {11, 2.0f}
    };

    public static readonly int[] TargetScore = new int[]
    {
        1000, 3000, 6000, 10000, 15000, 20000, 30000, 50000, 75000, 100000 
    };
}
