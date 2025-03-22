using System.Runtime.Remoting.Messaging;
using Unity.VisualScripting;
using UnityEngine.Assertions;

public class GameStateMachine<T>
{
    private T m_sender;
    public IState<T> CurState { get; set; }

    public GameStateMachine(T sender, IState<T> state)
    {
        m_sender = sender;
        SetState(state);
    }

    public void SetState(IState<T> state)
    {
        if (m_sender == null)
        {
            Assert.IsTrue(false, "m_sender is empty in GameStateMachine");
            return;
        }
        if (CurState == state)
        {
            Assert.IsTrue(false, "Current state is same as given state " + state);
            return;
        }

        if (CurState != null)
            CurState.OperateExit(m_sender);

        CurState = state;
        if (CurState != null)
            CurState.OperateEnter(m_sender);
    }
}