using GameFramework.Event;
using GameFramework;

public class AchievementCompletedEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(AchievementCompletedEventArgs).GetHashCode();

    public override int Id => EventId;

    public AchievementData AchievementData { get; private set; }

    public static AchievementCompletedEventArgs Create(AchievementData achievementData)
    {
        var args = ReferencePool.Acquire<AchievementCompletedEventArgs>();
        args.AchievementData = achievementData;

        return args;
    }

    public override void Clear()
    {
        AchievementData = null;
    }
}