using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static Global;

public class Board : MonoBehaviour
{
    public GameObject outer_horizontal_line;
    public GameObject outer_vertical_line;
    public GameObject inner_horizontal_line;
    public GameObject inner_vertical_line;

    public Deck deck;

    public Tilemap tile_board;
    private Vector3Int spawn_loc;

    private Mino[,] grid_array;

    private Piece Current_Piece;

    private bool is_game;
    public int level;
    private int max_level;
    private int drop_speed;
    private Vector2Int x_axis;
    private int x_counter;
    private int y_counter;
    private bool is_down;
    private int is_floor;
    private bool moved_this_frame;
    private bool spined_this_frame;

    private void Awake()
    {
        this.is_game = true;
        grid_array = new Mino[Global.grid_x, Global.grid_y];
        max_level = 5;
        x_axis = Vector2Int.zero;
        x_counter = -Global.update_delay_x;
        y_counter = 0;
        is_down = false;
        is_floor = -1;
        moved_this_frame = false;
        spined_this_frame = false;

    }

    void Start()
    {
        if (this.level <= 0 || this.level > max_level)
            this.level = 1;
        this.drop_speed = Global.update_delay_y / (this.level * 2);
        float outer_grid_size = 0.25f / Global.scale_background;
        float inner_grid_size = 0.1f / Global.scale_background;

        deck = Instantiate(deck);
        this.spawn_loc = new Vector3Int(0, Global.grid_y/2 - 2, 0);
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
        this.Current_Piece = null;
        Generate_Piece();
    }

    private void FixedUpdate()
    {
        if (this.is_game)
        {
            this.moved_this_frame = false;
            this.spined_this_frame = false;
            if (this.is_floor == -1 && this.y_counter >= this.drop_speed)
            {
                this.y_counter = 0;
                if (Valid_Position(Vector2Int.down))
                {
                    Clear_Mino(this.Current_Piece.mino_list, true);
                    this.Current_Piece.piece_pos += Vector2Int.down;
                    this.moved_this_frame = true;
                }
                else
                    this.is_floor = 0;
            }

            if (this.x_axis != Vector2Int.zero)
            {
                if ((this.x_counter == -Global.update_delay_x || this.x_counter == 0) && Valid_Position(this.x_axis))
                {
                    Clear_Mino(this.Current_Piece.mino_list, true);
                    this.Current_Piece.piece_pos += x_axis;
                    this.moved_this_frame = true;
                }
                this.x_counter++;
                if (this.x_counter >= Global.update_delay_x)
                {
                    this.x_counter = 0;
                }
            }

            if (this.is_floor >= 0)
            {
                if (Valid_Position(Vector2Int.down))
                    this.is_floor = -1;
                else
                    this.is_floor++;
            }

            else if (this.is_down)
                this.y_counter += max_level;
            else
                this.y_counter++;
        }
    }

    private void LateUpdate()
    {
        if (this.is_game)
        {
            if (this.moved_this_frame || this.spined_this_frame)
                Set_Mino(this.Current_Piece.mino_list, true);

            if (this.is_floor >= Global.floor_delay)
            {
                Stop_Piece();
                int current_score = 0;
                for (int i = 0; i < Global.grid_y; i++)
                {
                    if (Check_Line(i))
                    {
                        Clear_Line(i);
                        current_score++;
                        i--;
                    }
                }

                if (current_score != 0)
                {
                    Refresh_Board();
                    print(current_score);
                    
                }
            }

        }
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
        if (this.is_game)
        {
            this.Current_Piece = Instantiate(deck.Pop_Piece(), tile_board.transform);
            this.Current_Piece.piece_state = true;
            this.Current_Piece.piece_pos = (Vector2Int)spawn_loc;
            this.Current_Piece.spin_index = 0;
            if (Valid_Position(Vector2Int.zero) == false)
            {
                print("GameOver");
                this.is_game = false;
            }
            else
            {
                Set_Mino(this.Current_Piece.mino_list, true);
            }
        }
    }

    public void Set_Mino(Mino[] mino_list, bool is_current)
    {
        Vector3Int p_pos = (is_current ? (Vector3Int)this.Current_Piece.piece_pos : Vector3Int.zero);
        for (int i = 0; i < mino_list.Length; i++)
            tile_board.SetTile((Vector3Int)p_pos + (Vector3Int)mino_list[i].pos, mino_list[i].t_type);
    }

    public void Clear_Mino(Mino[] mino_list, bool is_current)
    {
        Vector2Int global_idx = new Vector2Int(Global.grid_x / 2, Global.grid_y / 2);
        Vector3Int p_pos = (is_current ? (Vector3Int)this.Current_Piece.piece_pos : Vector3Int.zero);
        for (int i = 0; i < mino_list.Length; i++)
        {
            this.grid_array[mino_list[i].pos.x + global_idx.x + p_pos.x, mino_list[i].pos.y + global_idx.y + p_pos.y] = null;
            tile_board.SetTile((Vector3Int)mino_list[i].pos + p_pos, null);
        }
    }

    public void Clear_Line(int idx_start)
    {
        for (int i = idx_start; i < Global.grid_y; i++)
        {
            if (i + 1 < Global.grid_y)
            {
                for (int j = 0; j < Global.grid_x; j++)
                    this.grid_array[j, i] = this.grid_array[j, i + 1];
            }
            else
            {
                for (int j = 0; j < Global.grid_x; j++)
                    this.grid_array[j, i] = null;
            }
        }
    }

    public bool Check_Line(int idx)
    {
        for (int i = 0; i < Global.grid_x; i++)
        {
            if (this.grid_array[i, idx] == null)
                return false;
        }
        return true;
    }

    public void Refresh_Board()
    {
        Vector3Int global_idx = new Vector3Int(Global.grid_x / 2, Global.grid_y / 2, 0);
        for (int i = 0; i < Global.grid_x; i++)
        {
            for (int j = 0; j < Global.grid_y; j++)
            {
                if(this.grid_array[i, j] != null)
                    tile_board.SetTile(new Vector3Int(i, j, 0) - global_idx, this.grid_array[i, j].t_type);
                else
                    tile_board.SetTile(new Vector3Int(i, j, 0) - global_idx, null);
            }
        }
    }

    public void Stop_Piece()
    {
        for (int i = 0; i < this.Current_Piece.block_count; i++)
        {
            Vector2Int position = this.Current_Piece.piece_pos + this.Current_Piece.mino_list[i].pos + new Vector2Int(Global.grid_x / 2, Global.grid_y / 2);
            if (this.grid_array[position.x, position.y] == null)
            {
                this.grid_array[position.x, position.y] = this.Current_Piece.mino_list[i];
            }
        }
        this.Current_Piece.spin_index = 0;
        if (tile_board.transform.childCount != 0)
            Destroy(tile_board.transform.GetChild(0).gameObject);
        Generate_Piece();
        this.is_floor = -1;
    }


    public bool Valid_Position(Vector2Int transition)
    {
        for (int i = 0; i < this.Current_Piece.block_count; i++)
        {
            Vector2Int position = this.Current_Piece.piece_pos + this.Current_Piece.mino_list[i].pos + transition;
            if (position.x < -Global.grid_x / 2 || position.x >= Global.grid_x / 2 || position.y < -Global.grid_y / 2 || position.y >= Global.grid_y / 2)
            {
                return false;
            }
            if (this.Current_Piece.mino_list[i].m_type != Global.Mino_Type.Ghost)
            {
                if (grid_array[position.x + Global.grid_x / 2, position.y + Global.grid_y / 2] != null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void Valid_Spin(int spin)
    {
        int temp_spin = this.Current_Piece.spin_index + spin;
        int size = Global.Spin_Data[this.Current_Piece.piece_name].Length / this.Current_Piece.block_count;
        if (temp_spin < 0)
            temp_spin += size;
        if (temp_spin >= size)
            temp_spin %= size;
        for (int i = 0; i < this.Current_Piece.block_count; i++)
        {
            Vector2Int position = this.Current_Piece.piece_pos + Global.Spin_Data[this.Current_Piece.piece_name][temp_spin, i];
            if (position.x < -Global.grid_x / 2 || position.x >= Global.grid_x / 2 || position.y < -Global.grid_y / 2 || position.y >= Global.grid_y / 2)
                return;
            if (this.Current_Piece.mino_list[i].m_type != Global.Mino_Type.Ghost)
            {
                if (grid_array[position.x + Global.grid_x / 2, position.y + Global.grid_y / 2] != null)
                    return;
            }
        }
        this.spined_this_frame = true;
        Clear_Mino(this.Current_Piece.mino_list, true);
        this.Current_Piece.spin_index = temp_spin;
        for (int i = 0; i < this.Current_Piece.block_count; i++)
            this.Current_Piece.mino_list[i].pos = Global.Spin_Data[this.Current_Piece.piece_name][temp_spin, i];
        return;
    }

    public void Move_Left(InputAction.CallbackContext context)
    {
        if (this.is_game && context.started)
            this.x_axis += Vector2Int.left;
        else if (this.is_game && context.canceled)
        {
            this.x_axis -= Vector2Int.left;
            if (this.x_axis == Vector2Int.zero)
                this.x_counter = -Global.update_delay_x;
        }
    }
    

    public void Move_Right(InputAction.CallbackContext context)
    {
        if (this.is_game && context.started)
            this.x_axis += Vector2Int.right;
        else if (this.is_game && context.canceled)
        {
            this.x_axis -= Vector2Int.right;
            if(this.x_axis == Vector2Int.zero)
                this.x_counter = -Global.update_delay_x;
        }
    }

    public void Move_Down(InputAction.CallbackContext context)
    {
        if (this.is_game && context.started)
        {
            this.y_counter = this.drop_speed;
            this.is_down = true;
        }
        else if (this.is_game && context.canceled)
            this.is_down = false;
    }

    public void Hard_Drop(InputAction.CallbackContext context)
    {
        if (this.is_game && context.started)
        {
            Clear_Mino(this.Current_Piece.mino_list, true);
            while (Valid_Position(Vector2Int.down))
                this.Current_Piece.piece_pos += Vector2Int.down;
            this.moved_this_frame = true;
            this.is_floor = Global.floor_delay;
        }
    }

    public void Rotate_Clockwise(InputAction.CallbackContext context)
    {
        if (this.is_game && context.started)
            Valid_Spin(1);
    }

    public void Rotate_CounterClockwise(InputAction.CallbackContext context)
    {
        if (this.is_game && context.started)
            Valid_Spin(-1);
    }
    

    //Debug Function
    public void Gen(InputAction.CallbackContext context)
    {
        if (this.is_game && context.started)
        {
            this.drop_speed = Global.update_delay_y / (this.level * 2);
            if (tile_board.transform.childCount != 0)
            {
                Clear_Mino(this.Current_Piece.mino_list, true);
                Destroy(tile_board.transform.GetChild(0).gameObject);
            }
            Generate_Piece();
            Set_Mino(this.Current_Piece.mino_list, true);
            this.y_counter = 0;
            this.is_floor = -1;
        }
    }
}
