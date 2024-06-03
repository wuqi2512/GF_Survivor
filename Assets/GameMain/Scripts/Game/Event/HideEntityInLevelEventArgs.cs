using GameFramework;
using GameFramework.Event;

public class HideEntityInLevelEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(HideEntityInLevelEventArgs).GetHashCode();
    public override int Id => EventId;

    public int SerialId { get; private set; }

    public bool IsEnemy { get; private set; }
    public bool IsEnemyDead {  get; private set; }

    public static HideEntityInLevelEventArgs Create(int serialId, bool isEnemy = false, bool isEnemyDead = false)
    {
        var args = ReferencePool.Acquire<HideEntityInLevelEventArgs>();
        args.SerialId = serialId;
        args.IsEnemy = isEnemy;
        args.IsEnemyDead = isEnemyDead;

        return args;
    }

    public override void Clear()
    {
        SerialId = 0;
        IsEnemy = false;
        IsEnemyDead = false;
    }
}