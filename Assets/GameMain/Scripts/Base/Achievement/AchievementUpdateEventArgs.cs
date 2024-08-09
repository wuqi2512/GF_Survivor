using GameFramework;
using GameFramework.Event;

public class AchievementUpdateEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(AchievementUpdateEventArgs).GetHashCode();
    
    public override int Id => EventId;

    public AchievementData AchievementData { get; private set; }

    public static AchievementUpdateEventArgs Create(AchievementData achievementData)
    {
        var args = ReferencePool.Acquire<AchievementUpdateEventArgs>();
        args.AchievementData = achievementData;

        return args;
    }

    public override void Clear()
    {
        AchievementData = null;
    }
}