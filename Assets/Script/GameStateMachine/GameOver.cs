using Unity.VisualScripting;
using UnityEngine;

public class GameOver : IState<GameManager>
{
    private GameManager _gameManager;
    private GameObject _gameover;

    public void OperateEnter(GameManager sender)
    {
        _gameManager = sender;
        if (_gameManager != null && this._gameover == null)
            this._gameover = _gameManager.Gen_GameOver();
    }
    public void OperateExit(GameManager sender)
    {
        if (this._gameover != null)
            _gameManager.Destroy_GameOver(_gameover);
        this._gameover = null;
    }
}