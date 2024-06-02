using GameFramework;
using GameFramework.Event;

public class PlayerHpChangedEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(PlayerHpChangedEventArgs).GetHashCode();

    public override int Id => EventId;

    public int LastHp { get; private set; }
    public int CurrentHp { get; private set; }

    public static PlayerHpChangedEventArgs Create(int lastHp, int currentHp)
    {
        var args = ReferencePool.Acquire<PlayerHpChangedEventArgs>();
        args.LastHp = lastHp;
        args.CurrentHp = currentHp;

        return args;
    }

    public override void Clear()
    {
        LastHp = 0;
        CurrentHp = 0;
    }
}