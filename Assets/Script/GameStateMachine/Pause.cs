using Unity.VisualScripting;
using UnityEngine;

public class Pause : IState<GameManager>
{
    private GameManager _gameManager;

    public void OperateEnter(GameManager sender)
    {
    }
    public void OperateExit(GameManager sender)
    {

    }
}