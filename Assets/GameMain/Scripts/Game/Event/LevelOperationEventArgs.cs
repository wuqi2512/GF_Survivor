using GameFramework;
using GameFramework.Event;

public class LevelOperationEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(LevelOperationEventArgs).GetHashCode();

    public override int Id => EventId;

    public LevelOperation LevelOperation { get; private set; }

    public static LevelOperationEventArgs Create(LevelOperation levelOperation)
    {
        var args = ReferencePool.Acquire<LevelOperationEventArgs>();
        args.LevelOperation = levelOperation;

        return args;
    }

    public override void Clear()
    {
        LevelOperation = LevelOperation.None;
    }
}