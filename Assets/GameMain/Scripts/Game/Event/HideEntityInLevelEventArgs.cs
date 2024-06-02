using GameFramework;
using GameFramework.Event;

public class HideEntityInLevelEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(HideEntityInLevelEventArgs).GetHashCode();
    public override int Id => EventId;

    public int SerialId { get; private set; }

    public static HideEntityInLevelEventArgs Create(int serialId)
    {
        var args = ReferencePool.Acquire<HideEntityInLevelEventArgs>();
        args.SerialId = serialId;

        return args;
    }

    public override void Clear()
    {
        SerialId = 0;
    }
}