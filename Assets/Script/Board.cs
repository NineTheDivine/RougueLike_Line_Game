using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject outer_horizontal_line;
    public GameObject outer_vertical_line;
    public GameObject inner_horizontal_line;
    public GameObject inner_vertical_line;

    public Deck deck;

    void Start()
    {
        deck = Instantiate(deck);
        GameObject.Find("Background").GetComponent<Transform>().localScale = new Vector3(Global.scale_background, Global.scale_background, 1);
        GameObject.Find("Background").GetComponent<SpriteRenderer>().size = new Vector2(Global.grid_x, Global.grid_y);
        GameObject.Find("Grid").GetComponent<Grid>().cellSize = new Vector3(Global.scale_background, Global.scale_background, 0);
        Transform out_parent = GameObject.Find("Outer_Line").transform;
        for (int i = -1; i < 2; i += 2)
        {
            GameObject temp_h = Instantiate(outer_horizontal_line);
            temp_h.GetComponent<Transform>().position = new Vector3(0, Global.grid_y * Global.scale_background / 2 * i, 0);
            temp_h.GetComponent<Transform>().localScale = new Vector3(Global.grid_x * Global.scale_background + 0.25f, 0.25f, 1);
            temp_h.transform.parent = out_parent;
            GameObject temp_v = Instantiate(outer_vertical_line);
            temp_v.GetComponent<Transform>().position = new Vector3(Global.grid_x * Global.scale_background / 2 * i, 0, 0);
            temp_v.GetComponent<Transform>().localScale = new Vector3(0.25f, Global.grid_y * Global.scale_background, 1);
            temp_v.transform.parent = out_parent;
        }
        Transform in_parent = GameObject.Find("Inner_Line").transform;
        for (int i = -Global.grid_y / 2 + 1; i < Global.grid_y / 2; i++)
        {
            GameObject temp_h = Instantiate(inner_horizontal_line);
            temp_h.GetComponent<Transform>().position = new Vector3(0, i * Global.scale_background, 0);
            temp_h.GetComponent<Transform>().localScale = new Vector3(Global.grid_x * Global.scale_background + 0.1f, 0.1f, 1);
            temp_h.transform.parent = in_parent;
        }
        for (int i = -Global.grid_x / 2 + 1; i < Global.grid_x / 2; i++)
        {
            GameObject temp_v = Instantiate(inner_vertical_line);
            temp_v.GetComponent<Transform>().position = new Vector3(i * Global.scale_background, 0, 0);
            temp_v.GetComponent<Transform>().localScale = new Vector3(0.1f, Global.grid_y * Global.scale_background, 1);
            temp_v.transform.parent = in_parent;
        }
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
        Instantiate(deck.Pop_Piece(), GameObject.Find("Current_Piece").transform);
    }
}
