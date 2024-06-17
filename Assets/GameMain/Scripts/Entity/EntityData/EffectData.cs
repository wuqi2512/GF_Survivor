using cfg;
using GameFramework;
using UnityEngine;

public class EffectData : EntityData
{
    public Effect DREffect { get; protected set; }
    public Transform Follow { get; protected set; }
    public Vector3 Offset { get; protected set; }
    public Vector3 Scale { get; protected set; }

    public EffectData()
    {
        Scale = Vector3.one;
    }

    public override int EntityId => DREffect.EntityId;

    public static EffectData Create(int effectId, int serialId, Transform follow)
    {
        EffectData effectData = ReferencePool.Acquire<EffectData>();
        effectData.DREffect = GameEntry.Luban.Tables.TbEffect.GetOrDefault(effectId);
        effectData.m_SerialId = serialId;
        effectData.Follow = follow;

        return effectData;
    }

    public static EffectData Create(int effectId, int serialId, Vector3 position, Quaternion rotation)
    {
        EffectData effectData = ReferencePool.Acquire<EffectData>();
        effectData.DREffect = GameEntry.Luban.Tables.TbEffect.GetOrDefault(effectId);
        effectData.m_SerialId = serialId;
        effectData.m_Position = position;
        effectData.m_Rotation = rotation;

        return effectData;
    }

    public static EffectData Create(int effectId, int serialId, Vector3 position)
    {
        EffectData effectData = ReferencePool.Acquire<EffectData>();
        effectData.DREffect = GameEntry.Luban.Tables.TbEffect.GetOrDefault(effectId);
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