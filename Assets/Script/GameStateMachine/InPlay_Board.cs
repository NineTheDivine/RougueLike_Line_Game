using Unity.VisualScripting;
using UnityEngine;

public class InPlay_Board : IState<GameManager>
{
    private GameManager _gameManager;

    public void OperateEnter(GameManager sender)
    {
        _gameManager = sender;
        if(_gameManager != null)
            _gameManager.board.enabled = true;
    }
    public void OperateExit(GameManager sender)
    {
        if (_gameManager != null)
            _gameManager.board.enabled = false;
    }
}