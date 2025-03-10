using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
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

    private Piece Current_Piece;

    public int level;
    private int max_level;
    private int drop_speed;
    private Vector2Int x_axis;
    private int x_counter;
    private int y_counter;
    private bool is_down;
    private bool is_floor;

    private void Awake()
    {
        grid_array = new Mino[Global.grid_x, Global.grid_y];
        max_level = 5;
        x_axis = Vector2Int.zero;
        x_counter = -Global.update_delay_x;
        y_counter = 0;
        is_down = false;
        is_floor = false;
    }

    void Start()
    {
        if (this.level <= 0 || this.level > max_level)
            this.level = 1;
        this.drop_speed = Global.update_delay_y / (this.level * 2);
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
        Generate_Piece();
        Set_Piece(this.Current_Piece);
    }

    private void FixedUpdate()
    {
        bool moved_this_frame = false;
        if (this.y_counter >= this.drop_speed)
        {
            this.y_counter = 0;
            if (Valid_Position(this.Current_Piece, Vector2Int.down))
            {
                Clear_Piece(this.Current_Piece.piece_pos, this.Current_Piece.mino_list);
                this.Current_Piece.piece_pos += Vector2Int.down;
                moved_this_frame = true;
            }
            else 
                this.is_floor = true;
        }

        if (this.is_down)
            this.y_counter += max_level;
        else
            this.y_counter++;

        if (this.x_axis != Vector2Int.zero)
        {
            if ((this.x_counter == -Global.update_delay_x || this.x_counter == 0) && Valid_Position(this.Current_Piece, this.x_axis))
            {
                Clear_Piece(this.Current_Piece.piece_pos, this.Current_Piece.mino_list);
                this.Current_Piece.piece_pos += x_axis;
                moved_this_frame = true;
            }
            this.x_counter++;
            if (this.x_counter >= Global.update_delay_x)
            {
                print (this.x_counter);
                this.x_counter = 0;
            }
        }
        if (moved_this_frame)
            Set_Piece(this.Current_Piece);
    }

    public bool Level_Up()
    {
        this.level++;
        if (this.level > this.max_level)
            return true;
        this.drop_speed = Global.update_delay_y  / (this.level*2);
        return false;
    }

    public void Generate_Piece()
    {
        this.Current_Piece = Instantiate(deck.Pop_Piece(),tile_board.transform);
        this.Current_Piece.piece_state = true;
        this.Current_Piece.piece_pos = (Vector2Int)spawn_loc;
    }

    public void Set_Piece(Piece p)
    {
        for (int i = 0; i < p.block_count; i++)
            tile_board.SetTile((Vector3Int)p.piece_pos + (Vector3Int)p.mino_list[i].pos, p.mino_list[i].t_type);
    }

    public void Clear_Piece(Vector2Int piece_pos, Mino[] mino_list)
    {
        for (int i = 0; i < mino_list.Length; i++)
        {
            tile_board.SetTile((Vector3Int)piece_pos + (Vector3Int)mino_list[i].pos, null);
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

    public void Move_Left(InputAction.CallbackContext context)
    {
        if (context.started)
            this.x_axis += Vector2Int.left;
        else if (context.canceled)
        {
            this.x_axis -= Vector2Int.left;
            if (this.x_axis == Vector2Int.zero)
                this.x_counter = -Global.update_delay_x;
        }
    }
    

    public void Move_Right(InputAction.CallbackContext context)
    {
        if (context.started)
            this.x_axis += Vector2Int.right;
        else if (context.canceled)
        {
            this.x_axis -= Vector2Int.right;
            if(this.x_axis == Vector2Int.zero)
                this.x_counter = -Global.update_delay_x;
        }
    }

    public void Move_Down(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            this.y_counter = this.drop_speed;
            this.is_down = true;
        }
        else if (context.canceled)
            this.is_down = false;
    }

    //Debug Function
    public void Gen(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            this.drop_speed = Global.update_delay_y / (this.level * 2);
            if (tile_board.transform.childCount != 0)
            {
                Clear_Piece(this.Current_Piece.piece_pos, this.Current_Piece.mino_list);
                Destroy(tile_board.transform.GetChild(0).gameObject);
            }
            Generate_Piece();
            Set_Piece(this.Current_Piece);
            this.y_counter = 0;
            this.is_floor = false;
        }
    }
}
