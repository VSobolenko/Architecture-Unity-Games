namespace Infrastructure
{
public interface IExitable
{
    void Exit();
}

public interface IState : IExitable
{
    void Enter();
}

public interface IPayloadedState<TPayload> : IExitable
{
    void Enter(TPayload payload);
}
}