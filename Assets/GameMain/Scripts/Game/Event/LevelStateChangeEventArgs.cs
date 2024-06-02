using GameFramework;
using GameFramework.Event;

public class LevelStateChangeEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(LevelStateChangeEventArgs).GetHashCode();

    public override int Id => EventId;

    public LevelState LastState { get; private set; }
    public LevelState CurrentState { get; private set; }

    public static LevelStateChangeEventArgs Create(LevelState lastState, LevelState currentState)
    {
        var args = ReferencePool.Acquire<LevelStateChangeEventArgs>();
        args.LastState = lastState;
        args.CurrentState = currentState;

        return args;
    }

    public override void Clear()
    {
        LastState = LevelState.None;
        CurrentState = LevelState.None;
    }
}