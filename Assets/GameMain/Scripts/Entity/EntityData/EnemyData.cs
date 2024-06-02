using GameFramework;
using UnityEngine;

public class EnemyData : EntityData
{
    private int m_EntityId;
    public override int EntityId => m_EntityId;

    public int MexHp => 50;

    public static EnemyData Create(int entityId, int serialId)
    {
        EnemyData enemyData = ReferencePool.Acquire<EnemyData>();
        enemyData.m_EntityId = entityId;
        enemyData.m_SerialId = serialId;
        return enemyData;
    }

    public static EnemyData Create(int entityId, int serialId, Vector3 position)
    {
        EnemyData enemyData = ReferencePool.Acquire<EnemyData>();
        enemyData.m_EntityId = entityId;
        enemyData.m_SerialId = serialId;
        enemyData.m_Position = position;
        return enemyData;
    }

    public override void Clear()
    {
        base.Clear();

        m_EntityId = 0;
    }
}