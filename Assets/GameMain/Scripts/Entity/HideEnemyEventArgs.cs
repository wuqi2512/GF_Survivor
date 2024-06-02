using GameFramework.Event;
using GameFramework;

public class HideEnemyEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(HideEnemyEventArgs).GetHashCode();

    public int EntityId
    {
        get;
        private set;
    }

    public HideEnemyEventArgs()
    {
        EntityId = -1;
    }

    public override int Id => EventId;

    public static HideEnemyEventArgs Create(int entityId)
    {
        HideEnemyEventArgs HideEnemyEventArgs = ReferencePool.Acquire<HideEnemyEventArgs>();
        HideEnemyEventArgs.EntityId = entityId;
        return HideEnemyEventArgs;
    }

    public override void Clear()
    {
        EntityId = -1;
    }
}