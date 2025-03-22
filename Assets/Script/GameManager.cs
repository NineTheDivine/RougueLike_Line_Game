using UnityEngine;
using static Global;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        GameOver,
        InPlay_Board,
        InPlay_PieceSelect,
        Pause,
        StageClear,
    }
    public GameState Current_GameState;

    private readonly Dictionary<GameState, IState<GameManager>> dicState = new Dictionary<GameState, IState<GameManager>>() {
        { GameState.GameOver, new GameOver()},
        { GameState.InPlay_Board, new InPlay_Board()},
        { GameState.InPlay_PieceSelect, new InPlay_PieceSelect()},
        { GameState.Pause, new Pause()},
        { GameState.StageClear, new StageClear()}
    };
    private GameStateMachine<GameManager> game_state_machine;

    public static int is_hold_count = 1;
    public static int base_reload_count = 5;

    public static int piece_selection_number = 3;

    public static Deck deck;
    public Deck starter_deck;

    public Board board;
    public Piece_Selection Piece_Select;
    public GameObject GameOver;


    private void Awake()
    {
        this.GameObject().GetComponent<Transform>().localScale = new Vector3(Global.scale_background, Global.scale_background, 1.0f);
        board.enabled = true;
        deck = Instantiate(starter_deck);
        this.Current_GameState = GameState.InPlay_Board;
        game_state_machine = new GameStateMachine<GameManager>(this, dicState[GameState.InPlay_Board]);
    }

    public void Change_State(GameState state)
    {
        if (dicState[state] == game_state_machine.CurState)
            return;
        this.Current_GameState = state;
        game_state_machine.SetState(dicState[state]);
    }

    public Piece_Selection Gen_Piece_Selection()
    {
        return Instantiate(this.Piece_Select, this.transform);
    }

    public void Destroy_Piece_Selection(Piece_Selection ps)
    {
        board.GetComponent<Board>().Generate_Piece(deck.Temp_Deck[board.GetComponent<Board>().level - 2]);
        Destroy(ps.gameObject);
    }

    public GameObject Gen_GameOver()
    {
        GameObject temp = Instantiate(this.GameOver, this.transform);
        temp.GetComponentInChildren<Animation>().Play("Gameover_visible");
        return temp;
    }
    public void Destroy_GameOver(GameObject go)
    {
        if(go.GetComponentInChildren<Animation>().IsPlaying("Gameover_visible"))
            go.GetComponentInChildren<Animation>().Stop();
        Destroy(go);
    }
}
