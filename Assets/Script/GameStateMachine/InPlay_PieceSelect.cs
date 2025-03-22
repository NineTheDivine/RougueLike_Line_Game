using Unity.VisualScripting;
using UnityEngine;
using static GameManager;

public class InPlay_PieceSelect : IState<GameManager>
{
    private GameManager _gameManager;
    private Piece_Selection _current_piece_select;

    public void OperateEnter(GameManager sender)
    {
        _gameManager = sender;
        if (_gameManager != null && this._current_piece_select == null)
            this._current_piece_select = _gameManager.Gen_Piece_Selection();
            
    }
    public void OperateExit(GameManager sender)
    {
        if (this._current_piece_select != null)
            _gameManager.Destroy_Piece_Selection(this._current_piece_select);
        this._current_piece_select = null;
    }
}