using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public GameObject outer_horizontal_line;
    public GameObject outer_vertical_line;
    public GameObject inner_horizontal_line;
    public GameObject inner_vertical_line;

    public Deck deck;

    public Tilemap tile_board;
    private Vector3Int spawn_loc;
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
            Generate_Piece();
        }
    }
    public void Generate_Piece()
    {
        Piece p = Instantiate(deck.Pop_Piece(),tile_board.transform);
        for(int i = 0; i < p.block_count; i++)
            tile_board.SetTile(spawn_loc + p.mino_pos[i], p.mino_list[i]);
    }
}
