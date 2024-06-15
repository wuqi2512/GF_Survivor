using GameFramework;
using GameFramework.DataTable;
using StarForce;
using Unity.VisualScripting;
using UnityEngine;
using UnityGameFramework.Runtime;

public class EffectData : EntityData
{
    public DREffect DREffect { get; protected set; }
    public Transform Follow { get;protected set; }
    public Vector3 Offset { get; protected set; }
    public Vector3 Scale { get; protected set; }

    public EffectData()
    {
        Scale = Vector3.one;
    }

    public override int EntityId => DREffect.EntityId;

    public static EffectData Create(int effectId, int serialId, Transform follow)
    {
        IDataTable<DREffect> dtEffect = GameEntry.DataTable.GetDataTable<DREffect>();
        DREffect drEffect = dtEffect.GetDataRow(effectId);
        if (drEffect == null)
        {
            Log.Warning("Can not load effect id '{0}' from data table.", effectId.ToString());
            return null;
        }

        EffectData effectData = ReferencePool.Acquire<EffectData>();
        effectData.DREffect = drEffect;
        effectData.m_SerialId = serialId;
        effectData.Follow = follow;

        return effectData;
    }

    public static EffectData Create(int effectId, int serialId, Vector3 position, Quaternion rotation)
    {
        IDataTable<DREffect> dtEffect = GameEntry.DataTable.GetDataTable<DREffect>();
        DREffect drEffect = dtEffect.GetDataRow(effectId);
        if (drEffect == null)
        {
            Log.Warning("Can not load effect id '{0}' from data table.", effectId.ToString());
            return null;
        }

        EffectData effectData = ReferencePool.Acquire<EffectData>();
        effectData.DREffect = drEffect;
        effectData.m_SerialId = serialId;
        effectData.m_Position = position;
        effectData.m_Rotation = rotation;

        return effectData;
    }

    public static EffectData Create(int effectId, int serialId, Vector3 position)
    {
        IDataTable<DREffect> dtEffect = GameEntry.DataTable.GetDataTable<DREffect>();
        DREffect drEffect = dtEffect.GetDataRow(effectId);
        if (drEffect == null)
        {
            Log.Warning("Can not load effect id '{0}' from data table.", effectId.ToString());
            return null;
        }

        EffectData effectData = ReferencePool.Acquire<EffectData>();
        effectData.DREffect = drEffect;
        effectData.m_SerialId = serialId;
        effectData.m_Position = position;

        return effectData;
    }

    public override void Clear()
    {
        base.Clear();

        Follow = null;
        Offset = Vector3.zero;
        Scale = Vector3.one;
    }
}