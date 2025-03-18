using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using TMPro;
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


    public Tilemap tile_board;
    public Tilemap ghost_board;
    private Piece ghost_piece_visualize;
    private Tile ghost_tile;
    public Tilemap hold_board;
    public GameObject next_board;
    private Vector3Int spawn_loc;

    private Mino[,] grid_array;

    private Piece Current_Piece;
    private List<Piece> Hold_Pieces;
    public int level;
    private int max_level;
    private int drop_speed;
    private int drop_reset_count;
    private Vector2Int x_axis;
    private int x_counter;
    private int y_counter;
    private bool is_down;
    private int is_floor;
    private int is_holdable;
    private bool moved_this_frame;
    private bool spined_this_frame;

    private void Awake()
    {
        ghost_tile = Resources.Load<Tile>("Prefab/Mino/Puzzle_Block_Transparent_0");
        grid_array = new Mino[Global.grid_x, Global.grid_y + 2];
        max_level = 5;
        this.Hold_Pieces = new List<Piece>();
        float outer_grid_size = 0.25f / Global.scale_background;
        float inner_grid_size = 0.1f / Global.scale_background;

        this.spawn_loc = new Vector3Int(0, Global.grid_y / 2 - 2, 0);
        Transform out_parent = GameObject.Find("Outer_Line").transform;
        for (int i = -1; i < 2; i += 2)
        {
            GameObject temp_h = Instantiate(outer_horizontal_line);
            temp_h.GetComponent<Transform>().position = new Vector3(0, Global.grid_y / 2 * i, 0);
            temp_h.GetComponent<Transform>().localScale = new Vector3(Global.grid_x + outer_grid_size, outer_grid_size, 1);
            temp_h.transform.parent = out_parent;
            GameObject temp_v = Instantiate(outer_vertical_line);
            temp_v.GetComponent<Transform>().position = new Vector3(Global.grid_x / 2 * i, 0, 0);
            temp_v.GetComponent<Transform>().localScale = new Vector3(outer_grid_size, Global.grid_y, 1);
            temp_v.transform.parent = out_parent;
        }
        Transform in_parent = GameObject.Find("Inner_Line").transform;
        for (int i = -Global.grid_y / 2 + 1; i < Global.grid_y / 2; i++)
        {
            GameObject temp_h = Instantiate(inner_horizontal_line);
            temp_h.GetComponent<Transform>().position = new Vector3(0, i, 0);
            temp_h.GetComponent<Transform>().localScale = new Vector3(Global.grid_x + inner_grid_size, inner_grid_size, 1);
            temp_h.transform.parent = in_parent;
        }
        for (int i = -Global.grid_x / 2 + 1; i < Global.grid_x / 2; i++)
        {
            GameObject temp_v = Instantiate(inner_vertical_line);
            temp_v.GetComponent<Transform>().position = new Vector3(i, 0, 0);
            temp_v.GetComponent<Transform>().localScale = new Vector3(inner_grid_size, Global.grid_y, 1);
            temp_v.transform.parent = in_parent;
        }
        this.Current_Piece = null;
    }

    void Start()
    {
        x_axis = Vector2Int.zero;
        x_counter = -Global.update_delay_x;
        y_counter = 0;
        is_down = false;
        is_floor = -1;
        is_holdable = GameManager.is_hold_count;
        moved_this_frame = false;
        spined_this_frame = false;
        drop_reset_count = 0;
        if (this.level <= 0 || this.level > max_level)
            this.level = 1;
        this.drop_speed = Global.update_delay_y / (this.level * 2);
        Refresh_Board();
        Refresh_Next(true);
    }

    private void FixedUpdate()
    {
        this.moved_this_frame = false;
        this.spined_this_frame = false;
        if (this.Current_Piece == null)
        {
            Piece temp_p = GameManager.deck.Pop_Piece();
            Generate_Piece(temp_p == null ? Pop_Hold() : temp_p);
            this.moved_this_frame = true;
        }
        if (this.is_floor == -1 && this.y_counter >= this.drop_speed)
        {
            this.y_counter = 0;
            if (Valid_Position(Vector2Int.down))
            {
                Clear_Mino(this.Current_Piece.mino_list, this.Current_Piece.piece_pos, this.tile_board);
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
                Clear_Mino(this.Current_Piece.mino_list, this.Current_Piece.piece_pos, this.tile_board);
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
            {
                this.drop_reset_count++;
                this.is_floor = -1;
            }
            else
                this.is_floor++;
        }

        else if (this.is_down)
            this.y_counter += max_level;
        else
            this.y_counter++;
    }

    private void LateUpdate()
    {
        if (GameManager.Current_GameState == GameState.InPlay_Board)
        {
            if (this.Current_Piece != null && (this.moved_this_frame || this.spined_this_frame))
            {
                Update_Ghost(false, this.spined_this_frame, this.x_axis);
                Set_Mino(this.Current_Piece.mino_list, true, this.tile_board);
            }

            if (this.is_floor >= Global.floor_delay || (this.drop_reset_count >= 10 && !Valid_Position(Vector2Int.down)))
            {
                Stop_Piece();
                this.Current_Piece = null;
                int current_score = 0;
                for (int i = 0; i < Global.grid_y + 2; i++)
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
                    Level_Up();
                }
                this.is_holdable = GameManager.is_hold_count;
            }
        }
    }

    public bool Level_Up()
    {
        Update_Ghost(true, false, Vector2Int.zero);
        this.level++;
        GameManager.deck.Reload_Count += GameManager.base_reload_count;
        if (this.level > this.max_level)
        {
            GameManager.Current_GameState = GameState.StageClear;
            GetComponentInParent<GameManager>().Invoke("Change_State", 0);
            print("Stage Clear!");
            return true;
        }
        this.drop_speed = Global.update_delay_y  / (this.level*2);
        Refresh_Next(false);
        GameManager.Current_GameState = GameState.InPlay_PieceSelect;
        GetComponentInParent<GameManager>().Invoke("Change_State", 0);
        return false;
    }

    public void Generate_Piece(Piece p)
    {
        if (p == null)
        {
            print("GameOver");
            GameManager.Current_GameState = GameState.GameOver;
            GetComponentInParent<GameManager>().Invoke("Change_State", 0);
            return;
        }
        this.Current_Piece = Instantiate(p, tile_board.transform);
        this.Current_Piece.piece_state = true;
        this.Current_Piece.piece_pos = (Vector2Int)spawn_loc;
        this.Current_Piece.spin_index = 0;
        Refresh_Next(false);
        if (Valid_Generation() == false)
        {
            print("GameOver");
            GameManager.Current_GameState = GameState.GameOver;
            GetComponentInParent<GameManager>().Invoke("Change_State", 0);
        }
        else
        {
            Update_Ghost(true, false, Vector2Int.zero);
            Set_Mino(this.Current_Piece.mino_list, true, this.tile_board);
        }
        
    }

    public void Update_Ghost(bool is_changed, bool is_spin, Vector2Int movement)
    {
        if (!is_changed && !is_spin && movement.x == 0)
            return;
        if (this.ghost_piece_visualize != null)
        {
            Clear_Mino(this.ghost_piece_visualize.mino_list, this.ghost_piece_visualize.piece_pos, ghost_board);
        }
        if (this.Current_Piece == null)
            return;

        if (is_changed)
        {
            if (ghost_board.transform.childCount != 0)
                Destroy(ghost_board.transform.GetChild(0).gameObject);
            this.ghost_piece_visualize = Instantiate(this.Current_Piece, ghost_board.transform);
        }
        this.ghost_piece_visualize.Copy(this.Current_Piece);

        if (ghost_piece_visualize.mino_list[0].m_type == Mino_Type.Ghost)
            return;

        while (Valid_Position(Vector2Int.down, this.ghost_piece_visualize))
            this.ghost_piece_visualize.piece_pos += Vector2Int.down;

        for (int i = 0; i < this.ghost_piece_visualize.mino_list.Length; i++)
        {
            if (this.ghost_piece_visualize.mino_list[i].pos.y < Global.grid_y / 2)
                ghost_board.SetTile((Vector3Int)this.ghost_piece_visualize.piece_pos + (Vector3Int)this.ghost_piece_visualize.mino_list[i].pos, ghost_tile);
        }
    }

    public void Set_Mino(Mino[] mino_list, bool is_current, Tilemap tilemap)
    {
        Vector3Int p_pos = (is_current ? (Vector3Int)this.Current_Piece.piece_pos : Vector3Int.zero);
        for (int i = 0; i < mino_list.Length; i++)
        {
            if(p_pos.y + mino_list[i].pos.y < Global.grid_y / 2)
                tilemap.SetTile((Vector3Int)p_pos + (Vector3Int)mino_list[i].pos, mino_list[i].t_type);
        }
    }

    public void Clear_Mino(Mino[] mino_list, Vector2Int p_pos, Tilemap tilemap)
    {
        for (int i = 0; i < mino_list.Length; i++)
        {
            tilemap.SetTile((Vector3Int)mino_list[i].pos + (Vector3Int)p_pos, null);
        }
    }
    /*
    public void Remove_Mino(Mino[] mino_list, Tilemap tilemap)
    {
        Vector2Int global_idx = new Vector2Int(Global.grid_x / 2, Global.grid_y / 2);

        for (int i = 0; i < mino_list.Length; i++)
        {
            this.grid_array[mino_list[i].pos.x + global_idx.x, mino_list[i].pos.y + global_idx.y] = null;
            tilemap.SetTile((Vector3Int)mino_list[i].pos, null);
        }
    }
    */

    public void Clear_Line(int idx_start)
    {
        for (int i = idx_start; i < Global.grid_y + 2; i++)
        {
            if (i < Global.grid_y + 1)
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

    public void Refresh_Next(bool init)
    {
        for (int i = 1; i < this.next_board.transform.childCount; i++)
        {
            GameObject child_tile = this.next_board.transform.GetChild(i).GetChild(0).GetChild(0).gameObject;
            GameObject child_X = this.next_board.transform.GetChild(i).GetChild(3).gameObject;
            GameObject child_count = this.next_board.transform.GetChild(i).GetChild(4).gameObject;
            if (child_tile.transform.childCount != 0)
            {
                Clear_Mino(child_tile.transform.GetChild(0).GetComponent<Piece>().mino_list, Vector2Int.zero, child_tile.GetComponent<Tilemap>());
                Destroy(child_tile.transform.GetChild(0).gameObject);
            }

            if (i-1 < GameManager.deck.Current_Deck.Count)
            {
                child_X.SetActive(false);
                if (!init && i + 1 < this.next_board.transform.childCount && this.next_board.transform.GetChild(i + 1).GetChild(4).gameObject.activeSelf)
                {
                    child_count.GetComponent<TextMeshPro>().text = this.next_board.transform.GetChild(i + 1).GetChild(4).GetComponent<TextMeshPro>().text;
                    child_count.SetActive(true);
                }
                else
                    child_count.SetActive(false);
                Piece p = Instantiate(GameManager.deck.Current_Deck[i-1], child_tile.transform);
                Set_Mino(p.mino_list, false, child_tile.GetComponent<Tilemap>());
            }
            else if (i - 1 == GameManager.deck.Current_Deck.Count)
            {
                
                if (GameManager.deck.Reload_Count > 0)
                {
                    GameManager.deck.Shuffle();
                    child_count.GetComponent<TextMeshPro>().text = GameManager.deck.Reload_Count.ToString();
                    child_count.SetActive(true);
                    Piece p = Instantiate(GameManager.deck.Current_Deck[i - 1], child_tile.transform);
                    Set_Mino(p.mino_list, false, child_tile.GetComponent<Tilemap>());
                    child_X.SetActive(false);
                }
                else
                {
                    child_X.SetActive(true);
                    child_count.GetComponent<TextMeshPro>().text = "X";
                }
            }
            else
            {
                child_count.SetActive(false);
                child_X.SetActive(true);
            }
        }
        this.next_board.transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text = GameManager.deck.Reload_Count.ToString();
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
            else if (this.Current_Piece.mino_list[i].m_type != Mino_Type.Ghost)
                Assert.IsTrue(true, "Piece already positioned in " + position);
        }
        this.Current_Piece.spin_index = 0;
        this.Current_Piece.piece_state = false;
        if (tile_board.transform.childCount != 0)
            Destroy(tile_board.transform.GetChild(0).gameObject);
        this.is_floor = -1;
        this.drop_reset_count = 0;
    }

    public Piece Pop_Hold()
    {
        if (this.Hold_Pieces.Count > 0)
        {
            Piece p = this.Hold_Pieces[0];
            Clear_Mino(p.mino_list, Vector2Int.zero, this.hold_board);
            Destroy(this.hold_board.transform.GetChild(0).gameObject);
            this.Hold_Pieces.RemoveAt(0);
            if (this.Hold_Pieces.Count > 0)
            {
                Set_Mino(this.Hold_Pieces[0].mino_list, false, this.hold_board);
            }
            return p;
        }
        return null;
    }

    public bool Valid_Position(Vector2Int transition)
    {
        for (int i = 0; i < this.Current_Piece.block_count; i++)
        {
            Vector2Int position = this.Current_Piece.piece_pos + this.Current_Piece.mino_list[i].pos + transition;
            if (position.x < -Global.grid_x / 2 || position.x >= Global.grid_x / 2 || position.y < -Global.grid_y / 2 || position.y >= (Global.grid_y / 2 + 2))
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

    public bool Valid_Position(Vector2Int transition, Piece p)
    {
        for (int i = 0; i < p.block_count; i++)
        {
            Vector2Int position = p.piece_pos + p.mino_list[i].pos + transition;
            if (position.x < -Global.grid_x / 2 || position.x >= Global.grid_x / 2 || position.y < -Global.grid_y / 2 || position.y >= (Global.grid_y / 2 + 2))
                return false;
            if (grid_array[position.x + Global.grid_x / 2, position.y + Global.grid_y / 2] != null)
                return false;
        }
        return true;
    }

    public bool Valid_Generation()
    {
        if (Valid_Position(Vector2Int.zero))
            return true;
        this.Current_Piece.piece_pos += Vector2Int.up;
        bool is_visible = false;
        for (int i = 0; i < this.Current_Piece.block_count; i++)
        {
            Vector2Int position = this.Current_Piece.piece_pos + this.Current_Piece.mino_list[i].pos;
            if (position.y <= Global.grid_y / 2)
            {
                is_visible = true;
            }
            if (this.Current_Piece.mino_list[i].m_type != Global.Mino_Type.Ghost)
            {
                if (grid_array[position.x + Global.grid_x / 2, position.y + Global.grid_y / 2] != null)
                {
                    return false;
                }
            }
        }
        return is_visible;
    }

    public void Valid_Spin(int spin)
    {
        int temp_spin = this.Current_Piece.spin_index + spin;
        int size = Global.Spin_Data[this.Current_Piece.piece_name].GetLength(0);
        if (temp_spin < 0)
            temp_spin += size;
        if (temp_spin >= size)
            temp_spin %= size;
        Vector2Int wallkick;
        for (int j = 0; j < Global.Wallkick_Data[this.Current_Piece.piece_type].GetLength(1); j++)
        {
            bool is_spin = true;
            wallkick = Global.Wallkick_Data[this.Current_Piece.piece_type][this.Current_Piece.spin_index, j] * spin;
            for (int i = 0; i < this.Current_Piece.block_count; i++)
            {
                Vector2Int position = this.Current_Piece.piece_pos + Global.Spin_Data[this.Current_Piece.piece_name][temp_spin, i] + wallkick;
                if (position.x < -Global.grid_x / 2 || position.x >= Global.grid_x / 2 || position.y < -Global.grid_y / 2 || (position.y >= Global.grid_y / 2 + 2))
                {
                    is_spin = false;
                    break;
                }
                if (this.Current_Piece.mino_list[i].m_type != Global.Mino_Type.Ghost)
                {
                    if (grid_array[position.x + Global.grid_x / 2, position.y + Global.grid_y / 2] != null)
                    {
                        is_spin = false;
                        break;
                    }
                }
            }
            if (is_spin)
            {
                this.spined_this_frame = true;
                Clear_Mino(this.Current_Piece.mino_list, this.Current_Piece.piece_pos, this.tile_board);
                this.Current_Piece.piece_pos += wallkick;
                this.Current_Piece.spin_index = temp_spin;
                for (int i = 0; i < this.Current_Piece.block_count; i++)
                    this.Current_Piece.mino_list[i].pos = Global.Spin_Data[this.Current_Piece.piece_name][temp_spin, i];
                return;
            }
        }
    }

    public void Move_Left(InputAction.CallbackContext context)
    {
        if (GameManager.Current_GameState == GameState.InPlay_Board && context.started)
            this.x_axis += Vector2Int.left;
        else if (GameManager.Current_GameState == GameState.InPlay_Board && context.canceled)
        {
            this.x_axis -= Vector2Int.left;
            if (this.x_axis == Vector2Int.zero)
                this.x_counter = -Global.update_delay_x;
        }
    }
    

    public void Move_Right(InputAction.CallbackContext context)
    {
        if (GameManager.Current_GameState == GameState.InPlay_Board && context.started)
            this.x_axis += Vector2Int.right;
        else if (GameManager.Current_GameState == GameState.InPlay_Board && context.canceled)
        {
            this.x_axis -= Vector2Int.right;
            if(this.x_axis == Vector2Int.zero)
                this.x_counter = -Global.update_delay_x;
        }
    }

    public void Move_Down(InputAction.CallbackContext context)
    {
        if (GameManager.Current_GameState == GameState.InPlay_Board && context.started)
        {
            this.y_counter = this.drop_speed;
            this.is_down = true;
        }
        else if (GameManager.Current_GameState == GameState.InPlay_Board && context.canceled)
            this.is_down = false;
    }
    
    public void Hard_Drop(InputAction.CallbackContext context)
    {
        if (GameManager.Current_GameState == GameState.InPlay_Board && context.started)
        {
            Clear_Mino(this.Current_Piece.mino_list, this.Current_Piece.piece_pos, this.tile_board);
            this.Current_Piece.piece_pos = ghost_piece_visualize.piece_pos;
            this.moved_this_frame = true;
            this.is_floor = Global.floor_delay;
        }
    }

    public void Rotate_Clockwise(InputAction.CallbackContext context)
    {
        if (GameManager.Current_GameState == GameState.InPlay_Board && context.started)
            Valid_Spin(1);
    }

    public void Rotate_CounterClockwise(InputAction.CallbackContext context)
    {
        if (GameManager.Current_GameState == GameState.InPlay_Board && context.started)
            Valid_Spin(-1);
    }

    public void Hold(InputAction.CallbackContext context)
    {
        if (GameManager.Current_GameState == GameState.InPlay_Board && this.is_holdable > 0 && context.started && !(this.Hold_Pieces.Count == 0 && GameManager.deck.Current_Deck.Count == 0))
        {
            this.is_holdable--;
            Clear_Mino(this.Current_Piece.mino_list, this.Current_Piece.piece_pos, this.tile_board);
            this.Current_Piece.Reset();
            Piece p = Instantiate(this.Current_Piece, this.hold_board.transform);
            this.Hold_Pieces.Add(p);
            if (tile_board.transform.childCount != 0)
                Destroy(tile_board.transform.GetChild(0).gameObject);
            if (this.Hold_Pieces.Count < GameManager.is_hold_count + 1)
            {
                Generate_Piece(GameManager.deck.Pop_Piece());
            }
            else
            {
                Generate_Piece(this.Hold_Pieces[0]);
                Clear_Mino(this.Hold_Pieces[0].mino_list, Vector2Int.zero, this.hold_board);
                Destroy(this.hold_board.transform.GetChild(0).gameObject);
                this.Hold_Pieces.RemoveAt(0);
            }
            if (this.Hold_Pieces.Count > 0)
            {
                Set_Mino(this.Hold_Pieces[0].mino_list, false, this.hold_board);
            }
        }
    }

    //Debug Function
    public void Gen(InputAction.CallbackContext context)
    {
        if (GameManager.Current_GameState == GameState.InPlay_Board && context.started)
        {
            this.drop_speed = Global.update_delay_y / (this.level * 2);
            if (tile_board.transform.childCount != 0)
            {
                Clear_Mino(this.Current_Piece.mino_list, this.Current_Piece.piece_pos, this.tile_board);
                Destroy(tile_board.transform.GetChild(0).gameObject);
            }
            Generate_Piece(GameManager.deck.Pop_Piece());
            Set_Mino(this.Current_Piece.mino_list, true, this.tile_board);
            this.y_counter = 0;
            this.is_floor = -1;
        }
    }
    public void Force_Refresh(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Refresh_Board();
            for (int i = 0; i < Global.grid_x; i++)
                for (int j = 0; j < Global.grid_y + 2; j++)
                    if (grid_array[i, j] != null)
                        print("x : " + i + " and y : " + j + "is filled");
            print("=============================");
        }
    }
}
