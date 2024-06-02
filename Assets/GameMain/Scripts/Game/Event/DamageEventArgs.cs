using GameFramework;
using GameFramework.Event;

public class DamageEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(DamageEventArgs).GetHashCode();

    public override int Id => EventId;

    public DamageInfo DamageInfo { get; private set; }

    public static DamageEventArgs Create(DamageInfo damageInfo)
    {
        DamageEventArgs args = ReferencePool.Acquire<DamageEventArgs>();
        args.DamageInfo = damageInfo;

        return args;
    }

    public override void Clear()
    {
        ReferencePool.Release(DamageInfo);
    }
}