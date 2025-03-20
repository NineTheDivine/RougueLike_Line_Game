using UnityEngine;
using static Global;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;

public class Piece_Selection : MonoBehaviour
{
    private int index = 0;
    private Piece[] pieces;
    public GameObject Selection_Board;

    private void Start()
    {
        this.index = 0;
        List<int> Random_Index_Selection = new List<int>();
        pieces = new Piece[GameManager.piece_selection_number];

        for (int i = 0; i < GameManager.piece_selection_number; i++)
        {
            GameObject temp_object = Instantiate(Selection_Board, this.transform.GetChild(0));
            temp_object.transform.localPosition = new Vector3((30 * (i - 1) / (GameManager.piece_selection_number)), -2, 0);
        }

        for (int i = 0; i < GameManager.piece_selection_number; i++)
        {
            if (Random_Index_Selection.Count >= Global.Piece_Data.Count)
                break;
            int random_i = Random.Range(0, Global.Piece_Data.Count - 1);
            while (Random_Index_Selection.Contains(random_i))
                random_i = Random.Range(0, Global.Piece_Data.Count - 1);
            Random_Index_Selection.Add(random_i);
            this.pieces[i] = gameObject.AddComponent<Piece>();
            this.pieces[i].Set_Piece_By_Name(Global.Piece_Data.ElementAt(random_i).Key);
        }
        for (int i = 0; i < GameManager.piece_selection_number; i++)
        {
            if (i == this.index)
            {
                //enable selection
                this.transform.GetChild(0).GetChild(i).GetChild(3).gameObject.SetActive(true);
            }
            else
            {
                //disable selection
                this.transform.GetChild(0).GetChild(i).GetChild(3).gameObject.SetActive(false);
            }

            //draw piece on their grid
            if (this.pieces[i] != null)
            {
                this.pieces[i].transform.parent = this.transform.GetChild(0).GetChild(i).GetChild(0).GetChild(0);
                for (int j = 0; j < this.pieces[i].block_count; j++)
                {
                    this.transform.GetChild(0).GetChild(i).GetChild(0).GetChild(0).GetComponent<Tilemap>().SetTile((Vector3Int)this.pieces[i].mino_list[j].pos, this.pieces[i].mino_list[j].t_type);
                }
            }
        }
        this.GetComponentInParent<PlayerInput>().actions["Right"].started += this.Select_Right;
        this.GetComponentInParent<PlayerInput>().actions["Left"].started += this.Select_Left;
        this.GetComponentInParent<PlayerInput>().actions["Select"].started += this.Select_Confirm;

    }

    private void Index_Change()
    {
        for (int i = 0; i < GameManager.piece_selection_number; i++)
        {
            Transform temp_object = this.transform.GetChild(0).GetChild(i);
            if (i == this.index)
            {
                //enable selection
                temp_object.GetChild(3).gameObject.SetActive(true);
            }
            else
            {
                //disable selection
                temp_object.GetChild(3).gameObject.SetActive(false);
            }
        }
    }

    public void Select_Right(InputAction.CallbackContext context)
    {
        if (GameManager.Current_GameState == Global.GameState.InPlay_PieceSelect && context.started)
        {
            this.index++;
            if(this.index >= GameManager.piece_selection_number)
                index %= GameManager.piece_selection_number;
            Invoke("Index_Change", 0);
        }
    }


    public void Select_Left(InputAction.CallbackContext context)
    {
        if (GameManager.Current_GameState == Global.GameState.InPlay_PieceSelect && context.started)
        {
            this.index--;
            if (this.index < 0)
                this.index += GameManager.piece_selection_number;
            Invoke("Index_Change", 0);
        }
    }

    public void Select_Confirm(InputAction.CallbackContext context)
    {
        if (GameManager.Current_GameState == Global.GameState.InPlay_PieceSelect && context.started)
        {
            GameManager.deck.From_Piece_To_Temp(this.pieces[index]);
            for (int i = 0; i < GameManager.piece_selection_number; i++)
            {
                if (this.pieces[i] != null)
                {
                    for (int j = 0; j < this.pieces[i].block_count; j++)
                    {
                        this.transform.GetChild(0).GetChild(i).GetChild(0).GetChild(0).GetComponent<Tilemap>().SetTile((Vector3Int)this.pieces[i].mino_list[j].pos, null);
                    }
                }
            }
            GameManager.Current_GameState = Global.GameState.InPlay_Board;
            GetComponentInParent<GameManager>().Invoke("Change_State", 0);
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        this.GetComponentInParent<PlayerInput>().actions["Right"].started -= this.Select_Right;
        this.GetComponentInParent<PlayerInput>().actions["Left"].started -= this.Select_Left;
        this.GetComponentInParent<PlayerInput>().actions["Select"].started -= this.Select_Confirm;
    }
}
