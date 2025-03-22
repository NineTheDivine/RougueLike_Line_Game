public interface IState<T>
{
    void OperateEnter(T sender);
    void OperateExit(T sender);
}
