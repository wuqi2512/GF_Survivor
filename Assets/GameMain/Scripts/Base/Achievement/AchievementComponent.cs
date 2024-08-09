using GameFramework;
using GameFramework.Event;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityGameFramework.Runtime;

// Event
// KillEnemies
// OpenChest
// GainEquipments
// GainCoin
// CompleteGames
public partial class AchievementComponent : GameFrameworkComponent
{
    private readonly string FileName = "Achievement.dat";
    private string FilePath;
    private AchievementSerializer m_Serializer;
    private EventComponent m_Event;
    private List<AchievementData> m_Achievements;
    private List<AchievementData> m_UncompletedAchievements;
    private HashSet<int> m_EventIds;

    protected override void Awake()
    {
        base.Awake();

        FilePath = Utility.Path.GetRegularPath(Path.Combine(Application.persistentDataPath, FileName));
        m_Serializer = new AchievementSerializer();
        m_Serializer.RegisterSerializeCallback(0, Serialize);
        m_Serializer.RegisterDeserializeCallback(0, Deserialize);

        m_UncompletedAchievements = new List<AchievementData>();
        m_EventIds = new HashSet<int>();
    }

    private void Start()
    {
        m_Event = GameEntry.Event;
    }

    public List<AchievementData> GetAllAchievementDatas()
    {
        return m_Achievements;
    }

    public void ReceiveAward(AchievementData achievementData)
    {
        if (achievementData == null || achievementData.HasReceiveAward)
        {
            return;
        }

        achievementData.HasReceiveAward = true;
        GameEntry.Player.AddDiamond(achievementData.Achievement.AwardDiamond);
        m_Event.Fire(this, AchievementUpdateEventArgs.Create(achievementData));
    }

    public void Load()
    {
        try
        {
            if (!File.Exists(FilePath))
            {
                m_Achievements = new List<AchievementData>();
                GetDefaultAchivementDatas(m_Achievements);
            }
            else
            {
                using (FileStream fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    m_Achievements = m_Serializer.Deserialize(fileStream);
                }
            }
        }
        catch (Exception exception)
        {
            m_Achievements = new List<AchievementData>();
            GetDefaultAchivementDatas(m_Achievements);

            Log.Warning("Load Achievement failure with exception '{0}'.", exception);
        }

        m_UncompletedAchievements.Clear();
        foreach (var achievement in m_Achievements)
        {
            if (!achievement.IsCompleted)
            {
                m_UncompletedAchievements.Add(achievement);
            }
        }

        m_EventIds.Clear();
        foreach (var achievement in m_Achievements)
        {
            if (achievement.IsCompleted || m_EventIds.Contains(achievement.EventId))
            {
                continue;
            }

            m_Event.Subscribe(achievement.EventId, EventHandler);
            m_EventIds.Add(achievement.EventId);
        }
    }

    public void Save()
    {
        try
        {
            using (FileStream fileStream = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
            {
                m_Serializer.Serialize(fileStream, m_Achievements);
            }
        }
        catch (Exception exception)
        {
            Log.Warning("Save Achievement failure with exception '{0}'.", exception);
        }
    }

    private static bool Serialize(Stream stream, List<AchievementData> achievements)
    {
        using (BinaryWriter writer = new BinaryWriter(stream))
        {
            writer.Write(achievements.Count);
            foreach (AchievementData achievementData in achievements)
            {
                writer.Write(achievementData.AchievementId);
                writer.Write(achievementData.Value);
                writer.Write(achievementData.HasReceiveAward);
            }
        }

        return true;
    }

    private static void GetDefaultAchivementDatas(List<AchievementData> list)
    {
        list.Clear();

        foreach (var achievement in GameEntry.Luban.Tables.TbAchievement.DataList)
        {
            list.Add(AchievementData.Create(achievement.Id, 0, false));
        }
    }

    private static List<AchievementData> Deserialize(Stream stream)
    {
        List<AchievementData> result = new List<AchievementData>();

        using (BinaryReader reader = new BinaryReader(stream))
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                int achievementId = reader.ReadInt32();
                int value = reader.ReadInt32();
                bool receive = reader.ReadBoolean();
                AchievementData achievementData = AchievementData.Create(achievementId, value, receive);
                result.Add(achievementData);
            }
        }

        return result;
    }

    private void EventHandler(object sender, GameEventArgs args)
    {
        int eventId = args.Id;
        var handler = s_Handlers[eventId];
        if (handler == null)
        {
            Log.Error($"Can not find Handler of Event '{args.GetType().Name}'.");
            return;
        }
        int value = handler.Invoke(args);
        bool flags = false;
        for (int i = m_UncompletedAchievements.Count - 1; i >= 0; i--)
        {
            var achievement = m_UncompletedAchievements[i];
            if (achievement.EventId == eventId)
            {
                flags = true;
                achievement.Value += value;
                m_Event.Fire(this, AchievementUpdateEventArgs.Create(achievement));
                if (achievement.IsCompleted)
                {
                    m_UncompletedAchievements.RemoveAt(i);
                    m_Event.Fire(this, AchievementCompletedEventArgs.Create(achievement));
                }
            }
        }

        if (!flags)
        {
            m_Event.Unsubscribe(eventId, EventHandler);
            m_EventIds.Remove(eventId);
        }
    }
}