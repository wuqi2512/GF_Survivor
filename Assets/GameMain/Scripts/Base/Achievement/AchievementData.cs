using cfg;
using GameFramework;
using System;

public class AchievementData : IReference
{
    private Achievement m_Achievement;
    private int m_EventId;
    private int m_Value;
    private bool m_HasReceiveAward;

    public static AchievementData Create(int achievementId, int value, bool receive)
    {
        AchievementData achievementData = ReferencePool.Acquire<AchievementData>();
        achievementData.m_Achievement = GameEntry.Luban.Tables.TbAchievement.Get(achievementId);
        achievementData.m_Value = value;
        achievementData.HasReceiveAward = receive;
        achievementData.m_EventId = Type.GetType(achievementData.m_Achievement.EventName).GetHashCode();

        return achievementData;
    }

    public int Value
    {
        get
        {
            return m_Value;
        }
        set
        {
            m_Value = Math.Min(MaxValue, value);
        }
    }
    public bool HasReceiveAward
    {
        get => m_HasReceiveAward;
        set => m_HasReceiveAward = value;
    }
    public int EventId => m_EventId;
    public int AchievementId => m_Achievement.Id;
    public int MaxValue => m_Achievement.MaxValue;
    public Achievement Achievement => m_Achievement;
    public bool IsCompleted => Value >= MaxValue;

    public void Clear()
    {
        m_Achievement = null;
        m_Value = 0;
        m_EventId = 0;
    }
}