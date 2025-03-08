using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Global;

public class Board : MonoBehaviour
{
    public GameObject outer_horizontal_line;
    public GameObject outer_vertical_line;
    public GameObject inner_horizontal_line;
    public GameObject inner_vertical_line;

    public GameObject Deletion_Piece;

    public Deck deck;

    public Tilemap tile_board;
    private Vector3Int spawn_loc;

    private Mino[,] grid_array;

    private void Awake()
    {
        grid_array = new Mino[Global.grid_x, Global.grid_y];
    }

    void Start()
    {
        float outer_grid_size = 0.25f / Global.scale_background;
        float inner_grid_size = 0.1f / Global.scale_background;

        deck = Instantiate(deck);
        this.spawn_loc = new Vector3Int(0, (int)Global.grid_y/2 - 2, 0);
        Transform out_parent = GameObject.Find("Outer_Line").transform;
        for (int i = -1; i < 2; i += 2)
        {
            GameObject temp_h = Instantiate(outer_horizontal_line);
            temp_h.GetComponent<Transform>().position = new Vector3(0, Global.grid_y / 2 * i, 0);
            temp_h.GetComponent<Transform>().localScale = new Vector3(Global.grid_x + outer_grid_size, outer_grid_size, 1);
            temp_h.transform.parent = out_parent;
            GameObject temp_v = Instantiate(outer_vertical_line);
            temp_v.GetComponent<Transform>().position = new Vector3(Global.grid_x / 2 * i, 0, 0);
            temp_v.GetComponent<Transform>().localScale = new Vector3(outer_grid_size, Global.grid_y , 1);
            temp_v.transform.parent = out_parent;
        }
        Transform in_parent = GameObject.Find("Inner_Line").transform;
        for (int i = -Global.grid_y / 2 + 1; i < Global.grid_y / 2; i++)
        {
            GameObject temp_h = Instantiate(inner_horizontal_line);
            temp_h.GetComponent<Transform>().position = new Vector3(0, i , 0);
            temp_h.GetComponent<Transform>().localScale = new Vector3(Global.grid_x  + inner_grid_size, inner_grid_size, 1);
            temp_h.transform.parent = in_parent;
        }
        for (int i = -Global.grid_x / 2 + 1; i < Global.grid_x / 2; i++)
        {
            GameObject temp_v = Instantiate(inner_vertical_line);
            temp_v.GetComponent<Transform>().position = new Vector3(i, 0, 0);
            temp_v.GetComponent<Transform>().localScale = new Vector3(inner_grid_size, Global.grid_y , 1);
            temp_v.transform.parent = in_parent;
        }
        this.GameObject().GetComponent<Transform>().localScale = new Vector3(Global.scale_background, Global.scale_background, 1.0f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (tile_board.transform.childCount != 0)
            {
                Transform t = tile_board.transform.GetChild(0);
                t.transform.parent = Deletion_Piece.transform;
                Clear_Piece(t.GetComponent<Piece>());
            }
            Generate_Piece();
        }
        if (tile_board.transform.childCount != 0)
        {
            Piece Current_Piece = tile_board.transform.GetChild(0).gameObject.GetComponent<Piece>();
            Clear_Piece(Current_Piece);
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (Valid_Position(Current_Piece, Vector2Int.left))
                {
                    Current_Piece.piece_pos += Vector2Int.left;
                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (Valid_Position(Current_Piece, Vector2Int.right))
                {
                    Current_Piece.piece_pos += Vector2Int.right;
                }
            }
            Set_Piece(Current_Piece);
        }
    }

    private void LateUpdate()
    {
        for(int i = 0; i < Deletion_Piece.transform.childCount; i++)
            Destroy(Deletion_Piece.transform.GetChild(0).gameObject);
    }

    public void Generate_Piece()
    {
        Piece p = Instantiate(deck.Pop_Piece(),tile_board.transform);
        p.piece_state = true;
        p.piece_pos = (Vector2Int)spawn_loc;
    }

    public void Set_Piece(Piece p)
    {
        for (int i = 0; i < p.block_count; i++)
            tile_board.SetTile((Vector3Int)p.piece_pos + (Vector3Int)p.mino_list[i].pos, p.mino_list[i].t_type);
    }

    public void Clear_Piece(Piece p)
    {
        for (int i = 0; i < p.block_count; i++)
        {
            tile_board.SetTile((Vector3Int)p.piece_pos + (Vector3Int)p.mino_list[i].pos, null);
        }
    }

    public bool Valid_Position(Piece p, Vector2Int transition)
    {
        for (int i = 0; i < p.block_count; i++)
        {
            Vector2Int position = p.piece_pos + p.mino_list[i].pos + transition;
            if (position.x < -Global.grid_x/2 || position.x >= Global.grid_x/2 || position.y < -Global.grid_y /2 || position.y >= Global.grid_y/2)
                return false;
            if (p.mino_list[i].m_type != Global.Mino_Type.Ghost)
            {
                if (grid_array[position.x + Global.grid_x/2,position.y + Global.grid_y/2] != null)
                    return false;
            }
        }
        return true;
    }
}
