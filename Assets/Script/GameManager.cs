using UnityEngine;
using static Global;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static Global.GameState Current_GameState;

    public static int is_hold_count = 1;
    public static int base_reload_count = 5;

    public static int piece_selection_number = 3;

    public static Deck deck;
    public Deck starter_deck;

    public Board board;
    public Piece_Selection Piece_Select;
    private Piece_Selection current_piece_select;

    

    

    private void Awake()
    {
        this.current_piece_select = null;
        this.GameObject().GetComponent<Transform>().localScale = new Vector3(Global.scale_background, Global.scale_background, 1.0f);
        board.enabled = true;
        Current_GameState = Global.GameState.InPlay_Board;
        deck = Instantiate(starter_deck);
        Invoke("Change_State", 0);

    }

    private void Update()
    {
    }

    public void Change_State()
    {
        print(Current_GameState);
        

        if (Current_GameState == GameState.InPlay_PieceSelect)
        {
            this.current_piece_select = Instantiate(Piece_Select, this.transform);
        }
        else
        {
            if (this.current_piece_select != null)
            {
                Destroy(this.current_piece_select.gameObject);
                this.board.GetComponent<Board>().Generate_Piece(deck.Temp_Deck[this.board.GetComponent<Board>().level - 2]);
            }
            this.current_piece_select = null;
        }

        if (Current_GameState == GameState.InPlay_Board)
        {
            this.board.enabled = true;
        }
        else
        {
            this.board.enabled = false;
        }

    }

}
