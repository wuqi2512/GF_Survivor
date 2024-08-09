using GameFramework;
using UnityEngine;

public class ChestData : EntityData
{
    private int m_EntityId;
    private DropPool m_DropPool;

    public override int EntityId => m_EntityId;
    public DropPool DropPool => m_DropPool;

    public static ChestData Create(int serialId, DropPool dropPool, Vector3 pos)
    {
        ChestData chestData = ReferencePool.Acquire<ChestData>();
        chestData.m_EntityId = (int)EntityType.Chest;
        chestData.m_SerialId = serialId;
        chestData.m_DropPool = dropPool;
        chestData.m_Position = pos;

        return chestData;
    }

    public override void Clear()
    {
        base.Clear();

        m_EntityId = 0;
    }
}