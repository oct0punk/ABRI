public abstract class FSM_BaseState
{
    public abstract void OnEnter(Lumberjack l);
    public abstract void Update(Lumberjack l);
    public abstract void OnExit(Lumberjack l);
}
