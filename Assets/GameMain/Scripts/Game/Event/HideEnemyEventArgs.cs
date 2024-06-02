using GameFramework;
using GameFramework.Event;

public class HideEnemyEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(HideEnemyEventArgs).GetHashCode();

    public int SerialId { get; private set; }
    public bool IsDead { get; private set; }

    public override int Id => EventId;

    public static HideEnemyEventArgs Create(int serialId, bool isDead)
    {
        HideEnemyEventArgs args = ReferencePool.Acquire<HideEnemyEventArgs>();
        args.SerialId = serialId;
        args.IsDead = isDead;
        return args;
    }

    public override void Clear()
    {
        SerialId = -1;
    }
}